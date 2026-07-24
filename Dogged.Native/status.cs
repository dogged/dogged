using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Status flags for a single file.
    ///
    ///  <para>
    ///   A combination of these values will be returned to indicate
    ///   the status of a file. Status compares the working directory,
    ///   the index, and the current HEAD of the repository. The
    ///   `GIT_STATUS_INDEX` set of flags represents the status of
    ///   file in the index relative to the HEAD, and the
    ///   `GIT_STATUS_WT` set of flags represent the status of the
    ///   file in the working directory relative to the index.
    ///  </para>
    /// </summary>
    public enum git_status_t
    {
        GIT_STATUS_CURRENT = 0,
        GIT_STATUS_INDEX_NEW = 1,
        GIT_STATUS_INDEX_MODIFIED = 2,
        GIT_STATUS_INDEX_DELETED = 4,
        GIT_STATUS_INDEX_RENAMED = 8,
        GIT_STATUS_INDEX_TYPECHANGE = 16,
        GIT_STATUS_WT_NEW = 128,
        GIT_STATUS_WT_MODIFIED = 256,
        GIT_STATUS_WT_DELETED = 512,
        GIT_STATUS_WT_TYPECHANGE = 1024,
        GIT_STATUS_WT_RENAMED = 2048,
        GIT_STATUS_WT_UNREADABLE = 4096,
        GIT_STATUS_IGNORED = 16384,
        GIT_STATUS_CONFLICTED = 32768,
    }

    /// <summary>
    ///  Function pointer to receive status on individual files
    /// </summary>
    /// <param name="path">is the path to the file</param>
    /// <param name="status_flags">
    ///  the `git_status_t` values for file's status
    /// </param>
    /// <param name="payload">
    ///  the user-specified payload to the foreach function
    /// </param>
    /// <returns>0 on success, or a negative number on failure</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_status_cb(
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
        uint status_flags,
        IntPtr payload);

    /// <summary>
    ///  Select the files on which to report status.
    ///
    ///  <para>
    ///   With `git_status_foreach_ext`, this will control which
    ///   changes get callbacks. With `git_status_list_new`, these
    ///   will control which changes are included in the list.
    ///  </para>
    /// </summary>
    public enum git_status_show_t
    {
        /// <summary>
        ///  The default. This roughly matches `git status --porcelain`
        ///  regarding which files are included and in what order.
        /// </summary>
        GIT_STATUS_SHOW_INDEX_AND_WORKDIR = 0,

        /// <summary>
        ///  Only gives status based on HEAD to index comparison, not
        ///  looking at working directory changes.
        /// </summary>
        GIT_STATUS_SHOW_INDEX_ONLY = 1,

        /// <summary>
        ///  Only gives status based on index to working directory
        ///  comparison, not comparing the index to the HEAD.
        /// </summary>
        GIT_STATUS_SHOW_WORKDIR_ONLY = 2,
    }

    /// <summary>
    ///  Flags to control status callbacks
    ///
    ///  <para>
    ///   Calling `git_status_foreach()` is like calling the extended
    ///   version with: GIT_STATUS_OPT_INCLUDE_IGNORED,
    ///   GIT_STATUS_OPT_INCLUDE_UNTRACKED, and
    ///   GIT_STATUS_OPT_RECURSE_UNTRACKED_DIRS. Those options are
    ///   bundled together as `GIT_STATUS_OPT_DEFAULTS` if you want
    ///   them as a baseline.
    ///  </para>
    /// </summary>
    public enum git_status_opt_t
    {
        /// <summary>
        ///  Says that callbacks should be made on untracked files. These
        ///  will only be made if the workdir files are included in the
        ///  status "show" option.
        /// </summary>
        GIT_STATUS_OPT_INCLUDE_UNTRACKED = 1,

        /// <summary>
        ///  Says that ignored files get callbacks. Again, these
        ///  callbacks will only be made if the workdir files are
        ///  included in the status "show" option.
        /// </summary>
        GIT_STATUS_OPT_INCLUDE_IGNORED = 2,

        /// <summary>
        ///  Indicates that callback should be made even on unmodified
        ///  files.
        /// </summary>
        GIT_STATUS_OPT_INCLUDE_UNMODIFIED = 4,

        /// <summary>
        ///  Indicates that submodules should be skipped. This only
        ///  applies if there are no pending typechanges to the submodule
        ///  (either from or to another type).
        /// </summary>
        GIT_STATUS_OPT_EXCLUDE_SUBMODULES = 8,

        /// <summary>
        ///  Indicates that all files in untracked directories should be
        ///  included. Normally if an entire directory is new, then just
        ///  the top-level directory is included (with a trailing slash
        ///  on the entry name). This flag says to include all of the
        ///  individual files in the directory instead.
        /// </summary>
        GIT_STATUS_OPT_RECURSE_UNTRACKED_DIRS = 16,

        /// <summary>
        ///  Indicates that the given path should be treated as a literal
        ///  path, and not as a pathspec pattern.
        /// </summary>
        GIT_STATUS_OPT_DISABLE_PATHSPEC_MATCH = 32,

        /// <summary>
        ///  Indicates that the contents of ignored directories should be
        ///  included in the status. This is like doing `git ls-files -o
        ///  -i --exclude-standard` with core git.
        /// </summary>
        GIT_STATUS_OPT_RECURSE_IGNORED_DIRS = 64,

        /// <summary>
        ///  Indicates that rename detection should be processed between
        ///  the head and the index and enables the
        ///  GIT_STATUS_INDEX_RENAMED as a possible status flag.
        /// </summary>
        GIT_STATUS_OPT_RENAMES_HEAD_TO_INDEX = 128,

        /// <summary>
        ///  Indicates that rename detection should be run between the
        ///  index and the working directory and enabled
        ///  GIT_STATUS_WT_RENAMED as a possible status flag.
        /// </summary>
        GIT_STATUS_OPT_RENAMES_INDEX_TO_WORKDIR = 256,

        /// <summary>
        ///  Overrides the native case sensitivity for the file system
        ///  and forces the output to be in case-sensitive order.
        /// </summary>
        GIT_STATUS_OPT_SORT_CASE_SENSITIVELY = 512,

        /// <summary>
        ///  Overrides the native case sensitivity for the file system
        ///  and forces the output to be in case-insensitive order.
        /// </summary>
        GIT_STATUS_OPT_SORT_CASE_INSENSITIVELY = 1024,

        /// <summary>
        ///  Iindicates that rename detection should include rewritten
        ///  files.
        /// </summary>
        GIT_STATUS_OPT_RENAMES_FROM_REWRITES = 2048,

        /// <summary>
        ///  Bypasses the default status behavior of doing a "soft" index
        ///  reload (i.e. reloading the index data if the file on disk
        ///  has been modified outside libgit2).
        /// </summary>
        GIT_STATUS_OPT_NO_REFRESH = 4096,

        /// <summary>
        ///  Tells libgit2 to refresh the stat cache in the index for
        ///  files that are unchanged but have out of date stat
        ///  einformation in the index. It will result in less work being
        ///  done on subsequent calls to get status. This is mutually
        ///  exclusive with the NO_REFRESH option.
        /// </summary>
        GIT_STATUS_OPT_UPDATE_INDEX = 8192,

        /// <summary>
        ///  Normally files that cannot be opened or read are ignored as
        ///  these are often transient files; this option will return
        ///  unreadable files as `GIT_STATUS_WT_UNREADABLE`.
        /// </summary>
        GIT_STATUS_OPT_INCLUDE_UNREADABLE = 16384,

        /// <summary>
        ///  Unreadable files will be detected and given the status
        ///  untracked instead of unreadable.
        /// </summary>
        GIT_STATUS_OPT_INCLUDE_UNREADABLE_AS_UNTRACKED = 32768,
    }

    /// <summary>
    ///  Options to control how `git_status_foreach_ext()` will issue
    ///  callbacks.
    ///
    ///  <para>
    ///   Initialize with `GIT_STATUS_OPTIONS_INIT`. Alternatively,
    ///   you can use `git_status_options_init`.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_status_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_STATUS_OPTIONS_VERSION = 1;


        /// <summary>
        ///  The struct version; pass `GIT_STATUS_OPTIONS_VERSION`.
        /// </summary>
        public uint version;

        /// <summary>
        ///  The `show` value is one of the `git_status_show_t` constants
        ///  that control which files to scan and in what order. The
        ///  default is `GIT_STATUS_SHOW_INDEX_AND_WORKDIR`.
        /// </summary>
        public git_status_show_t show;

        /// <summary>
        ///  The `flags` value is an OR'ed combination of the
        ///  `git_status_opt_t` values above. The default is
        ///  `GIT_STATUS_OPT_DEFAULTS`, which matches git's default
        ///  behavior.
        /// </summary>
        public uint flags;

        /// <summary>
        ///  The `pathspec` is an array of path patterns to match (using
        ///  fnmatch-style matching), or just an array of paths to match
        ///  exactly if `GIT_STATUS_OPT_DISABLE_PATHSPEC_MATCH` is
        ///  specified in the flags.
        /// </summary>
        public git_strarray pathspec;

        /// <summary>
        ///  The `baseline` is the tree to be used for comparison to the
        ///  working directory and index; defaults to HEAD.
        /// </summary>
        public IntPtr baseline;

        /// <summary>
        ///  Threshold above which similar files will be considered
        ///  renames. This is equivalent to the -M option. Defaults to
        ///  50.
        /// </summary>
        public ushort rename_threshold;
    }

    /// <summary>
    ///  A status entry, providing the differences between the file as it
    ///  exists in HEAD and the index, and providing the differences
    ///  between the index and the working directory.
    ///
    ///  <para>
    ///   The `status` value provides the status flags for this file.
    ///  </para>
    ///
    ///  <para>
    ///   The `head_to_index` value provides detailed information
    ///   about the differences between the file in HEAD and the file
    ///   in the index.
    ///  </para>
    ///
    ///  <para>
    ///   The `index_to_workdir` value provides detailed information
    ///   about the differences between the file in the index and the
    ///   file in the working directory.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_status_entry
    {
        public git_status_t status;

        public IntPtr head_to_index;

        public IntPtr index_to_workdir;
    }

    /// <summary>
    ///  Representation of a status collection
    /// </summary>
    public struct git_status_list
    {
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Initialize git_status_options structure
        ///
        ///  <para>
        ///   Initializes a `git_status_options` with default values.
        ///   Equivalent to creating an instance with
        ///   `GIT_STATUS_OPTIONS_INIT`.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_status_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass `GIT_STATUS_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_status_options_init(
            ref git_status_options opts,
            uint version);

        /// <summary>
        ///  Gather file statuses and run a callback for each one.
        ///
        ///  <para>
        ///   The callback is passed the path of the file, the status
        ///   (a combination of the `git_status_t` values above) and
        ///   the `payload` data pointer passed into this function.
        ///  </para>
        ///
        ///  <para>
        ///   If the callback returns a non-zero value, this function
        ///   will stop looping and return that value to caller.
        ///  </para>
        /// </summary>
        /// <param name="repo">A repository object</param>
        /// <param name="callback">The function to call on each file</param>
        /// <param name="payload">
        ///  Pointer to pass through to callback function
        /// </param>
        /// <returns>
        ///  0 on success, non-zero callback return value, or error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_status_foreach(
            git_repository* repo,
            git_status_cb callback,
            IntPtr payload);

        /// <summary>
        ///  Gather file status information and run callbacks as
        ///  requested.
        ///
        ///  <para>
        ///   This is an extended version of the
        ///   `git_status_foreach()` API that allows for more granular
        ///   control over which paths will be processed and in what
        ///   order. See the `git_status_options` structure for
        ///   details about the additional controls that this makes
        ///   available.
        ///  </para>
        ///
        ///  <para>
        ///   Note that if a `pathspec` is given in the
        ///   `git_status_options` to filter the status, then the
        ///   results from rename detection (if you enable it) may not
        ///   be accurate. To do rename detection properly, this must
        ///   be called with no `pathspec` so that all files can be
        ///   considered.
        ///  </para>
        /// </summary>
        /// <param name="repo">Repository object</param>
        /// <param name="opts">Status options structure</param>
        /// <param name="callback">The function to call on each file</param>
        /// <param name="payload">
        ///  Pointer to pass through to callback function
        /// </param>
        /// <returns>
        ///  0 on success, non-zero callback return value, or error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_status_foreach_ext(
            git_repository* repo,
            ref git_status_options opts,
            git_status_cb callback,
            IntPtr payload);

        /// <summary>
        ///  Get file status for a single file.
        ///
        ///  <para>
        ///   This tries to get status for the filename that you give.
        ///   If no files match that name (in either the HEAD, index,
        ///   or working directory), this returns GIT_ENOTFOUND.
        ///  </para>
        ///
        ///  <para>
        ///   If the name matches multiple files (for example, if the
        ///   `path` names a directory or if running on a case-
        ///   insensitive filesystem and yet the HEAD has two entries
        ///   that both match the path), then this returns
        ///   GIT_EAMBIGUOUS because it cannot give correct results.
        ///  </para>
        ///
        ///  <para>
        ///   This does not do any sort of rename detection. Renames
        ///   require a set of targets and because of the path
        ///   filtering, there is not enough information to check
        ///   renames correctly. To check file status with rename
        ///   detection, there is no choice but to do a full
        ///   `git_status_list_new` and scan through looking for the
        ///   path that you are interested in.
        ///  </para>
        /// </summary>
        /// <param name="status_flags">
        ///  Output combination of git_status_t values for file
        /// </param>
        /// <param name="repo">A repository object</param>
        /// <param name="path">
        ///  The exact path to retrieve status for relative to the
        ///  repository working directory
        /// </param>
        /// <returns>
        ///  0 on success, GIT_ENOTFOUND if the file is not found in the
        ///  HEAD, index, and work tree, GIT_EAMBIGUOUS if `path` matches
        ///  multiple files or if it refers to a folder, and -1 on other
        ///  errors.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_status_file(
            uint* status_flags,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path);

        /// <summary>
        ///  Gather file status information and populate the
        ///  `git_status_list`.
        ///
        ///  <para>
        ///   Note that if a `pathspec` is given in the
        ///   `git_status_options` to filter the status, then the
        ///   results from rename detection (if you enable it) may not
        ///   be accurate. To do rename detection properly, this must
        ///   be called with no `pathspec` so that all files can be
        ///   considered.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Pointer to store the status results in
        /// </param>
        /// <param name="repo">Repository object</param>
        /// <param name="opts">Status options structure</param>
        /// <returns>0 on success or error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_status_list_new(
            out git_status_list* _out,
            git_repository* repo,
            ref git_status_options opts);

        /// <summary>
        ///  Gets the count of status entries in this list.
        ///
        ///  <para>
        ///   If there are no changes in status (at least according
        ///   the options given when the status list was created),
        ///   this can return 0.
        ///  </para>
        /// </summary>
        /// <param name="statuslist">Existing status list object</param>
        /// <returns>the number of status entries</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_status_list_entrycount(
            git_status_list* statuslist);

        /// <summary>
        ///  Get a pointer to one of the entries in the status list.
        ///
        ///  <para>
        ///   The entry is not modifiable and should not be freed.
        ///  </para>
        /// </summary>
        /// <param name="statuslist">Existing status list object</param>
        /// <param name="idx">Position of the entry</param>
        /// <returns>Pointer to the entry; NULL if out of bounds</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_status_entry* git_status_byindex(
            git_status_list* statuslist,
            UIntPtr idx);

        /// <summary>
        ///  Free an existing status list
        /// </summary>
        /// <param name="statuslist">Existing status list object</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_status_list_free(
            git_status_list* statuslist);

        /// <summary>
        ///  Test if the ignore rules apply to a given file.
        ///
        ///  <para>
        ///   This function checks the ignore rules to see if they
        ///   would apply to the given file. This indicates if the
        ///   file would be ignored regardless of whether the file is
        ///   already in the index or committed to the repository.
        ///  </para>
        ///
        ///  <para>
        ///   One way to think of this is if you were to do "git add
        ///   ." on the directory containing the file, would it be
        ///   added or not?
        ///  </para>
        /// </summary>
        /// <param name="ignored">
        ///  Boolean returning 0 if the file is not ignored, 1 if it is
        /// </param>
        /// <param name="repo">A repository object</param>
        /// <param name="path">
        ///  The file to check ignores for, rooted at the repo's workdir.
        /// </param>
        /// <returns>
        ///  0 if ignore rules could be processed for the file
        ///  (regardless of whether it exists or not), or an error < 0 if
        ///  they could not.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_status_should_ignore(
            int* ignored,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path);
    }
}
