using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
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
        ///  from the `stat` system call).  For example, searching in a
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
        ///  $GIT_DISCOVERY_ACROSS_FILESYSTEM.  The opened repository
        ///  will respect $GIT_INDEX_FILE, $GIT_NAMESPACE,
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
    /// Representation of an existing git repository.
    /// </summary>
    public struct git_repository { };

    public static partial class libgit2
    {
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_discover(
            git_buf path,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string start_path,
            int across_fs,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string ceiling_dirs);

        /// <summary>
        /// Retrieve and resolve the reference pointed to by HEAD.
        ///
        /// <para>
        /// The returned <see cref="git_reference"/> will be owned by the
        /// caller and <see cref="git_reference_free"/> must be called when
        /// done to release the allocated memory.
        /// </para>
        /// </summary>
        /// <param name="reference">Pointer to the reference that will be retrieved</param>
        /// <param name="repo">The repository object</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_head(
            out git_reference* reference, git_repository* repo);

        /// <summary>
        /// Indicates if the repository's HEAD is detached.  A repository's
        /// HEAD is detached when it points directly to a commit instead
        /// of a branch.
        /// </summary>
        /// <param name="repo">The repository object</param>
        /// <returns>1 if the HEAD is detached, 0 if it's not, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_head_detached(
            git_repository* repo);

        /// <summary>
        /// Creates a new Git repository in the given folder.
        /// </summary>
        /// <param name="repo">Pointer to the repository that will be created.</param>
        /// <param name="path">The path to the repository</param>
        /// <param name="bare">If non-zero, a Git repository without a working directory is created at the given path. If zero, the provided path will be considered as the working directory into which the .git directory will be created.</param>
        /// <returns>0 on success or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_init(
            out git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            uint bare);

        /// <summary>
        /// Checks if a repository is bare.
        /// </summary>
        /// <param name="repo">Repository to test</param>
        /// <returns>1 if the repository is bare, 0 otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_is_bare(
            git_repository* repo);

        /// <summary>
        /// Get the index file for this repository.
        ///
        /// <para>
        /// If a custom index has not been set, the default index for the
        /// repository will be returned (the one located in `.git/index`).
        /// </para>
        ///
        /// <para>
        /// The index must be freed once it's no longer being used by the
        /// user.
        /// </para>
        /// </summary>
        /// <param name="index">Pointer to the index that will be opened</param>
        /// <param name="repo">The repository object</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_index(out git_index* index, git_repository* repo);

        /// <summary>
        /// Create a new repository with neither backends nor config object
        ///
        /// <para>
        /// Note that this is only useful if you wish to associate the repository
        /// with a non-filesystem-backed object database and config store.
        /// </para>
        /// </summary>
        /// <param name="repo">Pointer to the repository that will be created.</param>
        /// <returns>0 on success or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_new(out git_repository *repo);

        /// <summary>
        /// Get the object database ("ODB") for this repository.
        ///
        /// <para>
        /// If a custom ODB has not been set, the default database for the
        /// repository will be returned (the one located in `.git/objects`).
        /// </para>
        ///
        /// <para>
        /// The ODB must be freed once it's no longer being used by the user.
        /// </para>
        /// </summary>
        /// <param name="odb">Pointer to store the loaded ODB</param>
        /// <param name="repo">A repository object</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_odb(out git_odb* odb, git_repository* repo);

        /// <summary>
        /// Open a git repository.
        ///
        /// <para>
        /// The <paramref name="path"/> argument must point to either a Git
        /// repository folder, or an existing working tree.
        /// </para>
        ///
        /// <para>
        /// The method will automatically detect if <paramref name="path"/>
        /// is a working tree or a bare repository.  It will fail if the
        /// given path is neither.
        /// </para>
        /// </summary>
        /// <param name="repo">Pointer to the repository that will be opened.</param>
        /// <param name="path">The path to the repository</param>
        /// <returns>0 on success or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_open(
            out git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path);

        /// <summary>
        ///  Find and open a repository with extended controls.
        /// </summary>
        /// <param name="repository_out">
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
            out git_repository* repository_out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            uint flags,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string ceiling_dirs);

        /// <summary>
        /// Get the path of this repository
        ///
        /// <para>
        /// This is the path of the `.git` folder for normal
        /// repositories, or of the repository itself for bare
        /// repositories.
        /// </para>
        /// </summary>
        /// <param name="repo">A repository object</param>
        /// <returns>the path to the repository</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_repository_path(git_repository* repository);

        /// <summary>
        /// Set the index file for this repository
        ///
        /// <para>
        /// This index will be used for all index-related operations
        /// involving this repository.
        /// </para>
        ///
        /// <para>
        /// The repository will keep a reference to the index file;
        /// the user must still free the index after setting it
        /// to the repository, or it will leak.
        /// </para>
        /// <param name="repo">The repository to configure</param>
        /// <param name="odb">The object database to set</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_repository_set_index(git_repository* repo, git_index* index);

        /// <summary>
        /// Set the Object Database for this repository
        ///
        /// <para>
        /// The ODB will be used for all object-related operations
        /// involving this repository.
        /// </para>
        ///
        /// <para>
        /// The repository will keep a reference to the ODB; the user
        /// must still free the ODB object after setting it to the
        /// repository, or it will leak.
        /// </para>
        /// </summary>
        /// <param name="repo">The repository to configure</param>
        /// <param name="odb">The object database to set</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_repository_set_odb(git_repository* repo, git_odb* odb);

        /// <summary>
        /// Set the path to the working directory for this repository
        ///
        /// <para>
        /// The working directory doesn't need to be the same one
        /// that contains the `.git` folder for this repository.
        /// </para>
        ///
        /// <para>
        /// If this repository is bare, setting its working directory
        /// will turn it into a normal repository, capable of performing
        /// all the common workdir operations (checkout, status, index
        /// manipulation, etc).
        /// </para>
        /// </summary>
        /// <param name="repo">A repository object</param>
        /// <param name="workdir">The path to a working directory</param>
        /// <param name="update_gitlink">Create/update gitlink in workdir and set config "core.worktree" (if workdir is not the parent of the .git directory)</Param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_set_workdir(
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string workdir,
            int update_gitlink);

        /// <summary>
        /// Close a previously opened repository and free any native memory
        /// that was allocated.
        ///
        /// <para>
        /// Note that after a repository is free'd, all the objects it
        /// has spawned will still exist until they are manually closed
        /// by the user with `git_object_free`, but accessing any of
        /// the attributes of an object without a backing repository
        /// will result in undefined behavior.
        /// </para>
        /// </summary>
        /// <param name="repo">The repository handle to close.</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_repository_free(git_repository* repo);

        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_repository_workdir(git_repository* repository);

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
    }
}
