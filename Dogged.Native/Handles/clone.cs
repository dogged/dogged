using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
        /// <summary>
        ///  Clone a remote repository.
        ///
        ///  <para>
        ///   By default this creates its repository and initial
        ///   remote to match git's defaults. You can use the options
        ///   in the callback to customize how these are created.
        ///  </para>
        ///
        ///  <para>
        ///   Note that the libgit2 library _must_ be initialized
        ///   using `git_libgit2_init` before any APIs can be called,
        ///   including this one.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  pointer that will receive the resulting repository object
        /// </param>
        /// <param name="url">the remote repository to clone</param>
        /// <param name="local_path">local directory to clone to</param>
        /// <param name="options">
        ///  configuration options for the clone. If NULL, the function
        ///  works as though GIT_OPTIONS_INIT were passed.
        /// </param>
        /// <returns>
        ///  0 on success, any non-zero return value from a callback
        ///  function, or a negative value to indicate an error (use
        ///  `git_error_last` for a detailed error message)
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_clone(
            out RepositoryHandle _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string url,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string local_path,
            ref git_clone_options options);
    }
}
