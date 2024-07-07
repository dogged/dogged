using Dogged;
using Dogged.Native;

/// <summary>
/// Flags to control the way repositories are opened.
/// </summary>
public enum RepositoryOpenFlags
{
    /// <summary>
    ///  Only open the repository if it can be immediately found in
    ///  the start_path. Do not walk up from the start_path looking
    ///  at parent directories.
    /// </summary>
    DoNotSearch = git_repository_open_flag_t.GIT_REPOSITORY_OPEN_NO_SEARCH,

    /// <summary>
    ///  Unless this flag is set, open will not continue searching
    ///  across filesystem boundaries (i.e. when `st_dev` changes
    ///  from the `stat` system call).  For example, searching in a
    ///  user's home directory at "/home/user/source/" will not
    ///  return "/.git/" as the found repo if "/" is a different
    ///  filesystem than "/home".
    /// </summary>
    CrossFilesystems = git_repository_open_flag_t.GIT_REPOSITORY_OPEN_CROSS_FS,

    /// <summary>
    ///  Open repository as a bare repo regardless of core.bare
    ///  config, and defer loading config file for faster setup.
    ///  Unlike `git_repository_open_bare`, this can follow gitlinks.
    /// </summary>
    OpenBare = git_repository_open_flag_t.GIT_REPOSITORY_OPEN_BARE,

    /// <summary>
    ///  Do not check for a repository by appending /.git to the
    ///  start_path; only open the repository if start_path itself
    ///  points to the git directory.
    /// </summary>
    DoNotAppendDotGit = git_repository_open_flag_t.GIT_REPOSITORY_OPEN_NO_DOTGIT,

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
    OpenFromEnvironment = git_repository_open_flag_t.GIT_REPOSITORY_OPEN_FROM_ENV,
}

/// <summary>
/// The options used when opening a repository.
/// </summary>
public class RepositoryOpenOptions
{
    git_repository_open_flag_t nativeFlags;

    /// <summary>
    /// Create an options structure for opening a repository.
    /// </summary>
    public RepositoryOpenOptions()
    {
    }

    /// <summary>
    /// Flags to control opening a repository.
    /// </summary>
    public RepositoryOpenFlags Flags
    {
        get
        {
            return (RepositoryOpenFlags)nativeFlags;
        }
        set
        {
            nativeFlags = (git_repository_open_flag_t)((int)value);
        }
    }

    public string[] CeilingDirectories
    {
        get;
        set;
    }

    internal git_repository_open_flag_t NativeFlags
    {
        get
        {
            return nativeFlags;
        }
    }

    internal string NativeCeilingDirectories
    {
        get
        {
            if (CeilingDirectories == null)
            {
                return null;
            }

            return string.Join(libgit2.GIT_PATH_LIST_SEPARATOR, CeilingDirectories);
        }
    }
}
