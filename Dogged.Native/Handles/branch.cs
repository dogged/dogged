using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public sealed class BranchIteratorHandle : SafeHandle
    {
        public BranchIteratorHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe BranchIteratorHandle(git_branch_iterator* nativeBranchIterator) :
            base((IntPtr)nativeBranchIterator, true)
        {
        }

        public override bool IsInvalid
        {
            get
            {
                return handle == IntPtr.Zero;
            }
        }

        protected override unsafe bool ReleaseHandle()
        {
            libgit2.git_branch_iterator_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Create a new branch pointing at a target commit
        ///
        ///  <para>
        ///   A new direct reference will be created pointing to this
        ///   target commit. If `force` is true and a reference
        ///   already exists with the given name, it'll be replaced.
        ///  </para>
        ///
        ///  <para>
        ///   The returned reference must be freed by the user.
        ///  </para>
        ///
        ///  <para>
        ///   The branch name will be checked for validity. See
        ///   `git_tag_create()` for rules about valid names.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Pointer where to store the underlying reference.
        /// </param>
        /// <param name="repo">
        ///  the repository to create the branch in.
        /// </param>
        /// <param name="branch_name">
        ///  Name for the branch; this name is validated for consistency.
        ///  It should also not conflict with an already existing branch
        ///  name.
        /// </param>
        /// <param name="target">
        ///  Commit to which this branch should point. This object must
        ///  belong to the given `repo`.
        /// </param>
        /// <param name="force">Overwrite existing branch.</param>
        /// <returns>
        ///  0, GIT_EINVALIDSPEC or an error code. A proper reference is
        ///  written in the refs/heads namespace pointing to the provided
        ///  target commit.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_branch_create(
            out ReferenceHandle _out,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string branch_name,
            CommitHandle target,
            int force);

        /// <summary>
        ///  Create a new branch pointing at a target commit
        ///
        ///  <para>
        ///   This behaves like `git_branch_create()` but takes an
        ///   annotated commit, which lets you specify which extended
        ///   sha syntax string was specified by a user, allowing for
        ///   more exact reflog messages.
        ///  </para>
        /// </summary>
        /// <param name="ref_out">
        ///  Pointer where to store the underlying reference.
        /// </param>
        /// <param name="repo">
        ///  the repository to create the branch in.
        /// </param>
        /// <param name="branch_name">
        ///  Name for the branch; this name is validated for consistency.
        ///  It should also not conflict with an already existing branch
        ///  name.
        /// </param>
        /// <param name="target">
        ///  Annotated commit to which this branch should point. This
        ///  object must belong to the given `repo`.
        /// </param>
        /// <param name="force">Overwrite existing branch.</param>
        /// <returns>0, GIT_EINVALIDSPEC or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_branch_create_from_annotated(
            ReferenceHandle ref_out,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string branch_name,
            AnnotatedCommitHandle target,
            int force);

        /// <summary>
        ///  Delete an existing branch reference.
        ///
        ///  <para>
        ///   Note that if the deletion succeeds, the reference object
        ///   will not be valid anymore, and should be freed
        ///   immediately by the user using `git_reference_free()`.
        ///  </para>
        /// </summary>
        /// <param name="branch">
        ///  A valid reference representing a branch
        /// </param>
        /// <returns>0 on success, or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_branch_delete(
            ReferenceHandle branch);

        /// <summary>
        ///  Create an iterator which loops over the requested branches.
        /// </summary>
        /// <param name="_out">the iterator</param>
        /// <param name="repo">
        ///  Repository where to find the branches.
        /// </param>
        /// <param name="list_flags">
        ///  Filtering flags for the branch listing. Valid values are
        ///  GIT_BRANCH_LOCAL, GIT_BRANCH_REMOTE or GIT_BRANCH_ALL.
        /// </param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_branch_iterator_new(
            out BranchIteratorHandle _out,
            RepositoryHandle repo,
            git_branch_t list_flags);

        /// <summary>
        ///  Retrieve the next branch from the iterator
        /// </summary>
        /// <param name="_out">the reference</param>
        /// <param name="out_type">
        ///  the type of branch (local or remote-tracking)
        /// </param>
        /// <param name="iter">the branch iterator</param>
        /// <returns>
        ///  0 on success, GIT_ITEROVER if there are no more branches or
        ///  an error code.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_branch_next(
            out ReferenceHandle _out,
            git_branch_t* out_type,
            BranchIteratorHandle iter);

        /// <summary>
        ///  Free a branch iterator
        /// </summary>
        /// <param name="iter">the iterator to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_branch_iterator_free(
            IntPtr iter);

        /// <summary>
        ///  Move/rename an existing local branch reference.
        ///
        ///  <para>
        ///   The new branch name will be checked for validity. See
        ///   `git_tag_create()` for rules about valid names.
        ///  </para>
        ///
        ///  <para>
        ///   Note that if the move succeeds, the old reference object
        ///   will not be valid anymore, and should be freed
        ///   immediately by the user using `git_reference_free()`.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  New reference object for the updated name.
        /// </param>
        /// <param name="branch">
        ///  Current underlying reference of the branch.
        /// </param>
        /// <param name="new_branch_name">
        ///  Target name of the branch once the move is performed; this
        ///  name is validated for consistency.
        /// </param>
        /// <param name="force">Overwrite existing branch.</param>
        /// <returns>
        ///  0 on success, GIT_EINVALIDSPEC or an error code.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_branch_move(
            out ReferenceHandle _out,
            ReferenceHandle branch,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string new_branch_name,
            int force);

        /// <summary>
        ///  Lookup a branch by its name in a repository.
        ///
        ///  <para>
        ///   The generated reference must be freed by the user. The
        ///   branch name will be checked for validity.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  pointer to the looked-up branch reference
        /// </param>
        /// <param name="repo">the repository to look up the branch</param>
        /// <param name="branch_name">
        ///  Name of the branch to be looked-up; this name is validated
        ///  for consistency.
        /// </param>
        /// <param name="branch_type">
        ///  Type of the considered branch. This should be valued with
        ///  either GIT_BRANCH_LOCAL or GIT_BRANCH_REMOTE.
        /// </param>
        /// <returns>
        ///  0 on success; GIT_ENOTFOUND when no matching branch exists,
        ///  GIT_EINVALIDSPEC, otherwise an error code.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_branch_lookup(
            out ReferenceHandle _out,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string branch_name,
            git_branch_t branch_type);

        /// <summary>
        ///  Get the branch name
        ///
        ///  <para>
        ///   Given a reference object, this will check that it really
        ///   is a branch (ie. it lives under "refs/heads/" or
        ///   "refs/remotes/"), and return the branch part of it.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Pointer to the abbreviated reference name. Owned by ref, do
        ///  not free.
        /// </param>
        /// <param name="_ref">
        ///  A reference object, ideally pointing to a branch
        /// </param>
        /// <returns>
        ///  0 on success; GIT_EINVALID if the reference isn't either a
        ///  local or remote branch, otherwise an error code.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_branch_name(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))] out string _out,
            ReferenceHandle _ref);

        /// <summary>
        ///  Get the upstream of a branch
        ///
        ///  <para>
        ///   Given a reference, this will return a new reference
        ///   object corresponding to its remote tracking branch. The
        ///   reference must be a local branch.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Pointer where to store the retrieved reference.
        /// </param>
        /// <param name="branch">
        ///  Current underlying reference of the branch.
        /// </param>
        /// <returns>
        ///  0 on success; GIT_ENOTFOUND when no remote tracking
        ///  reference exists, otherwise an error code.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_branch_upstream(
            out ReferenceHandle _out,
            ReferenceHandle branch);

        /// <summary>
        ///  Set a branch's upstream branch
        ///
        ///  <para>
        ///   This will update the configuration to set the branch
        ///   named `branch_name` as the upstream of `branch`. Pass a
        ///   NULL name to unset the upstream information.
        ///  </para>
        /// </summary>
        /// <param name="branch">the branch to configure</param>
        /// <param name="branch_name">
        ///  remote-tracking or local branch to set as upstream.
        /// </param>
        /// <returns>
        ///  0 on success; GIT_ENOTFOUND if there's no branch named
        ///  `branch_name` or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_error_t git_branch_set_upstream(
            ReferenceHandle branch,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string branch_name);

        /// <summary>
        ///  Get the upstream name of a branch
        ///
        ///  <para>
        ///   Given a local branch, this will return its
        ///   remote-tracking branch information, as a full reference
        ///   name, ie. "feature/nice" would become
        ///   "refs/remote/origin/feature/nice", depending on that
        ///   branch's configuration.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  the buffer into which the name will be written.
        /// </param>
        /// <param name="repo">
        ///  the repository where the branches live.
        /// </param>
        /// <param name="refname">
        ///  reference name of the local branch.
        /// </param>
        /// <returns>
        ///  0 on success, GIT_ENOTFOUND when no remote tracking
        ///  reference exists, or an error code.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_branch_upstream_name(
            ref git_buf _out,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string refname);

        /// <summary>
        ///  Determine if HEAD points to the given branch
        /// </summary>
        /// <param name="branch">A reference to a local branch.</param>
        /// <returns>
        ///  1 if HEAD points at the branch, 0 if it isn't, or a negative
        ///  value as an error code.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_branch_is_head(
            ReferenceHandle branch);

        /// <summary>
        ///  Determine if any HEAD points to the current branch
        ///
        ///  <para>
        ///   This will iterate over all known linked repositories
        ///   (usually in the form of worktrees) and report whether
        ///   any HEAD is pointing at the current branch.
        ///  </para>
        /// </summary>
        /// <param name="branch">A reference to a local branch.</param>
        /// <returns>
        ///  1 if branch is checked out, 0 if it isn't, an error code
        ///  otherwise.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_branch_is_checked_out(
            ReferenceHandle branch);

        /// <summary>
        ///  Find the remote name of a remote-tracking branch
        ///
        ///  <para>
        ///   This will return the name of the remote whose fetch
        ///   refspec is matching the given branch. E.g. given a
        ///   branch "refs/remotes/test/master", it will extract the
        ///   "test" part. If refspecs from multiple remotes match,
        ///   the function will return GIT_EAMBIGUOUS.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  The buffer into which the name will be written.
        /// </param>
        /// <param name="repo">
        ///  The repository where the branch lives.
        /// </param>
        /// <param name="refname">
        ///  complete name of the remote tracking branch.
        /// </param>
        /// <returns>
        ///  0 on success, GIT_ENOTFOUND when no matching remote was
        ///  found, GIT_EAMBIGUOUS when the branch maps to several
        ///  remotes, otherwise an error code.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_branch_remote_name(
            ref git_buf _out,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string refname);

        /// <summary>
        ///  Retrieve the upstream remote of a local branch
        ///
        ///  <para>
        ///   This will return the currently configured
        ///   "branch.*.remote" for a given branch. This branch must
        ///   be local.
        ///  </para>
        /// </summary>
        /// <param name="buf">
        ///  the buffer into which to write the name
        /// </param>
        /// <param name="repo">the repository in which to look</param>
        /// <param name="refname">the full name of the branch</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_branch_upstream_remote(
            ref git_buf buf,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string refname);

        /// <summary>
        ///  Retrieve the upstream merge of a local branch
        ///
        ///  <para>
        ///   This will return the currently configured
        ///   "branch.*.merge" for a given branch. This branch must be
        ///   local.
        ///  </para>
        /// </summary>
        /// <param name="buf">
        ///  the buffer into which to write the name
        /// </param>
        /// <param name="repo">the repository in which to look</param>
        /// <param name="refname">the full name of the branch</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_branch_upstream_merge(
            ref git_buf buf,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string refname);
    }
}
