using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    /// An index is a map of paths in the repository to object contents.
    /// The repository has an index which acts as the staging area for
    /// changes to be committed and a cache of in-working tree files.
    /// </summary>
    public struct git_index
    {
        /// <summary>
        /// Bitmask used to extract an index entry's stage from the flags
        /// field.  Once extracted, this value should be shifted by the
        /// <see cref="GIT_INDEX_ENTRY_STAGESHIFT"/> to get the entry's
        /// stage.
        /// </summary>
        public const int GIT_INDEX_ENTRY_STAGEMASK = 0x3000;

        /// <summary>
        /// Bit shift quantity to use to shift the bitmasked stage value
        /// from the flags to get the entry's stage.
        /// </summary>
        public const int GIT_INDEX_ENTRY_STAGESHIFT = 12;
    }

    /// <summary>
    /// Time structure used for a git index entry.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_index_time
    {
        /// <summary>
        /// Time in seconds since the Unix epoch (midnight, January 1, 1970).
        /// </summary>
        public int seconds;

        /// <summary>
        /// Nanoseconds field when using high-resolution time in the index.
        /// </summary>
        public uint nanoseconds;
    }

    /// <summary>
    /// Representation of a file entry in the index.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_index_entry
    {
        /// <summary>
        /// Change time of the working tree file; used to cache information
        /// about the on-disk file.
        /// </summary>
        public git_index_time ctime;

        /// <summary>
        /// Modification time of the working tree file; used to cache
        /// information about the on-disk file.
        /// </summary>
        public git_index_time mtime;

        /// <summary>
        /// Device number of the working tree file; used to cache information
        /// about the on-disk file.
        /// </summary>
        public uint dev;

        /// <summary>
        /// inode of the working tree file; used to cache information about
        /// the on-disk file.
        /// </summary>
        public uint ino;

        /// <summary>
        /// Simplified mode of the file in the index; used both to cache
        /// information about the on-disk file and as the staging information
        /// to create the next commit.  For files this will either be
        /// non-executable (0644) or executable (0755) regardless of the
        /// explicit mode on disk.
        /// </summary>
        public uint mode;

        /// <summary>
        /// Owner uid of the working tree file; used to cache information
        /// about the on-disk file.
        /// </summary>
        public uint uid;

        /// <summary>
        /// Owner gid of the working tree file; used to cache information
        /// about the on-disk file.
        /// </summary>
        public uint gid;

        /// <summary>
        /// Size of the working tree file; used to cache information about
        /// the on-disk file.
        /// </summary>
        public uint file_size;

        /// <summary>
        /// Object ID of the index contents; used as staging information
        /// to create the next commit.
        /// </summary>
        public git_oid id;

        /// <summary>
        /// Flags that specify information about the index entry, including
        /// its stage level.
        /// </summary>
        public ushort flags;

        /// <summary>
        /// Additional flags specifying information about the index entry.
        /// </summary>
        public ushort extended_flags;

        /// <summary>
        /// Path of the file in the index.
        /// </summary>
        public byte* path;
    }

    public enum git_index_entry_flag_t
    {
        /// <summary>
        /// Flag to indicate that there are extended flag entries for this
        /// index entry.
        /// </summary>
        GIT_INDEX_ENTRY_EXTENDED = 0x4000,

        /// <summary>
        /// The working tree data should be assumed to be valid, and
        /// should not be compared to the index; the working tree
        /// should be assumed to be unchanged.
        /// </summary>
        GIT_INDEX_ENTRY_VALID = 0x8000,
    }

    public enum git_index_entry_extended_flag_t
    {
        /// <summary>
        /// This path is intended to be added later.
        /// </summary>
        GIT_INDEX_ENTRY_INTENT_TO_ADD = (1 << 13),

        /// <summary>
        /// The working tree version is assumed to be up-to-date and
        /// the index contents should be used regardless of what's on
        /// disk.
        /// </summary>
        GIT_INDEX_ENTRY_SKIP_WORKTREE = (1 << 14),
    }
}
