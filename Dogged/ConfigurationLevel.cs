using System;

namespace Dogged
{
    /// <summary>
    /// Priority level of a config file.  These priority levels correspond
    /// to the natural escalation logic (from higher to lower) when
    /// searching for config entries in git.
    /// </summary>
    public enum ConfigurationLevel
    {
        /// <summary>
        /// System-wide on Windows, for compatibility with portable git
        /// </summary>
        ProgramData = 1,

        /// <summary>
        /// System-wide configuration file; /etc/gitconfig on Linux
        /// systems.
        /// </summary>
        System = 2,

        /// <summary>
        /// XDG compatible configuration file; typically
        /// ~/.config/git/config
        /// </summary>
        XDG = 3,

        /// <summary>
        /// User-specific configuration file (also called Global
        /// configuration file); typically ~/.gitconfig
        /// </summary>
        Global = 4,

        /// <summary>
        /// Repository specific configuration file;
        /// $WORK_DIR/.git/config on non-bare repos.
        /// </summary>
        Local = 5,

        /// <summary>
        /// Application specific configuration file; freely defined by
        /// applications.
        /// </summary>
        Application = 6,

        /// <summary>
        /// Represents the highest level available config file (i.e. the
        /// most specific config file available that actually is loaded).
        /// </summary>
        HighestLevel = -1,
    }
}
