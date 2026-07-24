using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public sealed class SubmoduleHandle : SafeHandle
    {
        public SubmoduleHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe SubmoduleHandle(git_submodule* nativeSubmodule) :
            base((IntPtr)nativeSubmodule, true)
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
            libgit2.git_submodule_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public static partial class libgit2
    {
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
            SubmoduleHandle submodule,
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
            out SubmoduleHandle _out,
            RepositoryHandle repo,
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
            out SubmoduleHandle _out,
            SubmoduleHandle source);

        /// <summary>
        ///  Release a submodule
        /// </summary>
        /// <param name="submodule">Submodule object</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_submodule_free(
            IntPtr submodule);

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
            RepositoryHandle repo,
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
            out SubmoduleHandle _out,
            RepositoryHandle repo,
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
            out RepositoryHandle _out,
            SubmoduleHandle submodule,
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
            SubmoduleHandle submodule);

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
            SubmoduleHandle submodule,
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
            SubmoduleHandle submodule);

        /// <summary>
        ///  Get the name of submodule.
        /// </summary>
        /// <param name="submodule">Pointer to submodule object</param>
        /// <returns>Pointer to the submodule name</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_submodule_name(
            SubmoduleHandle submodule);

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
            SubmoduleHandle submodule);

        /// <summary>
        ///  Get the URL for the submodule.
        /// </summary>
        /// <param name="submodule">Pointer to submodule object</param>
        /// <returns>Pointer to the submodule url</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_submodule_url(
            SubmoduleHandle submodule);

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
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string url);

        /// <summary>
        ///  Get the branch for the submodule.
        /// </summary>
        /// <param name="submodule">Pointer to submodule object</param>
        /// <returns>Pointer to the submodule branch</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_submodule_branch(
            SubmoduleHandle submodule);

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
            RepositoryHandle repo,
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
            RepositoryHandle repo,
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
            SubmoduleHandle submodule);

        /// <summary>
        ///  Get the OID for the submodule in the current HEAD tree.
        /// </summary>
        /// <param name="submodule">Pointer to submodule object</param>
        /// <returns>
        ///  Pointer to git_oid or NULL if submodule is not in the HEAD.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_submodule_head_id(
            SubmoduleHandle submodule);

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
            SubmoduleHandle submodule);

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
            SubmoduleHandle submodule);

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
            RepositoryHandle repo,
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
            SubmoduleHandle submodule);

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
            RepositoryHandle repo,
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
            SubmoduleHandle submodule);

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
            RepositoryHandle repo,
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
            SubmoduleHandle submodule,
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
            out RepositoryHandle _out,
            SubmoduleHandle sm,
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
            SubmoduleHandle submodule);

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
            RepositoryHandle repo,
            SubmoduleHandle submodule);

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
            SubmoduleHandle submodule,
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
            RepositoryHandle repo,
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
            SubmoduleHandle submodule);
    }
}
