using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
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
            RemoteHandle owner,
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
            RemoteHandle owner,
            IntPtr payload);

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
            RemoteHandle owner,
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
            RemoteHandle owner,
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
            RemoteHandle owner,
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
            RemoteHandle owner,
            IntPtr payload);
    }
}
