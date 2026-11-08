using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  The negotiation state during a fetch smart transport
    ///  negotiation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_fetch_negotiation
    {
        public IntPtr refs;

        public UIntPtr refs_len;

        public IntPtr shallow_roots;

        public UIntPtr shallow_roots_len;

        public int depth;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_transport
    {
        /// <summary>
        ///  The struct version
        /// </summary>
        public uint version;

        /// <summary>
        ///  Connect the transport to the remote repository, using the
        ///  given direction.
        /// </summary>
        public IntPtr connect;

        /// <summary>
        ///  Resets the connect options for the given transport. This is
        ///  useful for updating settings or callbacks for an already
        ///  connected transport.
        /// </summary>
        public IntPtr set_connect_opts;

        /// <summary>
        ///  Gets the capabilities for this remote repository.
        ///
        ///  <para>
        ///   This function may be called after a successful call to
        ///   `connect()`.
        ///  </para>
        /// </summary>
        public IntPtr capabilities;

        /// <summary>
        ///  Gets the object type for the remote repository.
        ///
        ///  <para>
        ///   This function may be called after a successful call to
        ///   `connect()`.
        ///  </para>
        /// </summary>
        public IntPtr oid_type;

        /// <summary>
        ///  Get the list of available references in the remote
        ///  repository.
        ///
        ///  <para>
        ///   This function may be called after a successful call to
        ///   `connect()`. The array returned is owned by the
        ///   transport and must be kept valid until the next call to
        ///   one of its functions.
        ///  </para>
        /// </summary>
        public IntPtr ls;

        /// <summary>
        ///  Executes the push whose context is in the git_push object.
        /// </summary>
        public IntPtr push;

        /// <summary>
        ///  Negotiate a fetch with the remote repository.
        ///
        ///  <para>
        ///   This function may be called after a successful call to
        ///   `connect()`, when the direction is GIT_DIRECTION_FETCH.
        ///   The function performs a negotiation to calculate the
        ///   `wants` list for the fetch.
        ///  </para>
        /// </summary>
        public IntPtr negotiate_fetch;

        /// <summary>
        ///  Return the shallow roots of the remote.
        ///
        ///  <para>
        ///   This function may be called after a successful call to
        ///   `negotiate_fetch`.
        ///  </para>
        /// </summary>
        public IntPtr shallow_roots;

        /// <summary>
        ///  Start downloading the packfile from the remote repository.
        ///
        ///  <para>
        ///   This function may be called after a successful call to
        ///   negotiate_fetch(), when the direction is
        ///   GIT_DIRECTION_FETCH.
        ///  </para>
        /// </summary>
        public IntPtr download_pack;

        /// <summary>
        ///  Checks to see if the transport is connected
        /// </summary>
        public IntPtr is_connected;

        /// <summary>
        ///  Cancels any outstanding transport operation
        /// </summary>
        public IntPtr cancel;

        /// <summary>
        ///  Close the connection to the remote repository.
        ///
        ///  <para>
        ///   This function is the reverse of connect() -- it
        ///   terminates the connection to the remote end.
        ///  </para>
        /// </summary>
        public IntPtr close;

        /// <summary>
        ///  Frees/destructs the git_transport object.
        /// </summary>
        public IntPtr free;
    }

    /// <summary>
    ///  Actions that the smart transport can ask a subtransport to
    ///  perform
    /// </summary>
    public enum git_smart_service_t
    {
        GIT_SERVICE_UPLOADPACK_LS = 1,
        GIT_SERVICE_UPLOADPACK = 2,
        GIT_SERVICE_RECEIVEPACK_LS = 3,
        GIT_SERVICE_RECEIVEPACK = 4,
    }

    /// <summary>
    ///  A stream used by the smart transport to read and write data from
    ///  a subtransport.
    ///
    ///  <para>
    ///   This provides a customization point in case you need to
    ///   support some other communication method.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_smart_subtransport_stream
    {
        /// <summary>
        ///  The owning subtransport
        /// </summary>
        public IntPtr subtransport;

        /// <summary>
        ///  Read available data from the stream.
        ///
        ///  <para>
        ///   The implementation may read less than requested.
        ///  </para>
        /// </summary>
        public IntPtr read;

        /// <summary>
        ///  Write data to the stream
        ///
        ///  <para>
        ///   The implementation must write all data or return an
        ///   error.
        ///  </para>
        /// </summary>
        public IntPtr write;

        /// <summary>
        ///  Free the stream
        /// </summary>
        public IntPtr free;
    }

    /// <summary>
    ///  An implementation of a subtransport which carries data for the
    ///  smart transport
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_smart_subtransport
    {
        /// <summary>
        ///  Setup a subtransport stream for the requested action.
        /// </summary>
        public IntPtr action;

        /// <summary>
        ///  Close the subtransport.
        ///
        ///  <para>
        ///   Subtransports are guaranteed a call to close() between
        ///   calls to action(), except for the following two
        ///   "natural" progressions of actions against a constant
        ///   URL:
        ///  </para>
        ///
        ///  <para>
        ///   - UPLOADPACK_LS -> UPLOADPACK - RECEIVEPACK_LS ->
        ///   RECEIVEPACK
        ///  </para>
        /// </summary>
        public IntPtr close;

        /// <summary>
        ///  Free the subtransport
        /// </summary>
        public IntPtr free;
    }

    /// <summary>
    ///  A function that creates a new subtransport for the smart
    ///  transport
    /// </summary>
    /// <param name="_out">the smart subtransport</param>
    /// <param name="owner">the transport owner</param>
    /// <param name="param">the input parameter</param>
    /// <returns>0 on success, or an error code</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_smart_subtransport_cb(
        out git_smart_subtransport* _out,
        ref git_transport owner,
        IntPtr param);

    /// <summary>
    ///  Definition for a "subtransport"
    ///
    ///  <para>
    ///   The smart transport knows how to speak the git protocol, but
    ///   it has no knowledge of how to establish a connection between
    ///   it and another endpoint, or how to move data back and forth.
    ///   For this, a subtransport interface is declared, and the
    ///   smart transport delegates this work to the subtransports.
    ///  </para>
    ///
    ///  <para>
    ///   Three subtransports are provided by libgit2: ssh, git,
    ///   http(s).
    ///  </para>
    ///
    ///  <para>
    ///   Subtransports can either be RPC = 0 (persistent connection)
    ///   or RPC = 1 (request/response). The smart transport handles
    ///   the differences in its own logic. The git subtransport is
    ///   RPC = 0, while http is RPC = 1.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_smart_subtransport_definition
    {
        /// <summary>
        ///  The function to use to create the git_smart_subtransport
        /// </summary>
        public git_smart_subtransport_cb callback;

        /// <summary>
        ///  True if the protocol is stateless; false otherwise. For
        ///  example, http:// is stateless, but git:// is not.
        /// </summary>
        public uint rpc;

        /// <summary>
        ///  User-specified parameter passed to the callback
        /// </summary>
        public IntPtr param;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Initializes a `git_transport` with default values.
        ///  Equivalent to creating an instance with GIT_TRANSPORT_INIT.
        /// </summary>
        /// <param name="opts">
        ///  the `git_transport` struct to initialize
        /// </param>
        /// <param name="version">
        ///  Version of struct; pass `GIT_TRANSPORT_VERSION`
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_transport_init(
            ref git_transport opts,
            uint version);

        /// <summary>
        ///  Function to use to create a transport from a URL. The
        ///  transport database is scanned to find a transport that
        ///  implements the scheme of the URI (i.e. git:// or http://)
        ///  and a transport object is returned to the caller.
        /// </summary>
        /// <param name="_out">The newly created transport (out)</param>
        /// <param name="owner">
        ///  The git_remote which will own this transport
        /// </param>
        /// <param name="url">The URL to connect to</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_transport_new(
            out git_transport* _out,
            git_remote* owner,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string url);

        /// <summary>
        ///  Create an ssh transport with custom git command paths
        ///
        ///  <para>
        ///   This is a factory function suitable for setting as the
        ///   transport callback in a remote (or for a clone in the
        ///   options).
        ///  </para>
        ///
        ///  <para>
        ///   The payload argument must be a strarray pointer with the
        ///   paths for the `git-upload-pack` and `git-receive-pack`
        ///   at index 0 and 1.
        ///  </para>
        /// </summary>
        /// <param name="_out">the resulting transport</param>
        /// <param name="owner">the owning remote</param>
        /// <param name="payload">a strarray with the paths</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_transport_ssh_with_paths(
            out git_transport* _out,
            git_remote* owner,
            IntPtr payload);

        /// <summary>
        ///  Add a custom transport definition, to be used in addition to
        ///  the built-in set of transports that come with libgit2.
        ///
        ///  <para>
        ///   The caller is responsible for synchronizing calls to
        ///   git_transport_register and git_transport_unregister with
        ///   other calls to the library that instantiate transports.
        ///  </para>
        /// </summary>
        /// <param name="prefix">
        ///  The scheme to match, eg "git" or "https"
        /// </param>
        /// <param name="cb">
        ///  The callback used to create an instance of the transport
        /// </param>
        /// <param name="param">
        ///  A fixed parameter to pass to cb at creation time
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_transport_register(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string prefix,
            git_transport_cb cb,
            IntPtr param);

        /// <summary>
        ///  Unregister a custom transport definition which was
        ///  previously registered with git_transport_register.
        ///
        ///  <para>
        ///   The caller is responsible for synchronizing calls to
        ///   git_transport_register and git_transport_unregister with
        ///   other calls to the library that instantiate transports.
        ///  </para>
        /// </summary>
        /// <param name="prefix">
        ///  From the previous call to git_transport_register
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_transport_unregister(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string prefix);

        /// <summary>
        ///  Create an instance of the dummy transport.
        /// </summary>
        /// <param name="_out">The newly created transport (out)</param>
        /// <param name="owner">
        ///  The git_remote which will own this transport
        /// </param>
        /// <param name="payload">
        ///  You must pass NULL for this parameter.
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_transport_dummy(
            out git_transport* _out,
            git_remote* owner,
            IntPtr payload);

        /// <summary>
        ///  Create an instance of the local transport.
        /// </summary>
        /// <param name="_out">The newly created transport (out)</param>
        /// <param name="owner">
        ///  The git_remote which will own this transport
        /// </param>
        /// <param name="payload">
        ///  You must pass NULL for this parameter.
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_transport_local(
            out git_transport* _out,
            git_remote* owner,
            IntPtr payload);

        /// <summary>
        ///  Create an instance of the bundle transport.
        ///
        ///  <para>
        ///   The bundle transport reads references and objects from a
        ///   git bundle file on the local filesystem. It supports
        ///   fetching and cloning; it does not support pushing or
        ///   shallow operations.
        ///  </para>
        /// </summary>
        /// <param name="_out">The newly created transport (out)</param>
        /// <param name="owner">
        ///  The git_remote which will own this transport
        /// </param>
        /// <param name="payload">
        ///  You must pass NULL for this parameter.
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_transport_bundle(
            out git_transport* _out,
            git_remote* owner,
            IntPtr payload);

        /// <summary>
        ///  Create an instance of the smart transport.
        /// </summary>
        /// <param name="_out">The newly created transport (out)</param>
        /// <param name="owner">
        ///  The git_remote which will own this transport
        /// </param>
        /// <param name="payload">
        ///  A pointer to a git_smart_subtransport_definition
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_transport_smart(
            out git_transport* _out,
            git_remote* owner,
            IntPtr payload);

        /// <summary>
        ///  Call the certificate check for this transport.
        /// </summary>
        /// <param name="transport">a smart transport</param>
        /// <param name="cert">the certificate to pass to the caller</param>
        /// <param name="valid">
        ///  whether we believe the certificate is valid
        /// </param>
        /// <param name="hostname">the hostname we connected to</param>
        /// <returns>
        ///  the return value of the callback: 0 for no error,
        ///  GIT_PASSTHROUGH to indicate that there is no callback
        ///  registered (or the callback refused to validate the
        ///  certificate and callers should behave as if no callback was
        ///  set), or < 0 for an error
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_transport_smart_certificate_check(
            ref git_transport transport,
            ref git_cert cert,
            int valid,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string hostname);

        /// <summary>
        ///  Call the credentials callback for this transport
        /// </summary>
        /// <param name="_out">
        ///  the pointer where the creds are to be stored
        /// </param>
        /// <param name="transport">a smart transport</param>
        /// <param name="user">the user we saw on the url (if any)</param>
        /// <param name="methods">
        ///  available methods for authentication
        /// </param>
        /// <returns>
        ///  the return value of the callback: 0 for no error,
        ///  GIT_PASSTHROUGH to indicate that there is no callback
        ///  registered (or the callback refused to provide credentials
        ///  and callers should behave as if no callback was set), or < 0
        ///  for an error
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_transport_smart_credentials(
            out git_credential* _out,
            ref git_transport transport,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string user,
            int methods);

        /// <summary>
        ///  Get a copy of the remote connect options
        ///
        ///  <para>
        ///   All data is copied and must be freed by the caller by
        ///   calling `git_remote_connect_options_dispose`.
        ///  </para>
        /// </summary>
        /// <param name="_out">options struct to fill</param>
        /// <param name="transport">
        ///  the transport to extract the data from.
        /// </param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_transport_remote_connect_options(
            ref git_remote_connect_options _out,
            ref git_transport transport);

        /// <summary>
        ///  Create an instance of the http subtransport.
        ///
        ///  <para>
        ///   This subtransport also supports https.
        ///  </para>
        /// </summary>
        /// <param name="_out">The newly created subtransport</param>
        /// <param name="owner">
        ///  The smart transport to own this subtransport
        /// </param>
        /// <param name="param">
        ///  custom parameters for the subtransport
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_smart_subtransport_http(
            out git_smart_subtransport* _out,
            ref git_transport owner,
            IntPtr param);

        /// <summary>
        ///  Create an instance of the git subtransport.
        /// </summary>
        /// <param name="_out">The newly created subtransport</param>
        /// <param name="owner">
        ///  The smart transport to own this subtransport
        /// </param>
        /// <param name="param">
        ///  custom parameters for the subtransport
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_smart_subtransport_git(
            out git_smart_subtransport* _out,
            ref git_transport owner,
            IntPtr param);

        /// <summary>
        ///  Create an instance of the ssh subtransport.
        /// </summary>
        /// <param name="_out">The newly created subtransport</param>
        /// <param name="owner">
        ///  The smart transport to own this subtransport
        /// </param>
        /// <param name="param">
        ///  custom parameters for the subtransport
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_smart_subtransport_ssh(
            out git_smart_subtransport* _out,
            ref git_transport owner,
            IntPtr param);
    }
}
