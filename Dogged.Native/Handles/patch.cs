using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public sealed class PatchHandle : SafeHandle
    {
        public PatchHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe PatchHandle(git_patch* nativePatch) :
            base((IntPtr)nativePatch, true)
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
            libgit2.git_patch_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Get the repository associated with this patch. May be NULL.
        /// </summary>
        /// <param name="patch">the patch</param>
        /// <returns>a pointer to the repository</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_repository* git_patch_owner(
            PatchHandle patch);

        /// <summary>
        ///  Return a patch for an entry in the diff list.
        ///
        ///  <para>
        ///   The `git_patch` is a newly created object contains the
        ///   text diffs for the delta. You have to call
        ///   `git_patch_free()` when you are done with it. You can
        ///   use the patch object to loop over all the hunks and
        ///   lines in the diff of the one delta.
        ///  </para>
        ///
        ///  <para>
        ///   For an unchanged file or a binary file, no `git_patch`
        ///   will be created, the output will be set to NULL, and the
        ///   `binary` flag will be set true in the `git_diff_delta`
        ///   structure.
        ///  </para>
        ///
        ///  <para>
        ///   It is okay to pass NULL for either of the output
        ///   parameters; if you pass NULL for the `git_patch`, then
        ///   the text diff will not be calculated.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Output parameter for the delta patch object
        /// </param>
        /// <param name="diff">Diff list object</param>
        /// <param name="idx">Index into diff list</param>
        /// <returns>0 on success, other value < 0 on error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_patch_from_diff(
            out PatchHandle _out,
            DiffHandle diff,
            UIntPtr idx);

        /// <summary>
        ///  Directly generate a patch from the difference between two
        ///  blobs.
        ///
        ///  <para>
        ///   This is just like `git_diff_blobs()` except it generates
        ///   a patch object for the difference instead of directly
        ///   making callbacks. You can use the standard `git_patch`
        ///   accessor functions to read the patch data, and you must
        ///   call `git_patch_free()` on the patch when done.
        ///  </para>
        /// </summary>
        /// <param name="_out">The generated patch; NULL on error</param>
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
        /// <param name="opts">
        ///  Options for diff, or NULL for default options
        /// </param>
        /// <returns>0 on success or error code < 0</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_patch_from_blobs(
            out PatchHandle _out,
            BlobHandle old_blob,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string old_as_path,
            BlobHandle new_blob,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string new_as_path,
            ref git_diff_options opts);

        /// <summary>
        ///  Directly generate a patch from the difference between a blob
        ///  and a buffer.
        ///
        ///  <para>
        ///   This is just like `git_diff_blob_to_buffer()` except it
        ///   generates a patch object for the difference instead of
        ///   directly making callbacks. You can use the standard
        ///   `git_patch` accessor functions to read the patch data,
        ///   and you must call `git_patch_free()` on the patch when
        ///   done.
        ///  </para>
        /// </summary>
        /// <param name="_out">The generated patch; NULL on error</param>
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
        /// <param name="opts">
        ///  Options for diff, or NULL for default options
        /// </param>
        /// <returns>0 on success or error code < 0</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_patch_from_blob_and_buffer(
            out PatchHandle _out,
            BlobHandle old_blob,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string old_as_path,
            IntPtr buffer,
            UIntPtr buffer_len,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string buffer_as_path,
            ref git_diff_options opts);

        /// <summary>
        ///  Directly generate a patch from the difference between two
        ///  buffers.
        ///
        ///  <para>
        ///   This is just like `git_diff_buffers()` except it
        ///   generates a patch object for the difference instead of
        ///   directly making callbacks. You can use the standard
        ///   `git_patch` accessor functions to read the patch data,
        ///   and you must call `git_patch_free()` on the patch when
        ///   done.
        ///  </para>
        /// </summary>
        /// <param name="_out">The generated patch; NULL on error</param>
        /// <param name="old_buffer">
        ///  Raw data for old side of diff, or NULL for empty
        /// </param>
        /// <param name="old_len">
        ///  Length of the raw data for old side of the diff
        /// </param>
        /// <param name="old_as_path">
        ///  Treat old buffer as if it had this filename; can be NULL
        /// </param>
        /// <param name="new_buffer">
        ///  Raw data for new side of diff, or NULL for empty
        /// </param>
        /// <param name="new_len">
        ///  Length of raw data for new side of diff
        /// </param>
        /// <param name="new_as_path">
        ///  Treat buffer as if it had this filename; can be NULL
        /// </param>
        /// <param name="opts">
        ///  Options for diff, or NULL for default options
        /// </param>
        /// <returns>0 on success or error code < 0</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_patch_from_buffers(
            out PatchHandle _out,
            IntPtr old_buffer,
            UIntPtr old_len,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string old_as_path,
            IntPtr new_buffer,
            UIntPtr new_len,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string new_as_path,
            ref git_diff_options opts);

        /// <summary>
        ///  Free a git_patch object.
        /// </summary>
        /// <param name="patch">The patch to free.</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_patch_free(
            IntPtr patch);

        /// <summary>
        ///  Get the delta associated with a patch. This delta points to
        ///  internal data and you do not have to release it when you are
        ///  done with it.
        /// </summary>
        /// <param name="patch">The patch in which to get the delta.</param>
        /// <returns>The delta associated with the patch.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_diff_delta* git_patch_get_delta(
            PatchHandle patch);

        /// <summary>
        ///  Get the number of hunks in a patch
        /// </summary>
        /// <param name="patch">
        ///  The patch in which to get the number of hunks.
        /// </param>
        /// <returns>The number of hunks of the patch.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_patch_num_hunks(
            PatchHandle patch);

        /// <summary>
        ///  Get line counts of each type in a patch.
        ///
        ///  <para>
        ///   This helps imitate a diff --numstat type of output. For
        ///   that purpose, you only need the `total_additions` and
        ///   `total_deletions` values, but we include the
        ///   `total_context` line count in case you want the total
        ///   number of lines of diff output that will be generated.
        ///  </para>
        ///
        ///  <para>
        ///   All outputs are optional. Pass NULL if you don't need a
        ///   particular count.
        ///  </para>
        /// </summary>
        /// <param name="total_context">
        ///  Count of context lines in output, can be NULL.
        /// </param>
        /// <param name="total_additions">
        ///  Count of addition lines in output, can be NULL.
        /// </param>
        /// <param name="total_deletions">
        ///  Count of deletion lines in output, can be NULL.
        /// </param>
        /// <param name="patch">The git_patch object</param>
        /// <returns>0 on success, < 0 on error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_patch_line_stats(
            out UIntPtr total_context,
            out UIntPtr total_additions,
            out UIntPtr total_deletions,
            PatchHandle patch);

        /// <summary>
        ///  Get the information about a hunk in a patch
        ///
        ///  <para>
        ///   Given a patch and a hunk index into the patch, this
        ///   returns detailed information about that hunk. Any of the
        ///   output pointers can be passed as NULL if you don't care
        ///   about that particular piece of information.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Output pointer to git_diff_hunk of hunk
        /// </param>
        /// <param name="lines_in_hunk">
        ///  Output count of total lines in this hunk
        /// </param>
        /// <param name="patch">Input pointer to patch object</param>
        /// <param name="hunk_idx">
        ///  Input index of hunk to get information about
        /// </param>
        /// <returns>
        ///  0 on success, GIT_ENOTFOUND if hunk_idx out of range, < 0 on
        ///  error
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_patch_get_hunk(
            out git_diff_hunk* _out,
            out UIntPtr lines_in_hunk,
            PatchHandle patch,
            UIntPtr hunk_idx);

        /// <summary>
        ///  Get the number of lines in a hunk.
        /// </summary>
        /// <param name="patch">The git_patch object</param>
        /// <param name="hunk_idx">Index of the hunk</param>
        /// <returns>
        ///  Number of lines in hunk or GIT_ENOTFOUND if invalid hunk
        ///  index
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_patch_num_lines_in_hunk(
            PatchHandle patch,
            UIntPtr hunk_idx);

        /// <summary>
        ///  Get data about a line in a hunk of a patch.
        ///
        ///  <para>
        ///   Given a patch, a hunk index, and a line index in the
        ///   hunk, this will return a lot of details about that line.
        ///   If you pass a hunk index larger than the number of hunks
        ///   or a line index larger than the number of lines in the
        ///   hunk, this will return -1.
        ///  </para>
        /// </summary>
        /// <param name="_out">The git_diff_line data for this line</param>
        /// <param name="patch">The patch to look in</param>
        /// <param name="hunk_idx">The index of the hunk</param>
        /// <param name="line_of_hunk">
        ///  The index of the line in the hunk
        /// </param>
        /// <returns>0 on success, < 0 on failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_patch_get_line_in_hunk(
            out git_diff_line* _out,
            PatchHandle patch,
            UIntPtr hunk_idx,
            UIntPtr line_of_hunk);

        /// <summary>
        ///  Look up size of patch diff data in bytes
        ///
        ///  <para>
        ///   This returns the raw size of the patch data. This only
        ///   includes the actual data from the lines of the diff, not
        ///   the file or hunk headers.
        ///  </para>
        ///
        ///  <para>
        ///   If you pass `include_context` as true (non-zero), this
        ///   will be the size of all of the diff output; if you pass
        ///   it as false (zero), this will only include the actual
        ///   changed lines (as if `context_lines` was 0).
        ///  </para>
        /// </summary>
        /// <param name="patch">
        ///  A git_patch representing changes to one file
        /// </param>
        /// <param name="include_context">
        ///  Include context lines in size if non-zero
        /// </param>
        /// <param name="include_hunk_headers">
        ///  Include hunk header lines if non-zero
        /// </param>
        /// <param name="include_file_headers">
        ///  Include file header lines if non-zero
        /// </param>
        /// <returns>The number of bytes of data</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_patch_size(
            PatchHandle patch,
            int include_context,
            int include_hunk_headers,
            int include_file_headers);

        /// <summary>
        ///  Serialize the patch to text via callback.
        ///
        ///  <para>
        ///   Returning a non-zero value from the callback will
        ///   terminate the iteration and return that value to the
        ///   caller.
        ///  </para>
        /// </summary>
        /// <param name="patch">
        ///  A git_patch representing changes to one file
        /// </param>
        /// <param name="print_cb">
        ///  Callback function to output lines of the patch. Will be
        ///  called for file headers, hunk headers, and diff lines.
        /// </param>
        /// <param name="payload">
        ///  Reference pointer that will be passed to your callbacks.
        /// </param>
        /// <returns>
        ///  0 on success, non-zero callback return value, or error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_patch_print(
            PatchHandle patch,
            git_diff_line_cb print_cb,
            IntPtr payload);

        /// <summary>
        ///  Get the content of a patch as a single diff text.
        /// </summary>
        /// <param name="_out">The git_buf to be filled in</param>
        /// <param name="patch">
        ///  A git_patch representing changes to one file
        /// </param>
        /// <returns>0 on success, < 0 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_patch_to_buf(
            ref git_buf _out,
            PatchHandle patch);
    }
}
