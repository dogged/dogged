using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    /// Clone options structure
    /// </summary>
    public struct git_clone_options
    {
    }

    public static partial class libgit2
    {
        /// <summary>
        /// Clone a remote repository.
        ///
        /// <para>
        /// By default this creates its repository and initial remote to match
        /// git's defaults. You can use the options in the callback to
        /// customize how these are created.
        /// </para>
        /// </summary>
        /// <param name="repo">Pointer to the repository that will be cloned.</param>
        /// <param name="remotePath">The remote repository to clone</param>
        /// <param name="localPath">The local path to clone to</param>
        /// <param name="options">Configuration options for the clone.  If null, defaults will be used.</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_clone(
            out git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string remotePath,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string localPath,
            git_clone_options* options);
    }
}
