using System;

namespace Dogged.Native
{
    /// <summary>
    /// Combinations of these values describe the built-in features of
    /// libgit2.
    /// </summary>
    public enum git_feature_t
    {
        /// <summary>
        /// If set, libgit2 was built thread-aware and can be safely used
        /// from multiple threads.
        /// </summary>
        GIT_FEATURE_THREADS = (1 << 0),

        /// <summary>
        /// If set, libgit2 was built with and linked against a TLS
        /// implementation.  Custom TLS streams may still be added by the
        /// user to support HTTPS regardless of this.
        /// </summary>
        GIT_FEATURE_HTTPS = (1 << 1),

        /// <summary>
        /// If set, libgit2 was built with and linked against libssh2. A
        /// custom transport may still be added by the user to support
        /// ssh regardless of this.
        /// </summary>
        GIT_FEATURE_SSH     = (1 << 2),

        /// <summary>
        /// If set, libgit2 was built with support for sub-second resolution
        /// in file modification times.
        /// </summary>
        GIT_FEATURE_NSEC    = (1 << 3),
    }
}
