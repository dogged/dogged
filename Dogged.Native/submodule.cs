using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Opaque structure representing a submodule.
    /// </summary>
    public struct git_submodule
    {
        /// <summary>
        ///  Submodule source bits
        /// </summary>
        public const uint GIT_SUBMODULE_STATUS__IN_FLAGS = 0x000Fu;

        /// <summary>
        ///  Submodule index status
        /// </summary>
        public const uint GIT_SUBMODULE_STATUS__INDEX_FLAGS = 0x0070u;

        /// <summary>
        ///  Submodule working directory status
        /// </summary>
        public const uint GIT_SUBMODULE_STATUS__WD_FLAGS = 0x3F80u;
    }

    /// <summary>
    ///  Return codes for submodule status.
    ///
    ///  <para>
    ///   A combination of these flags will be returned to describe
    ///   the status of a submodule. Depending on the "ignore"
    ///   property of the submodule, some of the flags may never be
    ///   returned because they indicate changes that are supposed to
    ///   be ignored.
    ///  </para>
    ///
    ///  <para>
    ///   Submodule info is contained in 4 places: the HEAD tree, the
    ///   index, config files (both .git/config and .gitmodules), and
    ///   the working directory. Any or all of those places might be
    ///   missing information about the submodule depending on what
    ///   state the repo is in. We consider all four places to build
    ///   the combination of status flags.
    ///  </para>
    ///
    ///  <para>
    ///   There are four values that are not really status, but give
    ///   basic info about what sources of submodule data are
    ///   available. These will be returned even if ignore is set to
    ///   "ALL".
    ///  </para>
    ///
    ///  <para>
    ///   * IN_HEAD - superproject head contains submodule * IN_INDEX
    ///   - superproject index contains submodule * IN_CONFIG -
    ///   superproject gitmodules has submodule * IN_WD - superproject
    ///   workdir has submodule
    ///  </para>
    ///
    ///  <para>
    ///   The following values will be returned so long as ignore is
    ///   not "ALL".
    ///  </para>
    ///
    ///  <para>
    ///   * INDEX_ADDED - in index, not in head * INDEX_DELETED - in
    ///   head, not in index * INDEX_MODIFIED - index and head don't
    ///   match * WD_UNINITIALIZED - workdir contains empty directory
    ///   * WD_ADDED - in workdir, not index * WD_DELETED - in index,
    ///   not workdir * WD_MODIFIED - index and workdir head don't
    ///   match
    ///  </para>
    ///
    ///  <para>
    ///   The following can only be returned if ignore is "NONE" or
    ///   "UNTRACKED".
    ///  </para>
    ///
    ///  <para>
    ///   * WD_INDEX_MODIFIED - submodule workdir index is dirty *
    ///   WD_WD_MODIFIED - submodule workdir has modified files
    ///  </para>
    ///
    ///  <para>
    ///   Lastly, the following will only be returned for ignore
    ///   "NONE".
    ///  </para>
    ///
    ///  <para>
    ///   * WD_UNTRACKED - wd contains untracked files
    ///  </para>
    /// </summary>
    public enum git_submodule_status_t
    {
        GIT_SUBMODULE_STATUS_IN_HEAD = 1,
        GIT_SUBMODULE_STATUS_IN_INDEX = 2,
        GIT_SUBMODULE_STATUS_IN_CONFIG = 4,
        GIT_SUBMODULE_STATUS_IN_WD = 8,
        GIT_SUBMODULE_STATUS_INDEX_ADDED = 16,
        GIT_SUBMODULE_STATUS_INDEX_DELETED = 32,
        GIT_SUBMODULE_STATUS_INDEX_MODIFIED = 64,
        GIT_SUBMODULE_STATUS_WD_UNINITIALIZED = 128,
        GIT_SUBMODULE_STATUS_WD_ADDED = 256,
        GIT_SUBMODULE_STATUS_WD_DELETED = 512,
        GIT_SUBMODULE_STATUS_WD_MODIFIED = 1024,
        GIT_SUBMODULE_STATUS_WD_INDEX_MODIFIED = 2048,
        GIT_SUBMODULE_STATUS_WD_WD_MODIFIED = 4096,
        GIT_SUBMODULE_STATUS_WD_UNTRACKED = 8192,
    }

    /// <summary>
    ///  Function pointer to receive each submodule
    /// </summary>
    /// <param name="sm">git_submodule currently being visited</param>
    /// <param name="name">name of the submodule</param>
    /// <param name="payload">
    ///  value you passed to the foreach function as payload
    /// </param>
    /// <returns>0 on success or error code</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_submodule_cb(
        git_submodule* sm,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
        IntPtr payload);

    /// <summary>
    ///  Submodule update options structure
    ///
    ///  <para>
    ///   Initialize with `GIT_SUBMODULE_UPDATE_OPTIONS_INIT`.
    ///   Alternatively, you can use
    ///   `git_submodule_update_options_init`.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_submodule_update_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_SUBMODULE_UPDATE_OPTIONS_VERSION = 1;


        public uint version;

        /// <summary>
        ///  These options are passed to the checkout step. To disable
        ///  checkout, set the `checkout_strategy` to `GIT_CHECKOUT_NONE`
        ///  or `GIT_CHECKOUT_DRY_RUN`.
        /// </summary>
        public git_checkout_options checkout_opts;

        /// <summary>
        ///  Options which control the fetch, including callbacks.
        ///
        ///  <para>
        ///   The callbacks to use for reporting fetch progress, and
        ///   for acquiring credentials in the event they are needed.
        ///  </para>
        /// </summary>
        public git_fetch_options fetch_opts;

        /// <summary>
        ///  Allow fetching from the submodule's default remote if the
        ///  target commit isn't found. Enabled by default.
        /// </summary>
        public int allow_fetch;
    }

    /// <summary>
    ///  Submodule update values
    ///
    ///  <para>
    ///   These values represent settings for the
    ///   `submodule.$name.update` configuration value which says how
    ///   to handle `git submodule update` for this submodule. The
    ///   value is usually set in the ".gitmodules" file and copied to
    ///   ".git/config" when the submodule is initialized.
    ///  </para>
    ///
    ///  <para>
    ///   You can override this setting on a per-submodule basis with
    ///   `git_submodule_set_update()` and write the changed value to
    ///   disk using `git_submodule_save()`. If you have overwritten
    ///   the value, you can revert it by passing
    ///   `GIT_SUBMODULE_UPDATE_RESET` to the set function.
    ///  </para>
    ///
    ///  <para>
    ///   The values are:
    ///  </para>
    ///
    ///  <para>
    ///   - GIT_SUBMODULE_UPDATE_CHECKOUT: the default; when a
    ///   submodule is updated, checkout the new detached HEAD to the
    ///   submodule directory. - GIT_SUBMODULE_UPDATE_REBASE: update
    ///   by rebasing the current checked out branch onto the commit
    ///   from the superproject. - GIT_SUBMODULE_UPDATE_MERGE: update
    ///   by merging the commit in the superproject into the current
    ///   checkout out branch of the submodule. -
    ///   GIT_SUBMODULE_UPDATE_NONE: do not update this submodule even
    ///   when the commit in the superproject is updated. -
    ///   GIT_SUBMODULE_UPDATE_DEFAULT: not used except as static
    ///   initializer when we don't want any particular update rule to
    ///   be specified.
    ///  </para>
    /// </summary>
    public enum git_submodule_update_t
    {
        GIT_SUBMODULE_UPDATE_CHECKOUT = 1,
        GIT_SUBMODULE_UPDATE_REBASE = 2,
        GIT_SUBMODULE_UPDATE_MERGE = 3,
        GIT_SUBMODULE_UPDATE_NONE = 4,
        GIT_SUBMODULE_UPDATE_DEFAULT = 0,
    }

    /// <summary>
    ///  Submodule ignore values
    ///
    ///  <para>
    ///   These values represent settings for the
    ///   `submodule.$name.ignore` configuration value which says how
    ///   deeply to look at the working directory when getting
    ///   submodule status.
    ///  </para>
    ///
    ///  <para>
    ///   You can override this value in memory on a per-submodule
    ///   basis with `git_submodule_set_ignore()` and can write the
    ///   changed value to disk with `git_submodule_save()`. If you
    ///   have overwritten the value, you can revert to the on disk
    ///   value by using `GIT_SUBMODULE_IGNORE_RESET`.
    ///  </para>
    ///
    ///  <para>
    ///   The values are:
    ///  </para>
    ///
    ///  <para>
    ///   - GIT_SUBMODULE_IGNORE_UNSPECIFIED: use the submodule's
    ///   configuration - GIT_SUBMODULE_IGNORE_NONE: don't ignore any
    ///   change - i.e. even an untracked file, will mark the
    ///   submodule as dirty. Ignored files are still ignored, of
    ///   course. - GIT_SUBMODULE_IGNORE_UNTRACKED: ignore untracked
    ///   files; only changes to tracked files, or the index or the
    ///   HEAD commit will matter. - GIT_SUBMODULE_IGNORE_DIRTY:
    ///   ignore changes in the working directory, only considering
    ///   changes if the HEAD of submodule has moved from the value in
    ///   the superproject. - GIT_SUBMODULE_IGNORE_ALL: never check if
    ///   the submodule is dirty - GIT_SUBMODULE_IGNORE_DEFAULT: not
    ///   used except as static initializer when we don't want any
    ///   particular ignore rule to be specified.
    ///  </para>
    /// </summary>
    public enum git_submodule_ignore_t
    {
        /// <summary>
        ///  use the submodule's configuration
        /// </summary>
        GIT_SUBMODULE_IGNORE_UNSPECIFIED = -1,

        /// <summary>
        ///  any change or untracked == dirty
        /// </summary>
        GIT_SUBMODULE_IGNORE_NONE = 1,

        /// <summary>
        ///  dirty if tracked files change
        /// </summary>
        GIT_SUBMODULE_IGNORE_UNTRACKED = 2,

        /// <summary>
        ///  only dirty if HEAD moved
        /// </summary>
        GIT_SUBMODULE_IGNORE_DIRTY = 3,

        /// <summary>
        ///  never dirty
        /// </summary>
        GIT_SUBMODULE_IGNORE_ALL = 4,
    }

    /// <summary>
    ///  Options for submodule recurse.
    ///
    ///  <para>
    ///   Represent the value of
    ///   `submodule.$name.fetchRecurseSubmodules`
    ///  </para>
    ///
    ///  <para>
    ///   * GIT_SUBMODULE_RECURSE_NO - do no recurse into submodules *
    ///   GIT_SUBMODULE_RECURSE_YES - recurse into submodules *
    ///   GIT_SUBMODULE_RECURSE_ONDEMAND - recurse into submodules
    ///   only when commit not already in local clone
    ///  </para>
    /// </summary>
    public enum git_submodule_recurse_t
    {
        GIT_SUBMODULE_RECURSE_NO = 0,
        GIT_SUBMODULE_RECURSE_YES = 1,
        GIT_SUBMODULE_RECURSE_ONDEMAND = 2,
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Initialize git_submodule_update_options structure
        ///
        ///  <para>
        ///   Initializes a `git_submodule_update_options` with
        ///   default values. Equivalent to creating an instance with
        ///   `GIT_SUBMODULE_UPDATE_OPTIONS_INIT`.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_submodule_update_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass
        ///  `GIT_SUBMODULE_UPDATE_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_submodule_update_options_init(
            ref git_submodule_update_options opts,
            uint version);

        /// <summary>
        ///  Update a submodule. This will clone a missing submodule and
        ///  checkout the subrepository to the commit specified in the
        ///  index of the containing repository. If the submodule
        ///  repository doesn't contain the target commit (e.g. because
        ///  fetchRecurseSubmodules isn't set), then the submodule is
        ///  fetched using the fetch options supplied in options.
        /// </summary>
        /// <param name="submodule">Submodule object</param>
        /// <param name="init">
        ///  If the submodule is not initialized, setting this flag to
        ///  true will initialize the submodule before updating.
        ///  Otherwise, this will return an error if attempting to update
        ///  an uninitialized repository. but setting this to true forces
        ///  them to be updated.
        /// </param>
        /// <param name="options">
        ///  configuration options for the update. If NULL, the function
        ///  works as though GIT_SUBMODULE_UPDATE_OPTIONS_INIT was
        ///  passed.
        /// </param>
        /// <returns>
        ///  0 on success, any non-zero return value from a callback
        ///  function, or a negative value to indicate an error (use
        ///  `git_error_last` for a detailed error message).
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_submodule_update(
            git_submodule* submodule,
            int init,
            ref git_submodule_update_options options);

        /// <summary>
        ///  Lookup submodule information by name or path.
        ///
        ///  <para>
        ///   Given either the submodule name or path (they are
        ///   usually the same), this returns a structure describing
        ///   the submodule.
        ///  </para>
        ///
        ///  <para>
        ///   There are two expected error scenarios:
        ///  </para>
        ///
        ///  <para>
        ///   - The submodule is not mentioned in the HEAD, the index,
        ///   and the config, but does "exist" in the working
        ///   directory (i.e. there is a subdirectory that appears to
        ///   be a Git repository). In this case, this function
        ///   returns GIT_EEXISTS to indicate a sub-repository exists
        ///   but not in a state where a git_submodule can be
        ///   instantiated. - The submodule is not mentioned in the
        ///   HEAD, index, or config and the working directory doesn't
        ///   contain a value git repo at that path. There may or may
        ///   not be anything else at that path, but nothing that
        ///   looks like a submodule. In this case, this returns
        ///   GIT_ENOTFOUND.
        ///  </para>
        ///
        ///  <para>
        ///   You must call `git_submodule_free` when done with the
        ///   submodule.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Output ptr to submodule; pass NULL to just get return code
        /// </param>
        /// <param name="repo">The parent repository</param>
        /// <param name="name">
        ///  The name of or path to the submodule; trailing slashes okay
        /// </param>
        /// <returns>
        ///  0 on success, GIT_ENOTFOUND if submodule does not exist,
        ///  GIT_EEXISTS if a repository is found in working directory
        ///  only, -1 on other errors.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_submodule_lookup(
            out git_submodule* _out,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Create an in-memory copy of a submodule. The copy must be
        ///  explicitly free'd or it will leak.
        /// </summary>
        /// <param name="_out">
        ///  Pointer to store the copy of the submodule.
        /// </param>
        /// <param name="source">Original submodule to copy.</param>
        /// <returns>0</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_submodule_dup(
            out git_submodule* _out,
            git_submodule* source);

        /// <summary>
        ///  Release a submodule
        /// </summary>
        /// <param name="submodule">Submodule object</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_submodule_free(
            git_submodule* submodule);

        /// <summary>
        ///  Iterate over all tracked submodules of a repository.
        ///
        ///  <para>
        ///   See the note on `git_submodule` above. This iterates
        ///   over the tracked submodules as described therein.
        ///  </para>
        ///
        ///  <para>
        ///   If you are concerned about items in the working
        ///   directory that look like submodules but are not tracked,
        ///   the diff API will generate a diff record for workdir
        ///   items that look like submodules but are not tracked,
        ///   showing them as added in the workdir. Also, the status
        ///   API will treat the entire subdirectory of a contained
        ///   git repo as a single GIT_STATUS_WT_NEW item.
        ///  </para>
        /// </summary>
        /// <param name="repo">The repository</param>
        /// <param name="callback">
        ///  Function to be called with the name of each submodule.
        ///  Return a non-zero value to terminate the iteration.
        /// </param>
        /// <param name="payload">Extra data to pass to callback</param>
        /// <returns>
        ///  0 on success, -1 on error, or non-zero return value of
        ///  callback
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_submodule_foreach(
            git_repository* repo,
            git_submodule_cb callback,
            IntPtr payload);

        /// <summary>
        ///  Set up a new git submodule for checkout.
        ///
        ///  <para>
        ///   This does "git submodule add" up to the fetch and
        ///   checkout of the submodule contents. It preps a new
        ///   submodule, creates an entry in .gitmodules and creates
        ///   an empty initialized repository either at the given path
        ///   in the working directory or in .git/modules with a
        ///   gitlink from the working directory to the new repo.
        ///  </para>
        ///
        ///  <para>
        ///   To fully emulate "git submodule add" call this function,
        ///   then open the submodule repo and perform the clone step
        ///   as needed (if you don't need anything custom see
        ///   `git_submodule_add_clone()`). Lastly, call
        ///   `git_submodule_add_finalize()` to wrap up adding the new
        ///   submodule and .gitmodules to the index to be ready to
        ///   commit.
        ///  </para>
        ///
        ///  <para>
        ///   You must call `git_submodule_free` on the submodule
        ///   object when done.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  The newly created submodule ready to open for clone
        /// </param>
        /// <param name="repo">
        ///  The repository in which you want to create the submodule
        /// </param>
        /// <param name="url">URL for the submodule's remote</param>
        /// <param name="path">
        ///  Path at which the submodule should be created
        /// </param>
        /// <param name="use_gitlink">
        ///  Should workdir contain a gitlink to the repo in .git/modules
        ///  vs. repo directly in workdir.
        /// </param>
        /// <returns>
        ///  0 on success, GIT_EEXISTS if submodule already exists, -1 on
        ///  other errors.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_submodule_add_setup(
            out git_submodule* _out,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string url,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            int use_gitlink);

        /// <summary>
        ///  Perform the clone step for a newly created submodule.
        ///
        ///  <para>
        ///   This performs the necessary `git_clone` to setup a
        ///   newly-created submodule.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  The newly created repository object. Optional.
        /// </param>
        /// <param name="submodule">
        ///  The submodule currently waiting for its clone.
        /// </param>
        /// <param name="opts">The options to use.</param>
        /// <returns>
        ///  0 on success, -1 on other errors (see git_clone).
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_submodule_clone(
            out git_repository* _out,
            git_submodule* submodule,
            ref git_submodule_update_options opts);

        /// <summary>
        ///  Resolve the setup of a new git submodule.
        ///
        ///  <para>
        ///   This should be called on a submodule once you have
        ///   called add setup and done the clone of the submodule.
        ///   This adds the .gitmodules file and the newly cloned
        ///   submodule to the index to be ready to be committed (but
        ///   doesn't actually do the commit).
        ///  </para>
        /// </summary>
        /// <param name="submodule">The submodule to finish adding.</param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_submodule_add_finalize(
            git_submodule* submodule);

        /// <summary>
        ///  Add current submodule HEAD commit to index of superproject.
        /// </summary>
        /// <param name="submodule">
        ///  The submodule to add to the index
        /// </param>
        /// <param name="write_index">
        ///  Boolean if this should immediately write the index file. If
        ///  you pass this as false, you will have to get the git_index
        ///  and explicitly call `git_index_write()` on it to save the
        ///  change.
        /// </param>
        /// <returns>0 on success, < 0 on failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_submodule_add_to_index(
            git_submodule* submodule,
            int write_index);

        /// <summary>
        ///  Get the containing repository for a submodule.
        ///
        ///  <para>
        ///   This returns a pointer to the repository that contains
        ///   the submodule. This is a just a reference to the
        ///   repository that was passed to the original
        ///   `git_submodule_lookup()` call, so if that repository has
        ///   been freed, then this may be a dangling reference.
        ///  </para>
        /// </summary>
        /// <param name="submodule">Pointer to submodule object</param>
        /// <returns>Pointer to `git_repository`</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_repository* git_submodule_owner(
            git_submodule* submodule);

        /// <summary>
        ///  Get the name of submodule.
        /// </summary>
        /// <param name="submodule">Pointer to submodule object</param>
        /// <returns>Pointer to the submodule name</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_submodule_name(
            git_submodule* submodule);

        /// <summary>
        ///  Get the path to the submodule.
        ///
        ///  <para>
        ///   The path is almost always the same as the submodule
        ///   name, but the two are actually not required to match.
        ///  </para>
        /// </summary>
        /// <param name="submodule">Pointer to submodule object</param>
        /// <returns>Pointer to the submodule path</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_submodule_path(
            git_submodule* submodule);

        /// <summary>
        ///  Get the URL for the submodule.
        /// </summary>
        /// <param name="submodule">Pointer to submodule object</param>
        /// <returns>Pointer to the submodule url</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_submodule_url(
            git_submodule* submodule);

        /// <summary>
        ///  Resolve a submodule url relative to the given repository.
        /// </summary>
        /// <param name="_out">
        ///  buffer to store the absolute submodule url in
        /// </param>
        /// <param name="repo">Pointer to repository object</param>
        /// <param name="url">Relative url</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_submodule_resolve_url(
            ref git_buf _out,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string url);

        /// <summary>
        ///  Get the branch for the submodule.
        /// </summary>
        /// <param name="submodule">Pointer to submodule object</param>
        /// <returns>Pointer to the submodule branch</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_submodule_branch(
            git_submodule* submodule);

        /// <summary>
        ///  Set the branch for the submodule in the configuration
        ///
        ///  <para>
        ///   After calling this, you may wish to call
        ///   `git_submodule_sync()` to write the changes to the
        ///   checked out submodule repository.
        ///  </para>
        /// </summary>
        /// <param name="repo">the repository to affect</param>
        /// <param name="name">
        ///  the name of the submodule to configure
        /// </param>
        /// <param name="branch">
        ///  Branch that should be used for the submodule
        /// </param>
        /// <returns>0 on success, < 0 on failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_submodule_set_branch(
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string branch);

        /// <summary>
        ///  Set the URL for the submodule in the configuration
        ///
        ///  <para>
        ///   After calling this, you may wish to call
        ///   `git_submodule_sync()` to write the changes to the
        ///   checked out submodule repository.
        ///  </para>
        /// </summary>
        /// <param name="repo">the repository to affect</param>
        /// <param name="name">
        ///  the name of the submodule to configure
        /// </param>
        /// <param name="url">
        ///  URL that should be used for the submodule
        /// </param>
        /// <returns>0 on success, < 0 on failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_submodule_set_url(
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string url);

        /// <summary>
        ///  Get the OID for the submodule in the index.
        /// </summary>
        /// <param name="submodule">Pointer to submodule object</param>
        /// <returns>
        ///  Pointer to git_oid or NULL if submodule is not in index.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_submodule_index_id(
            git_submodule* submodule);

        /// <summary>
        ///  Get the OID for the submodule in the current HEAD tree.
        /// </summary>
        /// <param name="submodule">Pointer to submodule object</param>
        /// <returns>
        ///  Pointer to git_oid or NULL if submodule is not in the HEAD.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_submodule_head_id(
            git_submodule* submodule);

        /// <summary>
        ///  Get the OID for the submodule in the current working
        ///  directory.
        ///
        ///  <para>
        ///   This returns the OID that corresponds to looking up
        ///   'HEAD' in the checked out submodule. If there are
        ///   pending changes in the index or anything else, this
        ///   won't notice that. You should call
        ///   `git_submodule_status()` for a more complete picture
        ///   about the state of the working directory.
        ///  </para>
        /// </summary>
        /// <param name="submodule">Pointer to submodule object</param>
        /// <returns>
        ///  Pointer to git_oid or NULL if submodule is not checked out.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_submodule_wd_id(
            git_submodule* submodule);

        /// <summary>
        ///  Get the ignore rule that will be used for the submodule.
        ///
        ///  <para>
        ///   These values control the behavior of
        ///   `git_submodule_status()` for this submodule. There are
        ///   four ignore values:
        ///  </para>
        ///
        ///  <para>
        ///   - **GIT_SUBMODULE_IGNORE_NONE** will consider any change
        ///   to the contents of the submodule from a clean checkout
        ///   to be dirty, including the addition of untracked files.
        ///   This is the default if unspecified. -
        ///   **GIT_SUBMODULE_IGNORE_UNTRACKED** examines the contents
        ///   of the working tree (i.e. call `git_status_foreach()` on
        ///   the submodule) but UNTRACKED files will not count as
        ///   making the submodule dirty. -
        ///   **GIT_SUBMODULE_IGNORE_DIRTY** means to only check if
        ///   the HEAD of the submodule has moved for status. This is
        ///   fast since it does not need to scan the working tree of
        ///   the submodule at all. - **GIT_SUBMODULE_IGNORE_ALL**
        ///   means not to open the submodule repo. The working
        ///   directory will be consider clean so long as there is a
        ///   checked out version present.
        ///  </para>
        /// </summary>
        /// <param name="submodule">The submodule to check</param>
        /// <returns>
        ///  The current git_submodule_ignore_t valyue what will be used
        ///  for this submodule.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_submodule_ignore_t git_submodule_ignore(
            git_submodule* submodule);

        /// <summary>
        ///  Set the ignore rule for the submodule in the configuration
        ///
        ///  <para>
        ///   This does not affect any currently-loaded instances.
        ///  </para>
        /// </summary>
        /// <param name="repo">the repository to affect</param>
        /// <param name="name">the name of the submdule</param>
        /// <param name="ignore">The new value for the ignore rule</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_submodule_set_ignore(
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            git_submodule_ignore_t ignore);

        /// <summary>
        ///  Get the update rule that will be used for the submodule.
        ///
        ///  <para>
        ///   This value controls the behavior of the `git submodule
        ///   update` command. There are four useful values documented
        ///   with `git_submodule_update_t`.
        ///  </para>
        /// </summary>
        /// <param name="submodule">The submodule to check</param>
        /// <returns>
        ///  The current git_submodule_update_t value that will be used
        ///  for this submodule.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_submodule_update_t git_submodule_update_strategy(
            git_submodule* submodule);

        /// <summary>
        ///  Set the update rule for the submodule in the configuration
        ///
        ///  <para>
        ///   This setting won't affect any existing instances.
        ///  </para>
        /// </summary>
        /// <param name="repo">the repository to affect</param>
        /// <param name="name">
        ///  the name of the submodule to configure
        /// </param>
        /// <param name="update">The new value to use</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_submodule_set_update(
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            git_submodule_update_t update);

        /// <summary>
        ///  Read the fetchRecurseSubmodules rule for a submodule.
        ///
        ///  <para>
        ///   This accesses the submodule.
        ///   <name>.fetchRecurseSubmodules value for the submodule
        ///   that controls fetching behavior for the submodule.
        ///  </para>
        ///
        ///  <para>
        ///   Note that at this time, libgit2 does not honor this
        ///   setting and the fetch functionality current ignores
        ///   submodules.
        ///  </para>
        /// </summary>
        /// <param name="submodule">the submodule to examine</param>
        /// <returns>the submodule recursion configuration</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_submodule_recurse_t git_submodule_fetch_recurse_submodules(
            git_submodule* submodule);

        /// <summary>
        ///  Set the fetchRecurseSubmodules rule for a submodule in the
        ///  configuration
        ///
        ///  <para>
        ///   This setting won't affect any existing instances.
        ///  </para>
        /// </summary>
        /// <param name="repo">the repository to affect</param>
        /// <param name="name">the submodule to configure</param>
        /// <param name="fetch_recurse_submodules">
        ///  the submodule recursion configuration
        /// </param>
        /// <returns>old value for fetchRecurseSubmodules</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_submodule_set_fetch_recurse_submodules(
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            git_submodule_recurse_t fetch_recurse_submodules);

        /// <summary>
        ///  Copy submodule info into ".git/config" file.
        ///
        ///  <para>
        ///   Just like "git submodule init", this copies information
        ///   about the submodule into ".git/config". You can use the
        ///   accessor functions above to alter the in-memory
        ///   git_submodule object and control what is written to the
        ///   config, overriding what is in .gitmodules.
        ///  </para>
        /// </summary>
        /// <param name="submodule">
        ///  The submodule to write into the superproject config
        /// </param>
        /// <param name="overwrite">
        ///  By default, existing entries will not be overwritten, but
        ///  setting this to true forces them to be updated.
        /// </param>
        /// <returns>0 on success, < 0 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_submodule_init(
            git_submodule* submodule,
            int overwrite);

        /// <summary>
        ///  Set up the subrepository for a submodule in preparation for
        ///  clone.
        ///
        ///  <para>
        ///   This function can be called to init and set up a
        ///   submodule repository from a submodule in preparation to
        ///   clone it from its remote.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Output pointer to the created git repository.
        /// </param>
        /// <param name="sm">
        ///  The submodule to create a new subrepository from.
        /// </param>
        /// <param name="use_gitlink">
        ///  Should the workdir contain a gitlink to the repo in
        ///  .git/modules vs. repo directly in workdir.
        /// </param>
        /// <returns>0 on success, < 0 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_submodule_repo_init(
            out git_repository* _out,
            git_submodule* sm,
            int use_gitlink);

        /// <summary>
        ///  Copy submodule remote info into submodule repo.
        ///
        ///  <para>
        ///   This copies the information about the submodules URL
        ///   into the checked out submodule config, acting like "git
        ///   submodule sync". This is useful if you have altered the
        ///   URL for the submodule (or it has been altered by a fetch
        ///   of upstream changes) and you need to update your local
        ///   repo.
        ///  </para>
        /// </summary>
        /// <param name="submodule">The submodule to copy.</param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_submodule_sync(
            git_submodule* submodule);

        /// <summary>
        ///  Open the repository for a submodule.
        ///
        ///  <para>
        ///   This is a newly opened repository object. The caller is
        ///   responsible for calling `git_repository_free()` on it
        ///   when done. Multiple calls to this function will return
        ///   distinct `git_repository` objects. This will only work
        ///   if the submodule is checked out into the working
        ///   directory.
        ///  </para>
        /// </summary>
        /// <param name="repo">
        ///  Pointer to the submodule repo which was opened
        /// </param>
        /// <param name="submodule">Submodule to be opened</param>
        /// <returns>
        ///  0 on success, < 0 if submodule repo could not be opened.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_submodule_open(
            git_repository** repo,
            git_submodule* submodule);

        /// <summary>
        ///  Reread submodule info from config, index, and HEAD.
        ///
        ///  <para>
        ///   Call this to reread cached submodule information for
        ///   this submodule if you have reason to believe that it has
        ///   changed.
        ///  </para>
        /// </summary>
        /// <param name="submodule">The submodule to reload</param>
        /// <param name="force">
        ///  Force reload even if the data doesn't seem out of date
        /// </param>
        /// <returns>0 on success, < 0 on error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_submodule_reload(
            git_submodule* submodule,
            int force);

        /// <summary>
        ///  Get the status for a submodule.
        ///
        ///  <para>
        ///   This looks at a submodule and tries to determine the
        ///   status. It will return a combination of the
        ///   `GIT_SUBMODULE_STATUS` values above. How deeply it
        ///   examines the working directory to do this will depend on
        ///   the `git_submodule_ignore_t` value for the submodule.
        ///  </para>
        /// </summary>
        /// <param name="status">
        ///  Combination of `GIT_SUBMODULE_STATUS` flags
        /// </param>
        /// <param name="repo">the repository in which to look</param>
        /// <param name="name">name of the submodule</param>
        /// <param name="ignore">the ignore rules to follow</param>
        /// <returns>0 on success, < 0 on error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_submodule_status(
            uint* status,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            git_submodule_ignore_t ignore);

        /// <summary>
        ///  Get the locations of submodule information.
        ///
        ///  <para>
        ///   This is a bit like a very lightweight version of
        ///   `git_submodule_status`. It just returns a made of the
        ///   first four submodule status values (i.e. the ones like
        ///   GIT_SUBMODULE_STATUS_IN_HEAD, etc) that tell you where
        ///   the submodule data comes from (i.e. the HEAD commit,
        ///   gitmodules file, etc.). This can be useful if you want
        ///   to know if the submodule is present in the working
        ///   directory at this point in time, etc.
        ///  </para>
        /// </summary>
        /// <param name="location_status">
        ///  Combination of first four `GIT_SUBMODULE_STATUS` flags
        /// </param>
        /// <param name="submodule">
        ///  Submodule for which to get status
        /// </param>
        /// <returns>0 on success, < 0 on error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_submodule_location(
            uint* location_status,
            git_submodule* submodule);
    }
}
