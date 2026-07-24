using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
        /// <summary>
        ///  Find a single object, as specified by a revision string.
        ///
        ///  <para>
        ///   See `man gitrevisions`, or
        ///   http://git-scm.com/docs/git-rev-parse.html#_specifying_revisions
        ///   for information on the syntax accepted.
        ///  </para>
        ///
        ///  <para>
        ///   The returned object should be released with
        ///   `git_object_free` when no longer needed.
        ///  </para>
        /// </summary>
        /// <param name="_out">pointer to output object</param>
        /// <param name="repo">the repository to search in</param>
        /// <param name="spec">
        ///  the textual specification for an object
        /// </param>
        /// <returns>
        ///  0 on success, GIT_ENOTFOUND, GIT_EAMBIGUOUS,
        ///  GIT_EINVALIDSPEC or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_revparse_single(
            out ObjectHandle _out,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string spec);

        /// <summary>
        ///  Find a single object and intermediate reference by a
        ///  revision string.
        ///
        ///  <para>
        ///   See `man gitrevisions`, or
        ///   http://git-scm.com/docs/git-rev-parse.html#_specifying_revisions
        ///   for information on the syntax accepted.
        ///  </para>
        ///
        ///  <para>
        ///   In some cases (` @ { < -n>}` or ` <branchname> @
        ///   {upstream}`), the expression may point to an
        ///   intermediate reference. When such expressions are being
        ///   passed in, `reference_out` will be valued as well.
        ///  </para>
        ///
        ///  <para>
        ///   The returned object should be released with
        ///   `git_object_free` and the returned reference with
        ///   `git_reference_free` when no longer needed.
        ///  </para>
        /// </summary>
        /// <param name="object_out">pointer to output object</param>
        /// <param name="reference_out">
        ///  pointer to output reference or NULL
        /// </param>
        /// <param name="repo">the repository to search in</param>
        /// <param name="spec">
        ///  the textual specification for an object
        /// </param>
        /// <returns>
        ///  0 on success, GIT_ENOTFOUND, GIT_EAMBIGUOUS,
        ///  GIT_EINVALIDSPEC or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_revparse_ext(
            ObjectHandle object_out,
            ReferenceHandle reference_out,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string spec);

        /// <summary>
        ///  Parse a revision string for `from`, `to`, and intent.
        ///
        ///  <para>
        ///   See `man gitrevisions` or
        ///   http://git-scm.com/docs/git-rev-parse.html#_specifying_revisions
        ///   for information on the syntax accepted.
        ///  </para>
        /// </summary>
        /// <param name="revspec">
        ///  Pointer to an user-allocated git_revspec struct where the
        ///  result of the rev-parse will be stored
        /// </param>
        /// <param name="repo">the repository to search in</param>
        /// <param name="spec">the rev-parse spec to parse</param>
        /// <returns>
        ///  0 on success, GIT_INVALIDSPEC, GIT_ENOTFOUND, GIT_EAMBIGUOUS
        ///  or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_revparse(
            ref git_revspec revspec,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string spec);
    }
}
