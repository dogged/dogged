using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    /// Flags to control the functionality of `git_blob_filter`.
    /// </summary>
    public enum git_blob_filter_flag_t
    {
        /// <summary>
        /// When set, filters will not be applied to binary files.
        /// </summary>
        GIT_BLOB_FILTER_CHECK_FOR_BINARY = (1 << 0),

        /// <summary>
        /// When set, filters will no load configuration from the
        /// system-wide `gitattributes` in `/etc` (or system equivalent).
        /// </summary>
        GIT_BLOB_FILTER_NO_SYSTEM_ATTRIBUTES = (1 << 1),

        /// <summary>
        /// When set, filters will be loaded from a `.gitattributes` file
        /// in the HEAD commit.
        /// </summary>
        GIT_BLOB_FILTER_ATTTRIBUTES_FROM_HEAD = (1 << 2),
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_blob_filter_options
    {
        public int version;
        public git_blob_filter_flag_t flags;
    }
}
