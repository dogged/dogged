using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  The diff object that contains all individual file deltas.
    ///
    ///  <para>
    ///   A `diff` represents the cumulative list of differences
    ///   between two snapshots of a repository (possibly filtered by
    ///   a set of file name patterns).
    ///  </para>
    ///
    ///  <para>
    ///   Calculating diffs is generally done in two phases: building
    ///   a list of diffs then traversing it. This makes is easier to
    ///   share logic across the various types of diffs (tree vs tree,
    ///   workdir vs index, etc.), and also allows you to insert
    ///   optional diff post-processing phases, such as rename
    ///   detection, in between the steps. When you are done with a
    ///   diff object, it must be freed.
    ///  </para>
    ///
    ///  <para>
    ///   This is an opaque structure which will be allocated by one
    ///   of the diff generator functions below (such as
    ///   `git_diff_tree_to_tree`). You are responsible for releasing
    ///   the object memory when done, using the `git_diff_free()`
    ///   function.
    ///  </para>
    /// </summary>
    public struct git_diff
    {
        /// <summary>
        ///  Maximum size of the hunk header
        /// </summary>
        public const int GIT_DIFF_HUNK_HEADER_SIZE = 128;
    }

    /// <summary>
    ///  Flags for diff options. A combination of these flags can be
    ///  passed in via the `flags` value in the `git_diff_options`.
    /// </summary>
    public enum git_diff_option_t
    {
        /// <summary>
        ///  Normal diff, the default
        /// </summary>
        GIT_DIFF_NORMAL = 0,

        /// <summary>
        ///  Reverse the sides of the diff
        /// </summary>
        GIT_DIFF_REVERSE = 1,

        /// <summary>
        ///  Include ignored files in the diff
        /// </summary>
        GIT_DIFF_INCLUDE_IGNORED = 2,

        /// <summary>
        ///  Even with GIT_DIFF_INCLUDE_IGNORED, an entire ignored
        ///  directory will be marked with only a single entry in the
        ///  diff; this flag adds all files under the directory as
        ///  IGNORED entries, too.
        /// </summary>
        GIT_DIFF_RECURSE_IGNORED_DIRS = 4,

        /// <summary>
        ///  Include untracked files in the diff
        /// </summary>
        GIT_DIFF_INCLUDE_UNTRACKED = 8,

        /// <summary>
        ///  Even with GIT_DIFF_INCLUDE_UNTRACKED, an entire untracked
        ///  directory will be marked with only a single entry in the
        ///  diff (a la what core Git does in `git status`); this flag
        ///  adds *all* files under untracked directories as UNTRACKED
        ///  entries, too.
        /// </summary>
        GIT_DIFF_RECURSE_UNTRACKED_DIRS = 16,

        /// <summary>
        ///  Include unmodified files in the diff
        /// </summary>
        GIT_DIFF_INCLUDE_UNMODIFIED = 32,

        /// <summary>
        ///  Normally, a type change between files will be converted into
        ///  a DELETED record for the old and an ADDED record for the
        ///  new; this options enabled the generation of TYPECHANGE delta
        ///  records.
        /// </summary>
        GIT_DIFF_INCLUDE_TYPECHANGE = 64,

        /// <summary>
        ///  Even with GIT_DIFF_INCLUDE_TYPECHANGE, blob->tree changes
        ///  still generally show as a DELETED blob. This flag tries to
        ///  correctly label blob->tree transitions as TYPECHANGE records
        ///  with new_file's mode set to tree. Note: the tree SHA will
        ///  not be available.
        /// </summary>
        GIT_DIFF_INCLUDE_TYPECHANGE_TREES = 128,

        /// <summary>
        ///  Ignore file mode changes
        /// </summary>
        GIT_DIFF_IGNORE_FILEMODE = 256,

        /// <summary>
        ///  Treat all submodules as unmodified
        /// </summary>
        GIT_DIFF_IGNORE_SUBMODULES = 512,

        /// <summary>
        ///  Use case insensitive filename comparisons
        /// </summary>
        GIT_DIFF_IGNORE_CASE = 1024,

        /// <summary>
        ///  May be combined with `GIT_DIFF_IGNORE_CASE` to specify that
        ///  a file that has changed case will be returned as an
        ///  add/delete pair.
        /// </summary>
        GIT_DIFF_INCLUDE_CASECHANGE = 2048,

        /// <summary>
        ///  If the pathspec is set in the diff options, this flags
        ///  indicates that the paths will be treated as literal paths
        ///  instead of fnmatch patterns. Each path in the list must
        ///  either be a full path to a file or a directory. (A trailing
        ///  slash indicates that the path will _only_ match a
        ///  directory). If a directory is specified, all children will
        ///  be included.
        /// </summary>
        GIT_DIFF_DISABLE_PATHSPEC_MATCH = 4096,

        /// <summary>
        ///  Disable updating of the `binary` flag in delta records. This
        ///  is useful when iterating over a diff if you don't need hunk
        ///  and data callbacks and want to avoid having to load file
        ///  completely.
        /// </summary>
        GIT_DIFF_SKIP_BINARY_CHECK = 8192,

        /// <summary>
        ///  When diff finds an untracked directory, to match the
        ///  behavior of core Git, it scans the contents for IGNORED and
        ///  UNTRACKED files. If *all* contents are IGNORED, then the
        ///  directory is IGNORED; if any contents are not IGNORED, then
        ///  the directory is UNTRACKED. This is extra work that may not
        ///  matter in many cases. This flag turns off that scan and
        ///  immediately labels an untracked directory as UNTRACKED
        ///  (changing the behavior to not match core Git).
        /// </summary>
        GIT_DIFF_ENABLE_FAST_UNTRACKED_DIRS = 16384,

        /// <summary>
        ///  When diff finds a file in the working directory with stat
        ///  information different from the index, but the OID ends up
        ///  being the same, write the correct stat information into the
        ///  index. Note: without this flag, diff will always leave the
        ///  index untouched.
        /// </summary>
        GIT_DIFF_UPDATE_INDEX = 32768,

        /// <summary>
        ///  Include unreadable files in the diff
        /// </summary>
        GIT_DIFF_INCLUDE_UNREADABLE = 65536,

        /// <summary>
        ///  Include unreadable files in the diff
        /// </summary>
        GIT_DIFF_INCLUDE_UNREADABLE_AS_UNTRACKED = 131072,

        /// <summary>
        ///  Use a heuristic that takes indentation and whitespace into
        ///  account which generally can produce better diffs when
        ///  dealing with ambiguous diff hunks.
        /// </summary>
        GIT_DIFF_INDENT_HEURISTIC = 262144,

        /// <summary>
        ///  Ignore blank lines
        /// </summary>
        GIT_DIFF_IGNORE_BLANK_LINES = 524288,

        /// <summary>
        ///  Treat all files as text, disabling binary attributes &
        ///  detection
        /// </summary>
        GIT_DIFF_FORCE_TEXT = 1048576,

        /// <summary>
        ///  Treat all files as binary, disabling text diffs
        /// </summary>
        GIT_DIFF_FORCE_BINARY = 2097152,

        /// <summary>
        ///  Ignore all whitespace
        /// </summary>
        GIT_DIFF_IGNORE_WHITESPACE = 4194304,

        /// <summary>
        ///  Ignore changes in amount of whitespace
        /// </summary>
        GIT_DIFF_IGNORE_WHITESPACE_CHANGE = 8388608,

        /// <summary>
        ///  Ignore whitespace at end of line
        /// </summary>
        GIT_DIFF_IGNORE_WHITESPACE_EOL = 16777216,

        /// <summary>
        ///  When generating patch text, include the content of untracked
        ///  files. This automatically turns on
        ///  GIT_DIFF_INCLUDE_UNTRACKED but it does not turn on
        ///  GIT_DIFF_RECURSE_UNTRACKED_DIRS. Add that flag if you want
        ///  the content of every single UNTRACKED file.
        /// </summary>
        GIT_DIFF_SHOW_UNTRACKED_CONTENT = 33554432,

        /// <summary>
        ///  When generating output, include the names of unmodified
        ///  files if they are included in the git_diff. Normally these
        ///  are skipped in the formats that list files (e.g. name-only,
        ///  name-status, raw). Even with this, these will not be
        ///  included in patch format.
        /// </summary>
        GIT_DIFF_SHOW_UNMODIFIED = 67108864,

        /// <summary>
        ///  Use the "patience diff" algorithm
        /// </summary>
        GIT_DIFF_PATIENCE = 268435456,

        /// <summary>
        ///  Take extra time to find minimal diff
        /// </summary>
        GIT_DIFF_MINIMAL = 536870912,

        /// <summary>
        ///  Include the necessary deflate / delta information so that
        ///  `git-apply` can apply given diff information to binary
        ///  files.
        /// </summary>
        GIT_DIFF_SHOW_BINARY = 1073741824,
    }

    /// <summary>
    ///  Flags for the delta object and the file objects on each side.
    ///
    ///  <para>
    ///   These flags are used for both the `flags` value of the
    ///   `git_diff_delta` and the flags for the `git_diff_file`
    ///   objects representing the old and new sides of the delta.
    ///   Values outside of this public range should be considered
    ///   reserved for internal or future use.
    ///  </para>
    /// </summary>
    public enum git_diff_flag_t
    {
        /// <summary>
        ///  file(s) treated as binary data
        /// </summary>
        GIT_DIFF_FLAG_BINARY = 1,

        /// <summary>
        ///  file(s) treated as text data
        /// </summary>
        GIT_DIFF_FLAG_NOT_BINARY = 2,

        /// <summary>
        ///  `id` value is known correct
        /// </summary>
        GIT_DIFF_FLAG_VALID_ID = 4,

        /// <summary>
        ///  file exists at this side of the delta
        /// </summary>
        GIT_DIFF_FLAG_EXISTS = 8,

        /// <summary>
        ///  file size value is known correct
        /// </summary>
        GIT_DIFF_FLAG_VALID_SIZE = 16,
    }

    /// <summary>
    ///  What type of change is described by a git_diff_delta?
    ///
    ///  <para>
    ///   `GIT_DELTA_RENAMED` and `GIT_DELTA_COPIED` will only show up
    ///   if you run `git_diff_find_similar()` on the diff object.
    ///  </para>
    ///
    ///  <para>
    ///   `GIT_DELTA_TYPECHANGE` only shows up given
    ///   `GIT_DIFF_INCLUDE_TYPECHANGE` in the option flags (otherwise
    ///   type changes will be split into ADDED / DELETED pairs).
    ///  </para>
    /// </summary>
    public enum git_delta_t
    {
        /// <summary>
        ///  no changes
        /// </summary>
        GIT_DELTA_UNMODIFIED = 0,

        /// <summary>
        ///  entry does not exist in old version
        /// </summary>
        GIT_DELTA_ADDED = 1,

        /// <summary>
        ///  entry does not exist in new version
        /// </summary>
        GIT_DELTA_DELETED = 2,

        /// <summary>
        ///  entry content changed between old and new
        /// </summary>
        GIT_DELTA_MODIFIED = 3,

        /// <summary>
        ///  entry was renamed between old and new
        /// </summary>
        GIT_DELTA_RENAMED = 4,

        /// <summary>
        ///  entry was copied from another old entry
        /// </summary>
        GIT_DELTA_COPIED = 5,

        /// <summary>
        ///  entry is ignored item in workdir
        /// </summary>
        GIT_DELTA_IGNORED = 6,

        /// <summary>
        ///  entry is untracked item in workdir
        /// </summary>
        GIT_DELTA_UNTRACKED = 7,

        /// <summary>
        ///  type of entry changed between old and new
        /// </summary>
        GIT_DELTA_TYPECHANGE = 8,

        /// <summary>
        ///  entry is unreadable
        /// </summary>
        GIT_DELTA_UNREADABLE = 9,

        /// <summary>
        ///  entry in the index is conflicted
        /// </summary>
        GIT_DELTA_CONFLICTED = 10,
    }

    /// <summary>
    ///  Description of one side of a delta.
    ///
    ///  <para>
    ///   Although this is called a "file", it could represent a file,
    ///   a symbolic link, a submodule commit id, or even a tree
    ///   (although that only if you are tracking type changes or
    ///   ignored/untracked directories).
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_diff_file
    {
        /// <summary>
        ///  The `git_oid` of the item. If the entry represents an absent
        ///  side of a diff (e.g. the `old_file` of a `GIT_DELTA_ADDED`
        ///  delta), then the oid will be zeroes.
        /// </summary>
        public git_oid id;

        /// <summary>
        ///  The NUL-terminated path to the entry relative to the working
        ///  directory of the repository.
        /// </summary>
        public IntPtr path;

        /// <summary>
        ///  The size of the entry in bytes.
        /// </summary>
        public long size;

        /// <summary>
        ///  A combination of the `git_diff_flag_t` types
        /// </summary>
        public uint flags;

        /// <summary>
        ///  Roughly, the stat() `st_mode` value for the item. This will
        ///  be restricted to one of the `git_filemode_t` values.
        /// </summary>
        public ushort mode;

        /// <summary>
        ///  Represents the known length of the `id` field, when
        ///  converted to a hex string. It is generally
        ///  `GIT_OID_SHA1_HEXSIZE`, unless this delta was created from
        ///  reading a patch file, in which case it may be abbreviated to
        ///  something reasonable, like 7 characters.
        /// </summary>
        public ushort id_abbrev;
    }

    /// <summary>
    ///  Description of changes to one entry.
    ///
    ///  <para>
    ///   A `delta` is a file pair with an old and new revision. The
    ///   old version may be absent if the file was just created and
    ///   the new version may be absent if the file was deleted. A
    ///   diff is mostly just a list of deltas.
    ///  </para>
    ///
    ///  <para>
    ///   When iterating over a diff, this will be passed to most
    ///   callbacks and you can use the contents to understand exactly
    ///   what has changed.
    ///  </para>
    ///
    ///  <para>
    ///   The `old_file` represents the "from" side of the diff and
    ///   the `new_file` represents to "to" side of the diff. What
    ///   those means depend on the function that was used to generate
    ///   the diff and will be documented below. You can also use the
    ///   `GIT_DIFF_REVERSE` flag to flip it around.
    ///  </para>
    ///
    ///  <para>
    ///   Although the two sides of the delta are named "old_file" and
    ///   "new_file", they actually may correspond to entries that
    ///   represent a file, a symbolic link, a submodule commit id, or
    ///   even a tree (if you are tracking type changes or
    ///   ignored/untracked directories).
    ///  </para>
    ///
    ///  <para>
    ///   Under some circumstances, in the name of efficiency, not all
    ///   fields will be filled in, but we generally try to fill in as
    ///   much as possible. One example is that the "flags" field may
    ///   not have either the `BINARY` or the `NOT_BINARY` flag set to
    ///   avoid examining file contents if you do not pass in hunk
    ///   and/or line callbacks to the diff foreach iteration
    ///   function. It will just use the git attributes for those
    ///   files.
    ///  </para>
    ///
    ///  <para>
    ///   The similarity score is zero unless you call
    ///   `git_diff_find_similar()` which does a similarity analysis
    ///   of files in the diff. Use that function to do rename and
    ///   copy detection, and to split heavily modified files in
    ///   add/delete pairs. After that call, deltas with a status of
    ///   GIT_DELTA_RENAMED or GIT_DELTA_COPIED will have a similarity
    ///   score between 0 and 100 indicating how similar the old and
    ///   new sides are.
    ///  </para>
    ///
    ///  <para>
    ///   If you ask `git_diff_find_similar` to find heavily modified
    ///   files to break, but to not *actually* break the records,
    ///   then GIT_DELTA_MODIFIED records may have a non-zero
    ///   similarity score if the self-similarity is below the split
    ///   threshold. To display this value like core Git, invert the
    ///   score (a la `printf("M%03d", 100 - delta->similarity)`).
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_diff_delta
    {
        public git_delta_t status;

        /// <summary>
        ///  git_diff_flag_t values
        /// </summary>
        public uint flags;

        /// <summary>
        ///  for RENAMED and COPIED, value 0-100
        /// </summary>
        public ushort similarity;

        /// <summary>
        ///  number of files in this delta
        /// </summary>
        public ushort nfiles;

        public git_diff_file old_file;

        public git_diff_file new_file;
    }

    /// <summary>
    ///  Diff notification callback function.
    ///
    ///  <para>
    ///   The callback will be called for each file, just before the
    ///   `git_diff_delta` gets inserted into the diff.
    ///  </para>
    ///
    ///  <para>
    ///   When the callback: - returns < 0, the diff process will be
    ///   aborted. - returns > 0, the delta will not be inserted into
    ///   the diff, but the diff process continues. - returns 0, the
    ///   delta is inserted into the diff, and the diff process
    ///   continues.
    ///  </para>
    /// </summary>
    /// <param name="diff_so_far">
    ///  the diff structure as it currently exists
    /// </param>
    /// <param name="delta_to_add">the delta that is to be added</param>
    /// <param name="matched_pathspec">the pathspec</param>
    /// <param name="payload">the user-specified callback payload</param>
    /// <returns>
    ///  0 on success, 1 to skip this delta, or an error code
    /// </returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_diff_notify_cb(
        git_diff* diff_so_far,
        ref git_diff_delta delta_to_add,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string matched_pathspec,
        IntPtr payload);

    /// <summary>
    ///  Diff progress callback.
    ///
    ///  <para>
    ///   Called before each file comparison.
    ///  </para>
    /// </summary>
    /// <param name="diff_so_far">The diff being generated.</param>
    /// <param name="old_path">The path to the old file or NULL.</param>
    /// <param name="new_path">The path to the new file or NULL.</param>
    /// <param name="payload">the user-specified callback payload</param>
    /// <returns>0 or an error code</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_diff_progress_cb(
        git_diff* diff_so_far,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string old_path,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string new_path,
        IntPtr payload);

    /// <summary>
    ///  Structure describing options about how the diff should be
    ///  executed.
    ///
    ///  <para>
    ///   Setting all values of the structure to zero will yield the
    ///   default values. Similarly, passing NULL for the options
    ///   structure will give the defaults. The default values are
    ///   marked below.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_diff_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_DIFF_OPTIONS_VERSION = 1;


        /// <summary>
        ///  version for the struct
        /// </summary>
        public uint version;

        /// <summary>
        ///  A combination of `git_diff_option_t` values above. Defaults
        ///  to GIT_DIFF_NORMAL
        /// </summary>
        public uint flags;

        /// <summary>
        ///  Overrides the submodule ignore setting for all submodules in
        ///  the diff.
        /// </summary>
        public git_submodule_ignore_t ignore_submodules;

        /// <summary>
        ///  An array of paths / fnmatch patterns to constrain diff. All
        ///  paths are included by default.
        /// </summary>
        public git_strarray pathspec;

        /// <summary>
        ///  An optional callback function, notifying the consumer of
        ///  changes to the diff as new deltas are added.
        /// </summary>
        public git_diff_notify_cb notify_cb;

        /// <summary>
        ///  An optional callback function, notifying the consumer of
        ///  which files are being examined as the diff is generated.
        /// </summary>
        public git_diff_progress_cb progress_cb;

        /// <summary>
        ///  The payload to pass to the callback functions.
        /// </summary>
        public IntPtr payload;

        /// <summary>
        ///  The number of unchanged lines that define the boundary of a
        ///  hunk (and to display before and after). Defaults to 3.
        /// </summary>
        public uint context_lines;

        /// <summary>
        ///  The maximum number of unchanged lines between hunk
        ///  boundaries before the hunks will be merged into one.
        ///  Defaults to 0.
        /// </summary>
        public uint interhunk_lines;

        /// <summary>
        ///  The object ID type to emit in diffs; this is used by
        ///  functions that operate without a repository - namely
        ///  `git_diff_buffers`, or `git_diff_blobs` and
        ///  `git_diff_blob_to_buffer` when one blob is `NULL`.
        ///
        ///  <para>
        ///   This may be omitted (set to `0`). If a repository is
        ///   available, the object ID format of the repository will
        ///   be used. If no repository is available then the default
        ///   is `GIT_OID_SHA`.
        ///  </para>
        ///
        ///  <para>
        ///   If this is specified and a repository is available, then
        ///   the specified `oid_type` must match the repository's
        ///   object ID format.
        ///  </para>
        /// </summary>
        public git_oid_t oid_type;

        /// <summary>
        ///  The abbreviation length to use when formatting object ids.
        ///  Defaults to the value of 'core.abbrev' from the config, or 7
        ///  if unset.
        /// </summary>
        public ushort id_abbrev;

        /// <summary>
        ///  A size (in bytes) above which a blob will be marked as
        ///  binary automatically; pass a negative value to disable.
        ///  Defaults to 512MB.
        /// </summary>
        public IntPtr max_size;

        /// <summary>
        ///  The virtual "directory" prefix for old file names in hunk
        ///  headers. Default is "a".
        /// </summary>
        public IntPtr old_prefix;

        /// <summary>
        ///  The virtual "directory" prefix for new file names in hunk
        ///  headers. Defaults to "b".
        /// </summary>
        public IntPtr new_prefix;
    }

    /// <summary>
    ///  When iterating over a diff, callback that will be made per file.
    /// </summary>
    /// <param name="delta">A pointer to the delta data for the file</param>
    /// <param name="progress">Goes from 0 to 1 over the diff</param>
    /// <param name="payload">
    ///  User-specified pointer from foreach function
    /// </param>
    /// <returns>0 or an error code</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_diff_file_cb(
        ref git_diff_delta delta,
        float progress,
        IntPtr payload);

    /// <summary>
    ///  When producing a binary diff, the binary data returned will be
    ///  either the deflated full ("literal") contents of the file, or
    ///  the deflated binary delta between the two sides (whichever is
    ///  smaller).
    /// </summary>
    public enum git_diff_binary_t
    {
        /// <summary>
        ///  There is no binary delta.
        /// </summary>
        GIT_DIFF_BINARY_NONE,

        /// <summary>
        ///  The binary data is the literal contents of the file.
        /// </summary>
        GIT_DIFF_BINARY_LITERAL,

        /// <summary>
        ///  The binary data is the delta from one side to the other.
        /// </summary>
        GIT_DIFF_BINARY_DELTA,
    }

    /// <summary>
    ///  The contents of one of the files in a binary diff.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_diff_binary_file
    {
        /// <summary>
        ///  The type of binary data for this file.
        /// </summary>
        public git_diff_binary_t type;

        /// <summary>
        ///  The binary data, deflated.
        /// </summary>
        public IntPtr data;

        /// <summary>
        ///  The length of the binary data.
        /// </summary>
        public UIntPtr datalen;

        /// <summary>
        ///  The length of the binary data after inflation.
        /// </summary>
        public UIntPtr inflatedlen;
    }

    /// <summary>
    ///  Structure describing the binary contents of a diff.
    ///
    ///  <para>
    ///   A `binary` file / delta is a file (or pair) for which no
    ///   text diffs should be generated. A diff can contain delta
    ///   entries that are binary, but no diff content will be output
    ///   for those files. There is a base heuristic for binary
    ///   detection and you can further tune the behavior with git
    ///   attributes or diff flags and option settings.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_diff_binary
    {
        /// <summary>
        ///  Whether there is data in this binary structure or not.
        ///
        ///  <para>
        ///   If this is `1`, then this was produced and included
        ///   binary content. If this is `0` then this was generated
        ///   knowing only that a binary file changed but without
        ///   providing the data, probably from a patch that said
        ///   `Binary files a/file.txt and b/file.txt differ`.
        ///  </para>
        /// </summary>
        public uint contains_data;

        /// <summary>
        ///  The contents of the old file.
        /// </summary>
        public git_diff_binary_file old_file;

        /// <summary>
        ///  The contents of the new file.
        /// </summary>
        public git_diff_binary_file new_file;
    }

    /// <summary>
    ///  When iterating over a diff, callback that will be made for
    ///  binary content within the diff.
    /// </summary>
    /// <param name="delta">the delta</param>
    /// <param name="binary">the binary content</param>
    /// <param name="payload">the user-specified callback payload</param>
    /// <returns>0 or an error code</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_diff_binary_cb(
        ref git_diff_delta delta,
        ref git_diff_binary binary,
        IntPtr payload);

    /// <summary>
    ///  Structure describing a hunk of a diff.
    ///
    ///  <para>
    ///   A `hunk` is a span of modified lines in a delta along with
    ///   some stable surrounding context. You can configure the
    ///   amount of context and other properties of how hunks are
    ///   generated. Each hunk also comes with a header that described
    ///   where it starts and ends in both the old and new versions in
    ///   the delta.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_diff_hunk
    {
        /// <summary>
        ///  Starting line number in old_file
        /// </summary>
        public int old_start;

        /// <summary>
        ///  Number of lines in old_file
        /// </summary>
        public int old_lines;

        /// <summary>
        ///  Starting line number in new_file
        /// </summary>
        public int new_start;

        /// <summary>
        ///  Number of lines in new_file
        /// </summary>
        public int new_lines;

        /// <summary>
        ///  Number of bytes in header text
        /// </summary>
        public UIntPtr header_len;

        /// <summary>
        ///  Header text, NUL-byte terminated
        /// </summary>
        public byte* header;
    }

    /// <summary>
    ///  When iterating over a diff, callback that will be made per hunk.
    /// </summary>
    /// <param name="delta">the delta</param>
    /// <param name="hunk">the hunk</param>
    /// <param name="payload">the user-specified callback payload</param>
    /// <returns>0 or an error code</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_diff_hunk_cb(
        ref git_diff_delta delta,
        ref git_diff_hunk hunk,
        IntPtr payload);

    /// <summary>
    ///  Line origin constants.
    ///
    ///  <para>
    ///   These values describe where a line came from and will be
    ///   passed to the git_diff_line_cb when iterating over a diff.
    ///   There are some special origin constants at the end that are
    ///   used for the text output callbacks to demarcate lines that
    ///   are actually part of the file or hunk headers.
    ///  </para>
    /// </summary>
    public enum git_diff_line_t
    {
        GIT_DIFF_LINE_CONTEXT = 32,

        GIT_DIFF_LINE_ADDITION = 43,

        GIT_DIFF_LINE_DELETION = 45,

        /// <summary>
        ///  Both files have no LF at end
        /// </summary>
        GIT_DIFF_LINE_CONTEXT_EOFNL = 61,

        /// <summary>
        ///  Old has no LF at end, new does
        /// </summary>
        GIT_DIFF_LINE_ADD_EOFNL = 62,

        /// <summary>
        ///  Old has LF at end, new does not
        /// </summary>
        GIT_DIFF_LINE_DEL_EOFNL = 60,

        GIT_DIFF_LINE_FILE_HDR = 70,

        GIT_DIFF_LINE_HUNK_HDR = 72,

        /// <summary>
        ///  For "Binary files x and y differ"
        /// </summary>
        GIT_DIFF_LINE_BINARY = 66,
    }

    /// <summary>
    ///  Structure describing a line (or data span) of a diff.
    ///
    ///  <para>
    ///   A `line` is a range of characters inside a hunk. It could be
    ///   a context line (i.e. in both old and new versions), an added
    ///   line (i.e. only in the new version), or a removed line (i.e.
    ///   only in the old version). Unfortunately, we don't know
    ///   anything about the encoding of data in the file being
    ///   diffed, so we cannot tell you much about the line content.
    ///   Line data will not be NUL-byte terminated, however, because
    ///   it will be just a span of bytes inside the larger file.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_diff_line
    {
        /// <summary>
        ///  A git_diff_line_t value
        /// </summary>
        public char origin;

        /// <summary>
        ///  Line number in old file or -1 for added line
        /// </summary>
        public int old_lineno;

        /// <summary>
        ///  Line number in new file or -1 for deleted line
        /// </summary>
        public int new_lineno;

        /// <summary>
        ///  Number of newline characters in content
        /// </summary>
        public int num_lines;

        /// <summary>
        ///  Number of bytes of data
        /// </summary>
        public UIntPtr content_len;

        /// <summary>
        ///  Offset in the original file to the content
        /// </summary>
        public IntPtr content_offset;

        /// <summary>
        ///  Pointer to diff text, not NUL-byte terminated
        /// </summary>
        public IntPtr content;
    }

    /// <summary>
    ///  When iterating over a diff, callback that will be made per text
    ///  diff line. In this context, the provided range will be NULL.
    ///
    ///  <para>
    ///   When printing a diff, callback that will be made to output
    ///   each line of text. This uses some extra GIT_DIFF_LINE_...
    ///   constants for output of lines of file and hunk headers.
    ///  </para>
    /// </summary>
    /// <param name="delta">the delta that contains the line</param>
    /// <param name="hunk">the hunk that contains the line</param>
    /// <param name="line">the line in the diff</param>
    /// <param name="payload">the user-specified callback payload</param>
    /// <returns>0 or an error code</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_diff_line_cb(
        ref git_diff_delta delta,
        ref git_diff_hunk hunk,
        ref git_diff_line line,
        IntPtr payload);

    /// <summary>
    ///  Flags to control the behavior of diff rename/copy detection.
    /// </summary>
    public enum git_diff_find_t
    {
        /// <summary>
        ///  Obey `diff.renames`. Overridden by any other
        ///  GIT_DIFF_FIND_... flag.
        /// </summary>
        GIT_DIFF_FIND_BY_CONFIG = 0,

        /// <summary>
        ///  Look for renames? (`--find-renames`)
        /// </summary>
        GIT_DIFF_FIND_RENAMES = 1,

        /// <summary>
        ///  Consider old side of MODIFIED for renames?
        ///  (`--break-rewrites=N`)
        /// </summary>
        GIT_DIFF_FIND_RENAMES_FROM_REWRITES = 2,

        /// <summary>
        ///  Look for copies? (a la `--find-copies`).
        /// </summary>
        GIT_DIFF_FIND_COPIES = 4,

        /// <summary>
        ///  Consider UNMODIFIED as copy sources?
        ///  (`--find-copies-harder`).
        ///
        ///  <para>
        ///   For this to work correctly, use
        ///   GIT_DIFF_INCLUDE_UNMODIFIED when the initial `git_diff`
        ///   is being generated.
        ///  </para>
        /// </summary>
        GIT_DIFF_FIND_COPIES_FROM_UNMODIFIED = 8,

        /// <summary>
        ///  Mark significant rewrites for split (`--break-rewrites=/M`)
        /// </summary>
        GIT_DIFF_FIND_REWRITES = 16,

        /// <summary>
        ///  Actually split large rewrites into delete/add pairs
        /// </summary>
        GIT_DIFF_BREAK_REWRITES = 32,

        /// <summary>
        ///  Mark rewrites for split and break into delete/add pairs
        /// </summary>
        GIT_DIFF_FIND_AND_BREAK_REWRITES = 48,

        /// <summary>
        ///  Find renames/copies for UNTRACKED items in working
        ///  directory.
        ///
        ///  <para>
        ///   For this to work correctly, use
        ///   GIT_DIFF_INCLUDE_UNTRACKED when the initial `git_diff`
        ///   is being generated (and obviously the diff must be
        ///   against the working directory for this to make sense).
        ///  </para>
        /// </summary>
        GIT_DIFF_FIND_FOR_UNTRACKED = 64,

        /// <summary>
        ///  Turn on all finding features.
        /// </summary>
        GIT_DIFF_FIND_ALL = 255,

        /// <summary>
        ///  Measure similarity ignoring leading whitespace (default)
        /// </summary>
        GIT_DIFF_FIND_IGNORE_LEADING_WHITESPACE = 0,

        /// <summary>
        ///  Measure similarity ignoring all whitespace
        /// </summary>
        GIT_DIFF_FIND_IGNORE_WHITESPACE = 4096,

        /// <summary>
        ///  Measure similarity including all data
        /// </summary>
        GIT_DIFF_FIND_DONT_IGNORE_WHITESPACE = 8192,

        /// <summary>
        ///  Measure similarity only by comparing SHAs (fast and cheap)
        /// </summary>
        GIT_DIFF_FIND_EXACT_MATCH_ONLY = 16384,

        /// <summary>
        ///  Do not break rewrites unless they contribute to a rename.
        ///
        ///  <para>
        ///   Normally, GIT_DIFF_FIND_AND_BREAK_REWRITES will measure
        ///   the self- similarity of modified files and split the
        ///   ones that have changed a lot into a DELETE / ADD pair.
        ///   Then the sides of that pair will be considered
        ///   candidates for rename and copy detection.
        ///  </para>
        ///
        ///  <para>
        ///   If you add this flag in and the split pair is *not* used
        ///   for an actual rename or copy, then the modified record
        ///   will be restored to a regular MODIFIED record instead of
        ///   being split.
        ///  </para>
        /// </summary>
        GIT_DIFF_BREAK_REWRITES_FOR_RENAMES_ONLY = 32768,

        /// <summary>
        ///  Remove any UNMODIFIED deltas after find_similar is done.
        ///
        ///  <para>
        ///   Using GIT_DIFF_FIND_COPIES_FROM_UNMODIFIED to emulate
        ///   the --find-copies-harder behavior requires building a
        ///   diff with the GIT_DIFF_INCLUDE_UNMODIFIED flag. If you
        ///   do not want UNMODIFIED records in the final result, pass
        ///   this flag to have them removed.
        ///  </para>
        /// </summary>
        GIT_DIFF_FIND_REMOVE_UNMODIFIED = 65536,
    }

    /// <summary>
    ///  Pluggable similarity metric
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_diff_similarity_metric
    {
        public IntPtr file_signature;

        public IntPtr buffer_signature;

        public IntPtr free_signature;

        public IntPtr similarity;

        public IntPtr payload;
    }

    /// <summary>
    ///  Control behavior of rename and copy detection
    ///
    ///  <para>
    ///   These options mostly mimic parameters that can be passed to
    ///   git-diff.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_diff_find_options
    {
        public uint version;

        /// <summary>
        ///  Combination of git_diff_find_t values (default
        ///  GIT_DIFF_FIND_BY_CONFIG). NOTE: if you don't explicitly set
        ///  this, `diff.renames` could be set to false, resulting in
        ///  `git_diff_find_similar` doing nothing.
        /// </summary>
        public uint flags;

        /// <summary>
        ///  Threshold above which similar files will be considered
        ///  renames. This is equivalent to the -M option. Defaults to
        ///  50.
        /// </summary>
        public ushort rename_threshold;

        /// <summary>
        ///  Threshold below which similar files will be eligible to be a
        ///  rename source. This is equivalent to the first part of the
        ///  -B option. Defaults to 50.
        /// </summary>
        public ushort rename_from_rewrite_threshold;

        /// <summary>
        ///  Threshold above which similar files will be considered
        ///  copies. This is equivalent to the -C option. Defaults to 50.
        /// </summary>
        public ushort copy_threshold;

        /// <summary>
        ///  Threshold below which similar files will be split into a
        ///  delete/add pair. This is equivalent to the last part of the
        ///  -B option. Defaults to 60.
        /// </summary>
        public ushort break_rewrite_threshold;

        /// <summary>
        ///  Maximum number of matches to consider for a particular file.
        ///
        ///  <para>
        ///   This is a little different from the `-l` option from Git
        ///   because we will still process up to this many matches
        ///   before abandoning the search. Defaults to 1000.
        ///  </para>
        /// </summary>
        public UIntPtr rename_limit;

        /// <summary>
        ///  The `metric` option allows you to plug in a custom
        ///  similarity metric.
        ///
        ///  <para>
        ///   Set it to NULL to use the default internal metric.
        ///  </para>
        ///
        ///  <para>
        ///   The default metric is based on sampling hashes of ranges
        ///   of data in the file, which is a pretty good similarity
        ///   approximation that should work fairly well for both text
        ///   and binary data while still being pretty fast with a
        ///   fixed memory overhead.
        ///  </para>
        /// </summary>
        public IntPtr metric;
    }

    /// <summary>
    ///  Possible output formats for diff data
    /// </summary>
    public enum git_diff_format_t
    {
        /// <summary>
        ///  full git diff
        /// </summary>
        GIT_DIFF_FORMAT_PATCH = 1,

        /// <summary>
        ///  just the file headers of patch
        /// </summary>
        GIT_DIFF_FORMAT_PATCH_HEADER = 2,

        /// <summary>
        ///  like git diff --raw
        /// </summary>
        GIT_DIFF_FORMAT_RAW = 3,

        /// <summary>
        ///  like git diff --name-only
        /// </summary>
        GIT_DIFF_FORMAT_NAME_ONLY = 4,

        /// <summary>
        ///  like git diff --name-status
        /// </summary>
        GIT_DIFF_FORMAT_NAME_STATUS = 5,

        /// <summary>
        ///  git diff as used by git patch-id
        /// </summary>
        GIT_DIFF_FORMAT_PATCH_ID = 6,
    }

    /// <summary>
    ///  Options for parsing a diff / patch file.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_diff_parse_options
    {
        public uint version;

        /// <summary>
        ///  Object ID type used in the patch file.
        /// </summary>
        public git_oid_t oid_type;
    }

    /// <summary>
    ///  This is an opaque structure which is allocated by
    ///  `git_diff_get_stats`. You are responsible for releasing the
    ///  object memory when done, using the `git_diff_stats_free()`
    ///  function.
    /// </summary>
    public struct git_diff_stats
    {
    }

    /// <summary>
    ///  Formatting options for diff stats
    /// </summary>
    public enum git_diff_stats_format_t
    {
        /// <summary>
        ///  No stats
        /// </summary>
        GIT_DIFF_STATS_NONE = 0,

        /// <summary>
        ///  Full statistics, equivalent of `--stat`
        /// </summary>
        GIT_DIFF_STATS_FULL = 1,

        /// <summary>
        ///  Short statistics, equivalent of `--shortstat`
        /// </summary>
        GIT_DIFF_STATS_SHORT = 2,

        /// <summary>
        ///  Number statistics, equivalent of `--numstat`
        /// </summary>
        GIT_DIFF_STATS_NUMBER = 4,

        /// <summary>
        ///  Extended header information such as creations, renames and
        ///  mode changes, equivalent of `--summary`
        /// </summary>
        GIT_DIFF_STATS_INCLUDE_SUMMARY = 8,
    }

    /// <summary>
    ///  Patch ID options structure
    ///
    ///  <para>
    ///   Initialize with `GIT_PATCHID_OPTIONS_INIT`. Alternatively,
    ///   you can use `git_diff_patchid_options_init`.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_diff_patchid_options
    {
        public uint version;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Initialize git_diff_options structure
        ///
        ///  <para>
        ///   Initializes a `git_diff_options` with default values.
        ///   Equivalent to creating an instance with
        ///   GIT_DIFF_OPTIONS_INIT.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_diff_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass `GIT_DIFF_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_diff_options_init(
            ref git_diff_options opts,
            uint version);

        /// <summary>
        ///  Initialize git_diff_find_options structure
        ///
        ///  <para>
        ///   Initializes a `git_diff_find_options` with default
        ///   values. Equivalent to creating an instance with
        ///   GIT_DIFF_FIND_OPTIONS_INIT.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_diff_find_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass `GIT_DIFF_FIND_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_diff_find_options_init(
            ref git_diff_find_options opts,
            uint version);

        /// <summary>
        ///  Deallocate a diff.
        /// </summary>
        /// <param name="diff">
        ///  The previously created diff; cannot be used after free.
        /// </param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_diff_free(
            git_diff* diff);

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
            git_diff** diff,
            git_repository* repo,
            git_tree* old_tree,
            git_tree* new_tree,
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
            git_diff** diff,
            git_repository* repo,
            git_tree* old_tree,
            git_index* index,
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
            git_diff** diff,
            git_repository* repo,
            git_index* index,
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
            git_diff** diff,
            git_repository* repo,
            git_tree* old_tree,
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
            git_diff** diff,
            git_repository* repo,
            git_tree* old_tree,
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
            git_diff** diff,
            git_repository* repo,
            git_index* old_index,
            git_index* new_index,
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
            git_diff* onto,
            git_diff* from);

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
            git_diff* diff,
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
            git_diff* diff);

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
            git_diff* diff,
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
            git_diff* diff,
            UIntPtr idx);

        /// <summary>
        ///  Check if deltas are sorted case sensitively or
        ///  insensitively.
        /// </summary>
        /// <param name="diff">diff to check</param>
        /// <returns>0 if case sensitive, 1 if case is ignored</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_diff_is_sorted_icase(
            git_diff* diff);

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
            git_diff* diff,
            git_diff_file_cb file_cb,
            git_diff_binary_cb binary_cb,
            git_diff_hunk_cb hunk_cb,
            git_diff_line_cb line_cb,
            IntPtr payload);

        /// <summary>
        ///  Look up the single character abbreviation for a delta status
        ///  code.
        ///
        ///  <para>
        ///   When you run `git diff --name-status` it uses single
        ///   letter codes in the output such as 'A' for added, 'D'
        ///   for deleted, 'M' for modified, etc. This function
        ///   converts a git_delta_t value into these letters for your
        ///   own purposes. GIT_DELTA_UNTRACKED will return a space
        ///   (i.e. ' ').
        ///  </para>
        /// </summary>
        /// <param name="status">The git_delta_t value to look up</param>
        /// <returns>The single character label for that code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe char git_diff_status_char(
            git_delta_t status);

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
            git_diff* diff,
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
            git_diff* diff,
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
            git_blob* old_blob,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string old_as_path,
            git_blob* new_blob,
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
            git_blob* old_blob,
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
        ///  Directly run a diff between two buffers.
        ///
        ///  <para>
        ///   Even more than with `git_diff_blobs`, comparing two
        ///   buffer lacks context, so the `git_diff_file` parameters
        ///   to the callbacks will be faked a la the rules for
        ///   `git_diff_blobs()`.
        ///  </para>
        /// </summary>
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
        public static extern unsafe int git_diff_buffers(
            IntPtr old_buffer,
            UIntPtr old_len,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string old_as_path,
            IntPtr new_buffer,
            UIntPtr new_len,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string new_as_path,
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
            out git_diff* _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string content,
            UIntPtr content_len);

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
            out git_diff_stats* _out,
            git_diff* diff);

        /// <summary>
        ///  Get the total number of files changed in a diff
        /// </summary>
        /// <param name="stats">
        ///  A `git_diff_stats` generated by one of the above functions.
        /// </param>
        /// <returns>total number of files changed in the diff</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_diff_stats_files_changed(
            git_diff_stats* stats);

        /// <summary>
        ///  Get the total number of insertions in a diff
        /// </summary>
        /// <param name="stats">
        ///  A `git_diff_stats` generated by one of the above functions.
        /// </param>
        /// <returns>total number of insertions in the diff</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_diff_stats_insertions(
            git_diff_stats* stats);

        /// <summary>
        ///  Get the total number of deletions in a diff
        /// </summary>
        /// <param name="stats">
        ///  A `git_diff_stats` generated by one of the above functions.
        /// </param>
        /// <returns>total number of deletions in the diff</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_diff_stats_deletions(
            git_diff_stats* stats);

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
            git_diff_stats* stats,
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
            git_diff_stats* stats);

        /// <summary>
        ///  Initialize git_diff_patchid_options structure
        ///
        ///  <para>
        ///   Initializes a `git_diff_patchid_options` with default
        ///   values. Equivalent to creating an instance with
        ///   `GIT_DIFF_PATCHID_OPTIONS_INIT`.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_diff_patchid_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass `GIT_DIFF_PATCHID_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_diff_patchid_options_init(
            ref git_diff_patchid_options opts,
            uint version);

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
            git_diff* diff,
            ref git_diff_patchid_options opts);
    }
}
