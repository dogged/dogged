using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Connect the stream
    /// </summary>
    /// <param name="stream">the stream to connect</param>
    /// <returns>0 on success, or an error code</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_stream_connect_cb(
        ref git_stream stream);

    /// <summary>
    ///  Certificate lookup for the stream
    /// </summary>
    /// <param name="_out">the certificate</param>
    /// <param name="stream">the stream to get the certificate for</param>
    /// <returns>0 on success, or an error code</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_stream_certificate_cb(
        out git_cert* _out,
        ref git_stream stream);

    /// <summary>
    ///  Set the proxy for the stream
    /// </summary>
    /// <param name="stream">the stream to set the proxy for</param>
    /// <param name="proxy_opts">the proxy options</param>
    /// <returns>0 on success, or an error code</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_stream_set_proxy_cb(
        ref git_stream stream,
        ref git_proxy_options proxy_opts);

    /// <summary>
    ///  Read from the stream.
    /// </summary>
    /// <param name="stream">the stream to read from</param>
    /// <param name="buf">the buffer to write to</param>
    /// <param name="size">the size of the buffer</param>
    /// <returns>the number of bytes read, or an error code</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate IntPtr git_stream_read_cb(
        ref git_stream stream,
        IntPtr buf,
        UIntPtr size);

    /// <summary>
    ///  Write to the stream
    /// </summary>
    /// <param name="stream">the stream to write to</param>
    /// <param name="data">the data to write</param>
    /// <param name="len">the length of the data to write</param>
    /// <param name="flags">the write flags</param>
    /// <returns>the number of bytes written, or an error code</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate IntPtr git_stream_write_cb(
        ref git_stream stream,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string data,
        UIntPtr len,
        int flags);

    /// <summary>
    ///  Close the stream
    /// </summary>
    /// <param name="stream">the stream to close</param>
    /// <returns>0 on succes, or an error code</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_stream_close_cb(
        ref git_stream stream);

    /// <summary>
    ///  Free the stream
    /// </summary>
    /// <param name="stream">the stream to free</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void git_stream_free_cb(
        ref git_stream stream);

    /// <summary>
    ///  Every stream must have this struct as its first element, so the
    ///  API can talk to it. You'd define your stream as
    ///
    ///  <para>
    ///   struct my_stream { git_stream parent; ... }
    ///  </para>
    ///
    ///  <para>
    ///   and fill the functions
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_stream
    {
        public int version;

        public uint encrypted;

        public uint proxy_support;

        /// <summary>
        ///  Timeout for read and write operations; can be set to `0` to
        ///  block indefinitely.
        /// </summary>
        public int timeout;

        /// <summary>
        ///  Timeout to connect to the remote server; can be set to `0`
        ///  to use the system defaults. This can be shorter than the
        ///  system default - often 75 seconds - but cannot be longer.
        /// </summary>
        public int connect_timeout;

        /// <summary>
        ///  Callback to connect the stream.
        /// </summary>
        public git_stream_connect_cb connect;

        /// <summary>
        ///  Certificate lookup.
        /// </summary>
        public git_stream_certificate_cb certificate;

        /// <summary>
        ///  Set the proxy for the stream.
        /// </summary>
        public git_stream_set_proxy_cb set_proxy;

        /// <summary>
        ///  Read from the stream.
        /// </summary>
        public git_stream_read_cb read;

        /// <summary>
        ///  Write to the stream.
        /// </summary>
        public git_stream_write_cb write;

        /// <summary>
        ///  Close the stream.
        /// </summary>
        public git_stream_close_cb close;

        /// <summary>
        ///  Free the stream.
        /// </summary>
        public git_stream_free_cb free;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_stream_registration
    {
        /// <summary>
        ///  The `version` field should be set to `GIT_STREAM_VERSION`.
        /// </summary>
        public int version;

        /// <summary>
        ///  Called to create a new connection to a given host.
        /// </summary>
        public IntPtr init;

        /// <summary>
        ///  Called to create a new connection on top of the given
        ///  stream. If this is a TLS stream, then this function may be
        ///  used to proxy a TLS stream over an HTTP CONNECT session. If
        ///  this is unset, then HTTP CONNECT proxies will not be
        ///  supported.
        /// </summary>
        public IntPtr wrap;
    }

    /// <summary>
    ///  The type of stream to register.
    /// </summary>
    public enum git_stream_t
    {
        /// <summary>
        ///  A standard (non-TLS) socket.
        /// </summary>
        GIT_STREAM_STANDARD = 1,

        /// <summary>
        ///  A TLS-encrypted socket.
        /// </summary>
        GIT_STREAM_TLS = 2,
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_stream_cb(
        IntPtr a,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string b,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string c);

    public static partial class libgit2
    {
        /// <summary>
        ///  Register stream constructors for the library to use
        ///
        ///  <para>
        ///   If a registration structure is already set, it will be
        ///   overwritten. Pass `NULL` in order to deregister the
        ///   current constructor and return to the system defaults.
        ///  </para>
        ///
        ///  <para>
        ///   The type parameter may be a bitwise AND of types.
        ///  </para>
        /// </summary>
        /// <param name="type">
        ///  the type or types of stream to register
        /// </param>
        /// <param name="registration">the registration data</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_stream_register(
            git_stream_t type,
            ref git_stream_registration registration);

        /// <summary>
        ///  Register a TLS stream constructor for the library to use.
        ///  This stream will not support HTTP CONNECT proxies. This
        ///  internally calls `git_stream_register` and is preserved for
        ///  backward compatibility.
        ///
        ///  <para>
        ///   This function is deprecated, but there is no plan to
        ///   remove this function at this time.
        ///  </para>
        /// </summary>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_stream_register_tls(
            git_stream_cb ctor);
    }
}
