using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public sealed class IndexHandle : SafeHandle
    {
        public IndexHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe IndexHandle(git_index* nativeIndex) :
            base((IntPtr)nativeIndex, true)
        {
        }

        public override bool IsInvalid
        {
            get
            {
                return handle == IntPtr.Zero;
            }
        }

        protected override unsafe bool ReleaseHandle()
        {
            libgit2.git_index_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public sealed class IndexIteratorHandle : SafeHandle
    {
        public IndexIteratorHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe IndexIteratorHandle(git_index_iterator* nativeIndexIterator) :
            base((IntPtr)nativeIndexIterator, true)
        {
        }

        public override bool IsInvalid
        {
            get
            {
                return handle == IntPtr.Zero;
            }
        }

        protected override unsafe bool ReleaseHandle()
        {
            libgit2.git_index_iterator_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public sealed class IndexConflictIteratorHandle : SafeHandle
    {
        public IndexConflictIteratorHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe IndexConflictIteratorHandle(git_index_conflict_iterator* nativeIndexConflictIterator) :
            base((IntPtr)nativeIndexConflictIterator, true)
        {
        }

        public override bool IsInvalid
        {
            get
            {
                return handle == IntPtr.Zero;
            }
        }

        protected override unsafe bool ReleaseHandle()
        {
            libgit2.git_index_conflict_iterator_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Create a new bare Git index object as a memory
        ///  representation of the Git index file in 'index_path',
        ///  without a repository to back it.
        ///
        ///  <para>
        ///   Since there is no ODB or working directory behind this
        ///   index, any Index methods which rely on these (e.g.
        ///   index_add_bypath) will fail with the GIT_ERROR error
        ///   code.
        ///  </para>
        ///
        ///  <para>
        ///   If you need to access the index of an actual repository,
        ///   use the `git_repository_index` wrapper.
        ///  </para>
        ///
        ///  <para>
        ///   The index must be freed once it's no longer in use.
        ///  </para>
        /// </summary>
        /// <param name="index_out">the pointer for the new index</param>
        /// <param name="index_path">
        ///  the path to the index file in disk
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_open(
            out IndexHandle index_out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string index_path);

        /// <summary>
        ///  Create an in-memory index object.
        ///
        ///  <para>
        ///   This index object cannot be read/written to the
        ///   filesystem, but may be used to perform in-memory index
        ///   operations.
        ///  </para>
        ///
        ///  <para>
        ///   The index must be freed once it's no longer in use.
        ///  </para>
        /// </summary>
        /// <param name="index_out">the pointer for the new index</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_new(
            out IndexHandle index_out);

        /// <summary>
        ///  Free an existing index object.
        /// </summary>
        /// <param name="index">an existing index object</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_index_free(
            IntPtr index);

        /// <summary>
        ///  Get the repository this index relates to
        /// </summary>
        /// <param name="index">The index</param>
        /// <returns>A pointer to the repository</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_repository* git_index_owner(
            IndexHandle index);

        /// <summary>
        ///  Read index capabilities flags.
        /// </summary>
        /// <param name="index">An existing index object</param>
        /// <returns>A combination of GIT_INDEX_CAPABILITY values</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_caps(
            IndexHandle index);

        /// <summary>
        ///  Set index capabilities flags.
        ///
        ///  <para>
        ///   If you pass `GIT_INDEX_CAPABILITY_FROM_OWNER` for the
        ///   caps, then capabilities will be read from the config of
        ///   the owner object, looking at `core.ignorecase`,
        ///   `core.filemode`, `core.symlinks`.
        ///  </para>
        /// </summary>
        /// <param name="index">An existing index object</param>
        /// <param name="caps">
        ///  A combination of GIT_INDEX_CAPABILITY values
        /// </param>
        /// <returns>0 on success, -1 on failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_set_caps(
            IndexHandle index,
            int caps);

        /// <summary>
        ///  Get index on-disk version.
        ///
        ///  <para>
        ///   Valid return values are 2, 3, or 4. If 3 is returned, an
        ///   index with version 2 may be written instead, if the
        ///   extension data in version 3 is not necessary.
        ///  </para>
        /// </summary>
        /// <param name="index">An existing index object</param>
        /// <returns>the index version</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe uint git_index_version(
            IndexHandle index);

        /// <summary>
        ///  Set index on-disk version.
        ///
        ///  <para>
        ///   Valid values are 2, 3, or 4. If 2 is given,
        ///   git_index_write may write an index with version 3
        ///   instead, if necessary to accurately represent the index.
        ///  </para>
        /// </summary>
        /// <param name="index">An existing index object</param>
        /// <param name="version">The new version number</param>
        /// <returns>0 on success, -1 on failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_set_version(
            IndexHandle index,
            uint version);

        /// <summary>
        ///  Update the contents of an existing index object in memory by
        ///  reading from the hard disk.
        ///
        ///  <para>
        ///   If `force` is true, this performs a "hard" read that
        ///   discards in-memory changes and always reloads the
        ///   on-disk index data. If there is no on-disk version, the
        ///   index will be cleared.
        ///  </para>
        ///
        ///  <para>
        ///   If `force` is false, this does a "soft" read that
        ///   reloads the index data from disk only if it has changed
        ///   since the last time it was loaded. Purely in-memory
        ///   index data will be untouched. Be aware: if there are
        ///   changes on disk, unwritten in-memory changes are
        ///   discarded.
        ///  </para>
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <param name="force">
        ///  if true, always reload, vs. only read if file has changed
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_read(
            IndexHandle index,
            int force);

        /// <summary>
        ///  Write an existing index object from memory back to disk
        ///  using an atomic file lock.
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_write(
            IndexHandle index);

        /// <summary>
        ///  Get the full path to the index file on disk.
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <returns>
        ///  path to index file or NULL for in-memory index
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_index_path(
            IndexHandle index);

        /// <summary>
        ///  Get the checksum of the index
        ///
        ///  <para>
        ///   This checksum is the SHA-1 hash over the index file
        ///   (except the last 20 bytes which are the checksum
        ///   itself). In cases where the index does not exist
        ///   on-disk, it will be zeroed out.
        ///  </para>
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <returns>a pointer to the checksum of the index</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_index_checksum(
            IndexHandle index);

        /// <summary>
        ///  Read a tree into the index file with stats
        ///
        ///  <para>
        ///   The current index contents will be replaced by the
        ///   specified tree.
        ///  </para>
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <param name="tree">tree to read</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_read_tree(
            IndexHandle index,
            TreeHandle tree);

        /// <summary>
        ///  Write the index as a tree
        ///
        ///  <para>
        ///   This method will scan the index and write a
        ///   representation of its current state back to disk; it
        ///   recursively creates tree objects for each of the
        ///   subtrees stored in the index, but only returns the OID
        ///   of the root tree. This is the OID that can be used e.g.
        ///   to create a commit.
        ///  </para>
        ///
        ///  <para>
        ///   The index instance cannot be bare, and needs to be
        ///   associated to an existing repository.
        ///  </para>
        ///
        ///  <para>
        ///   The index must not contain any file in conflict.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Pointer where to store the OID of the written tree
        /// </param>
        /// <param name="index">Index to write</param>
        /// <returns>
        ///  0 on success, GIT_EUNMERGED when the index is not clean or
        ///  an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_write_tree(
            ref git_oid _out,
            IndexHandle index);

        /// <summary>
        ///  Write the index as a tree to the given repository
        ///
        ///  <para>
        ///   This method will do the same as `git_index_write_tree`,
        ///   but letting the user choose the repository where the
        ///   tree will be written.
        ///  </para>
        ///
        ///  <para>
        ///   The index must not contain any file in conflict.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Pointer where to store OID of the written tree
        /// </param>
        /// <param name="index">Index to write</param>
        /// <param name="repo">Repository where to write the tree</param>
        /// <returns>
        ///  0 on success, GIT_EUNMERGED when the index is not clean or
        ///  an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_write_tree_to(
            ref git_oid _out,
            IndexHandle index,
            RepositoryHandle repo);

        /// <summary>
        ///  Get the count of entries currently in the index
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <returns>integer of count of current entries</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_index_entrycount(
            IndexHandle index);

        /// <summary>
        ///  Clear the contents (all the entries) of an index object.
        ///
        ///  <para>
        ///   This clears the index object in memory; changes must be
        ///   explicitly written to disk for them to take effect
        ///   persistently.
        ///  </para>
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <returns>0 on success, error code < 0 on failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_clear(
            IndexHandle index);

        /// <summary>
        ///  Get a pointer to one of the entries in the index
        ///
        ///  <para>
        ///   The entry is not modifiable and should not be freed.
        ///   Because the `git_index_entry` struct is a publicly
        ///   defined struct, you should be able to make your own
        ///   permanent copy of the data if necessary.
        ///  </para>
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <param name="n">the position of the entry</param>
        /// <returns>a pointer to the entry; NULL if out of bounds</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_index_entry* git_index_get_byindex(
            IndexHandle index,
            UIntPtr n);

        /// <summary>
        ///  Get a pointer to one of the entries in the index
        ///
        ///  <para>
        ///   The entry is not modifiable and should not be freed.
        ///   Because the `git_index_entry` struct is a publicly
        ///   defined struct, you should be able to make your own
        ///   permanent copy of the data if necessary.
        ///  </para>
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <param name="path">path to search</param>
        /// <param name="stage">stage to search</param>
        /// <returns>
        ///  a pointer to the entry; NULL if it was not found
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_index_entry* git_index_get_bypath(
            IndexHandle index,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            int stage);

        /// <summary>
        ///  Remove an entry from the index
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <param name="path">path to search</param>
        /// <param name="stage">stage to search</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_remove(
            IndexHandle index,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            int stage);

        /// <summary>
        ///  Remove all entries from the index under a given directory
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <param name="dir">container directory path</param>
        /// <param name="stage">stage to search</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_remove_directory(
            IndexHandle index,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string dir,
            int stage);

        /// <summary>
        ///  Add or update an index entry from an in-memory struct
        ///
        ///  <para>
        ///   If a previous index entry exists that has the same path
        ///   and stage as the given 'source_entry', it will be
        ///   replaced. Otherwise, the 'source_entry' will be added.
        ///  </para>
        ///
        ///  <para>
        ///   A full copy (including the 'path' string) of the given
        ///   'source_entry' will be inserted on the index.
        ///  </para>
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <param name="source_entry">new entry object</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_add(
            IndexHandle index,
            ref git_index_entry source_entry);

        /// <summary>
        ///  Create an iterator that will return every entry contained in
        ///  the index at the time of creation. Entries are returned in
        ///  order, sorted by path. This iterator is backed by a snapshot
        ///  that allows callers to modify the index while iterating
        ///  without affecting the iterator.
        /// </summary>
        /// <param name="iterator_out">The newly created iterator</param>
        /// <param name="index">The index to iterate</param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_iterator_new(
            out IndexIteratorHandle iterator_out,
            IndexHandle index);

        /// <summary>
        ///  Return the next index entry in-order from the iterator.
        /// </summary>
        /// <param name="_out">Pointer to store the index entry in</param>
        /// <param name="iterator">The iterator</param>
        /// <returns>
        ///  0, GIT_ITEROVER on iteration completion or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_iterator_next(
            out git_index_entry* _out,
            IndexIteratorHandle iterator);

        /// <summary>
        ///  Free the index iterator
        /// </summary>
        /// <param name="iterator">The iterator to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_index_iterator_free(
            IntPtr iterator);

        /// <summary>
        ///  Add or update an index entry from a file on disk
        ///
        ///  <para>
        ///   The file `path` must be relative to the repository's
        ///   working folder and must be readable.
        ///  </para>
        ///
        ///  <para>
        ///   This method will fail in bare index instances.
        ///  </para>
        ///
        ///  <para>
        ///   This forces the file to be added to the index, not
        ///   looking at gitignore rules. Those rules can be evaluated
        ///   through the git_status APIs (in status.h) before calling
        ///   this.
        ///  </para>
        ///
        ///  <para>
        ///   If this file currently is the result of a merge
        ///   conflict, this file will no longer be marked as
        ///   conflicting. The data about the conflict will be moved
        ///   to the "resolve undo" (REUC) section.
        ///  </para>
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <param name="path">filename to add</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_add_bypath(
            IndexHandle index,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path);

        /// <summary>
        ///  Add or update an index entry from a buffer in memory
        ///
        ///  <para>
        ///   This method will create a blob in the repository that
        ///   owns the index and then add the index entry to the
        ///   index. The `path` of the entry represents the position
        ///   of the blob relative to the repository's root folder.
        ///  </para>
        ///
        ///  <para>
        ///   If a previous index entry exists that has the same path
        ///   as the given 'entry', it will be replaced. Otherwise,
        ///   the 'entry' will be added.
        ///  </para>
        ///
        ///  <para>
        ///   This forces the file to be added to the index, not
        ///   looking at gitignore rules. Those rules can be evaluated
        ///   through the git_status APIs (in status.h) before calling
        ///   this.
        ///  </para>
        ///
        ///  <para>
        ///   If this file currently is the result of a merge
        ///   conflict, this file will no longer be marked as
        ///   conflicting. The data about the conflict will be moved
        ///   to the "resolve undo" (REUC) section.
        ///  </para>
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <param name="entry">filename to add</param>
        /// <param name="buffer">data to be written into the blob</param>
        /// <param name="len">length of the data</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_add_from_buffer(
            IndexHandle index,
            ref git_index_entry entry,
            IntPtr buffer,
            UIntPtr len);

        /// <summary>
        ///  Remove an index entry corresponding to a file on disk
        ///
        ///  <para>
        ///   The file `path` must be relative to the repository's
        ///   working folder. It may exist.
        ///  </para>
        ///
        ///  <para>
        ///   If this file currently is the result of a merge
        ///   conflict, this file will no longer be marked as
        ///   conflicting. The data about the conflict will be moved
        ///   to the "resolve undo" (REUC) section.
        ///  </para>
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <param name="path">filename to remove</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_remove_bypath(
            IndexHandle index,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path);

        /// <summary>
        ///  Add or update index entries matching files in the working
        ///  directory.
        ///
        ///  <para>
        ///   This method will fail in bare index instances.
        ///  </para>
        ///
        ///  <para>
        ///   The `pathspec` is a list of file names or shell glob
        ///   patterns that will be matched against files in the
        ///   repository's working directory. Each file that matches
        ///   will be added to the index (either updating an existing
        ///   entry or adding a new entry). You can disable glob
        ///   expansion and force exact matching with the
        ///   `GIT_INDEX_ADD_DISABLE_PATHSPEC_MATCH` flag.
        ///  </para>
        ///
        ///  <para>
        ///   Files that are ignored will be skipped (unlike
        ///   `git_index_add_bypath`). If a file is already tracked in
        ///   the index, then it *will* be updated even if it is
        ///   ignored. Pass the `GIT_INDEX_ADD_FORCE` flag to skip the
        ///   checking of ignore rules.
        ///  </para>
        ///
        ///  <para>
        ///   To emulate `git add -A` and generate an error if the
        ///   pathspec contains the exact path of an ignored file
        ///   (when not using FORCE), add the
        ///   `GIT_INDEX_ADD_CHECK_PATHSPEC` flag. This checks that
        ///   each entry in the `pathspec` that is an exact match to a
        ///   filename on disk is either not ignored or already in the
        ///   index. If this check fails, the function will return
        ///   GIT_EINVALIDSPEC.
        ///  </para>
        ///
        ///  <para>
        ///   To emulate `git add -A` with the "dry-run" option, just
        ///   use a callback function that always returns a positive
        ///   value. See below for details.
        ///  </para>
        ///
        ///  <para>
        ///   If any files are currently the result of a merge
        ///   conflict, those files will no longer be marked as
        ///   conflicting. The data about the conflicts will be moved
        ///   to the "resolve undo" (REUC) section.
        ///  </para>
        ///
        ///  <para>
        ///   If you provide a callback function, it will be invoked
        ///   on each matching item in the working directory
        ///   immediately *before* it is added to / updated in the
        ///   index. Returning zero will add the item to the index,
        ///   greater than zero will skip the item, and less than zero
        ///   will abort the scan and return that value to the caller.
        ///  </para>
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <param name="pathspec">array of path patterns</param>
        /// <param name="flags">
        ///  combination of git_index_add_option_t flags
        /// </param>
        /// <param name="callback">
        ///  notification callback for each added/updated path (also gets
        ///  index of matching pathspec entry); can be NULL; return 0 to
        ///  add, >0 to skip, < 0 to abort scan.
        /// </param>
        /// <param name="payload">
        ///  payload passed through to callback function
        /// </param>
        /// <returns>
        ///  0 on success, negative callback return value, or error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_add_all(
            IndexHandle index,
            ref git_strarray pathspec,
            uint flags,
            git_index_matched_path_cb callback,
            IntPtr payload);

        /// <summary>
        ///  Remove all matching index entries.
        ///
        ///  <para>
        ///   If you provide a callback function, it will be invoked
        ///   on each matching item in the index immediately *before*
        ///   it is removed. Return 0 to remove the item, > 0 to skip
        ///   the item, and < 0 to abort the scan.
        ///  </para>
        /// </summary>
        /// <param name="index">An existing index object</param>
        /// <param name="pathspec">array of path patterns</param>
        /// <param name="callback">
        ///  notification callback for each removed path (also gets index
        ///  of matching pathspec entry); can be NULL; return 0 to add,
        ///  >0 to skip, < 0 to abort scan.
        /// </param>
        /// <param name="payload">
        ///  payload passed through to callback function
        /// </param>
        /// <returns>
        ///  0 on success, negative callback return value, or error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_remove_all(
            IndexHandle index,
            ref git_strarray pathspec,
            git_index_matched_path_cb callback,
            IntPtr payload);

        /// <summary>
        ///  Update all index entries to match the working directory
        ///
        ///  <para>
        ///   This method will fail in bare index instances.
        ///  </para>
        ///
        ///  <para>
        ///   This scans the existing index entries and synchronizes
        ///   them with the working directory, deleting them if the
        ///   corresponding working directory file no longer exists
        ///   otherwise updating the information (including adding the
        ///   latest version of file to the ODB if needed).
        ///  </para>
        ///
        ///  <para>
        ///   If you provide a callback function, it will be invoked
        ///   on each matching item in the index immediately *before*
        ///   it is updated (either refreshed or removed depending on
        ///   working directory state). Return 0 to proceed with
        ///   updating the item, > 0 to skip the item, and < 0 to
        ///   abort the scan.
        ///  </para>
        /// </summary>
        /// <param name="index">An existing index object</param>
        /// <param name="pathspec">array of path patterns</param>
        /// <param name="callback">
        ///  notification callback for each updated path (also gets index
        ///  of matching pathspec entry); can be NULL; return 0 to add,
        ///  >0 to skip, < 0 to abort scan.
        /// </param>
        /// <param name="payload">
        ///  payload passed through to callback function
        /// </param>
        /// <returns>
        ///  0 on success, negative callback return value, or error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_update_all(
            IndexHandle index,
            ref git_strarray pathspec,
            git_index_matched_path_cb callback,
            IntPtr payload);

        /// <summary>
        ///  Find the first position of any entries which point to given
        ///  path in the Git index.
        /// </summary>
        /// <param name="at_pos">
        ///  the address to which the position of the index entry is
        ///  written (optional)
        /// </param>
        /// <param name="index">an existing index object</param>
        /// <param name="path">path to search</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_find(
            out UIntPtr at_pos,
            IndexHandle index,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path);

        /// <summary>
        ///  Find the first position of any entries matching a prefix. To
        ///  find the first position of a path inside a given folder,
        ///  suffix the prefix with a '/'.
        /// </summary>
        /// <param name="at_pos">
        ///  the address to which the position of the index entry is
        ///  written (optional)
        /// </param>
        /// <param name="index">an existing index object</param>
        /// <param name="prefix">the prefix to search for</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_find_prefix(
            out UIntPtr at_pos,
            IndexHandle index,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string prefix);

        /// <summary>
        ///  Add or update index entries to represent a conflict. Any
        ///  staged entries that exist at the given paths will be
        ///  removed.
        ///
        ///  <para>
        ///   The entries are the entries from the tree included in
        ///   the merge. Any entry may be null to indicate that that
        ///   file was not present in the trees during the merge. For
        ///   example, ancestor_entry may be NULL to indicate that a
        ///   file was added in both branches and must be resolved.
        ///  </para>
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <param name="ancestor_entry">
        ///  the entry data for the ancestor of the conflict
        /// </param>
        /// <param name="our_entry">
        ///  the entry data for our side of the merge conflict
        /// </param>
        /// <param name="their_entry">
        ///  the entry data for their side of the merge conflict
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_conflict_add(
            IndexHandle index,
            ref git_index_entry ancestor_entry,
            ref git_index_entry our_entry,
            ref git_index_entry their_entry);

        /// <summary>
        ///  Get the index entries that represent a conflict of a single
        ///  file.
        ///
        ///  <para>
        ///   The entries are not modifiable and should not be freed.
        ///   Because the `git_index_entry` struct is a publicly
        ///   defined struct, you should be able to make your own
        ///   permanent copy of the data if necessary.
        ///  </para>
        /// </summary>
        /// <param name="ancestor_out">
        ///  Pointer to store the ancestor entry
        /// </param>
        /// <param name="our_out">Pointer to store the our entry</param>
        /// <param name="their_out">Pointer to store the their entry</param>
        /// <param name="index">an existing index object</param>
        /// <param name="path">path to search</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_conflict_get(
            out git_index_entry* ancestor_out,
            out git_index_entry* our_out,
            out git_index_entry* their_out,
            IndexHandle index,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path);

        /// <summary>
        ///  Removes the index entries that represent a conflict of a
        ///  single file.
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <param name="path">path to remove conflicts for</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_conflict_remove(
            IndexHandle index,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path);

        /// <summary>
        ///  Remove all conflicts in the index (entries with a stage
        ///  greater than 0).
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_conflict_cleanup(
            IndexHandle index);

        /// <summary>
        ///  Determine if the index contains entries representing file
        ///  conflicts.
        /// </summary>
        /// <param name="index">An existing index object.</param>
        /// <returns>
        ///  1 if at least one conflict is found, 0 otherwise.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_has_conflicts(
            IndexHandle index);

        /// <summary>
        ///  Create an iterator for the conflicts in the index.
        ///
        ///  <para>
        ///   The index must not be modified while iterating; the
        ///   results are undefined.
        ///  </para>
        /// </summary>
        /// <param name="iterator_out">
        ///  The newly created conflict iterator
        /// </param>
        /// <param name="index">The index to scan</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_conflict_iterator_new(
            out IndexConflictIteratorHandle iterator_out,
            IndexHandle index);

        /// <summary>
        ///  Returns the current conflict (ancestor, ours and theirs
        ///  entry) and advance the iterator internally to the next
        ///  value.
        /// </summary>
        /// <param name="ancestor_out">
        ///  Pointer to store the ancestor side of the conflict
        /// </param>
        /// <param name="our_out">
        ///  Pointer to store our side of the conflict
        /// </param>
        /// <param name="their_out">
        ///  Pointer to store their side of the conflict
        /// </param>
        /// <param name="iterator">The conflict iterator.</param>
        /// <returns>
        ///  0 (no error), GIT_ITEROVER (iteration is done) or an error
        ///  code (negative value)
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_conflict_next(
            out git_index_entry* ancestor_out,
            out git_index_entry* our_out,
            out git_index_entry* their_out,
            IndexConflictIteratorHandle iterator);

        /// <summary>
        ///  Frees a `git_index_conflict_iterator`.
        /// </summary>
        /// <param name="iterator">pointer to the iterator</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_index_conflict_iterator_free(
            IntPtr iterator);
    }
}
