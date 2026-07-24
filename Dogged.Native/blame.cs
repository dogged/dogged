using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Opaque structure to hold blame results
    /// </summary>
    public struct git_blame
    {
    }

    /// <summary>
    ///  Flags for indicating option behavior for git_blame APIs.
    /// </summary>
    public enum git_blame_flag_t
    {
        /// <summary>
        ///  Normal blame, the default
        /// </summary>
        GIT_BLAME_NORMAL = 0,

        /// <summary>
        ///  Track lines that have moved within a file (like `git blame
        ///  -M`).
        ///
        ///  <para>
        ///   This is not yet implemented and reserved for future use.
        ///  </para>
        /// </summary>
        GIT_BLAME_TRACK_COPIES_SAME_FILE = 1,

        /// <summary>
        ///  Track lines that have moved across files in the same commit
        ///  (like `git blame -C`).
        ///
        ///  <para>
        ///   This is not yet implemented and reserved for future use.
        ///  </para>
        /// </summary>
        GIT_BLAME_TRACK_COPIES_SAME_COMMIT_MOVES = 2,

        /// <summary>
        ///  Track lines that have been copied from another file that
        ///  exists in the same commit (like `git blame -CC`). Implies
        ///  SAME_FILE.
        ///
        ///  <para>
        ///   This is not yet implemented and reserved for future use.
        ///  </para>
        /// </summary>
        GIT_BLAME_TRACK_COPIES_SAME_COMMIT_COPIES = 4,

        /// <summary>
        ///  Track lines that have been copied from another file that
        ///  exists in *any* commit (like `git blame -CCC`). Implies
        ///  SAME_COMMIT_COPIES.
        ///
        ///  <para>
        ///   This is not yet implemented and reserved for future use.
        ///  </para>
        /// </summary>
        GIT_BLAME_TRACK_COPIES_ANY_COMMIT_COPIES = 8,

        /// <summary>
        ///  Restrict the search of commits to those reachable following
        ///  only the first parents.
        /// </summary>
        GIT_BLAME_FIRST_PARENT = 16,

        /// <summary>
        ///  Use mailmap file to map author and committer names and email
        ///  addresses to canonical real names and email addresses. The
        ///  mailmap will be read from the working directory, or HEAD in
        ///  a bare repository.
        /// </summary>
        GIT_BLAME_USE_MAILMAP = 32,

        /// <summary>
        ///  Ignore whitespace differences
        /// </summary>
        GIT_BLAME_IGNORE_WHITESPACE = 64,
    }

    /// <summary>
    ///  Blame options structure
    ///
    ///  <para>
    ///   Initialize with `GIT_BLAME_OPTIONS_INIT`. Alternatively, you
    ///   can use `git_blame_options_init`.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_blame_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_BLAME_OPTIONS_VERSION = 1;


        public uint version;

        /// <summary>
        ///  A combination of `git_blame_flag_t`
        /// </summary>
        public uint flags;

        /// <summary>
        ///  The lower bound on the number of alphanumeric characters
        ///  that must be detected as moving/copying within a file for it
        ///  to associate those lines with the parent commit. The default
        ///  value is 20.
        ///
        ///  <para>
        ///   This value only takes effect if any of the
        ///   `GIT_BLAME_TRACK_COPIES_*` flags are specified.
        ///  </para>
        /// </summary>
        public ushort min_match_characters;

        /// <summary>
        ///  The id of the newest commit to consider. The default is
        ///  HEAD.
        /// </summary>
        public git_oid newest_commit;

        /// <summary>
        ///  The id of the oldest commit to consider. The default is the
        ///  first commit encountered with a NULL parent.
        /// </summary>
        public git_oid oldest_commit;

        /// <summary>
        ///  The first line in the file to blame. The default is 1 (line
        ///  numbers start with 1).
        /// </summary>
        public UIntPtr min_line;

        /// <summary>
        ///  The last line in the file to blame. The default is the last
        ///  line of the file.
        /// </summary>
        public UIntPtr max_line;
    }

    /// <summary>
    ///  Structure that represents a blame hunk.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_blame_hunk
    {
        /// <summary>
        ///  The number of lines in this hunk.
        /// </summary>
        public UIntPtr lines_in_hunk;

        /// <summary>
        ///  The OID of the commit where this line was last changed.
        /// </summary>
        public git_oid final_commit_id;

        /// <summary>
        ///  The 1-based line number where this hunk begins, in the final
        ///  version of the file.
        /// </summary>
        public UIntPtr final_start_line_number;

        /// <summary>
        ///  The author of `final_commit_id`. If `GIT_BLAME_USE_MAILMAP`
        ///  has been specified, it will contain the canonical real name
        ///  and email address.
        /// </summary>
        public IntPtr final_signature;

        /// <summary>
        ///  The committer of `final_commit_id`. If
        ///  `GIT_BLAME_USE_MAILMAP` has been specified, it will contain
        ///  the canonical real name and email address.
        /// </summary>
        public IntPtr final_committer;

        /// <summary>
        ///  The OID of the commit where this hunk was found. This will
        ///  usually be the same as `final_commit_id`, except when
        ///  `GIT_BLAME_TRACK_COPIES_ANY_COMMIT_COPIES` has been
        ///  specified.
        /// </summary>
        public git_oid orig_commit_id;

        /// <summary>
        ///  The path to the file where this hunk originated, as of the
        ///  commit specified by `orig_commit_id`.
        /// </summary>
        public IntPtr orig_path;

        /// <summary>
        ///  The 1-based line number where this hunk begins in the file
        ///  named by `orig_path` in the commit specified by
        ///  `orig_commit_id`.
        /// </summary>
        public UIntPtr orig_start_line_number;

        /// <summary>
        ///  The author of `orig_commit_id`. If `GIT_BLAME_USE_MAILMAP`
        ///  has been specified, it will contain the canonical real name
        ///  and email address.
        /// </summary>
        public IntPtr orig_signature;

        /// <summary>
        ///  The committer of `orig_commit_id`. If
        ///  `GIT_BLAME_USE_MAILMAP` has been specified, it will contain
        ///  the canonical real name and email address.
        /// </summary>
        public IntPtr orig_committer;

        public IntPtr summary;

        /// <summary>
        ///  The 1 iff the hunk has been tracked to a boundary commit
        ///  (the root, or the commit specified in
        ///  git_blame_options.oldest_commit)
        /// </summary>
        public char boundary;
    }

    /// <summary>
    ///  Structure that represents a line in a blamed file.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_blame_line
    {
        public IntPtr ptr;

        public UIntPtr len;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Initialize git_blame_options structure
        ///
        ///  <para>
        ///   Initializes a `git_blame_options` with default values.
        ///   Equivalent to creating an instance with
        ///   GIT_BLAME_OPTIONS_INIT.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_blame_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass `GIT_BLAME_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_blame_options_init(
            ref git_blame_options opts,
            uint version);

        /// <summary>
        ///  Gets the number of lines that exist in the blame structure.
        /// </summary>
        /// <param name="blame">The blame structure to query.</param>
        /// <returns>The number of line.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_blame_linecount(
            git_blame* blame);

        /// <summary>
        ///  Gets the number of hunks that exist in the blame structure.
        /// </summary>
        /// <param name="blame">The blame structure to query.</param>
        /// <returns>The number of hunks.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_blame_hunkcount(
            git_blame* blame);

        /// <summary>
        ///  Gets the blame hunk at the given index.
        /// </summary>
        /// <param name="blame">the blame structure to query</param>
        /// <param name="index">index of the hunk to retrieve</param>
        /// <returns>the hunk at the given index, or NULL on error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_blame_hunk* git_blame_hunk_byindex(
            git_blame* blame,
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
            git_blame* blame,
            UIntPtr lineno);

        /// <summary>
        ///  Gets the information about the line in the blame.
        /// </summary>
        /// <param name="blame">the blame structure to query</param>
        /// <param name="idx">the (1-based) line number</param>
        /// <returns>the blamed line, or NULL on error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_blame_line* git_blame_line_byindex(
            git_blame* blame,
            UIntPtr idx);

        /// <summary>
        ///  Gets the number of hunks that exist in the blame structure.
        /// </summary>
        /// <param name="blame">The blame structure to query.</param>
        /// <returns>The number of hunks.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe uint git_blame_get_hunk_count(
            git_blame* blame);

        /// <summary>
        ///  Gets the blame hunk at the given index.
        /// </summary>
        /// <param name="blame">the blame structure to query</param>
        /// <param name="index">index of the hunk to retrieve</param>
        /// <returns>the hunk at the given index, or NULL on error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_blame_hunk* git_blame_get_hunk_byindex(
            git_blame* blame,
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
            git_blame* blame,
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
            out git_blame* _out,
            git_repository* repo,
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
            out git_blame* _out,
            git_repository* repo,
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
            out git_blame* _out,
            git_blame* _base,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string buffer,
            UIntPtr buffer_len);

        /// <summary>
        ///  Free memory allocated by git_blame_file or git_blame_buffer.
        /// </summary>
        /// <param name="blame">the blame structure to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_blame_free(
            git_blame* blame);
    }
}
