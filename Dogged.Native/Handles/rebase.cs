using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public sealed class RebaseHandle : SafeHandle
    {
        public RebaseHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe RebaseHandle(git_rebase* nativeRebase) :
            base((IntPtr)nativeRebase, true)
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
            libgit2.git_rebase_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Initializes a rebase operation to rebase the changes in
        ///  `branch` relative to `upstream` onto another branch. To
        ///  begin the rebase process, call `git_rebase_next`. When you
        ///  have finished with this object, call `git_rebase_free`.
        /// </summary>
        /// <param name="_out">Pointer to store the rebase object</param>
        /// <param name="repo">The repository to perform the rebase</param>
        /// <param name="branch">
        ///  The terminal commit to rebase, or NULL to rebase the current
        ///  branch
        /// </param>
        /// <param name="upstream">
        ///  The commit to begin rebasing from, or NULL to rebase all
        ///  reachable commits
        /// </param>
        /// <param name="onto">
        ///  The branch to rebase onto, or NULL to rebase onto the given
        ///  upstream
        /// </param>
        /// <param name="opts">
        ///  Options to specify how rebase is performed, or NULL
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_rebase_init(
            out RebaseHandle _out,
            RepositoryHandle repo,
            AnnotatedCommitHandle branch,
            AnnotatedCommitHandle upstream,
            AnnotatedCommitHandle onto,
            ref git_rebase_options opts);

        /// <summary>
        ///  Opens an existing rebase that was previously started by
        ///  either an invocation of `git_rebase_init` or by another
        ///  client.
        /// </summary>
        /// <param name="_out">Pointer to store the rebase object</param>
        /// <param name="repo">
        ///  The repository that has a rebase in-progress
        /// </param>
        /// <param name="opts">
        ///  Options to specify how rebase is performed
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_rebase_open(
            out RebaseHandle _out,
            RepositoryHandle repo,
            ref git_rebase_options opts);

        /// <summary>
        ///  Gets the original `HEAD` ref name for merge rebases.
        /// </summary>
        /// <param name="rebase">The in-progress rebase.</param>
        /// <returns>The original `HEAD` ref name</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_rebase_orig_head_name(
            RebaseHandle rebase);

        /// <summary>
        ///  Gets the original `HEAD` id for merge rebases.
        /// </summary>
        /// <param name="rebase">The in-progress rebase.</param>
        /// <returns>The original `HEAD` id</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_rebase_orig_head_id(
            RebaseHandle rebase);

        /// <summary>
        ///  Gets the `onto` ref name for merge rebases.
        /// </summary>
        /// <param name="rebase">The in-progress rebase.</param>
        /// <returns>The `onto` ref name</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_rebase_onto_name(
            RebaseHandle rebase);

        /// <summary>
        ///  Gets the `onto` id for merge rebases.
        /// </summary>
        /// <param name="rebase">The in-progress rebase.</param>
        /// <returns>The `onto` id</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_rebase_onto_id(
            RebaseHandle rebase);

        /// <summary>
        ///  Gets the count of rebase operations that are to be applied.
        /// </summary>
        /// <param name="rebase">The in-progress rebase</param>
        /// <returns>The number of rebase operations in total</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_rebase_operation_entrycount(
            RebaseHandle rebase);

        /// <summary>
        ///  Gets the index of the rebase operation that is currently
        ///  being applied. If the first operation has not yet been
        ///  applied (because you have called `init` but not yet `next`)
        ///  then this returns `GIT_REBASE_NO_OPERATION`.
        /// </summary>
        /// <param name="rebase">The in-progress rebase</param>
        /// <returns>
        ///  The index of the rebase operation currently being applied.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_rebase_operation_current(
            RebaseHandle rebase);

        /// <summary>
        ///  Gets the rebase operation specified by the given index.
        /// </summary>
        /// <param name="rebase">The in-progress rebase</param>
        /// <param name="idx">
        ///  The index of the rebase operation to retrieve
        /// </param>
        /// <returns>
        ///  The rebase operation or NULL if `idx` was out of bounds
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_rebase_operation* git_rebase_operation_byindex(
            RebaseHandle rebase,
            UIntPtr idx);

        /// <summary>
        ///  Performs the next rebase operation and returns the
        ///  information about it. If the operation is one that applies a
        ///  patch (which is any operation except
        ///  GIT_REBASE_OPERATION_EXEC) then the patch will be applied
        ///  and the index and working directory will be updated with the
        ///  changes. If there are conflicts, you will need to address
        ///  those before committing the changes.
        /// </summary>
        /// <param name="operation">
        ///  Pointer to store the rebase operation that is to be
        ///  performed next
        /// </param>
        /// <param name="rebase">The rebase in progress</param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_rebase_next(
            IntPtr operation,
            RebaseHandle rebase);

        /// <summary>
        ///  Gets the index produced by the last operation, which is the
        ///  result of `git_rebase_next` and which will be committed by
        ///  the next invocation of `git_rebase_commit`. This is useful
        ///  for resolving conflicts in an in-memory rebase before
        ///  committing them. You must call `git_index_free` when you are
        ///  finished with this.
        ///
        ///  <para>
        ///   This is only applicable for in-memory rebases; for
        ///   rebases within a working directory, the changes were
        ///   applied to the repository's index.
        ///  </para>
        /// </summary>
        /// <param name="index">
        ///  The result index of the last operation.
        /// </param>
        /// <param name="rebase">The in-progress rebase.</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_rebase_inmemory_index(
            IndexHandle index,
            RebaseHandle rebase);

        /// <summary>
        ///  Commits the current patch. You must have resolved any
        ///  conflicts that were introduced during the patch application
        ///  from the `git_rebase_next` invocation.
        /// </summary>
        /// <param name="id">
        ///  Pointer in which to store the OID of the newly created
        ///  commit
        /// </param>
        /// <param name="rebase">The rebase that is in-progress</param>
        /// <param name="author">
        ///  The author of the updated commit, or NULL to keep the author
        ///  from the original commit
        /// </param>
        /// <param name="committer">The committer of the rebase</param>
        /// <param name="message_encoding">
        ///  The encoding for the message in the commit, represented with
        ///  a standard encoding name. If message is NULL, this should
        ///  also be NULL, and the encoding from the original commit will
        ///  be maintained. If message is specified, this may be NULL to
        ///  indicate that "UTF-8" is to be used.
        /// </param>
        /// <param name="message">
        ///  The message for this commit, or NULL to use the message from
        ///  the original commit.
        /// </param>
        /// <returns>
        ///  Zero on success, GIT_EUNMERGED if there are unmerged changes
        ///  in the index, GIT_EAPPLIED if the current commit has already
        ///  been applied to the upstream and there is nothing to commit,
        ///  -1 on failure.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_rebase_commit(
            ref git_oid id,
            RebaseHandle rebase,
            ref git_signature author,
            ref git_signature committer,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string message_encoding,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string message);

        /// <summary>
        ///  Aborts a rebase that is currently in progress, resetting the
        ///  repository and working directory to their state before
        ///  rebase began.
        /// </summary>
        /// <param name="rebase">The rebase that is in-progress</param>
        /// <returns>
        ///  Zero on success; GIT_ENOTFOUND if a rebase is not in
        ///  progress, -1 on other errors.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_rebase_abort(
            RebaseHandle rebase);

        /// <summary>
        ///  Finishes a rebase that is currently in progress once all
        ///  patches have been applied.
        /// </summary>
        /// <param name="rebase">The rebase that is in-progress</param>
        /// <param name="signature">
        ///  The identity that is finishing the rebase (optional)
        /// </param>
        /// <returns>Zero on success; -1 on error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_rebase_finish(
            RebaseHandle rebase,
            ref git_signature signature);

        /// <summary>
        ///  Frees the `git_rebase` object.
        /// </summary>
        /// <param name="rebase">The rebase object</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_rebase_free(
            IntPtr rebase);
    }
}
