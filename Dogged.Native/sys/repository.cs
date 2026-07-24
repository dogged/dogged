using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  The options for creating an repository from scratch.
    ///
    ///  <para>
    ///   Initialize with `GIT_REPOSITORY_NEW_OPTIONS_INIT`.
    ///   Alternatively, you can use
    ///   `git_repository_new_options_init`.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_repository_new_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_REPOSITORY_NEW_OPTIONS_VERSION = 1;


        /// <summary>
        ///  The version
        /// </summary>
        public uint version;

        /// <summary>
        ///  The object ID type for the object IDs that exist in the
        ///  index.
        ///
        ///  <para>
        ///   If this is not specified, this defaults to
        ///   `GIT_OID_SHA1`.
        ///  </para>
        /// </summary>
        public git_oid_t oid_type;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Initialize git_repository_new_options structure
        ///
        ///  <para>
        ///   Initializes a `git_repository_new_options` with default
        ///   values. Equivalent to creating an instance with
        ///   `GIT_REPOSITORY_NEW_OPTIONS_INIT`.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_repository_new_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass
        ///  `GIT_REPOSITORY_NEW_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_new_options_init(
            ref git_repository_new_options opts,
            uint version);

        /// <summary>
        ///  Create a new repository with neither backends nor config
        ///  object
        ///
        ///  <para>
        ///   Note that this is only useful if you wish to associate
        ///   the repository with a non-filesystem-backed object
        ///   database and config store.
        ///  </para>
        ///
        ///  <para>
        ///   Caveats: since this repository has no physical location,
        ///   some systems can fail to function properly: locations
        ///   under $GIT_DIR, $GIT_COMMON_DIR, or $GIT_INFO_DIR are
        ///   impacted.
        ///  </para>
        /// </summary>
        /// <param name="_out">The blank repository</param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_new(
            out git_repository* _out);

        /// <summary>
        ///  Reset all the internal state in a repository.
        ///
        ///  <para>
        ///   This will free all the mapped memory and internal
        ///   objects of the repository and leave it in a "blank"
        ///   state.
        ///  </para>
        ///
        ///  <para>
        ///   There's no need to call this function directly unless
        ///   you're trying to aggressively cleanup the repo before
        ///   its deallocation. `git_repository_free` already performs
        ///   this operation before deallocating the repo.
        ///  </para>
        /// </summary>
        /// <param name="repo">The repository to clean up</param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository__cleanup(
            git_repository* repo);

        /// <summary>
        ///  Update the filesystem config settings for an open repository
        ///
        ///  <para>
        ///   When a repository is initialized, config values are set
        ///   based on the properties of the filesystem that the
        ///   repository is on, such as "core.ignorecase",
        ///   "core.filemode", "core.symlinks", etc. If the repository
        ///   is moved to a new filesystem, these properties may no
        ///   longer be correct and API calls may not behave as
        ///   expected. This call reruns the phase of repository
        ///   initialization that sets those properties to compensate
        ///   for the current filesystem of the repo.
        ///  </para>
        /// </summary>
        /// <param name="repo">A repository object</param>
        /// <param name="recurse_submodules">
        ///  Should submodules be updated recursively
        /// </param>
        /// <returns>0 on success, < 0 on error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_reinit_filesystem(
            git_repository* repo,
            int recurse_submodules);

        /// <summary>
        ///  Set the configuration file for this repository
        ///
        ///  <para>
        ///   This configuration file will be used for all
        ///   configuration queries involving this repository.
        ///  </para>
        ///
        ///  <para>
        ///   The repository will keep a reference to the config file;
        ///   the user must still free the config after setting it to
        ///   the repository, or it will leak.
        ///  </para>
        /// </summary>
        /// <param name="repo">A repository object</param>
        /// <param name="config">A Config object</param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_set_config(
            git_repository* repo,
            git_config* config);

        /// <summary>
        ///  Set the Object Database for this repository
        ///
        ///  <para>
        ///   The ODB will be used for all object-related operations
        ///   involving this repository.
        ///  </para>
        ///
        ///  <para>
        ///   The repository will keep a reference to the ODB; the
        ///   user must still free the ODB object after setting it to
        ///   the repository, or it will leak.
        ///  </para>
        /// </summary>
        /// <param name="repo">A repository object</param>
        /// <param name="odb">An ODB object</param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_set_odb(
            git_repository* repo,
            git_odb* odb);

        /// <summary>
        ///  Set the Reference Database Backend for this repository
        ///
        ///  <para>
        ///   The refdb will be used for all reference related
        ///   operations involving this repository.
        ///  </para>
        ///
        ///  <para>
        ///   The repository will keep a reference to the refdb; the
        ///   user must still free the refdb object after setting it
        ///   to the repository, or it will leak.
        ///  </para>
        /// </summary>
        /// <param name="repo">A repository object</param>
        /// <param name="refdb">An refdb object</param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_set_refdb(
            git_repository* repo,
            git_refdb* refdb);

        /// <summary>
        ///  Set the index file for this repository
        ///
        ///  <para>
        ///   This index will be used for all index-related operations
        ///   involving this repository.
        ///  </para>
        ///
        ///  <para>
        ///   The repository will keep a reference to the index file;
        ///   the user must still free the index after setting it to
        ///   the repository, or it will leak.
        ///  </para>
        /// </summary>
        /// <param name="repo">A repository object</param>
        /// <param name="index">An index object</param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_set_index(
            git_repository* repo,
            git_index* index);

        /// <summary>
        ///  Set a repository to be bare.
        ///
        ///  <para>
        ///   Clear the working directory and set core.bare to true.
        ///   You may also want to call
        ///   `git_repository_set_index(repo, NULL)` since a bare repo
        ///   typically does not have an index, but this function will
        ///   not do that for you.
        ///  </para>
        /// </summary>
        /// <param name="repo">Repo to make bare</param>
        /// <returns>0 on success, < 0 on failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_set_bare(
            git_repository* repo);

        /// <summary>
        ///  Load and cache all submodules.
        ///
        ///  <para>
        ///   Because the `.gitmodules` file is unstructured, loading
        ///   submodules is an O(N) operation. Any operation (such as
        ///   `git_rebase_init`) that requires accessing all
        ///   submodules is O(N^2) in the number of submodules, if it
        ///   has to look each one up individually. This function
        ///   loads all submodules and caches them so that subsequent
        ///   calls to `git_submodule_lookup` are O(1).
        ///  </para>
        /// </summary>
        /// <param name="repo">
        ///  the repository whose submodules will be cached.
        /// </param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_submodule_cache_all(
            git_repository* repo);

        /// <summary>
        ///  Clear the submodule cache.
        ///
        ///  <para>
        ///   Clear the submodule cache populated by
        ///   `git_repository_submodule_cache_all`. If there is no
        ///   cache, do nothing.
        ///  </para>
        ///
        ///  <para>
        ///   The cache incorporates data from the repository's
        ///   configuration, as well as the state of the working tree,
        ///   the index, and HEAD. So any time any of these has
        ///   changed, the cache might become invalid.
        ///  </para>
        /// </summary>
        /// <param name="repo">
        ///  the repository whose submodule cache will be cleared
        /// </param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_submodule_cache_clear(
            git_repository* repo);
    }
}
