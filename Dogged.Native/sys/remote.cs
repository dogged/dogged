using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  A remote's capabilities.
    /// </summary>
    public enum git_remote_capability_t
    {
        /// <summary>
        ///  Remote supports fetching an advertised object by ID.
        /// </summary>
        GIT_REMOTE_CAPABILITY_TIP_OID = 1,

        /// <summary>
        ///  Remote supports fetching an individual reachable object.
        /// </summary>
        GIT_REMOTE_CAPABILITY_REACHABLE_OID = 2,

        /// <summary>
        ///  Remote supports push options.
        /// </summary>
        GIT_REMOTE_CAPABILITY_PUSH_OPTIONS = 4,
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Disposes libgit2-initialized fields from a
        ///  git_remote_connect_options. This should only be used for
        ///  git_remote_connect_options returned by
        ///  git_transport_remote_connect_options.
        ///
        ///  <para>
        ///   Note that this does not free the
        ///   `git_remote_connect_options` itself, just the memory
        ///   pointed to by it.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_remote_connect_options` struct to dispose.
        /// </param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_remote_connect_options_dispose(
            ref git_remote_connect_options opts);
    }
}
