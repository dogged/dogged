using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Revparse flags. These indicate the intended behavior of the spec
    ///  passed to git_revparse.
    /// </summary>
    public enum git_revspec_t
    {
        /// <summary>
        ///  The spec targeted a single object.
        /// </summary>
        GIT_REVSPEC_SINGLE = 1,

        /// <summary>
        ///  The spec targeted a range of commits.
        /// </summary>
        GIT_REVSPEC_RANGE = 2,

        /// <summary>
        ///  The spec used the '...' operator, which invokes special
        ///  semantics.
        /// </summary>
        GIT_REVSPEC_MERGE_BASE = 4,
    }

    /// <summary>
    ///  Git Revision Spec: output of a `git_revparse` operation
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_revspec
    {
        /// <summary>
        ///  The left element of the revspec; must be freed by the user
        /// </summary>
        public git_object* from;

        /// <summary>
        ///  The right element of the revspec; must be freed by the user
        /// </summary>
        public git_object* to;

        /// <summary>
        ///  The intent of the revspec (i.e. `git_revspec_mode_t` flags)
        /// </summary>
        public uint flags;
    }

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
            out git_object* _out,
            git_repository* repo,
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
            git_object** object_out,
            git_reference** reference_out,
            git_repository* repo,
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
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string spec);
    }
}
