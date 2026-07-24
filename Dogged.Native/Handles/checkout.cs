using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
        /// <summary>
        ///  Updates files in the index and the working tree to match the
        ///  content of the commit pointed at by HEAD.
        ///
        ///  <para>
        ///   Note that this is _not_ the correct mechanism used to
        ///   switch branches; do not change your `HEAD` and then call
        ///   this method, that would leave you with checkout
        ///   conflicts since your working directory would then appear
        ///   to be dirty. Instead, checkout the target of the branch
        ///   and then update `HEAD` using `git_repository_set_head`
        ///   to point to the branch you checked out.
        ///  </para>
        /// </summary>
        /// <param name="repo">
        ///  repository to check out (must be non-bare)
        /// </param>
        /// <param name="opts">
        ///  specifies checkout options (may be NULL)
        /// </param>
        /// <returns>
        ///  0 on success, GIT_EUNBORNBRANCH if HEAD points to a non
        ///  existing branch, non-zero value returned by `notify_cb`, or
        ///  other error code < 0 (use git_error_last for error details)
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_checkout_head(
            RepositoryHandle repo,
            ref git_checkout_options opts);

        /// <summary>
        ///  Updates files in the working tree to match the content of
        ///  the index.
        /// </summary>
        /// <param name="repo">
        ///  repository into which to check out (must be non-bare)
        /// </param>
        /// <param name="index">
        ///  index to be checked out (or NULL to use repository index)
        /// </param>
        /// <param name="opts">
        ///  specifies checkout options (may be NULL)
        /// </param>
        /// <returns>
        ///  0 on success, non-zero return value from `notify_cb`, or
        ///  error code < 0 (use git_error_last for error details)
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_checkout_index(
            RepositoryHandle repo,
            IndexHandle index,
            ref git_checkout_options opts);

        /// <summary>
        ///  Updates files in the index and working tree to match the
        ///  content of the tree pointed at by the treeish.
        /// </summary>
        /// <param name="repo">
        ///  repository to check out (must be non-bare)
        /// </param>
        /// <param name="treeish">
        ///  a commit, tag or tree which content will be used to update
        ///  the working directory (or NULL to use HEAD)
        /// </param>
        /// <param name="opts">
        ///  specifies checkout options (may be NULL)
        /// </param>
        /// <returns>
        ///  0 on success, non-zero return value from `notify_cb`, or
        ///  error code < 0 (use git_error_last for error details)
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_checkout_tree(
            RepositoryHandle repo,
            ObjectHandle treeish,
            ref git_checkout_options opts);
    }
}
