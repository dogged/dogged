using System;

namespace Dogged.Native
{

    /// <summary>
    /// Priority level of a config file.
    ///
    /// <para>
    /// These priority levels correspond to the natural escalation logic
    /// (from higher to lower) when searching for config entries in git.
    ///
    /// <see cref="git_config_open_default"/> and
    /// <see cref="git_repository_config"/> honor those priority levels
    /// as well.
    /// </para>
    /// </summary>
    public enum git_config_level_t
    {
        /// <summary>
        /// System-wide on Windows, for compatibility with portable git
        /// </summary>
        GIT_CONFIG_LEVEL_PROGRAMDATA = 1,

        /// <summary>
        /// System-wide configuration file; /etc/gitconfig on Linux
        /// systems.
        /// </summary>
        GIT_CONFIG_LEVEL_SYSTEM = 2,

        /// <summary>
        /// XDG compatible configuration file; typically
        /// ~/.config/git/config
        /// </summary>
        GIT_CONFIG_LEVEL_XDG = 3,

        /// <summary>
        /// User-specific configuration file (also called Global
        /// configuration file); typically ~/.gitconfig
        /// </summary>
        GIT_CONFIG_LEVEL_GLOBAL = 4,

        /// <summary>
        /// Repository specific configuration file;
        /// $WORK_DIR/.git/config on non-bare repos.
        /// </summary>
        GIT_CONFIG_LEVEL_LOCAL = 5,

        /// <summary>
        /// Application specific configuration file; freely defined by
        /// applications.
        /// </summary>
        GIT_CONFIG_LEVEL_APP = 6,

        /// <summary>
        /// Represents the highest level available config file (i.e. the
        /// most specific config file available that actually is loaded).
        /// </summary>
        GIT_CONFIG_HIGHEST_LEVEL = -1,
    }
}
