using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Checkout behavior flags
    ///
    ///  <para>
    ///   In libgit2, checkout is used to update the working directory
    ///   and index to match a target tree. Unlike git checkout, it
    ///   does not move the HEAD commit for you - use
    ///   `git_repository_set_head` or the like to do that.
    ///  </para>
    ///
    ///  <para>
    ///   Checkout looks at (up to) four things: the "target" tree you
    ///   want to check out, the "baseline" tree of what was checked
    ///   out previously, the working directory for actual files, and
    ///   the index for staged changes.
    ///  </para>
    ///
    ///  <para>
    ///   You give checkout one of two strategies for update:
    ///  </para>
    ///
    ///  <para>
    ///   - `GIT_CHECKOUT_SAFE` is the default, and similar to git's
    ///   default, which will make modifications that will not lose
    ///   changes in the working directory.
    ///  </para>
    ///
    ///  <para>
    ///   | target == baseline | target != baseline |
    ///   ---------------------|-----------------------|----------------------|
    ///   workdir == baseline | no action | create, update, or | | |
    ///   delete file |
    ///   ---------------------|-----------------------|----------------------|
    ///   workdir exists and | no action | conflict (notify | is !=
    ///   baseline | notify dirty MODIFIED | and cancel checkout) |
    ///   ---------------------|-----------------------|----------------------|
    ///   workdir missing, | notify dirty DELETED | create file |
    ///   baseline present | | |
    ///   ---------------------|-----------------------|----------------------|
    ///  </para>
    ///
    ///  <para>
    ///   - `GIT_CHECKOUT_FORCE` will take any action to make the
    ///   working directory match the target (including potentially
    ///   discarding modified files).
    ///  </para>
    ///
    ///  <para>
    ///   To emulate `git checkout`, use `GIT_CHECKOUT_SAFE` with a
    ///   checkout notification callback (see below) that displays
    ///   information about dirty files. The default behavior will
    ///   cancel checkout on conflicts.
    ///  </para>
    ///
    ///  <para>
    ///   To emulate `git checkout-index`, use `GIT_CHECKOUT_SAFE`
    ///   with a notification callback that cancels the operation if a
    ///   dirty-but-existing file is found in the working directory.
    ///   This core git command isn't quite "force" but is sensitive
    ///   about some types of changes.
    ///  </para>
    ///
    ///  <para>
    ///   To emulate `git checkout -f`, use `GIT_CHECKOUT_FORCE`.
    ///  </para>
    ///
    ///  <para>
    ///   There are some additional flags to modify the behavior of
    ///   checkout:
    ///  </para>
    ///
    ///  <para>
    ///   - `GIT_CHECKOUT_DRY_RUN` is a dry-run strategy that checks
    ///   for conflicts, etc., but doesn't make any actual changes.
    ///  </para>
    ///
    ///  <para>
    ///   - GIT_CHECKOUT_ALLOW_CONFLICTS makes SAFE mode apply safe
    ///   file updates even if there are conflicts (instead of
    ///   cancelling the checkout).
    ///  </para>
    ///
    ///  <para>
    ///   - GIT_CHECKOUT_REMOVE_UNTRACKED means remove untracked files
    ///   (i.e. not in target, baseline, or index, and not ignored)
    ///   from the working dir.
    ///  </para>
    ///
    ///  <para>
    ///   - GIT_CHECKOUT_REMOVE_IGNORED means remove ignored files
    ///   (that are also untracked) from the working directory as
    ///   well.
    ///  </para>
    ///
    ///  <para>
    ///   - GIT_CHECKOUT_UPDATE_ONLY means to only update the content
    ///   of files that already exist. Files will not be created nor
    ///   deleted. This just skips applying adds, deletes, and
    ///   typechanges.
    ///  </para>
    ///
    ///  <para>
    ///   - GIT_CHECKOUT_DONT_UPDATE_INDEX prevents checkout from
    ///   writing the updated files' information to the index.
    ///  </para>
    ///
    ///  <para>
    ///   - Normally, checkout will reload the index and git
    ///   attributes from disk before any operations.
    ///   GIT_CHECKOUT_NO_REFRESH prevents this reload.
    ///  </para>
    ///
    ///  <para>
    ///   - Unmerged index entries are conflicts.
    ///   GIT_CHECKOUT_SKIP_UNMERGED skips files with unmerged index
    ///   entries instead. GIT_CHECKOUT_USE_OURS and
    ///   GIT_CHECKOUT_USE_THEIRS to proceed with the checkout using
    ///   either the stage 2 ("ours") or stage 3 ("theirs") version of
    ///   files in the index.
    ///  </para>
    ///
    ///  <para>
    ///   - GIT_CHECKOUT_DONT_OVERWRITE_IGNORED prevents ignored files
    ///   from being overwritten. Normally, files that are ignored in
    ///   the working directory are not considered "precious" and may
    ///   be overwritten if the checkout target contains that file.
    ///  </para>
    ///
    ///  <para>
    ///   - GIT_CHECKOUT_DONT_REMOVE_EXISTING prevents checkout from
    ///   removing files or folders that fold to the same name on case
    ///   insensitive filesystems. This can cause files to retain
    ///   their existing names and write through existing symbolic
    ///   links.
    ///  </para>
    /// </summary>
    [Flags]
    public enum git_checkout_strategy_t
    {
        /// <summary>
        ///  Allow safe updates that cannot overwrite uncommitted data.
        ///  If the uncommitted changes don't conflict with the checked
        ///  out files, the checkout will still proceed, leaving the
        ///  changes intact.
        /// </summary>
        GIT_CHECKOUT_SAFE = 0,

        /// <summary>
        ///  Allow all updates to force working directory to look like
        ///  the index, potentially losing data in the process.
        /// </summary>
        GIT_CHECKOUT_FORCE = 2,

        /// <summary>
        ///  Allow checkout to recreate missing files
        /// </summary>
        GIT_CHECKOUT_RECREATE_MISSING = 4,

        /// <summary>
        ///  Allow checkout to make safe updates even if conflicts are
        ///  found
        /// </summary>
        GIT_CHECKOUT_ALLOW_CONFLICTS = 16,

        /// <summary>
        ///  Remove untracked files not in index (that are not ignored)
        /// </summary>
        GIT_CHECKOUT_REMOVE_UNTRACKED = 32,

        /// <summary>
        ///  Remove ignored files not in index
        /// </summary>
        GIT_CHECKOUT_REMOVE_IGNORED = 64,

        /// <summary>
        ///  Only update existing files, don't create new ones
        /// </summary>
        GIT_CHECKOUT_UPDATE_ONLY = 128,

        /// <summary>
        ///  Normally checkout updates index entries as it goes; this
        ///  stops that. Implies `GIT_CHECKOUT_DONT_WRITE_INDEX`.
        /// </summary>
        GIT_CHECKOUT_DONT_UPDATE_INDEX = 256,

        /// <summary>
        ///  Don't refresh index/config/etc before doing checkout
        /// </summary>
        GIT_CHECKOUT_NO_REFRESH = 512,

        /// <summary>
        ///  Allow checkout to skip unmerged files
        /// </summary>
        GIT_CHECKOUT_SKIP_UNMERGED = 1024,

        /// <summary>
        ///  For unmerged files, checkout stage 2 from index
        /// </summary>
        GIT_CHECKOUT_USE_OURS = 2048,

        /// <summary>
        ///  For unmerged files, checkout stage 3 from index
        /// </summary>
        GIT_CHECKOUT_USE_THEIRS = 4096,

        /// <summary>
        ///  Treat pathspec as simple list of exact match file paths
        /// </summary>
        GIT_CHECKOUT_DISABLE_PATHSPEC_MATCH = 8192,

        /// <summary>
        ///  Ignore directories in use, they will be left empty
        /// </summary>
        GIT_CHECKOUT_SKIP_LOCKED_DIRECTORIES = 262144,

        /// <summary>
        ///  Don't overwrite ignored files that exist in the checkout
        ///  target
        /// </summary>
        GIT_CHECKOUT_DONT_OVERWRITE_IGNORED = 524288,

        /// <summary>
        ///  Write normal merge files for conflicts
        /// </summary>
        GIT_CHECKOUT_CONFLICT_STYLE_MERGE = 1048576,

        /// <summary>
        ///  Include common ancestor data in diff3 format files for
        ///  conflicts
        /// </summary>
        GIT_CHECKOUT_CONFLICT_STYLE_DIFF3 = 2097152,

        /// <summary>
        ///  Don't overwrite existing files or folders
        /// </summary>
        GIT_CHECKOUT_DONT_REMOVE_EXISTING = 4194304,

        /// <summary>
        ///  Normally checkout writes the index upon completion; this
        ///  prevents that.
        /// </summary>
        GIT_CHECKOUT_DONT_WRITE_INDEX = 8388608,

        /// <summary>
        ///  Perform a "dry run", reporting what _would_ be done but
        ///  without actually making changes in the working directory or
        ///  the index.
        /// </summary>
        GIT_CHECKOUT_DRY_RUN = 16777216,

        /// <summary>
        ///  Include common ancestor data in zdiff3 format for conflicts
        /// </summary>
        GIT_CHECKOUT_CONFLICT_STYLE_ZDIFF3 = 33554432,

        /// <summary>
        ///  Do not do a checkout and do not fire callbacks; this is
        ///  primarily useful only for internal functions that will
        ///  perform the checkout themselves but need to pass checkout
        ///  options into another function, for example, `git_clone`.
        /// </summary>
        GIT_CHECKOUT_NONE = 1073741824,

        /// <summary>
        ///  Recursively checkout submodules with same options (NOT
        ///  IMPLEMENTED)
        /// </summary>
        GIT_CHECKOUT_UPDATE_SUBMODULES = 65536,

        /// <summary>
        ///  Recursively checkout submodules if HEAD moved in super repo
        ///  (NOT IMPLEMENTED)
        /// </summary>
        GIT_CHECKOUT_UPDATE_SUBMODULES_IF_CHANGED = 131072,
    }

    /// <summary>
    ///  Checkout notification flags
    ///
    ///  <para>
    ///   Checkout will invoke an options notification callback
    ///   (`notify_cb`) for certain cases - you pick which ones via
    ///   `notify_flags`:
    ///  </para>
    ///
    ///  <para>
    ///   Returning a non-zero value from this callback will cancel
    ///   the checkout. The non-zero return value will be propagated
    ///   back and returned by the git_checkout_... call.
    ///  </para>
    ///
    ///  <para>
    ///   Notification callbacks are made prior to modifying any files
    ///   on disk, so canceling on any notification will still happen
    ///   prior to any files being modified.
    ///  </para>
    /// </summary>
    [Flags]
    public enum git_checkout_notify_t
    {
        GIT_CHECKOUT_NOTIFY_NONE = 0,

        /// <summary>
        ///  Invokes checkout on conflicting paths.
        /// </summary>
        GIT_CHECKOUT_NOTIFY_CONFLICT = 1,

        /// <summary>
        ///  Notifies about "dirty" files, i.e. those that do not need an
        ///  update but no longer match the baseline. Core git displays
        ///  these files when checkout runs, but won't stop the checkout.
        /// </summary>
        GIT_CHECKOUT_NOTIFY_DIRTY = 2,

        /// <summary>
        ///  Sends notification for any file changed.
        /// </summary>
        GIT_CHECKOUT_NOTIFY_UPDATED = 4,

        /// <summary>
        ///  Notifies about untracked files.
        /// </summary>
        GIT_CHECKOUT_NOTIFY_UNTRACKED = 8,

        /// <summary>
        ///  Notifies about ignored files.
        /// </summary>
        GIT_CHECKOUT_NOTIFY_IGNORED = 16,

        /// <summary>
        ///  Notifies about ignored files.
        /// </summary>
        GIT_CHECKOUT_NOTIFY_ALL = 65535,
    }

    /// <summary>
    ///  Checkout performance-reporting structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_checkout_perfdata
    {
        public UIntPtr mkdir_calls;

        public UIntPtr stat_calls;

        public UIntPtr chmod_calls;
    }

    /// <summary>
    ///  Checkout notification callback function.
    /// </summary>
    /// <param name="why">the notification reason</param>
    /// <param name="path">the path to the file being checked out</param>
    /// <param name="baseline">the baseline's diff file information</param>
    /// <param name="target">
    ///  the checkout target diff file information
    /// </param>
    /// <param name="workdir">
    ///  the working directory diff file information
    /// </param>
    /// <param name="payload">the user-supplied callback payload</param>
    /// <returns>0 on success, or an error code</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_checkout_notify_cb(
        git_checkout_notify_t why,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
        ref git_diff_file baseline,
        ref git_diff_file target,
        ref git_diff_file workdir,
        IntPtr payload);

    /// <summary>
    ///  Checkout progress notification function.
    /// </summary>
    /// <param name="path">the path to the file being checked out</param>
    /// <param name="completed_steps">
    ///  number of checkout steps completed
    /// </param>
    /// <param name="total_steps">
    ///  number of total steps in the checkout process
    /// </param>
    /// <param name="payload">the user-supplied callback payload</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void git_checkout_progress_cb(
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
        UIntPtr completed_steps,
        UIntPtr total_steps,
        IntPtr payload);

    /// <summary>
    ///  Checkout performance data reporting function.
    /// </summary>
    /// <param name="perfdata">the performance data for the checkout</param>
    /// <param name="payload">the user-supplied callback payload</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void git_checkout_perfdata_cb(
        ref git_checkout_perfdata perfdata,
        IntPtr payload);

    /// <summary>
    ///  Checkout options structure
    ///
    ///  <para>
    ///   Initialize with `GIT_CHECKOUT_OPTIONS_INIT`. Alternatively,
    ///   you can use `git_checkout_options_init`.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_checkout_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_CHECKOUT_OPTIONS_VERSION = 1;


        /// <summary>
        ///  The version
        /// </summary>
        public uint version;

        /// <summary>
        ///  default will be a safe checkout
        /// </summary>
        public uint checkout_strategy;

        /// <summary>
        ///  don't apply filters like CRLF conversion
        /// </summary>
        public int disable_filters;

        /// <summary>
        ///  default is 0755
        /// </summary>
        public uint dir_mode;

        /// <summary>
        ///  default is 0644 or 0755 as dictated by blob
        /// </summary>
        public uint file_mode;

        /// <summary>
        ///  default is O_CREAT | O_TRUNC | O_WRONLY
        /// </summary>
        public int file_open_flags;

        /// <summary>
        ///  Checkout notification flags specify what operations the
        ///  notify callback is invoked for.
        /// </summary>
        public git_checkout_notify_t notify_flags;

        /// <summary>
        ///  Optional callback to get notifications on specific file
        ///  states.
        /// </summary>
        public git_checkout_notify_cb notify_cb;

        /// <summary>
        ///  Payload passed to notify_cb
        /// </summary>
        public IntPtr notify_payload;

        /// <summary>
        ///  Optional callback to notify the consumer of checkout
        ///  progress.
        /// </summary>
        public git_checkout_progress_cb progress_cb;

        /// <summary>
        ///  Payload passed to progress_cb
        /// </summary>
        public IntPtr progress_payload;

        /// <summary>
        ///  A list of wildmatch patterns or paths.
        ///
        ///  <para>
        ///   By default, all paths are processed. If you pass an
        ///   array of wildmatch patterns, those will be used to
        ///   filter which paths should be taken into account.
        ///  </para>
        ///
        ///  <para>
        ///   Use GIT_CHECKOUT_DISABLE_PATHSPEC_MATCH to treat as a
        ///   simple list.
        ///  </para>
        /// </summary>
        public git_strarray paths;

        /// <summary>
        ///  The expected content of the working directory; defaults to
        ///  HEAD.
        ///
        ///  <para>
        ///   If the working directory does not match this baseline
        ///   information, that will produce a checkout conflict.
        ///  </para>
        /// </summary>
        public IntPtr baseline;

        /// <summary>
        ///  Like `baseline` above, though expressed as an index. This
        ///  option overrides `baseline`.
        /// </summary>
        public IntPtr baseline_index;

        /// <summary>
        ///  alternative checkout path to workdir
        /// </summary>
        public IntPtr target_directory;

        /// <summary>
        ///  the name of the common ancestor side of conflicts
        /// </summary>
        public IntPtr ancestor_label;

        /// <summary>
        ///  the name of the "our" side of conflicts
        /// </summary>
        public IntPtr our_label;

        /// <summary>
        ///  the name of the "their" side of conflicts
        /// </summary>
        public IntPtr their_label;

        /// <summary>
        ///  Optional callback to notify the consumer of performance
        ///  data.
        /// </summary>
        public git_checkout_perfdata_cb perfdata_cb;

        /// <summary>
        ///  Payload passed to perfdata_cb
        /// </summary>
        public IntPtr perfdata_payload;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Initialize git_checkout_options structure
        ///
        ///  <para>
        ///   Initializes a `git_checkout_options` with default
        ///   values. Equivalent to creating an instance with
        ///   GIT_CHECKOUT_OPTIONS_INIT.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_checkout_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass `GIT_CHECKOUT_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_checkout_options_init(
            ref git_checkout_options opts,
            uint version);

        /// <summary>
        ///  Updates files in the index and the working tree to match the
        ///  content of the commit pointed at by HEAD.
        ///
        ///  <para>
        ///   Note that this is _not_ the correct mechanism used to
        ///   switch branches; do not change your `HEAD` and then call
        ///   this method, that would leave you with checkout
        ///   conflicts since your working directory would then appear
        ///   to be dirty. Instead, checkout the target of the branch
        ///   and then update `HEAD` using `git_repository_set_head`
        ///   to point to the branch you checked out.
        ///  </para>
        /// </summary>
        /// <param name="repo">
        ///  repository to check out (must be non-bare)
        /// </param>
        /// <param name="opts">
        ///  specifies checkout options (may be NULL)
        /// </param>
        /// <returns>
        ///  0 on success, GIT_EUNBORNBRANCH if HEAD points to a non
        ///  existing branch, non-zero value returned by `notify_cb`, or
        ///  other error code < 0 (use git_error_last for error details)
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_checkout_head(
            git_repository* repo,
            ref git_checkout_options opts);

        /// <summary>
        ///  Updates files in the working tree to match the content of
        ///  the index.
        /// </summary>
        /// <param name="repo">
        ///  repository into which to check out (must be non-bare)
        /// </param>
        /// <param name="index">
        ///  index to be checked out (or NULL to use repository index)
        /// </param>
        /// <param name="opts">
        ///  specifies checkout options (may be NULL)
        /// </param>
        /// <returns>
        ///  0 on success, non-zero return value from `notify_cb`, or
        ///  error code < 0 (use git_error_last for error details)
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_checkout_index(
            git_repository* repo,
            git_index* index,
            ref git_checkout_options opts);

        /// <summary>
        ///  Updates files in the index and working tree to match the
        ///  content of the tree pointed at by the treeish.
        /// </summary>
        /// <param name="repo">
        ///  repository to check out (must be non-bare)
        /// </param>
        /// <param name="treeish">
        ///  a commit, tag or tree which content will be used to update
        ///  the working directory (or NULL to use HEAD)
        /// </param>
        /// <param name="opts">
        ///  specifies checkout options (may be NULL)
        /// </param>
        /// <returns>
        ///  0 on success, non-zero return value from `notify_cb`, or
        ///  error code < 0 (use git_error_last for error details)
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_checkout_tree(
            git_repository* repo,
            git_object* treeish,
            ref git_checkout_options opts);
    }
}
