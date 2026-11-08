using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
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
            out RepositoryHandle _out);

        /// <summary>
        ///  Create a new repository with no backends.
        /// </summary>
        /// <param name="_out">The blank repository</param>
        /// <param name="opts">
        ///  the options for repository creation, or NULL for defaults
        /// </param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_new_ext(
            out RepositoryHandle _out,
            ref git_repository_new_options opts);

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
            RepositoryHandle repo);

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
            RepositoryHandle repo,
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
            RepositoryHandle repo,
            ConfigHandle config);

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
            RepositoryHandle repo,
            OdbHandle odb);

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
            RepositoryHandle repo,
            RefdbHandle refdb);

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
            RepositoryHandle repo,
            IndexHandle index);

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
            RepositoryHandle repo);

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
            RepositoryHandle repo);

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
            RepositoryHandle repo);
    }
}
