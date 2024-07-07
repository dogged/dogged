using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
        /// <summary>
        /// Parse a revision string for `from`, `to`, and intent.
        ///
        /// <para>
        ///  See `man gitrevisions` or
        /// http://git-scm.com/docs/git-rev-parse.html#_specifying_revisions
        /// for information on the syntax accepted.
        /// </para>
        ///
        /// <para>
        /// The contents of the revspec object must be freed by the user.
        /// </para>
        /// <param name="revspec">Pointer to an user-allocated git_revspec struct where the result of the rev-parse will be stored</param>
        /// <param name="repo">the repository to search in</param>
        /// <param name="spec">the revision spec to parse</param>
        /// </summary>
        /// <returns>0 on success, GIT_INVALIDSPEC, GIT_ENOTFOUND, GIT_EAMBIGUOUS or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_revparse(
            git_revspec revspec,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string spec);
    }
}
