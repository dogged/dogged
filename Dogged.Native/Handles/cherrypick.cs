using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
        /// <summary>
        ///  Cherry-picks the given commit against the given "our"
        ///  commit, producing an index that reflects the result of the
        ///  cherry-pick.
        ///
        ///  <para>
        ///   The returned index must be freed explicitly with
        ///   `git_index_free`.
        ///  </para>
        /// </summary>
        /// <param name="_out">pointer to store the index result in</param>
        /// <param name="repo">
        ///  the repository that contains the given commits
        /// </param>
        /// <param name="cherrypick_commit">
        ///  the commit to cherry-pick
        /// </param>
        /// <param name="our_commit">
        ///  the commit to cherry-pick against (eg, HEAD)
        /// </param>
        /// <param name="mainline">
        ///  the parent of the `cherrypick_commit`, if it is a merge
        /// </param>
        /// <param name="merge_options">
        ///  the merge options (or null for defaults)
        /// </param>
        /// <returns>zero on success, -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_cherrypick_commit(
            out IndexHandle _out,
            RepositoryHandle repo,
            CommitHandle cherrypick_commit,
            CommitHandle our_commit,
            uint mainline,
            ref git_merge_options merge_options);

        /// <summary>
        ///  Cherry-pick the given commit, producing changes in the index
        ///  and working directory.
        /// </summary>
        /// <param name="repo">the repository to cherry-pick</param>
        /// <param name="commit">the commit to cherry-pick</param>
        /// <param name="cherrypick_options">
        ///  the cherry-pick options (or null for defaults)
        /// </param>
        /// <returns>zero on success, -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_cherrypick(
            RepositoryHandle repo,
            CommitHandle commit,
            ref git_cherrypick_options cherrypick_options);
    }
}
