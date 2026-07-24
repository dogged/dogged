using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
        /// <summary>
        ///  Sets the current head to the specified commit oid and
        ///  optionally resets the index and working tree to match.
        ///
        ///  <para>
        ///   SOFT reset means the Head will be moved to the commit.
        ///  </para>
        ///
        ///  <para>
        ///   MIXED reset will trigger a SOFT reset, plus the index
        ///   will be replaced with the content of the commit tree.
        ///  </para>
        ///
        ///  <para>
        ///   HARD reset will trigger a MIXED reset and the working
        ///   directory will be replaced with the content of the
        ///   index. (Untracked and ignored files will be left alone,
        ///   however.)
        ///  </para>
        ///
        ///  <para>
        ///   TODO: Implement remaining kinds of resets.
        ///  </para>
        /// </summary>
        /// <param name="repo">
        ///  Repository where to perform the reset operation.
        /// </param>
        /// <param name="target">
        ///  Committish to which the Head should be moved to. This object
        ///  must belong to the given `repo` and can either be a
        ///  git_commit or a git_tag. When a git_tag is being passed, it
        ///  should be dereferenceable to a git_commit which oid will be
        ///  used as the target of the branch.
        /// </param>
        /// <param name="reset_type">
        ///  Kind of reset operation to perform.
        /// </param>
        /// <param name="checkout_opts">
        ///  Optional checkout options to be used for a HARD reset. The
        ///  checkout_strategy field will be overridden (based on
        ///  reset_type). This parameter can be used to propagate notify
        ///  and progress callbacks.
        /// </param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reset(
            RepositoryHandle repo,
            ObjectHandle target,
            git_reset_t reset_type,
            ref git_checkout_options checkout_opts);

        /// <summary>
        ///  Sets the current head to the specified commit oid and
        ///  optionally resets the index and working tree to match.
        ///
        ///  <para>
        ///   This behaves like `git_reset()` but takes an annotated
        ///   commit, which lets you specify which extended sha syntax
        ///   string was specified by a user, allowing for more exact
        ///   reflog messages.
        ///  </para>
        ///
        ///  <para>
        ///   See the documentation for `git_reset()`.
        ///  </para>
        /// </summary>
        /// <param name="repo">
        ///  Repository where to perform the reset operation.
        /// </param>
        /// <param name="target">
        ///  Annotated commit to which the Head should be moved to. This
        ///  object must belong to the given `repo`, it will be
        ///  dereferenced to a git_commit which oid will be used as the
        ///  target of the branch.
        /// </param>
        /// <param name="reset_type">
        ///  Kind of reset operation to perform.
        /// </param>
        /// <param name="checkout_opts">
        ///  Optional checkout options to be used for a HARD reset. The
        ///  checkout_strategy field will be overridden (based on
        ///  reset_type). This parameter can be used to propagate notify
        ///  and progress callbacks.
        /// </param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reset_from_annotated(
            RepositoryHandle repo,
            AnnotatedCommitHandle target,
            git_reset_t reset_type,
            ref git_checkout_options checkout_opts);

        /// <summary>
        ///  Updates some entries in the index from the target commit
        ///  tree.
        ///
        ///  <para>
        ///   The scope of the updated entries is determined by the
        ///   paths being passed in the `pathspec` parameters.
        ///  </para>
        ///
        ///  <para>
        ///   Passing a NULL `target` will result in removing entries
        ///   in the index matching the provided pathspecs.
        ///  </para>
        /// </summary>
        /// <param name="repo">
        ///  Repository where to perform the reset operation.
        /// </param>
        /// <param name="target">
        ///  The committish which content will be used to reset the
        ///  content of the index.
        /// </param>
        /// <param name="pathspecs">List of pathspecs to operate on.</param>
        /// <returns>0 on success or an error code < 0</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reset_default(
            RepositoryHandle repo,
            ObjectHandle target,
            ref git_strarray pathspecs);
    }
}
