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

        /// <summary>
        /// When set, filters will be loaded from a `.gitattributes` file
        /// in the specified commit.
        /// </summary>
        GIT_BLOB_FILTER_ATTTRIBUTES_FROM_COMMIT = (1 << 3),
    }

    /// <summary>
    /// The options used when applying filter options to a file.
    /// Initialize with `GIT_BLOB_FILTER_OPTIONS_INIT`.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_blob_filter_options
    {
        /// <summary>
        /// Version of the options.
        /// </summary>
        public int version;

        /// <summary>
        /// Flags to control the filtering process, see
        /// `git_blob_filter_flag_t` above.
        /// </summary>
        public git_blob_filter_flag_t flags;

        public IntPtr reserved;

        /// <summary>
        /// The commit to load attributes from, when
        /// `GIT_BLOB_FILTER_ATTRIBUTES_FROM_COMMIT` is specified.
        /// </summary>
        public git_oid attr_commit_id;

        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static int GIT_BLOB_FILTER_OPTIONS_VERSION
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// The default values for our options structure.
        /// </summary>
        public static git_blob_filter_options GIT_BLOB_FILTER_OPTIONS_INIT
        {
            get
            {
                return new git_blob_filter_options() {
                    version = GIT_BLOB_FILTER_OPTIONS_VERSION,
                    flags = git_blob_filter_flag_t.GIT_BLOB_FILTER_CHECK_FOR_BINARY
                };
            }
        }
    }
}
