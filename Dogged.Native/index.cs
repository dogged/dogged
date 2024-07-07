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
    /// An object to contain the iteration of a snapshot of an index.
    /// </summary>
    public struct git_index_iterator { };

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

    public static partial class libgit2
    {
        /// <summary>
        /// Create an in-memory index object.
        ///
        /// <para>
        /// This index object cannot be read/written to the filesystem,
        /// but may be used to perform in-memory index operations.
        /// </para>
        ///
        /// <para>
        /// The index must be freed once it's no longer in use.
        /// </para>
        /// </summary>
        /// <param name="index">the pointer for the new index</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_new(out git_index *index);

        /// <summary>
        /// Create a new bare Git index object as a memory representation
        /// of the Git index file in 'index_path', without a repository
        /// to back it.
        ///
        /// <para>
        /// Since there is no ODB or working directory behind this index,
        /// any Index methods which rely on these (e.g. index_add_bypath)
        /// will fail with the GIT_ERROR error code.
        /// </para>
        ///
        /// <para>
        /// If you need to access the index of an actual repository,
        /// use the `git_repository_index` wrapper.
        /// </para>
        ///
        /// <para>
        /// The index must be freed once it's no longer in use.
        /// </para>
        /// </summary>
        /// <param name="index">the pointer for the new index</param>
        /// <param name="path">the path to the index file in disk</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_open(
            out git_index *index,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path);

        /// <summary>
        /// Get a pointer to an entry in the index for the given path at the
        /// given stage level.
        ///
        /// <para>
        /// A "stage level" is a construct for handling conflicted files
        /// during a merge; generally, files are in stage level 0
        /// (sometimes called the "main index"); if a file is in conflict
        /// after a merge, there will be no entry at stage level 0, instead
        /// there will be entries at stages 1-3 representing the conflicting
        /// contents of the common ancestor, the file in "our" branch and
        /// the file in "their" branch.
        /// </para>
        ///
        /// <para>
        /// The entry is not modifiable and should not be freed.  Because the
        /// `git_index_entry` struct is a publicly defined struct, you should
        /// be able to make your own permanent copy of the data if necessary.
        /// </para>
        /// </summary>
        /// <param name="index">The index to read the entry from</param>
        /// <param name="path">The path for the index entry</param>
        /// <param name="stage">The stage level to query</param>
        /// <returns>A pointer to the entry or NULL if out of bounds</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_index_entry* git_index_get_bypath(
            git_index* index,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            int stage);

        /// <summary>
        /// Get a pointer to an entry in the index at the the given position.
        ///
        /// <para>
        /// The entry is not modifiable and should not be freed.  Because the
        /// `git_index_entry` struct is a publicly defined struct, you should
        /// be able to make your own permanent copy of the data if necessary.
        /// </para>
        /// </summary>
        /// <param name="index">The index to read the entry from</param>
        /// <param name="n">The position of the index entry</param>
        /// <returns>A pointer to the entry or NULL if out of bounds</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_index_entry* git_index_get_byindex(git_index* index, UIntPtr n);

        /// <summary>
        /// Get the count of entries currently in the index.
        /// </summary>
        /// <param name="index">The index to read the entry from</param>
        /// <returns>Count of the current entries</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_index_entrycount(git_index* index);

        /// <summary>
        /// Free an existing index object.
        /// </summary>
        /// <param name="index">The index to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_index_free(git_index* index);

        /// <summary>
        /// Create a new index iterator; this will take a snapshot of the
        /// given index for iteration.
        /// </summary>
        /// <param name="iterator">Pointer to the iterator that was created</param>
        /// <param name="index">The index to iterate over</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_iterator_new(out git_index_iterator* iterator, git_index* index);

        /// <summary>
        /// Get the next index entry in the iterator.  This entry is owned
        /// by the iterator and should not be freed.
        /// </summary>
        /// <param name="entry">Pointer to the entry in the index</param>
        /// <param name="iterator">The iterator to query</param>
        /// <returns>0 on success, GIT_ITEROVER on successful completion of iteration, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_iterator_next(out git_index_entry* entry, git_index_iterator* iterator);

        /// <summary>
        /// Free an existing index iterator.
        /// </summary>
        /// <param name="iterator">The iterator to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_index_iterator_free(git_index_iterator* iterator);
    }
}
