using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Representation of an existing git repository, including all its
    ///  object contents
    /// </summary>
    public struct git_repository
    {
    }

    /// <summary>
    ///  Option flags for `git_repository_open_ext`.
    /// </summary>
    public enum git_repository_open_flag_t
    {
        /// <summary>
        ///  Only open the repository if it can be immediately found in
        ///  the start_path. Do not walk up from the start_path looking
        ///  at parent directories.
        /// </summary>
        GIT_REPOSITORY_OPEN_NO_SEARCH = 1,

        /// <summary>
        ///  Unless this flag is set, open will not continue searching
        ///  across filesystem boundaries (i.e. when `st_dev` changes
        ///  from the `stat` system call). For example, searching in a
        ///  user's home directory at "/home/user/source/" will not
        ///  return "/.git/" as the found repo if "/" is a different
        ///  filesystem than "/home".
        /// </summary>
        GIT_REPOSITORY_OPEN_CROSS_FS = 2,

        /// <summary>
        ///  Open repository as a bare repo regardless of core.bare
        ///  config, and defer loading config file for faster setup.
        ///  Unlike `git_repository_open_bare`, this can follow gitlinks.
        /// </summary>
        GIT_REPOSITORY_OPEN_BARE = 4,

        /// <summary>
        ///  Do not check for a repository by appending /.git to the
        ///  start_path; only open the repository if start_path itself
        ///  points to the git directory.
        /// </summary>
        GIT_REPOSITORY_OPEN_NO_DOTGIT = 8,

        /// <summary>
        ///  Find and open a git repository, respecting the environment
        ///  variables used by the git command-line tools. If set,
        ///  `git_repository_open_ext` will ignore the other flags and
        ///  the `ceiling_dirs` argument, and will allow a NULL `path` to
        ///  use `GIT_DIR` or search from the current directory. The
        ///  search for a repository will respect
        ///  $GIT_CEILING_DIRECTORIES and
        ///  $GIT_DISCOVERY_ACROSS_FILESYSTEM. The opened repository will
        ///  respect $GIT_INDEX_FILE, $GIT_NAMESPACE,
        ///  $GIT_OBJECT_DIRECTORY, and
        ///  $GIT_ALTERNATE_OBJECT_DIRECTORIES. In the future, this flag
        ///  will also cause `git_repository_open_ext` to respect
        ///  $GIT_WORK_TREE and $GIT_COMMON_DIR; currently,
        ///  `git_repository_open_ext` with this flag will error out if
        ///  either $GIT_WORK_TREE or $GIT_COMMON_DIR is set.
        /// </summary>
        GIT_REPOSITORY_OPEN_FROM_ENV = 16,
    }

    /// <summary>
    ///  Option flags for `git_repository_init_ext`.
    ///
    ///  <para>
    ///   These flags configure extra behaviors to
    ///   `git_repository_init_ext`. In every case, the default
    ///   behavior is the zero value (i.e. flag is not set). Just OR
    ///   the flag values together for the `flags` parameter when
    ///   initializing a new repo.
    ///  </para>
    /// </summary>
    public enum git_repository_init_flag_t
    {
        /// <summary>
        ///  Create a bare repository with no working directory.
        /// </summary>
        GIT_REPOSITORY_INIT_BARE = 1,

        /// <summary>
        ///  Return an GIT_EEXISTS error if the repo_path appears to
        ///  already be an git repository.
        /// </summary>
        GIT_REPOSITORY_INIT_NO_REINIT = 2,

        /// <summary>
        ///  Make the repo_path (and workdir_path) as needed. Init is
        ///  always willing to create the ".git" directory even without
        ///  this flag. This flag tells init to create the trailing
        ///  component of the repo and workdir paths as needed.
        /// </summary>
        GIT_REPOSITORY_INIT_MKDIR = 8,

        /// <summary>
        ///  Recursively make all components of the repo and workdir
        ///  paths as necessary.
        /// </summary>
        GIT_REPOSITORY_INIT_MKPATH = 16,

        /// <summary>
        ///  libgit2 normally uses internal templates to initialize a new
        ///  repo. This flags enables external templates, looking the
        ///  "template_path" from the options if set, or the
        ///  `init.templatedir` global config if not, or falling back on
        ///  "/usr/share/git-core/templates" if it exists.
        /// </summary>
        GIT_REPOSITORY_INIT_EXTERNAL_TEMPLATE = 32,

        /// <summary>
        ///  If an alternate workdir is specified, use relative paths for
        ///  the gitdir and core.worktree.
        /// </summary>
        GIT_REPOSITORY_INIT_RELATIVE_GITLINK = 64,
    }

    /// <summary>
    ///  Mode options for `git_repository_init_ext`.
    ///
    ///  <para>
    ///   Set the mode field of the `git_repository_init_options`
    ///   structure either to the custom mode that you would like, or
    ///   to one of the defined modes.
    ///  </para>
    /// </summary>
    public enum git_repository_init_mode_t
    {
        /// <summary>
        ///  Use permissions configured by umask - the default.
        /// </summary>
        GIT_REPOSITORY_INIT_SHARED_UMASK = 0,

        /// <summary>
        ///  Use "--shared=group" behavior, chmod'ing the new repo to be
        ///  group writable and "g+sx" for sticky group assignment.
        /// </summary>
        GIT_REPOSITORY_INIT_SHARED_GROUP = 1533,

        /// <summary>
        ///  Use "--shared=all" behavior, adding world readability.
        /// </summary>
        GIT_REPOSITORY_INIT_SHARED_ALL = 1535,
    }

    /// <summary>
    ///  Extended options structure for `git_repository_init_ext`.
    ///
    ///  <para>
    ///   This contains extra options for `git_repository_init_ext`
    ///   that enable additional initialization features.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_repository_init_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_REPOSITORY_INIT_OPTIONS_VERSION = 1;


        public uint version;

        /// <summary>
        ///  Combination of GIT_REPOSITORY_INIT flags above.
        /// </summary>
        public uint flags;

        /// <summary>
        ///  Set to one of the standard GIT_REPOSITORY_INIT_SHARED_...
        ///  constants above, or to a custom value that you would like.
        /// </summary>
        public uint mode;

        /// <summary>
        ///  The path to the working dir or NULL for default (i.e.
        ///  repo_path parent on non-bare repos). IF THIS IS RELATIVE
        ///  PATH, IT WILL BE EVALUATED RELATIVE TO THE REPO_PATH. If
        ///  this is not the "natural" working directory, a .git gitlink
        ///  file will be created here linking to the repo_path.
        /// </summary>
        public IntPtr workdir_path;

        /// <summary>
        ///  If set, this will be used to initialize the "description"
        ///  file in the repository, instead of using the template
        ///  content.
        /// </summary>
        public IntPtr description;

        /// <summary>
        ///  When GIT_REPOSITORY_INIT_EXTERNAL_TEMPLATE is set, this
        ///  contains the path to use for the template directory. If this
        ///  is NULL, the config or default directory options will be
        ///  used instead.
        /// </summary>
        public IntPtr template_path;

        /// <summary>
        ///  The name of the head to point HEAD at. If NULL, then this
        ///  will be treated as "master" and the HEAD ref will be set to
        ///  "refs/heads/master". If this begins with "refs/" it will be
        ///  used verbatim; otherwise "refs/heads/" will be prefixed.
        /// </summary>
        public IntPtr initial_head;

        /// <summary>
        ///  If this is non-NULL, then after the rest of the repository
        ///  initialization is completed, an "origin" remote will be
        ///  added pointing to this URL.
        /// </summary>
        public IntPtr origin_url;

        /// <summary>
        ///  Type of object IDs to use for this repository, or 0 for
        ///  default (currently SHA1).
        /// </summary>
        public git_oid_t oid_type;

        /// <summary>
        ///  Type of the reference database to use for this repository,
        ///  or 0 for default (currently "files").
        /// </summary>
        public git_refdb_t refdb_type;
    }

    /// <summary>
    ///  List of items which belong to the git repository layout
    /// </summary>
    public enum git_repository_item_t
    {
        GIT_REPOSITORY_ITEM_GITDIR,
        GIT_REPOSITORY_ITEM_WORKDIR,
        GIT_REPOSITORY_ITEM_COMMONDIR,
        GIT_REPOSITORY_ITEM_INDEX,
        GIT_REPOSITORY_ITEM_OBJECTS,
        GIT_REPOSITORY_ITEM_REFS,
        GIT_REPOSITORY_ITEM_PACKED_REFS,
        GIT_REPOSITORY_ITEM_REMOTES,
        GIT_REPOSITORY_ITEM_CONFIG,
        GIT_REPOSITORY_ITEM_INFO,
        GIT_REPOSITORY_ITEM_HOOKS,
        GIT_REPOSITORY_ITEM_LOGS,
        GIT_REPOSITORY_ITEM_MODULES,
        GIT_REPOSITORY_ITEM_WORKTREES,
        GIT_REPOSITORY_ITEM_WORKTREE_CONFIG,
        GIT_REPOSITORY_ITEM__LAST,
    }

    /// <summary>
    ///  Callback used to iterate over each FETCH_HEAD entry
    /// </summary>
    /// <param name="ref_name">The reference name</param>
    /// <param name="remote_url">The remote URL</param>
    /// <param name="oid">The reference target OID</param>
    /// <param name="is_merge">
    ///  Was the reference the result of a merge
    /// </param>
    /// <param name="payload">
    ///  Payload passed to git_repository_fetchhead_foreach
    /// </param>
    /// <returns>non-zero to terminate the iteration</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_repository_fetchhead_foreach_cb(
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string ref_name,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string remote_url,
        ref git_oid oid,
        uint is_merge,
        IntPtr payload);

    /// <summary>
    ///  Callback used to iterate over each MERGE_HEAD entry
    /// </summary>
    /// <param name="oid">The merge OID</param>
    /// <param name="payload">
    ///  Payload passed to git_repository_mergehead_foreach
    /// </param>
    /// <returns>non-zero to terminate the iteration</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_repository_mergehead_foreach_cb(
        ref git_oid oid,
        IntPtr payload);

    /// <summary>
    ///  Repository state
    ///
    ///  <para>
    ///   These values represent possible states for the repository to
    ///   be in, based on the current operation which is ongoing.
    ///  </para>
    /// </summary>
    public enum git_repository_state_t
    {
        GIT_REPOSITORY_STATE_NONE,
        GIT_REPOSITORY_STATE_MERGE,
        GIT_REPOSITORY_STATE_REVERT,
        GIT_REPOSITORY_STATE_REVERT_SEQUENCE,
        GIT_REPOSITORY_STATE_CHERRYPICK,
        GIT_REPOSITORY_STATE_CHERRYPICK_SEQUENCE,
        GIT_REPOSITORY_STATE_BISECT,
        GIT_REPOSITORY_STATE_REBASE,
        GIT_REPOSITORY_STATE_REBASE_INTERACTIVE,
        GIT_REPOSITORY_STATE_REBASE_MERGE,
        GIT_REPOSITORY_STATE_APPLY_MAILBOX,
        GIT_REPOSITORY_STATE_APPLY_MAILBOX_OR_REBASE,
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Open a git repository.
        ///
        ///  <para>
        ///   The 'path' argument must point to either a git
        ///   repository folder, or an existing work dir.
        ///  </para>
        ///
        ///  <para>
        ///   The method will automatically detect if 'path' is a
        ///   normal or bare repository or fail is 'path' is neither.
        ///  </para>
        ///
        ///  <para>
        ///   Note that the libgit2 library _must_ be initialized
        ///   using `git_libgit2_init` before any APIs can be called,
        ///   including this one.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  pointer to the repo which will be opened
        /// </param>
        /// <param name="path">the path to the repository</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_open(
            out git_repository* _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path);

        /// <summary>
        ///  Open working tree as a repository
        ///
        ///  <para>
        ///   Open the working directory of the working tree as a
        ///   normal repository that can then be worked on.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Output pointer containing opened repository
        /// </param>
        /// <param name="wt">Working tree to open</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_open_from_worktree(
            out git_repository* _out,
            git_worktree* wt);

        /// <summary>
        ///  Create a "fake" repository to wrap an object database
        ///
        ///  <para>
        ///   Create a repository object to wrap an object database to
        ///   be used with the API when all you have is an object
        ///   database. This doesn't have any paths associated with
        ///   it, so use with care.
        ///  </para>
        /// </summary>
        /// <param name="_out">pointer to the repo</param>
        /// <param name="odb">the object database to wrap</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_wrap_odb(
            out git_repository* _out,
            git_odb* odb);

        /// <summary>
        ///  Look for a git repository and copy its path in the given
        ///  buffer. The lookup start from base_path and walk across
        ///  parent directories if nothing has been found. The lookup
        ///  ends when the first repository is found, or when reaching a
        ///  directory referenced in ceiling_dirs or when the filesystem
        ///  changes (in case across_fs is true).
        ///
        ///  <para>
        ///   The method will automatically detect if the repository
        ///   is bare (if there is a repository).
        ///  </para>
        ///
        ///  <para>
        ///   Note that the libgit2 library _must_ be initialized
        ///   using `git_libgit2_init` before any APIs can be called,
        ///   including this one.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  A pointer to a user-allocated git_buf which will contain the
        ///  found path.
        /// </param>
        /// <param name="start_path">
        ///  The base path where the lookup starts.
        /// </param>
        /// <param name="across_fs">
        ///  If true, then the lookup will not stop when a filesystem
        ///  device change is detected while exploring parent
        ///  directories.
        /// </param>
        /// <param name="ceiling_dirs">
        ///  A GIT_PATH_LIST_SEPARATOR separated list of absolute
        ///  symbolic link free paths. The lookup will stop when any of
        ///  this paths is reached. Note that the lookup always performs
        ///  on start_path no matter start_path appears in ceiling_dirs
        ///  ceiling_dirs might be NULL (which is equivalent to an empty
        ///  string)
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_discover(
            ref git_buf _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string start_path,
            int across_fs,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string ceiling_dirs);

        /// <summary>
        ///  Find and open a repository with extended controls.
        ///
        ///  <para>
        ///   Note that the libgit2 library _must_ be initialized
        ///   using `git_libgit2_init` before any APIs can be called,
        ///   including this one.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Pointer to the repo which will be opened. This can actually
        ///  be NULL if you only want to use the error code to see if a
        ///  repo at this path could be opened.
        /// </param>
        /// <param name="path">
        ///  Path to open as git repository. If the flags permit
        ///  "searching", then this can be a path to a subdirectory
        ///  inside the working directory of the repository. May be NULL
        ///  if flags is GIT_REPOSITORY_OPEN_FROM_ENV.
        /// </param>
        /// <param name="flags">
        ///  A combination of the GIT_REPOSITORY_OPEN flags above.
        /// </param>
        /// <param name="ceiling_dirs">
        ///  A GIT_PATH_LIST_SEPARATOR delimited list of path prefixes at
        ///  which the search for a containing repository should
        ///  terminate.
        /// </param>
        /// <returns>
        ///  0 on success, GIT_ENOTFOUND if no repository could be found,
        ///  or -1 if there was a repository but open failed for some
        ///  reason (such as repo corruption or system errors).
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_open_ext(
            out git_repository* _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            uint flags,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string ceiling_dirs);

        /// <summary>
        ///  Open a bare repository on the serverside.
        ///
        ///  <para>
        ///   This is a fast open for bare repositories that will come
        ///   in handy if you're e.g. hosting git repositories and
        ///   need to access them efficiently
        ///  </para>
        ///
        ///  <para>
        ///   Note that the libgit2 library _must_ be initialized
        ///   using `git_libgit2_init` before any APIs can be called,
        ///   including this one.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Pointer to the repo which will be opened.
        /// </param>
        /// <param name="bare_path">
        ///  Direct path to the bare repository
        /// </param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_open_bare(
            out git_repository* _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string bare_path);

        /// <summary>
        ///  Free a previously allocated repository
        ///
        ///  <para>
        ///   Note that after a repository is free'd, all the objects
        ///   it has spawned will still exist until they are manually
        ///   closed by the user with `git_object_free`, but accessing
        ///   any of the attributes of an object without a backing
        ///   repository will result in undefined behavior
        ///  </para>
        /// </summary>
        /// <param name="repo">
        ///  repository handle to close. If NULL nothing occurs.
        /// </param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_repository_free(
            git_repository* repo);

        /// <summary>
        ///  Creates a new Git repository in the given folder.
        ///
        ///  <para>
        ///   TODO: - Reinit the repository
        ///  </para>
        ///
        ///  <para>
        ///   Note that the libgit2 library _must_ be initialized
        ///   using `git_libgit2_init` before any APIs can be called,
        ///   including this one.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  pointer to the repo which will be created or reinitialized
        /// </param>
        /// <param name="path">the path to the repository</param>
        /// <param name="is_bare">
        ///  if true, a Git repository without a working directory is
        ///  created at the pointed path. If false, provided path will be
        ///  considered as the working directory into which the .git
        ///  directory will be created.
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_init(
            out git_repository* _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            uint is_bare);

        /// <summary>
        ///  Initialize git_repository_init_options structure
        ///
        ///  <para>
        ///   Initializes a `git_repository_init_options` with default
        ///   values. Equivalent to creating an instance with
        ///   `GIT_REPOSITORY_INIT_OPTIONS_INIT`.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_repository_init_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass
        ///  `GIT_REPOSITORY_INIT_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_init_options_init(
            ref git_repository_init_options opts,
            uint version);

        /// <summary>
        ///  Create a new Git repository in the given folder with
        ///  extended controls.
        ///
        ///  <para>
        ///   This will initialize a new git repository (creating the
        ///   repo_path if requested by flags) and working directory
        ///   as needed. It will auto-detect the case sensitivity of
        ///   the file system and if the file system supports file
        ///   mode bits correctly.
        ///  </para>
        ///
        ///  <para>
        ///   Note that the libgit2 library _must_ be initialized
        ///   using `git_libgit2_init` before any APIs can be called,
        ///   including this one.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Pointer to the repo which will be created or reinitialized.
        /// </param>
        /// <param name="repo_path">The path to the repository.</param>
        /// <param name="opts">
        ///  Pointer to git_repository_init_options struct.
        /// </param>
        /// <returns>0 or an error code on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_init_ext(
            out git_repository* _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string repo_path,
            ref git_repository_init_options opts);

        /// <summary>
        ///  Retrieve and resolve the reference pointed at by HEAD.
        ///
        ///  <para>
        ///   The returned `git_reference` will be owned by caller and
        ///   `git_reference_free()` must be called when done with it
        ///   to release the allocated memory and prevent a leak.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  pointer to the reference which will be retrieved
        /// </param>
        /// <param name="repo">a repository object</param>
        /// <returns>
        ///  0 on success, GIT_EUNBORNBRANCH when HEAD points to a non
        ///  existing branch, GIT_ENOTFOUND when HEAD is missing; an
        ///  error code otherwise
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_head(
            out git_reference* _out,
            git_repository* repo);

        /// <summary>
        ///  Retrieve the referenced HEAD for the worktree
        /// </summary>
        /// <param name="_out">
        ///  pointer to the reference which will be retrieved
        /// </param>
        /// <param name="repo">a repository object</param>
        /// <param name="name">
        ///  name of the worktree to retrieve HEAD for
        /// </param>
        /// <returns>0 when successful, error-code otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_head_for_worktree(
            out git_reference* _out,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Check if a repository's HEAD is detached
        ///
        ///  <para>
        ///   A repository's HEAD is detached when it points directly
        ///   to a commit instead of a branch.
        ///  </para>
        /// </summary>
        /// <param name="repo">Repo to test</param>
        /// <returns>
        ///  1 if HEAD is detached, 0 if it's not; error code if there
        ///  was an error.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_head_detached(
            git_repository* repo);

        /// <summary>
        ///  Check if a worktree's HEAD is detached
        ///
        ///  <para>
        ///   A worktree's HEAD is detached when it points directly to
        ///   a commit instead of a branch.
        ///  </para>
        /// </summary>
        /// <param name="repo">a repository object</param>
        /// <param name="name">
        ///  name of the worktree to retrieve HEAD for
        /// </param>
        /// <returns>
        ///  1 if HEAD is detached, 0 if its not; error code if there was
        ///  an error
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_head_detached_for_worktree(
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Check if the current branch is unborn
        ///
        ///  <para>
        ///   An unborn branch is one named from HEAD but which
        ///   doesn't exist in the refs namespace, because it doesn't
        ///   have any commit to point to.
        ///  </para>
        /// </summary>
        /// <param name="repo">Repo to test</param>
        /// <returns>
        ///  1 if the current branch is unborn, 0 if it's not; error code
        ///  if there was an error
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_head_unborn(
            git_repository* repo);

        /// <summary>
        ///  Check if a repository is empty
        ///
        ///  <para>
        ///   An empty repository has just been initialized and
        ///   contains no references apart from HEAD, which must be
        ///   pointing to the unborn master branch, or the branch
        ///   specified for the repository in the `init.defaultBranch`
        ///   configuration variable.
        ///  </para>
        /// </summary>
        /// <param name="repo">Repo to test</param>
        /// <returns>
        ///  1 if the repository is empty, 0 if it isn't, error code if
        ///  the repository is corrupted
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_is_empty(
            git_repository* repo);

        /// <summary>
        ///  Get the location of a specific repository file or directory
        ///
        ///  <para>
        ///   This function will retrieve the path of a specific
        ///   repository item. It will thereby honor things like the
        ///   repository's common directory, gitdir, etc. In case a
        ///   file path cannot exist for a given item (e.g. the
        ///   working directory of a bare repository), GIT_ENOTFOUND
        ///   is returned.
        ///  </para>
        /// </summary>
        /// <param name="_out">Buffer to store the path at</param>
        /// <param name="repo">Repository to get path for</param>
        /// <param name="item">
        ///  The repository item for which to retrieve the path
        /// </param>
        /// <returns>
        ///  0, GIT_ENOTFOUND if the path cannot exist or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_item_path(
            ref git_buf _out,
            git_repository* repo,
            git_repository_item_t item);

        /// <summary>
        ///  Get the path of this repository
        ///
        ///  <para>
        ///   This is the path of the `.git` folder for normal
        ///   repositories, or of the repository itself for bare
        ///   repositories.
        ///  </para>
        /// </summary>
        /// <param name="repo">A repository object</param>
        /// <returns>the path to the repository</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_repository_path(
            git_repository* repo);

        /// <summary>
        ///  Get the path of the working directory for this repository
        ///
        ///  <para>
        ///   If the repository is bare, this function will always
        ///   return NULL.
        ///  </para>
        /// </summary>
        /// <param name="repo">A repository object</param>
        /// <returns>the path to the working dir, if it exists</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_repository_workdir(
            git_repository* repo);

        /// <summary>
        ///  Get the path of the shared common directory for this
        ///  repository.
        ///
        ///  <para>
        ///   If the repository is bare, it is the root directory for
        ///   the repository. If the repository is a worktree, it is
        ///   the parent repo's gitdir. Otherwise, it is the gitdir.
        ///  </para>
        /// </summary>
        /// <param name="repo">A repository object</param>
        /// <returns>the path to the common dir</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_repository_commondir(
            git_repository* repo);

        /// <summary>
        ///  Set the path to the working directory for this repository
        ///
        ///  <para>
        ///   The working directory doesn't need to be the same one
        ///   that contains the `.git` folder for this repository.
        ///  </para>
        ///
        ///  <para>
        ///   If this repository is bare, setting its working
        ///   directory will turn it into a normal repository, capable
        ///   of performing all the common workdir operations
        ///   (checkout, status, index manipulation, etc).
        ///  </para>
        /// </summary>
        /// <param name="repo">A repository object</param>
        /// <param name="workdir">The path to a working directory</param>
        /// <param name="update_gitlink">
        ///  Create/update gitlink in workdir and set config
        ///  "core.worktree" (if workdir is not the parent of the .git
        ///  directory)
        /// </param>
        /// <returns>0, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_set_workdir(
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string workdir,
            int update_gitlink);

        /// <summary>
        ///  Check if a repository is bare
        /// </summary>
        /// <param name="repo">Repo to test</param>
        /// <returns>1 if the repository is bare, 0 otherwise.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_is_bare(
            git_repository* repo);

        /// <summary>
        ///  Check if a repository is a linked work tree
        /// </summary>
        /// <param name="repo">Repo to test</param>
        /// <returns>
        ///  1 if the repository is a linked work tree, 0 otherwise.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_is_worktree(
            git_repository* repo);

        /// <summary>
        ///  Get the configuration file for this repository.
        ///
        ///  <para>
        ///   If a configuration file has not been set, the default
        ///   config set for the repository will be returned,
        ///   including global and system configurations (if they are
        ///   available).
        ///  </para>
        ///
        ///  <para>
        ///   The configuration file must be freed once it's no longer
        ///   being used by the user.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Pointer to store the loaded configuration
        /// </param>
        /// <param name="repo">A repository object</param>
        /// <returns>0, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_config(
            out git_config* _out,
            git_repository* repo);

        /// <summary>
        ///  Get a snapshot of the repository's configuration
        ///
        ///  <para>
        ///   Convenience function to take a snapshot from the
        ///   repository's configuration. The contents of this
        ///   snapshot will not change, even if the underlying config
        ///   files are modified.
        ///  </para>
        ///
        ///  <para>
        ///   The configuration file must be freed once it's no longer
        ///   being used by the user.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Pointer to store the loaded configuration
        /// </param>
        /// <param name="repo">the repository</param>
        /// <returns>0, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_config_snapshot(
            out git_config* _out,
            git_repository* repo);

        /// <summary>
        ///  Get the Object Database for this repository.
        ///
        ///  <para>
        ///   If a custom ODB has not been set, the default database
        ///   for the repository will be returned (the one located in
        ///   `.git/objects`).
        ///  </para>
        ///
        ///  <para>
        ///   The ODB must be freed once it's no longer being used by
        ///   the user.
        ///  </para>
        /// </summary>
        /// <param name="_out">Pointer to store the loaded ODB</param>
        /// <param name="repo">A repository object</param>
        /// <returns>0, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_odb(
            out git_odb* _out,
            git_repository* repo);

        /// <summary>
        ///  Get the Reference Database Backend for this repository.
        ///
        ///  <para>
        ///   If a custom refsdb has not been set, the default
        ///   database for the repository will be returned (the one
        ///   that manipulates loose and packed references in the
        ///   `.git` directory).
        ///  </para>
        ///
        ///  <para>
        ///   The refdb must be freed once it's no longer being used
        ///   by the user.
        ///  </para>
        /// </summary>
        /// <param name="_out">Pointer to store the loaded refdb</param>
        /// <param name="repo">A repository object</param>
        /// <returns>0, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_refdb(
            out git_refdb* _out,
            git_repository* repo);

        /// <summary>
        ///  Get the Index file for this repository.
        ///
        ///  <para>
        ///   If a custom index has not been set, the default index
        ///   for the repository will be returned (the one located in
        ///   `.git/index`).
        ///  </para>
        ///
        ///  <para>
        ///   The index must be freed once it's no longer being used
        ///   by the user.
        ///  </para>
        /// </summary>
        /// <param name="_out">Pointer to store the loaded index</param>
        /// <param name="repo">A repository object</param>
        /// <returns>0, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_index(
            out git_index* _out,
            git_repository* repo);

        /// <summary>
        ///  Retrieve git's prepared message
        ///
        ///  <para>
        ///   Operations such as git revert/cherry-pick/merge with the
        ///   -n option stop just short of creating a commit with the
        ///   changes and save their prepared message in
        ///   .git/MERGE_MSG so the next git-commit execution can
        ///   present it to the user for them to amend if they wish.
        ///  </para>
        ///
        ///  <para>
        ///   Use this function to get the contents of this file.
        ///   Don't forget to remove the file after you create the
        ///   commit.
        ///  </para>
        /// </summary>
        /// <param name="_out">git_buf to write data into</param>
        /// <param name="repo">
        ///  Repository to read prepared message from
        /// </param>
        /// <returns>
        ///  0, GIT_ENOTFOUND if no message exists or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_message(
            ref git_buf _out,
            git_repository* repo);

        /// <summary>
        ///  Remove git's prepared message.
        ///
        ///  <para>
        ///   Remove the message that `git_repository_message`
        ///   retrieves.
        ///  </para>
        /// </summary>
        /// <param name="repo">
        ///  Repository to remove prepared message from.
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_message_remove(
            git_repository* repo);

        /// <summary>
        ///  Remove all the metadata associated with an ongoing command
        ///  like merge, revert, cherry-pick, etc. For example:
        ///  MERGE_HEAD, MERGE_MSG, etc.
        /// </summary>
        /// <param name="repo">A repository object</param>
        /// <returns>0 on success, or error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_state_cleanup(
            git_repository* repo);

        /// <summary>
        ///  Invoke 'callback' for each entry in the given FETCH_HEAD
        ///  file.
        ///
        ///  <para>
        ///   Return a non-zero value from the callback to stop the
        ///   loop.
        ///  </para>
        /// </summary>
        /// <param name="repo">A repository object</param>
        /// <param name="callback">Callback function</param>
        /// <param name="payload">
        ///  Pointer to callback data (optional)
        /// </param>
        /// <returns>
        ///  0 on success, non-zero callback return value, GIT_ENOTFOUND
        ///  if there is no FETCH_HEAD file, or other error code.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_fetchhead_foreach(
            git_repository* repo,
            git_repository_fetchhead_foreach_cb callback,
            IntPtr payload);

        /// <summary>
        ///  If a merge is in progress, invoke 'callback' for each commit
        ///  ID in the MERGE_HEAD file.
        ///
        ///  <para>
        ///   Return a non-zero value from the callback to stop the
        ///   loop.
        ///  </para>
        /// </summary>
        /// <param name="repo">A repository object</param>
        /// <param name="callback">Callback function</param>
        /// <param name="payload">
        ///  Pointer to callback data (optional)
        /// </param>
        /// <returns>
        ///  0 on success, non-zero callback return value, GIT_ENOTFOUND
        ///  if there is no MERGE_HEAD file, or other error code.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_mergehead_foreach(
            git_repository* repo,
            git_repository_mergehead_foreach_cb callback,
            IntPtr payload);

        /// <summary>
        ///  Calculate hash of file using repository filtering rules.
        ///
        ///  <para>
        ///   If you simply want to calculate the hash of a file on
        ///   disk with no filters, you can just use the
        ///   `git_odb_hashfile()` API. However, if you want to hash a
        ///   file in the repository and you want to apply filtering
        ///   rules (e.g. crlf filters) before generating the SHA,
        ///   then use this function.
        ///  </para>
        ///
        ///  <para>
        ///   Note: if the repository has `core.safecrlf` set to fail
        ///   and the filtering triggers that failure, then this
        ///   function will return an error and not calculate the hash
        ///   of the file.
        ///  </para>
        /// </summary>
        /// <param name="_out">Output value of calculated SHA</param>
        /// <param name="repo">Repository pointer</param>
        /// <param name="path">
        ///  Path to file on disk whose contents should be hashed. This
        ///  may be an absolute path or a relative path, in which case it
        ///  will be treated as a path within the working directory.
        /// </param>
        /// <param name="type">
        ///  The object type to hash as (e.g. GIT_OBJECT_BLOB)
        /// </param>
        /// <param name="as_path">
        ///  The path to use to look up filtering rules. If this is an
        ///  empty string then no filters will be applied when
        ///  calculating the hash. If this is `NULL` and the `path`
        ///  parameter is a file within the repository's working
        ///  directory, then the `path` will be used.
        /// </param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_hashfile(
            ref git_oid _out,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            git_object_t type,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string as_path);

        /// <summary>
        ///  Make the repository HEAD point to the specified reference.
        ///
        ///  <para>
        ///   If the provided reference points to a Tree or a Blob,
        ///   the HEAD is unaltered and -1 is returned.
        ///  </para>
        ///
        ///  <para>
        ///   If the provided reference points to a branch, the HEAD
        ///   will point to that branch, staying attached, or become
        ///   attached if it isn't yet. If the branch doesn't exist
        ///   yet, no error will be return. The HEAD will then be
        ///   attached to an unborn branch.
        ///  </para>
        ///
        ///  <para>
        ///   Otherwise, the HEAD will be detached and will directly
        ///   point to the Commit.
        ///  </para>
        /// </summary>
        /// <param name="repo">Repository pointer</param>
        /// <param name="refname">
        ///  Canonical name of the reference the HEAD should point at
        /// </param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_set_head(
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string refname);

        /// <summary>
        ///  Make the repository HEAD directly point to the Commit.
        ///
        ///  <para>
        ///   If the provided committish cannot be found in the
        ///   repository, the HEAD is unaltered and GIT_ENOTFOUND is
        ///   returned.
        ///  </para>
        ///
        ///  <para>
        ///   If the provided committish cannot be peeled into a
        ///   commit, the HEAD is unaltered and -1 is returned.
        ///  </para>
        ///
        ///  <para>
        ///   Otherwise, the HEAD will eventually be detached and will
        ///   directly point to the peeled Commit.
        ///  </para>
        /// </summary>
        /// <param name="repo">Repository pointer</param>
        /// <param name="committish">
        ///  Object id of the Commit the HEAD should point to
        /// </param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_set_head_detached(
            git_repository* repo,
            ref git_oid committish);

        /// <summary>
        ///  Make the repository HEAD directly point to the Commit.
        ///
        ///  <para>
        ///   This behaves like `git_repository_set_head_detached()`
        ///   but takes an annotated commit, which lets you specify
        ///   which extended sha syntax string was specified by a
        ///   user, allowing for more exact reflog messages.
        ///  </para>
        ///
        ///  <para>
        ///   See the documentation for
        ///   `git_repository_set_head_detached()`.
        ///  </para>
        /// </summary>
        /// <param name="repo">Repository pointer</param>
        /// <param name="committish">
        ///  annotated commit to point HEAD to
        /// </param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_set_head_detached_from_annotated(
            git_repository* repo,
            git_annotated_commit* committish);

        /// <summary>
        ///  Detach the HEAD.
        ///
        ///  <para>
        ///   If the HEAD is already detached and points to a Commit,
        ///   0 is returned.
        ///  </para>
        ///
        ///  <para>
        ///   If the HEAD is already detached and points to a Tag, the
        ///   HEAD is updated into making it point to the peeled
        ///   Commit, and 0 is returned.
        ///  </para>
        ///
        ///  <para>
        ///   If the HEAD is already detached and points to a non
        ///   committish, the HEAD is unaltered, and -1 is returned.
        ///  </para>
        ///
        ///  <para>
        ///   Otherwise, the HEAD will be detached and point to the
        ///   peeled Commit.
        ///  </para>
        /// </summary>
        /// <param name="repo">Repository pointer</param>
        /// <returns>
        ///  0 on success, GIT_EUNBORNBRANCH when HEAD points to a non
        ///  existing branch or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_detach_head(
            git_repository* repo);

        /// <summary>
        ///  Determines the status of a git repository - ie, whether an
        ///  operation (merge, cherry-pick, etc) is in progress.
        /// </summary>
        /// <param name="repo">Repository pointer</param>
        /// <returns>The state of the repository</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_state(
            git_repository* repo);

        /// <summary>
        ///  Sets the active namespace for this Git Repository
        ///
        ///  <para>
        ///   This namespace affects all reference operations for the
        ///   repo. See `man gitnamespaces`
        ///  </para>
        /// </summary>
        /// <param name="repo">The repo</param>
        /// <param name="nmspace">
        ///  The namespace. This should not include the refs folder, e.g.
        ///  to namespace all references under `refs/namespaces/foo/`,
        ///  use `foo` as the namespace.
        /// </param>
        /// <returns>0 on success, -1 on error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_set_namespace(
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string nmspace);

        /// <summary>
        ///  Get the currently active namespace for this repository
        /// </summary>
        /// <param name="repo">The repo</param>
        /// <returns>
        ///  the active namespace, or NULL if there isn't one
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_repository_get_namespace(
            git_repository* repo);

        /// <summary>
        ///  Determine if the repository was a shallow clone
        /// </summary>
        /// <param name="repo">The repository</param>
        /// <returns>1 if shallow, zero if not</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_is_shallow(
            git_repository* repo);

        /// <summary>
        ///  Retrieve the configured identity to use for reflogs
        ///
        ///  <para>
        ///   The memory is owned by the repository and must not be
        ///   freed by the user.
        ///  </para>
        /// </summary>
        /// <param name="name">
        ///  where to store the pointer to the name
        /// </param>
        /// <param name="email">
        ///  where to store the pointer to the email
        /// </param>
        /// <param name="repo">the repository</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_ident(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))] out string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))] out string email,
            git_repository* repo);

        /// <summary>
        ///  Set the identity to be used for writing reflogs
        ///
        ///  <para>
        ///   If both are set, this name and email will be used to
        ///   write to the reflog. Pass NULL to unset. When unset, the
        ///   identity will be taken from the repository's
        ///   configuration.
        ///  </para>
        /// </summary>
        /// <param name="repo">the repository to configure</param>
        /// <param name="name">
        ///  the name to use for the reflog entries
        /// </param>
        /// <param name="email">
        ///  the email to use for the reflog entries
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_set_ident(
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string email);

        /// <summary>
        ///  Gets the object type used by this repository.
        /// </summary>
        /// <param name="repo">the repository</param>
        /// <returns>the object id type</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid_t git_repository_oid_type(
            git_repository* repo);

        /// <summary>
        ///  Gets the parents of the next commit, given the current
        ///  repository state. Generally, this is the HEAD commit, except
        ///  when performing a merge, in which case it is two or more
        ///  commits.
        /// </summary>
        /// <param name="commits">
        ///  a `git_commitarray` that will contain the commit parents
        /// </param>
        /// <param name="repo">the repository</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_commit_parents(
            ref git_commitarray commits,
            git_repository* repo);
    }
}
