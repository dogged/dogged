using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Annotated commits are commits with additional metadata about how
    ///  the commit was resolved, which can be used for maintaining the
    ///  user's "intent" through commands like merge and rebase.
    ///
    ///  <para>
    ///   For example, if a user wants to conceptually "merge `HEAD`",
    ///   then the commit portion of an annotated commit will point to
    ///   the `HEAD` commit, but the _annotation_ will denote the ref
    ///   `HEAD`. This allows git to perform the internal bookkeeping
    ///   so that the system knows both the content of what is being
    ///   merged but also how the content was looked up so that it can
    ///   be recorded in the reflog appropriately.
    ///  </para>
    /// </summary>
    public struct git_annotated_commit
    {
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Creates a `git_annotated_commit` from the given reference.
        ///  The resulting git_annotated_commit must be freed with
        ///  `git_annotated_commit_free`.
        /// </summary>
        /// <param name="_out">
        ///  pointer to store the git_annotated_commit result in
        /// </param>
        /// <param name="repo">
        ///  repository that contains the given reference
        /// </param>
        /// <param name="_ref">
        ///  reference to use to lookup the git_annotated_commit
        /// </param>
        /// <returns>0 on success or error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_annotated_commit_from_ref(
            out git_annotated_commit* _out,
            git_repository* repo,
            git_reference* _ref);

        /// <summary>
        ///  Creates a `git_annotated_commit` from the given fetch head
        ///  data. The resulting git_annotated_commit must be freed with
        ///  `git_annotated_commit_free`.
        /// </summary>
        /// <param name="_out">
        ///  pointer to store the git_annotated_commit result in
        /// </param>
        /// <param name="repo">
        ///  repository that contains the given commit
        /// </param>
        /// <param name="branch_name">name of the (remote) branch</param>
        /// <param name="remote_url">url of the remote</param>
        /// <param name="id">
        ///  the commit object id of the remote branch
        /// </param>
        /// <returns>0 on success or error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_annotated_commit_from_fetchhead(
            out git_annotated_commit* _out,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string branch_name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string remote_url,
            ref git_oid id);

        /// <summary>
        ///  Creates a `git_annotated_commit` from the given commit id.
        ///  The resulting git_annotated_commit must be freed with
        ///  `git_annotated_commit_free`.
        ///
        ///  <para>
        ///   An annotated commit contains information about how it
        ///   was looked up, which may be useful for functions like
        ///   merge or rebase to provide context to the operation. For
        ///   example, conflict files will include the name of the
        ///   source or target branches being merged. It is therefore
        ///   preferable to use the most specific function (eg
        ///   `git_annotated_commit_from_ref`) instead of this one
        ///   when that data is known.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  pointer to store the git_annotated_commit result in
        /// </param>
        /// <param name="repo">
        ///  repository that contains the given commit
        /// </param>
        /// <param name="id">the commit object id to lookup</param>
        /// <returns>0 on success or error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_annotated_commit_lookup(
            out git_annotated_commit* _out,
            git_repository* repo,
            ref git_oid id);

        /// <summary>
        ///  Creates a `git_annotated_commit` from a revision string.
        ///
        ///  <para>
        ///   See `man gitrevisions`, or
        ///   http://git-scm.com/docs/git-rev-parse.html#_specifying_revisions
        ///   for information on the syntax accepted.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  pointer to store the git_annotated_commit result in
        /// </param>
        /// <param name="repo">
        ///  repository that contains the given commit
        /// </param>
        /// <param name="revspec">
        ///  the extended sha syntax string to use to lookup the commit
        /// </param>
        /// <returns>0 on success or error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_annotated_commit_from_revspec(
            out git_annotated_commit* _out,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string revspec);

        /// <summary>
        ///  Gets the commit ID that the given `git_annotated_commit`
        ///  refers to.
        /// </summary>
        /// <param name="commit">the given annotated commit</param>
        /// <returns>commit id</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_annotated_commit_id(
            git_annotated_commit* commit);

        /// <summary>
        ///  Get the refname that the given `git_annotated_commit` refers
        ///  to.
        /// </summary>
        /// <param name="commit">the given annotated commit</param>
        /// <returns>ref name.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_annotated_commit_ref(
            git_annotated_commit* commit);

        /// <summary>
        ///  Frees a `git_annotated_commit`.
        /// </summary>
        /// <param name="commit">annotated commit to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_annotated_commit_free(
            git_annotated_commit* commit);
    }
}
