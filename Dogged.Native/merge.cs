using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  The file inputs to `git_merge_file`. Callers should populate the
    ///  `git_merge_file_input` structure with descriptions of the files
    ///  in each side of the conflict for use in producing the merge
    ///  file.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_merge_file_input
    {
        public uint version;

        /// <summary>
        ///  Pointer to the contents of the file.
        /// </summary>
        public IntPtr ptr;

        /// <summary>
        ///  Size of the contents pointed to in `ptr`.
        /// </summary>
        public UIntPtr size;

        /// <summary>
        ///  File name of the conflicted file, or `NULL` to not merge the
        ///  path.
        /// </summary>
        public IntPtr path;

        /// <summary>
        ///  File mode of the conflicted file, or `0` to not merge the
        ///  mode.
        /// </summary>
        public uint mode;
    }

    /// <summary>
    ///  Flags for `git_merge` options. A combination of these flags can
    ///  be passed in via the `flags` value in the `git_merge_options`.
    /// </summary>
    public enum git_merge_flag_t
    {
        /// <summary>
        ///  Detect renames that occur between the common ancestor and
        ///  the "ours" side or the common ancestor and the "theirs"
        ///  side. This will enable the ability to merge between a
        ///  modified and renamed file.
        /// </summary>
        GIT_MERGE_FIND_RENAMES = 1,

        /// <summary>
        ///  If a conflict occurs, exit immediately instead of attempting
        ///  to continue resolving conflicts. The merge operation will
        ///  fail with GIT_EMERGECONFLICT and no index will be returned.
        /// </summary>
        GIT_MERGE_FAIL_ON_CONFLICT = 2,

        /// <summary>
        ///  Do not write the REUC extension on the generated index
        /// </summary>
        GIT_MERGE_SKIP_REUC = 4,

        /// <summary>
        ///  If the commits being merged have multiple merge bases, do
        ///  not build a recursive merge base (by merging the multiple
        ///  merge bases), instead simply use the first base. This flag
        ///  provides a similar merge base to `git-merge-resolve`.
        /// </summary>
        GIT_MERGE_NO_RECURSIVE = 8,

        /// <summary>
        ///  Treat this merge as if it is to produce the virtual base of
        ///  a recursive merge. This will ensure that there are no
        ///  conflicts, any conflicting regions will keep conflict
        ///  markers in the merge result.
        /// </summary>
        GIT_MERGE_VIRTUAL_BASE = 16,
    }

    /// <summary>
    ///  Merge file favor options for `git_merge_options` instruct the
    ///  file-level merging functionality how to deal with conflicting
    ///  regions of the files.
    /// </summary>
    public enum git_merge_file_favor_t
    {
        /// <summary>
        ///  When a region of a file is changed in both branches, a
        ///  conflict will be recorded in the index so that
        ///  `git_checkout` can produce a merge file with conflict
        ///  markers in the working directory. This is the default.
        /// </summary>
        GIT_MERGE_FILE_FAVOR_NORMAL = 0,

        /// <summary>
        ///  When a region of a file is changed in both branches, the
        ///  file created in the index will contain the "ours" side of
        ///  any conflicting region. The index will not record a
        ///  conflict.
        /// </summary>
        GIT_MERGE_FILE_FAVOR_OURS = 1,

        /// <summary>
        ///  When a region of a file is changed in both branches, the
        ///  file created in the index will contain the "theirs" side of
        ///  any conflicting region. The index will not record a
        ///  conflict.
        /// </summary>
        GIT_MERGE_FILE_FAVOR_THEIRS = 2,

        /// <summary>
        ///  When a region of a file is changed in both branches, the
        ///  file created in the index will contain each unique line from
        ///  each side, which has the result of combining both files. The
        ///  index will not record a conflict.
        /// </summary>
        GIT_MERGE_FILE_FAVOR_UNION = 3,
    }

    /// <summary>
    ///  File merging flags
    /// </summary>
    public enum git_merge_file_flag_t
    {
        /// <summary>
        ///  Defaults
        /// </summary>
        GIT_MERGE_FILE_DEFAULT = 0,

        /// <summary>
        ///  Create standard conflicted merge files
        /// </summary>
        GIT_MERGE_FILE_STYLE_MERGE = 1,

        /// <summary>
        ///  Create diff3-style files
        /// </summary>
        GIT_MERGE_FILE_STYLE_DIFF3 = 2,

        /// <summary>
        ///  Condense non-alphanumeric regions for simplified diff file
        /// </summary>
        GIT_MERGE_FILE_SIMPLIFY_ALNUM = 4,

        /// <summary>
        ///  Ignore all whitespace
        /// </summary>
        GIT_MERGE_FILE_IGNORE_WHITESPACE = 8,

        /// <summary>
        ///  Ignore changes in amount of whitespace
        /// </summary>
        GIT_MERGE_FILE_IGNORE_WHITESPACE_CHANGE = 16,

        /// <summary>
        ///  Ignore whitespace at end of line
        /// </summary>
        GIT_MERGE_FILE_IGNORE_WHITESPACE_EOL = 32,

        /// <summary>
        ///  Use the "patience diff" algorithm
        /// </summary>
        GIT_MERGE_FILE_DIFF_PATIENCE = 64,

        /// <summary>
        ///  Take extra time to find minimal diff
        /// </summary>
        GIT_MERGE_FILE_DIFF_MINIMAL = 128,

        /// <summary>
        ///  Create zdiff3 ("zealous diff3")-style files
        /// </summary>
        GIT_MERGE_FILE_STYLE_ZDIFF3 = 256,

        /// <summary>
        ///  Do not produce file conflicts when common regions have
        ///  changed; keep the conflict markers in the file and accept
        ///  that as the merge result.
        /// </summary>
        GIT_MERGE_FILE_ACCEPT_CONFLICTS = 512,
    }

    /// <summary>
    ///  Options for merging a file
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_merge_file_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_MERGE_FILE_OPTIONS_VERSION = 1;


        public uint version;

        /// <summary>
        ///  Label for the ancestor file side of the conflict which will
        ///  be prepended to labels in diff3-format merge files.
        /// </summary>
        public IntPtr ancestor_label;

        /// <summary>
        ///  Label for our file side of the conflict which will be
        ///  prepended to labels in merge files.
        /// </summary>
        public IntPtr our_label;

        /// <summary>
        ///  Label for their file side of the conflict which will be
        ///  prepended to labels in merge files.
        /// </summary>
        public IntPtr their_label;

        /// <summary>
        ///  The file to favor in region conflicts.
        /// </summary>
        public git_merge_file_favor_t favor;

        /// <summary>
        ///  see `git_merge_file_flag_t` above
        /// </summary>
        public uint flags;

        /// <summary>
        ///  The size of conflict markers (eg, "<<<<<<<"). Default is
        ///  GIT_MERGE_CONFLICT_MARKER_SIZE.
        /// </summary>
        public ushort marker_size;
    }

    /// <summary>
    ///  Information about file-level merging
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_merge_file_result
    {
        /// <summary>
        ///  True if the output was automerged, false if the output
        ///  contains conflict markers.
        /// </summary>
        public uint automergeable;

        /// <summary>
        ///  The path that the resultant merge file should use, or NULL
        ///  if a filename conflict would occur.
        /// </summary>
        public IntPtr path;

        /// <summary>
        ///  The mode that the resultant merge file should use.
        /// </summary>
        public uint mode;

        /// <summary>
        ///  The contents of the merge.
        /// </summary>
        public IntPtr ptr;

        /// <summary>
        ///  The length of the merge contents.
        /// </summary>
        public UIntPtr len;
    }

    /// <summary>
    ///  Merging options
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_merge_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_MERGE_OPTIONS_VERSION = 1;


        public uint version;

        /// <summary>
        ///  See `git_merge_flag_t` above
        /// </summary>
        public uint flags;

        /// <summary>
        ///  Similarity to consider a file renamed (default 50). If
        ///  `GIT_MERGE_FIND_RENAMES` is enabled, added files will be
        ///  compared with deleted files to determine their similarity.
        ///  Files that are more similar than the rename threshold
        ///  (percentage-wise) will be treated as a rename.
        /// </summary>
        public uint rename_threshold;

        /// <summary>
        ///  Maximum similarity sources to examine for renames (default
        ///  200). If the number of rename candidates (add / delete
        ///  pairs) is greater than this value, inexact rename detection
        ///  is aborted.
        ///
        ///  <para>
        ///   This setting overrides the `merge.renameLimit`
        ///   configuration value.
        ///  </para>
        /// </summary>
        public uint target_limit;

        /// <summary>
        ///  Pluggable similarity metric; pass NULL to use internal
        ///  metric
        /// </summary>
        public IntPtr metric;

        /// <summary>
        ///  Maximum number of times to merge common ancestors to build a
        ///  virtual merge base when faced with criss-cross merges. When
        ///  this limit is reached, the next ancestor will simply be used
        ///  instead of attempting to merge it. The default is unlimited.
        /// </summary>
        public uint recursion_limit;

        /// <summary>
        ///  Default merge driver to be used when both sides of a merge
        ///  have changed. The default is the `text` driver.
        /// </summary>
        public IntPtr default_driver;

        /// <summary>
        ///  Flags for handling conflicting content, to be used with the
        ///  standard (`text`) merge driver.
        /// </summary>
        public git_merge_file_favor_t file_favor;

        /// <summary>
        ///  see `git_merge_file_flag_t` above
        /// </summary>
        public uint file_flags;
    }

    /// <summary>
    ///  The results of `git_merge_analysis` indicate the merge
    ///  opportunities.
    /// </summary>
    public enum git_merge_analysis_t
    {
        /// <summary>
        ///  No merge is possible. (Unused.)
        /// </summary>
        GIT_MERGE_ANALYSIS_NONE = 0,

        /// <summary>
        ///  A "normal" merge; both HEAD and the given merge input have
        ///  diverged from their common ancestor. The divergent commits
        ///  must be merged.
        /// </summary>
        GIT_MERGE_ANALYSIS_NORMAL = 1,

        /// <summary>
        ///  All given merge inputs are reachable from HEAD, meaning the
        ///  repository is up-to-date and no merge needs to be performed.
        /// </summary>
        GIT_MERGE_ANALYSIS_UP_TO_DATE = 2,

        /// <summary>
        ///  The given merge input is a fast-forward from HEAD and no
        ///  merge needs to be performed. Instead, the client can check
        ///  out the given merge input.
        /// </summary>
        GIT_MERGE_ANALYSIS_FASTFORWARD = 4,

        /// <summary>
        ///  The HEAD of the current repository is "unborn" and does not
        ///  point to a valid commit. No merge can be performed, but the
        ///  caller may wish to simply set HEAD to the target commit(s).
        /// </summary>
        GIT_MERGE_ANALYSIS_UNBORN = 8,
    }

    /// <summary>
    ///  The user's stated preference for merges.
    /// </summary>
    public enum git_merge_preference_t
    {
        /// <summary>
        ///  No configuration was found that suggests a preferred
        ///  behavior for merge.
        /// </summary>
        GIT_MERGE_PREFERENCE_NONE = 0,

        /// <summary>
        ///  There is a `merge.ff=false` configuration setting,
        ///  suggesting that the user does not want to allow a
        ///  fast-forward merge.
        /// </summary>
        GIT_MERGE_PREFERENCE_NO_FASTFORWARD = 1,

        /// <summary>
        ///  There is a `merge.ff=only` configuration setting, suggesting
        ///  that the user only wants fast-forward merges.
        /// </summary>
        GIT_MERGE_PREFERENCE_FASTFORWARD_ONLY = 2,
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Initializes a `git_merge_file_input` with default values.
        ///  Equivalent to creating an instance with
        ///  GIT_MERGE_FILE_INPUT_INIT.
        /// </summary>
        /// <param name="opts">
        ///  the `git_merge_file_input` instance to initialize.
        /// </param>
        /// <param name="version">
        ///  the version of the struct; you should pass
        ///  `GIT_MERGE_FILE_INPUT_VERSION` here.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_merge_file_input_init(
            ref git_merge_file_input opts,
            uint version);

        /// <summary>
        ///  Initialize git_merge_file_options structure
        ///
        ///  <para>
        ///   Initializes a `git_merge_file_options` with default
        ///   values. Equivalent to creating an instance with
        ///   `GIT_MERGE_FILE_OPTIONS_INIT`.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_merge_file_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass `GIT_MERGE_FILE_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_merge_file_options_init(
            ref git_merge_file_options opts,
            uint version);

        /// <summary>
        ///  Initialize git_merge_options structure
        ///
        ///  <para>
        ///   Initializes a `git_merge_options` with default values.
        ///   Equivalent to creating an instance with
        ///   `GIT_MERGE_OPTIONS_INIT`.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_merge_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass `GIT_MERGE_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_merge_options_init(
            ref git_merge_options opts,
            uint version);

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
            git_repository* repo,
            git_annotated_commit** their_heads,
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
            git_repository* repo,
            git_reference* our_ref,
            git_annotated_commit** their_heads,
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
            git_repository* repo,
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
            git_repository* repo,
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
            git_repository* repo,
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
            git_repository* repo,
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
            git_repository* repo,
            UIntPtr length,
            ref git_oid input_array);

        /// <summary>
        ///  Merge two files as they exist in the in-memory data
        ///  structures, using the given common ancestor as the baseline,
        ///  producing a `git_merge_file_result` that reflects the merge
        ///  result. The `git_merge_file_result` must be freed with
        ///  `git_merge_file_result_free`.
        ///
        ///  <para>
        ///   Note that this function does not reference a repository
        ///   and any configuration must be passed as
        ///   `git_merge_file_options`.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  The git_merge_file_result to be filled in
        /// </param>
        /// <param name="ancestor">The contents of the ancestor file</param>
        /// <param name="ours">
        ///  The contents of the file in "our" side
        /// </param>
        /// <param name="theirs">
        ///  The contents of the file in "their" side
        /// </param>
        /// <param name="opts">
        ///  The merge file options or `NULL` for defaults
        /// </param>
        /// <returns>0 on success or error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_merge_file(
            ref git_merge_file_result _out,
            ref git_merge_file_input ancestor,
            ref git_merge_file_input ours,
            ref git_merge_file_input theirs,
            ref git_merge_file_options opts);

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
            git_repository* repo,
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
            ref git_merge_file_result result);

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
            out git_index* _out,
            git_repository* repo,
            git_tree* ancestor_tree,
            git_tree* our_tree,
            git_tree* their_tree,
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
            out git_index* _out,
            git_repository* repo,
            git_commit* our_commit,
            git_commit* their_commit,
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
            git_repository* repo,
            git_annotated_commit** their_heads,
            UIntPtr their_heads_len,
            ref git_merge_options merge_opts,
            ref git_checkout_options checkout_opts);
    }
}
