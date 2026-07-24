using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public sealed class BlameHandle : SafeHandle
    {
        public BlameHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe BlameHandle(git_blame* nativeBlame) :
            base((IntPtr)nativeBlame, true)
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
            libgit2.git_blame_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Gets the number of lines that exist in the blame structure.
        /// </summary>
        /// <param name="blame">The blame structure to query.</param>
        /// <returns>The number of line.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_blame_linecount(
            BlameHandle blame);

        /// <summary>
        ///  Gets the number of hunks that exist in the blame structure.
        /// </summary>
        /// <param name="blame">The blame structure to query.</param>
        /// <returns>The number of hunks.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_blame_hunkcount(
            BlameHandle blame);

        /// <summary>
        ///  Gets the blame hunk at the given index.
        /// </summary>
        /// <param name="blame">the blame structure to query</param>
        /// <param name="index">index of the hunk to retrieve</param>
        /// <returns>the hunk at the given index, or NULL on error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_blame_hunk* git_blame_hunk_byindex(
            BlameHandle blame,
            UIntPtr index);

        /// <summary>
        ///  Gets the hunk that relates to the given line number in the
        ///  newest commit.
        /// </summary>
        /// <param name="blame">the blame structure to query</param>
        /// <param name="lineno">
        ///  the (1-based) line number to find a hunk for
        /// </param>
        /// <returns>
        ///  the hunk that contains the given line, or NULL on error
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_blame_hunk* git_blame_hunk_byline(
            BlameHandle blame,
            UIntPtr lineno);

        /// <summary>
        ///  Gets the information about the line in the blame.
        /// </summary>
        /// <param name="blame">the blame structure to query</param>
        /// <param name="idx">the (1-based) line number</param>
        /// <returns>the blamed line, or NULL on error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_blame_line* git_blame_line_byindex(
            BlameHandle blame,
            UIntPtr idx);

        /// <summary>
        ///  Gets the number of hunks that exist in the blame structure.
        /// </summary>
        /// <param name="blame">The blame structure to query.</param>
        /// <returns>The number of hunks.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe uint git_blame_get_hunk_count(
            BlameHandle blame);

        /// <summary>
        ///  Gets the blame hunk at the given index.
        /// </summary>
        /// <param name="blame">the blame structure to query</param>
        /// <param name="index">index of the hunk to retrieve</param>
        /// <returns>the hunk at the given index, or NULL on error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_blame_hunk* git_blame_get_hunk_byindex(
            BlameHandle blame,
            uint index);

        /// <summary>
        ///  Gets the hunk that relates to the given line number in the
        ///  newest commit.
        /// </summary>
        /// <param name="blame">the blame structure to query</param>
        /// <param name="lineno">
        ///  the (1-based) line number to find a hunk for
        /// </param>
        /// <returns>
        ///  the hunk that contains the given line, or NULL on error
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_blame_hunk* git_blame_get_hunk_byline(
            BlameHandle blame,
            UIntPtr lineno);

        /// <summary>
        ///  Get the blame for a single file in the repository.
        /// </summary>
        /// <param name="_out">
        ///  pointer that will receive the blame object
        /// </param>
        /// <param name="repo">
        ///  repository whose history is to be walked
        /// </param>
        /// <param name="path">path to file to consider</param>
        /// <param name="options">
        ///  options for the blame operation or NULL
        /// </param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_blame_file(
            out BlameHandle _out,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            ref git_blame_options options);

        /// <summary>
        ///  Get the blame for a single file in the repository, using the
        ///  specified buffer contents as the uncommitted changes of the
        ///  file (the working directory contents).
        /// </summary>
        /// <param name="_out">
        ///  pointer that will receive the blame object
        /// </param>
        /// <param name="repo">
        ///  repository whose history is to be walked
        /// </param>
        /// <param name="path">path to file to consider</param>
        /// <param name="contents">the uncommitted changes</param>
        /// <param name="contents_len">
        ///  the length of the changes buffer
        /// </param>
        /// <param name="options">
        ///  options for the blame operation or NULL
        /// </param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_blame_file_from_buffer(
            out BlameHandle _out,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string contents,
            UIntPtr contents_len,
            ref git_blame_options options);

        /// <summary>
        ///  Get blame data for a file that has been modified in memory.
        ///  The `blame` parameter is a pre-calculated blame for the
        ///  in-odb history of the file. This means that once a file
        ///  blame is completed (which can be expensive), updating the
        ///  buffer blame is very fast.
        ///
        ///  <para>
        ///   Lines that differ between the buffer and the committed
        ///   version are marked as having a zero OID for their
        ///   final_commit_id.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  pointer that will receive the resulting blame data
        /// </param>
        /// <param name="_base">
        ///  cached blame from the history of the file (usually the
        ///  output from git_blame_file)
        /// </param>
        /// <param name="buffer">
        ///  the (possibly) modified contents of the file
        /// </param>
        /// <param name="buffer_len">
        ///  number of valid bytes in the buffer
        /// </param>
        /// <returns>
        ///  0 on success, or an error code. (use git_error_last for
        ///  information about the error)
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_blame_buffer(
            out BlameHandle _out,
            BlameHandle _base,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string buffer,
            UIntPtr buffer_len);

        /// <summary>
        ///  Free memory allocated by git_blame_file or git_blame_buffer.
        /// </summary>
        /// <param name="blame">the blame structure to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_blame_free(
            IntPtr blame);
    }
}
