using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
        /// <summary>
        ///  Reverts the given commit against the given "our" commit,
        ///  producing an index that reflects the result of the revert.
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
        /// <param name="revert_commit">the commit to revert</param>
        /// <param name="our_commit">
        ///  the commit to revert against (eg, HEAD)
        /// </param>
        /// <param name="mainline">
        ///  the parent of the revert commit, if it is a merge
        /// </param>
        /// <param name="merge_options">
        ///  the merge options (or null for defaults)
        /// </param>
        /// <returns>zero on success, -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_revert_commit(
            out IndexHandle _out,
            RepositoryHandle repo,
            CommitHandle revert_commit,
            CommitHandle our_commit,
            uint mainline,
            ref git_merge_options merge_options);

        /// <summary>
        ///  Reverts the given commit, producing changes in the index and
        ///  working directory.
        /// </summary>
        /// <param name="repo">the repository to revert</param>
        /// <param name="commit">the commit to revert</param>
        /// <param name="given_opts">
        ///  the revert options (or null for defaults)
        /// </param>
        /// <returns>zero on success, -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_revert(
            RepositoryHandle repo,
            CommitHandle commit,
            ref git_revert_options given_opts);
    }
}
