using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
        /// <summary>
        ///  Analyzes the given branch(es) and determines the
        ///  opportunities for merging them into the HEAD of the
        ///  repository.
        /// </summary>
        /// <param name="analysis_out">
        ///  analysis enumeration that the result is written into
        /// </param>
        /// <param name="preference_out">
        ///  One of the `git_merge_preference_t` flag.
        /// </param>
        /// <param name="repo">the repository to merge</param>
        /// <param name="their_heads">the heads to merge into</param>
        /// <param name="their_heads_len">
        ///  the number of heads to merge
        /// </param>
        /// <returns>0 on success or error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_merge_analysis(
            git_merge_analysis_t* analysis_out,
            git_merge_preference_t* preference_out,
            RepositoryHandle repo,
            AnnotatedCommitHandle their_heads,
            UIntPtr their_heads_len);

        /// <summary>
        ///  Analyzes the given branch(es) and determines the
        ///  opportunities for merging them into a reference.
        /// </summary>
        /// <param name="analysis_out">
        ///  analysis enumeration that the result is written into
        /// </param>
        /// <param name="preference_out">
        ///  One of the `git_merge_preference_t` flag.
        /// </param>
        /// <param name="repo">the repository to merge</param>
        /// <param name="our_ref">
        ///  the reference to perform the analysis from
        /// </param>
        /// <param name="their_heads">the heads to merge into</param>
        /// <param name="their_heads_len">
        ///  the number of heads to merge
        /// </param>
        /// <returns>0 on success or error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_merge_analysis_for_ref(
            git_merge_analysis_t* analysis_out,
            git_merge_preference_t* preference_out,
            RepositoryHandle repo,
            ReferenceHandle our_ref,
            AnnotatedCommitHandle their_heads,
            UIntPtr their_heads_len);

        /// <summary>
        ///  Find a merge base between two commits
        /// </summary>
        /// <param name="_out">
        ///  the OID of a merge base between 'one' and 'two'
        /// </param>
        /// <param name="repo">
        ///  the repository where the commits exist
        /// </param>
        /// <param name="one">one of the commits</param>
        /// <param name="two">the other commit</param>
        /// <returns>
        ///  0 on success, GIT_ENOTFOUND if not found or error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_merge_base(
            ref git_oid _out,
            RepositoryHandle repo,
            ref git_oid one,
            ref git_oid two);

        /// <summary>
        ///  Find merge bases between two commits
        /// </summary>
        /// <param name="_out">
        ///  array in which to store the resulting ids
        /// </param>
        /// <param name="repo">
        ///  the repository where the commits exist
        /// </param>
        /// <param name="one">one of the commits</param>
        /// <param name="two">the other commit</param>
        /// <returns>
        ///  0 on success, GIT_ENOTFOUND if not found or error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_merge_bases(
            ref git_oidarray _out,
            RepositoryHandle repo,
            ref git_oid one,
            ref git_oid two);

        /// <summary>
        ///  Find a merge base given a list of commits
        /// </summary>
        /// <param name="_out">
        ///  the OID of a merge base considering all the commits
        /// </param>
        /// <param name="repo">
        ///  the repository where the commits exist
        /// </param>
        /// <param name="length">
        ///  The number of commits in the provided `input_array`
        /// </param>
        /// <param name="input_array">oids of the commits</param>
        /// <returns>
        ///  Zero on success; GIT_ENOTFOUND or -1 on failure.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_merge_base_many(
            ref git_oid _out,
            RepositoryHandle repo,
            UIntPtr length,
            ref git_oid input_array);

        /// <summary>
        ///  Find all merge bases given a list of commits
        ///
        ///  <para>
        ///   This behaves similar to [`git
        ///   merge-base`](https://git-scm.com/docs/git-merge-base#_discussion).
        ///  </para>
        ///
        ///  <para>
        ///   Given three commits `a`, `b`, and `c`, `merge_base_many`
        ///   will compute a hypothetical commit `m`, which is a merge
        ///   between `b` and `c`.
        ///  </para>
        ///
        ///  <para>
        ///   For example, with the following topology: ```text
        ///   o---o---o---o---C / / o---o---o---B / /
        ///   ---2---1---o---o---o---A ```
        ///  </para>
        ///
        ///  <para>
        ///   the result of `merge_base_many` given `a`, `b`, and `c`
        ///   is 1. This is because the equivalent topology with the
        ///   imaginary merge commit `m` between `b` and `c` is:
        ///   ```text o---o---o---o---o / \ / o---o---o---o---M / /
        ///   ---2---1---o---o---o---A ```
        ///  </para>
        ///
        ///  <para>
        ///   and the result of `merge_base_many` given `a` and `m` is
        ///   1.
        ///  </para>
        ///
        ///  <para>
        ///   If you're looking to recieve the common ancestor between
        ///   all the given commits, use `merge_base_octopus`.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  array in which to store the resulting ids
        /// </param>
        /// <param name="repo">
        ///  the repository where the commits exist
        /// </param>
        /// <param name="length">
        ///  The number of commits in the provided `input_array`
        /// </param>
        /// <param name="input_array">oids of the commits</param>
        /// <returns>
        ///  Zero on success; GIT_ENOTFOUND or -1 on failure.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_merge_bases_many(
            ref git_oidarray _out,
            RepositoryHandle repo,
            UIntPtr length,
            ref git_oid input_array);

        /// <summary>
        ///  Find a merge base in preparation for an octopus merge
        /// </summary>
        /// <param name="_out">
        ///  the OID of a merge base considering all the commits
        /// </param>
        /// <param name="repo">
        ///  the repository where the commits exist
        /// </param>
        /// <param name="length">
        ///  The number of commits in the provided `input_array`
        /// </param>
        /// <param name="input_array">oids of the commits</param>
        /// <returns>
        ///  Zero on success; GIT_ENOTFOUND or -1 on failure.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_merge_base_octopus(
            ref git_oid _out,
            RepositoryHandle repo,
            UIntPtr length,
            ref git_oid input_array);

        /// <summary>
        ///  Merge two files as they exist in the index, using the given
        ///  common ancestor as the baseline, producing a
        ///  `git_merge_file_result` that reflects the merge result. The
        ///  `git_merge_file_result` must be freed with
        ///  `git_merge_file_result_free`.
        ///
        ///  <para>
        ///   At least one of `ancestor`, `ours`, or `theirs` must be
        ///   non-null.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  The git_merge_file_result to be filled in
        /// </param>
        /// <param name="repo">The repository</param>
        /// <param name="ancestor">
        ///  The index entry for the ancestor file (stage level 1)
        /// </param>
        /// <param name="ours">
        ///  The index entry for our file (stage level 2)
        /// </param>
        /// <param name="theirs">
        ///  The index entry for their file (stage level 3)
        /// </param>
        /// <param name="opts">The merge file options or NULL</param>
        /// <returns>0 on success or error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_merge_file_from_index(
            ref git_merge_file_result _out,
            RepositoryHandle repo,
            ref git_index_entry ancestor,
            ref git_index_entry ours,
            ref git_index_entry theirs,
            ref git_merge_file_options opts);

        /// <summary>
        ///  Frees a `git_merge_file_result`.
        /// </summary>
        /// <param name="result">The result to free or `NULL`</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_merge_file_result_free(
            IntPtr result);

        /// <summary>
        ///  Merge two trees, producing a `git_index` that reflects the
        ///  result of the merge. The index may be written as-is to the
        ///  working directory or checked out. If the index is to be
        ///  converted to a tree, the caller should resolve any conflicts
        ///  that arose as part of the merge.
        ///
        ///  <para>
        ///   The returned index must be freed explicitly with
        ///   `git_index_free`.
        ///  </para>
        /// </summary>
        /// <param name="_out">pointer to store the index result in</param>
        /// <param name="repo">
        ///  repository that contains the given trees
        /// </param>
        /// <param name="ancestor_tree">
        ///  the common ancestor between the trees (or null if none)
        /// </param>
        /// <param name="our_tree">
        ///  the tree that reflects the destination tree
        /// </param>
        /// <param name="their_tree">
        ///  the tree to merge in to `our_tree`
        /// </param>
        /// <param name="opts">
        ///  the merge tree options (or null for defaults)
        /// </param>
        /// <returns>0 on success or error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_merge_trees(
            out IndexHandle _out,
            RepositoryHandle repo,
            TreeHandle ancestor_tree,
            TreeHandle our_tree,
            TreeHandle their_tree,
            ref git_merge_options opts);

        /// <summary>
        ///  Merge two commits, producing a `git_index` that reflects the
        ///  result of the merge. The index may be written as-is to the
        ///  working directory or checked out. If the index is to be
        ///  converted to a tree, the caller should resolve any conflicts
        ///  that arose as part of the merge.
        ///
        ///  <para>
        ///   The returned index must be freed explicitly with
        ///   `git_index_free`.
        ///  </para>
        /// </summary>
        /// <param name="_out">pointer to store the index result in</param>
        /// <param name="repo">
        ///  repository that contains the given trees
        /// </param>
        /// <param name="our_commit">
        ///  the commit that reflects the destination tree
        /// </param>
        /// <param name="their_commit">
        ///  the commit to merge in to `our_commit`
        /// </param>
        /// <param name="opts">
        ///  the merge tree options (or null for defaults)
        /// </param>
        /// <returns>0 on success or error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_merge_commits(
            out IndexHandle _out,
            RepositoryHandle repo,
            CommitHandle our_commit,
            CommitHandle their_commit,
            ref git_merge_options opts);

        /// <summary>
        ///  Merges the given commit(s) into HEAD, writing the results
        ///  into the working directory. Any changes are staged for
        ///  commit and any conflicts are written to the index. Callers
        ///  should inspect the repository's index after this completes,
        ///  resolve any conflicts and prepare a commit.
        ///
        ///  <para>
        ///   For compatibility with git, the repository is put into a
        ///   merging state. Once the commit is done (or if the user
        ///   wishes to abort), you should clear this state by calling
        ///   `git_repository_state_cleanup()`.
        ///  </para>
        /// </summary>
        /// <param name="repo">the repository to merge</param>
        /// <param name="their_heads">the heads to merge into</param>
        /// <param name="their_heads_len">
        ///  the number of heads to merge
        /// </param>
        /// <param name="merge_opts">merge options</param>
        /// <param name="checkout_opts">checkout options</param>
        /// <returns>0 on success or error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_merge(
            RepositoryHandle repo,
            AnnotatedCommitHandle their_heads,
            UIntPtr their_heads_len,
            ref git_merge_options merge_opts,
            ref git_checkout_options checkout_opts);
    }
}
