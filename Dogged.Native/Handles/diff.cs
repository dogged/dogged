using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public sealed class DiffHandle : SafeHandle
    {
        public DiffHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe DiffHandle(git_diff* nativeDiff) :
            base((IntPtr)nativeDiff, true)
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
            libgit2.git_diff_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public sealed class DiffStatsHandle : SafeHandle
    {
        public DiffStatsHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe DiffStatsHandle(git_diff_stats* nativeDiffStats) :
            base((IntPtr)nativeDiffStats, true)
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
            libgit2.git_diff_stats_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Deallocate a diff.
        /// </summary>
        /// <param name="diff">
        ///  The previously created diff; cannot be used after free.
        /// </param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_diff_free(
            IntPtr diff);

        /// <summary>
        ///  Create a diff with the difference between two tree objects.
        ///
        ///  <para>
        ///   This is equivalent to `git diff <old -tree> <new -tree>`
        ///  </para>
        ///
        ///  <para>
        ///   The first tree will be used for the "old_file" side of
        ///   the delta and the second tree will be used for the
        ///   "new_file" side of the delta. You can pass NULL to
        ///   indicate an empty tree, although it is an error to pass
        ///   NULL for both the `old_tree` and `new_tree`.
        ///  </para>
        /// </summary>
        /// <param name="diff">
        ///  Output pointer to a git_diff pointer to be allocated.
        /// </param>
        /// <param name="repo">The repository containing the trees.</param>
        /// <param name="old_tree">
        ///  A git_tree object to diff from, or NULL for empty tree.
        /// </param>
        /// <param name="new_tree">
        ///  A git_tree object to diff to, or NULL for empty tree.
        /// </param>
        /// <param name="opts">
        ///  Structure with options to influence diff or NULL for
        ///  defaults.
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_diff_tree_to_tree(
            DiffHandle diff,
            RepositoryHandle repo,
            TreeHandle old_tree,
            TreeHandle new_tree,
            ref git_diff_options opts);

        /// <summary>
        ///  Create a diff between a tree and repository index.
        ///
        ///  <para>
        ///   This is equivalent to `git diff --cached <treeish>` or
        ///   if you pass the HEAD tree, then like `git diff
        ///   --cached`.
        ///  </para>
        ///
        ///  <para>
        ///   The tree you pass will be used for the "old_file" side
        ///   of the delta, and the index will be used for the
        ///   "new_file" side of the delta.
        ///  </para>
        ///
        ///  <para>
        ///   If you pass NULL for the index, then the existing index
        ///   of the `repo` will be used. In this case, the index will
        ///   be refreshed from disk (if it has changed) before the
        ///   diff is generated.
        ///  </para>
        /// </summary>
        /// <param name="diff">
        ///  Output pointer to a git_diff pointer to be allocated.
        /// </param>
        /// <param name="repo">
        ///  The repository containing the tree and index.
        /// </param>
        /// <param name="old_tree">
        ///  A git_tree object to diff from, or NULL for empty tree.
        /// </param>
        /// <param name="index">
        ///  The index to diff with; repo index used if NULL.
        /// </param>
        /// <param name="opts">
        ///  Structure with options to influence diff or NULL for
        ///  defaults.
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_diff_tree_to_index(
            DiffHandle diff,
            RepositoryHandle repo,
            TreeHandle old_tree,
            IndexHandle index,
            ref git_diff_options opts);

        /// <summary>
        ///  Create a diff between the repository index and the workdir
        ///  directory.
        ///
        ///  <para>
        ///   This matches the `git diff` command. See the note below
        ///   on `git_diff_tree_to_workdir` for a discussion of the
        ///   difference between `git diff` and `git diff HEAD` and
        ///   how to emulate a `git diff <treeish>` using libgit2.
        ///  </para>
        ///
        ///  <para>
        ///   The index will be used for the "old_file" side of the
        ///   delta, and the working directory will be used for the
        ///   "new_file" side of the delta.
        ///  </para>
        ///
        ///  <para>
        ///   If you pass NULL for the index, then the existing index
        ///   of the `repo` will be used. In this case, the index will
        ///   be refreshed from disk (if it has changed) before the
        ///   diff is generated.
        ///  </para>
        /// </summary>
        /// <param name="diff">
        ///  Output pointer to a git_diff pointer to be allocated.
        /// </param>
        /// <param name="repo">The repository.</param>
        /// <param name="index">
        ///  The index to diff from; repo index used if NULL.
        /// </param>
        /// <param name="opts">
        ///  Structure with options to influence diff or NULL for
        ///  defaults.
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_diff_index_to_workdir(
            DiffHandle diff,
            RepositoryHandle repo,
            IndexHandle index,
            ref git_diff_options opts);

        /// <summary>
        ///  Create a diff between a tree and the working directory.
        ///
        ///  <para>
        ///   The tree you provide will be used for the "old_file"
        ///   side of the delta, and the working directory will be
        ///   used for the "new_file" side.
        ///  </para>
        ///
        ///  <para>
        ///   This is not the same as `git diff <treeish>` or `git
        ///   diff-index <treeish>`. Those commands use information
        ///   from the index, whereas this function strictly returns
        ///   the differences between the tree and the files in the
        ///   working directory, regardless of the state of the index.
        ///   Use `git_diff_tree_to_workdir_with_index` to emulate
        ///   those commands.
        ///  </para>
        ///
        ///  <para>
        ///   To see difference between this and
        ///   `git_diff_tree_to_workdir_with_index`, consider the
        ///   example of a staged file deletion where the file has
        ///   then been put back into the working dir and further
        ///   modified. The tree-to-workdir diff for that file is
        ///   'modified', but `git diff` would show status 'deleted'
        ///   since there is a staged delete.
        ///  </para>
        /// </summary>
        /// <param name="diff">
        ///  A pointer to a git_diff pointer that will be allocated.
        /// </param>
        /// <param name="repo">The repository containing the tree.</param>
        /// <param name="old_tree">
        ///  A git_tree object to diff from, or NULL for empty tree.
        /// </param>
        /// <param name="opts">
        ///  Structure with options to influence diff or NULL for
        ///  defaults.
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_diff_tree_to_workdir(
            DiffHandle diff,
            RepositoryHandle repo,
            TreeHandle old_tree,
            ref git_diff_options opts);

        /// <summary>
        ///  Create a diff between a tree and the working directory using
        ///  index data to account for staged deletes, tracked files,
        ///  etc.
        ///
        ///  <para>
        ///   This emulates `git diff <tree>` by diffing the tree to
        ///   the index and the index to the working directory and
        ///   blending the results into a single diff that includes
        ///   staged deleted, etc.
        ///  </para>
        /// </summary>
        /// <param name="diff">
        ///  A pointer to a git_diff pointer that will be allocated.
        /// </param>
        /// <param name="repo">The repository containing the tree.</param>
        /// <param name="old_tree">
        ///  A git_tree object to diff from, or NULL for empty tree.
        /// </param>
        /// <param name="opts">
        ///  Structure with options to influence diff or NULL for
        ///  defaults.
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_diff_tree_to_workdir_with_index(
            DiffHandle diff,
            RepositoryHandle repo,
            TreeHandle old_tree,
            ref git_diff_options opts);

        /// <summary>
        ///  Create a diff with the difference between two index objects.
        ///
        ///  <para>
        ///   The first index will be used for the "old_file" side of
        ///   the delta and the second index will be used for the
        ///   "new_file" side of the delta.
        ///  </para>
        /// </summary>
        /// <param name="diff">
        ///  Output pointer to a git_diff pointer to be allocated.
        /// </param>
        /// <param name="repo">
        ///  The repository containing the indexes.
        /// </param>
        /// <param name="old_index">A git_index object to diff from.</param>
        /// <param name="new_index">A git_index object to diff to.</param>
        /// <param name="opts">
        ///  Structure with options to influence diff or NULL for
        ///  defaults.
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_diff_index_to_index(
            DiffHandle diff,
            RepositoryHandle repo,
            IndexHandle old_index,
            IndexHandle new_index,
            ref git_diff_options opts);

        /// <summary>
        ///  Merge one diff into another.
        ///
        ///  <para>
        ///   This merges items from the "from" list into the "onto"
        ///   list. The resulting diff will have all items that appear
        ///   in either list. If an item appears in both lists, then
        ///   it will be "merged" to appear as if the old version was
        ///   from the "onto" list and the new version is from the
        ///   "from" list (with the exception that if the item has a
        ///   pending DELETE in the middle, then it will show as
        ///   deleted).
        ///  </para>
        /// </summary>
        /// <param name="onto">Diff to merge into.</param>
        /// <param name="from">Diff to merge.</param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_diff_merge(
            DiffHandle onto,
            DiffHandle from);

        /// <summary>
        ///  Transform a diff marking file renames, copies, etc.
        ///
        ///  <para>
        ///   This modifies a diff in place, replacing old entries
        ///   that look like renames or copies with new entries
        ///   reflecting those changes. This also will, if requested,
        ///   break modified files into add/remove pairs if the amount
        ///   of change is above a threshold.
        ///  </para>
        /// </summary>
        /// <param name="diff">diff to run detection algorithms on</param>
        /// <param name="options">
        ///  Control how detection should be run, NULL for defaults
        /// </param>
        /// <returns>0 on success, -1 on failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_diff_find_similar(
            DiffHandle diff,
            ref git_diff_find_options options);

        /// <summary>
        ///  Query how many diff records are there in a diff.
        /// </summary>
        /// <param name="diff">
        ///  A git_diff generated by one of the above functions
        /// </param>
        /// <returns>Count of number of deltas in the list</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_diff_num_deltas(
            DiffHandle diff);

        /// <summary>
        ///  Query how many diff deltas are there in a diff filtered by
        ///  type.
        ///
        ///  <para>
        ///   This works just like `git_diff_num_deltas()` with an
        ///   extra parameter that is a `git_delta_t` and returns just
        ///   the count of how many deltas match that particular type.
        ///  </para>
        /// </summary>
        /// <param name="diff">
        ///  A git_diff generated by one of the above functions
        /// </param>
        /// <param name="type">
        ///  A git_delta_t value to filter the count
        /// </param>
        /// <returns>
        ///  Count of number of deltas matching delta_t type
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_diff_num_deltas_of_type(
            DiffHandle diff,
            git_delta_t type);

        /// <summary>
        ///  Return the diff delta for an entry in the diff list.
        ///
        ///  <para>
        ///   The `git_diff_delta` pointer points to internal data and
        ///   you do not have to release it when you are done with it.
        ///   It will go away when the * `git_diff` (or any associated
        ///   `git_patch`) goes away.
        ///  </para>
        ///
        ///  <para>
        ///   Note that the flags on the delta related to whether it
        ///   has binary content or not may not be set if there are no
        ///   attributes set for the file and there has been no reason
        ///   to load the file data at this point. For now, if you
        ///   need those flags to be up to date, your only option is
        ///   to either use `git_diff_foreach` or create a
        ///   `git_patch`.
        ///  </para>
        /// </summary>
        /// <param name="diff">Diff list object</param>
        /// <param name="idx">Index into diff list</param>
        /// <returns>
        ///  Pointer to git_diff_delta (or NULL if `idx` out of range)
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_diff_delta* git_diff_get_delta(
            DiffHandle diff,
            UIntPtr idx);

        /// <summary>
        ///  Check if deltas are sorted case sensitively or
        ///  insensitively.
        /// </summary>
        /// <param name="diff">diff to check</param>
        /// <returns>0 if case sensitive, 1 if case is ignored</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_diff_is_sorted_icase(
            DiffHandle diff);

        /// <summary>
        ///  Loop over all deltas in a diff issuing callbacks.
        ///
        ///  <para>
        ///   This will iterate through all of the files described in
        ///   a diff. You should provide a file callback to learn
        ///   about each file.
        ///  </para>
        ///
        ///  <para>
        ///   The "hunk" and "line" callbacks are optional, and the
        ///   text diff of the files will only be calculated if they
        ///   are not NULL. Of course, these callbacks will not be
        ///   invoked for binary files on the diff or for files whose
        ///   only changed is a file mode change.
        ///  </para>
        ///
        ///  <para>
        ///   Returning a non-zero value from any of the callbacks
        ///   will terminate the iteration and return the value to the
        ///   user.
        ///  </para>
        /// </summary>
        /// <param name="diff">
        ///  A git_diff generated by one of the above functions.
        /// </param>
        /// <param name="file_cb">
        ///  Callback function to make per file in the diff.
        /// </param>
        /// <param name="binary_cb">
        ///  Optional callback to make for binary files.
        /// </param>
        /// <param name="hunk_cb">
        ///  Optional callback to make per hunk of text diff. This
        ///  callback is called to describe a range of lines in the diff.
        ///  It will not be issued for binary files.
        /// </param>
        /// <param name="line_cb">
        ///  Optional callback to make per line of diff text. This same
        ///  callback will be made for context lines, added, and removed
        ///  lines, and even for a deleted trailing newline.
        /// </param>
        /// <param name="payload">
        ///  Reference pointer that will be passed to your callbacks.
        /// </param>
        /// <returns>
        ///  0 on success, non-zero callback return value, or error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_diff_foreach(
            DiffHandle diff,
            git_diff_file_cb file_cb,
            git_diff_binary_cb binary_cb,
            git_diff_hunk_cb hunk_cb,
            git_diff_line_cb line_cb,
            IntPtr payload);

        /// <summary>
        ///  Iterate over a diff generating formatted text output.
        ///
        ///  <para>
        ///   Returning a non-zero value from the callbacks will
        ///   terminate the iteration and return the non-zero value to
        ///   the caller.
        ///  </para>
        /// </summary>
        /// <param name="diff">
        ///  A git_diff generated by one of the above functions.
        /// </param>
        /// <param name="format">
        ///  A git_diff_format_t value to pick the text format.
        /// </param>
        /// <param name="print_cb">
        ///  Callback to make per line of diff text.
        /// </param>
        /// <param name="payload">
        ///  Reference pointer that will be passed to your callback.
        /// </param>
        /// <returns>
        ///  0 on success, non-zero callback return value, or error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_diff_print(
            DiffHandle diff,
            git_diff_format_t format,
            git_diff_line_cb print_cb,
            IntPtr payload);

        /// <summary>
        ///  Produce the complete formatted text output from a diff into
        ///  a buffer.
        /// </summary>
        /// <param name="_out">
        ///  A pointer to a user-allocated git_buf that will contain the
        ///  diff text
        /// </param>
        /// <param name="diff">
        ///  A git_diff generated by one of the above functions.
        /// </param>
        /// <param name="format">
        ///  A git_diff_format_t value to pick the text format.
        /// </param>
        /// <returns>0 on success or error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_diff_to_buf(
            ref git_buf _out,
            DiffHandle diff,
            git_diff_format_t format);

        /// <summary>
        ///  Directly run a diff on two blobs.
        ///
        ///  <para>
        ///   Compared to a file, a blob lacks some contextual
        ///   information. As such, the `git_diff_file` given to the
        ///   callback will have some fake data; i.e. `mode` will be 0
        ///   and `path` will be NULL.
        ///  </para>
        ///
        ///  <para>
        ///   NULL is allowed for either `old_blob` or `new_blob` and
        ///   will be treated as an empty blob, with the `oid` set to
        ///   NULL in the `git_diff_file` data. Passing NULL for both
        ///   blobs is a noop; no callbacks will be made at all.
        ///  </para>
        ///
        ///  <para>
        ///   We do run a binary content check on the blob content and
        ///   if either blob looks like binary data, the
        ///   `git_diff_delta` binary attribute will be set to 1 and
        ///   no call to the hunk_cb nor line_cb will be made (unless
        ///   you pass `GIT_DIFF_FORCE_TEXT` of course).
        ///  </para>
        /// </summary>
        /// <param name="old_blob">
        ///  Blob for old side of diff, or NULL for empty blob
        /// </param>
        /// <param name="old_as_path">
        ///  Treat old blob as if it had this filename; can be NULL
        /// </param>
        /// <param name="new_blob">
        ///  Blob for new side of diff, or NULL for empty blob
        /// </param>
        /// <param name="new_as_path">
        ///  Treat new blob as if it had this filename; can be NULL
        /// </param>
        /// <param name="options">
        ///  Options for diff, or NULL for default options
        /// </param>
        /// <param name="file_cb">
        ///  Callback for "file"; made once if there is a diff; can be
        ///  NULL
        /// </param>
        /// <param name="binary_cb">
        ///  Callback for binary files; can be NULL
        /// </param>
        /// <param name="hunk_cb">
        ///  Callback for each hunk in diff; can be NULL
        /// </param>
        /// <param name="line_cb">
        ///  Callback for each line in diff; can be NULL
        /// </param>
        /// <param name="payload">
        ///  Payload passed to each callback function
        /// </param>
        /// <returns>
        ///  0 on success, non-zero callback return value, or error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_diff_blobs(
            BlobHandle old_blob,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string old_as_path,
            BlobHandle new_blob,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string new_as_path,
            ref git_diff_options options,
            git_diff_file_cb file_cb,
            git_diff_binary_cb binary_cb,
            git_diff_hunk_cb hunk_cb,
            git_diff_line_cb line_cb,
            IntPtr payload);

        /// <summary>
        ///  Directly run a diff between a blob and a buffer.
        ///
        ///  <para>
        ///   As with `git_diff_blobs`, comparing a blob and buffer
        ///   lacks some context, so the `git_diff_file` parameters to
        ///   the callbacks will be faked a la the rules for
        ///   `git_diff_blobs()`.
        ///  </para>
        ///
        ///  <para>
        ///   Passing NULL for `old_blob` will be treated as an empty
        ///   blob (i.e. the `file_cb` will be invoked with
        ///   GIT_DELTA_ADDED and the diff will be the entire content
        ///   of the buffer added). Passing NULL to the buffer will do
        ///   the reverse, with GIT_DELTA_REMOVED and blob content
        ///   removed.
        ///  </para>
        /// </summary>
        /// <param name="old_blob">
        ///  Blob for old side of diff, or NULL for empty blob
        /// </param>
        /// <param name="old_as_path">
        ///  Treat old blob as if it had this filename; can be NULL
        /// </param>
        /// <param name="buffer">
        ///  Raw data for new side of diff, or NULL for empty
        /// </param>
        /// <param name="buffer_len">
        ///  Length of raw data for new side of diff
        /// </param>
        /// <param name="buffer_as_path">
        ///  Treat buffer as if it had this filename; can be NULL
        /// </param>
        /// <param name="options">
        ///  Options for diff, or NULL for default options
        /// </param>
        /// <param name="file_cb">
        ///  Callback for "file"; made once if there is a diff; can be
        ///  NULL
        /// </param>
        /// <param name="binary_cb">
        ///  Callback for binary files; can be NULL
        /// </param>
        /// <param name="hunk_cb">
        ///  Callback for each hunk in diff; can be NULL
        /// </param>
        /// <param name="line_cb">
        ///  Callback for each line in diff; can be NULL
        /// </param>
        /// <param name="payload">
        ///  Payload passed to each callback function
        /// </param>
        /// <returns>
        ///  0 on success, non-zero callback return value, or error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_diff_blob_to_buffer(
            BlobHandle old_blob,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string old_as_path,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string buffer,
            UIntPtr buffer_len,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string buffer_as_path,
            ref git_diff_options options,
            git_diff_file_cb file_cb,
            git_diff_binary_cb binary_cb,
            git_diff_hunk_cb hunk_cb,
            git_diff_line_cb line_cb,
            IntPtr payload);

        /// <summary>
        ///  Read the contents of a git patch file into a `git_diff`
        ///  object.
        ///
        ///  <para>
        ///   The diff object produced is similar to the one that
        ///   would be produced if you actually produced it
        ///   computationally by comparing two trees, however there
        ///   may be subtle differences. For example, a patch file
        ///   likely contains abbreviated object IDs, so the object
        ///   IDs in a `git_diff_delta` produced by this function will
        ///   also be abbreviated.
        ///  </para>
        ///
        ///  <para>
        ///   This function will only read patch files created by a
        ///   git implementation, it will not read unified diffs
        ///   produced by the `diff` program, nor any other types of
        ///   patch files.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  A pointer to a git_diff pointer that will be allocated.
        /// </param>
        /// <param name="content">The contents of a patch file</param>
        /// <param name="content_len">
        ///  The length of the patch file contents
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_diff_from_buffer(
            out DiffHandle _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string content,
            UIntPtr content_len);

        /// <summary>
        ///  Read the contents of a git patch file into a `git_diff`
        ///  object.
        ///
        ///  <para>
        ///   The diff object produced is similar to the one that
        ///   would be produced if you actually produced it
        ///   computationally by comparing two trees, however there
        ///   may be subtle differences. For example, a patch file
        ///   likely contains abbreviated object IDs, so the object
        ///   IDs in a `git_diff_delta` produced by this function will
        ///   also be abbreviated.
        ///  </para>
        ///
        ///  <para>
        ///   This function will only read patch files created by a
        ///   git implementation, it will not read unified diffs
        ///   produced by the `diff` program, nor any other types of
        ///   patch files.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  A pointer to a git_diff pointer that will be allocated.
        /// </param>
        /// <param name="content">The contents of a patch file</param>
        /// <param name="content_len">
        ///  The length of the patch file contents
        /// </param>
        /// <param name="opts">Options controlling diff parsing</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_diff_from_buffer_ext(
            DiffHandle _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string content,
            UIntPtr content_len,
            ref git_diff_parse_options opts);

        /// <summary>
        ///  Accumulate diff statistics for all patches.
        /// </summary>
        /// <param name="_out">
        ///  Structure containing the diff statistics.
        /// </param>
        /// <param name="diff">
        ///  A git_diff generated by one of the above functions.
        /// </param>
        /// <returns>0 on success; non-zero on error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_diff_get_stats(
            out DiffStatsHandle _out,
            DiffHandle diff);

        /// <summary>
        ///  Get the total number of files changed in a diff
        /// </summary>
        /// <param name="stats">
        ///  A `git_diff_stats` generated by one of the above functions.
        /// </param>
        /// <returns>total number of files changed in the diff</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_diff_stats_files_changed(
            DiffStatsHandle stats);

        /// <summary>
        ///  Get the total number of insertions in a diff
        /// </summary>
        /// <param name="stats">
        ///  A `git_diff_stats` generated by one of the above functions.
        /// </param>
        /// <returns>total number of insertions in the diff</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_diff_stats_insertions(
            DiffStatsHandle stats);

        /// <summary>
        ///  Get the total number of deletions in a diff
        /// </summary>
        /// <param name="stats">
        ///  A `git_diff_stats` generated by one of the above functions.
        /// </param>
        /// <returns>total number of deletions in the diff</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_diff_stats_deletions(
            DiffStatsHandle stats);

        /// <summary>
        ///  Print diff statistics to a `git_buf`.
        /// </summary>
        /// <param name="_out">
        ///  buffer to store the formatted diff statistics in.
        /// </param>
        /// <param name="stats">
        ///  A `git_diff_stats` generated by one of the above functions.
        /// </param>
        /// <param name="format">Formatting option.</param>
        /// <param name="width">
        ///  Target width for output (only affects GIT_DIFF_STATS_FULL)
        /// </param>
        /// <returns>0 on success; non-zero on error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_diff_stats_to_buf(
            ref git_buf _out,
            DiffStatsHandle stats,
            git_diff_stats_format_t format,
            UIntPtr width);

        /// <summary>
        ///  Deallocate a `git_diff_stats`.
        /// </summary>
        /// <param name="stats">
        ///  The previously created statistics object; cannot be used
        ///  after free.
        /// </param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_diff_stats_free(
            IntPtr stats);

        /// <summary>
        ///  Calculate the patch ID for the given patch.
        ///
        ///  <para>
        ///   Calculate a stable patch ID for the given patch by
        ///   summing the hash of the file diffs, ignoring whitespace
        ///   and line numbers. This can be used to derive whether two
        ///   diffs are the same with a high probability.
        ///  </para>
        ///
        ///  <para>
        ///   Currently, this function only calculates stable patch
        ///   IDs, as defined in git-patch-id(1), and should in fact
        ///   generate the same IDs as the upstream git project does.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Pointer where the calculated patch ID should be stored
        /// </param>
        /// <param name="diff">The diff to calculate the ID for</param>
        /// <param name="opts">
        ///  Options for how to calculate the patch ID. This is intended
        ///  for future changes, as currently no options are available.
        /// </param>
        /// <returns>0 on success, an error code otherwise.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_diff_patchid(
            ref git_oid _out,
            DiffHandle diff,
            ref git_diff_patchid_options opts);
    }
}
