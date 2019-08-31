using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    /// An object (blob, commit, tree, etc) in a Git repository.
    /// </summary>
    public struct git_filter_list { };

    /// <summary>
    /// Flags to control the functionality of `git_blob_filter`.
    /// </summary>
    public enum git_filter_mode_t
    {
    	GIT_FILTER_TO_WORKTREE = 0,
        GIT_FILTER_TO_ODB = 1,

        GIT_FILTER_SMUDGE = GIT_FILTER_TO_WORKTREE,
        GIT_FILTER_CLEAN = GIT_FILTER_TO_ODB,
    }

    public enum git_filter_flags_t
    {
        GIT_FILTER_DEFAULT = 0,

        /// <summary>
        /// Don't error for `safecrlf` violations, allow them to continue.
        /// </summary>
        GIT_FILTER_ALLOW_UNSAFE = (1 << 0),

        /// <summary>
        /// When set, filters will no load configuration from the
        /// system-wide `gitattributes` in `/etc` (or system equivalent).
        /// </summary>
        GIT_FILTER_NO_SYSTEM_ATTRIBUTES = (1 << 1),

        /// <summary>
        /// When set, filters will be loaded from a `.gitattributes` file
        /// in the HEAD commit.
        /// </summary>
        GIT_FILTER_ATTRIBUTES_FROM_HEAD = (1 << 2),
    }
}
