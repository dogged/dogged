using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Representation of a tree object.
    /// </summary>
    public struct git_tree
    {
    }

    /// <summary>
    ///  Callback for git_treebuilder_filter
    ///
    ///  <para>
    ///   The return value is treated as a boolean, with zero
    ///   indicating that the entry should be left alone and any
    ///   non-zero value meaning that the entry should be removed from
    ///   the treebuilder list (i.e. filtered out).
    ///  </para>
    /// </summary>
    /// <param name="entry">
    ///  the tree entry for the callback to examine
    /// </param>
    /// <param name="payload">the payload from the caller</param>
    /// <returns>0 to do nothing, non-zero to remove the entry</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_treebuilder_filter_cb(
        git_tree_entry* entry,
        IntPtr payload);

    /// <summary>
    ///  Callback for the tree traversal method.
    /// </summary>
    /// <param name="root">the current (relative) root to the entry</param>
    /// <param name="entry">the tree entry</param>
    /// <param name="payload">the caller-provided callback payload</param>
    /// <returns>
    ///  a positive value to skip the entry, a negative value to stop the
    ///  walk
    /// </returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_treewalk_cb(
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string root,
        git_tree_entry* entry,
        IntPtr payload);

    /// <summary>
    ///  Tree traversal modes
    /// </summary>
    public enum git_treewalk_mode
    {
        GIT_TREEWALK_PRE = 0,
        GIT_TREEWALK_POST = 1,
    }

    /// <summary>
    ///  The kind of update to perform
    /// </summary>
    public enum git_tree_update_t
    {
        /// <summary>
        ///  Update or insert an entry at the specified path
        /// </summary>
        GIT_TREE_UPDATE_UPSERT,

        /// <summary>
        ///  Remove an entry from the specified path
        /// </summary>
        GIT_TREE_UPDATE_REMOVE,
    }

    /// <summary>
    ///  An action to perform during the update of a tree
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_tree_update
    {
        /// <summary>
        ///  Update action. If it's an removal, only the path is looked
        ///  at
        /// </summary>
        public git_tree_update_t action;

        /// <summary>
        ///  The entry's id
        /// </summary>
        public git_oid id;

        /// <summary>
        ///  The filemode/kind of object
        /// </summary>
        public git_filemode_t filemode;

        /// <summary>
        ///  The full path from the root tree
        /// </summary>
        public IntPtr path;
    }

    /// <summary>
    ///  Representation of each one of the entries in a tree object.
    /// </summary>
    public struct git_tree_entry
    {
    }

    /// <summary>
    ///  Constructor for in-memory trees
    /// </summary>
    public struct git_treebuilder
    {
    }

    /// <summary>
    ///  Valid modes for index and tree entries.
    /// </summary>
    public enum git_filemode_t
    {
        GIT_FILEMODE_UNREADABLE = 0,
        GIT_FILEMODE_TREE = 16384,
        GIT_FILEMODE_BLOB = 33188,
        GIT_FILEMODE_BLOB_EXECUTABLE = 33261,
        GIT_FILEMODE_LINK = 40960,
        GIT_FILEMODE_COMMIT = 57344,
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Lookup a tree object from the repository.
        /// </summary>
        /// <param name="_out">Pointer to the looked up tree</param>
        /// <param name="repo">
        ///  The repo to use when locating the tree.
        /// </param>
        /// <param name="id">Identity of the tree to locate.</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_tree_lookup(
            out git_tree* _out,
            git_repository* repo,
            ref git_oid id);

        /// <summary>
        ///  Lookup a tree object from the repository, given a prefix of
        ///  its identifier (short id).
        /// </summary>
        /// <param name="_out">pointer to the looked up tree</param>
        /// <param name="repo">
        ///  the repo to use when locating the tree.
        /// </param>
        /// <param name="id">identity of the tree to locate.</param>
        /// <param name="len">the length of the short identifier</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_tree_lookup_prefix(
            out git_tree* _out,
            git_repository* repo,
            ref git_oid id,
            UIntPtr len);

        /// <summary>
        ///  Close an open tree
        ///
        ///  <para>
        ///   You can no longer use the git_tree pointer after this
        ///   call.
        ///  </para>
        ///
        ///  <para>
        ///   IMPORTANT: You MUST call this method when you stop using
        ///   a tree to release memory. Failure to do so will cause a
        ///   memory leak.
        ///  </para>
        /// </summary>
        /// <param name="tree">The tree to close</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_tree_free(
            git_tree* tree);

        /// <summary>
        ///  Get the id of a tree.
        /// </summary>
        /// <param name="tree">a previously loaded tree.</param>
        /// <returns>object identity for the tree.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_tree_id(
            git_tree* tree);

        /// <summary>
        ///  Get the repository that contains the tree.
        /// </summary>
        /// <param name="tree">A previously loaded tree.</param>
        /// <returns>Repository that contains this tree.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_repository* git_tree_owner(
            git_tree* tree);

        /// <summary>
        ///  Get the number of entries listed in a tree
        /// </summary>
        /// <param name="tree">a previously loaded tree.</param>
        /// <returns>the number of entries in the tree</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_tree_entrycount(
            git_tree* tree);

        /// <summary>
        ///  Lookup a tree entry by its filename
        ///
        ///  <para>
        ///   This returns a git_tree_entry that is owned by the
        ///   git_tree. You don't have to free it, but you must not
        ///   use it after the git_tree is released.
        ///  </para>
        /// </summary>
        /// <param name="tree">a previously loaded tree.</param>
        /// <param name="filename">the filename of the desired entry</param>
        /// <returns>the tree entry; NULL if not found</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_tree_entry* git_tree_entry_byname(
            git_tree* tree,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string filename);

        /// <summary>
        ///  Lookup a tree entry by its position in the tree
        ///
        ///  <para>
        ///   This returns a git_tree_entry that is owned by the
        ///   git_tree. You don't have to free it, but you must not
        ///   use it after the git_tree is released.
        ///  </para>
        /// </summary>
        /// <param name="tree">a previously loaded tree.</param>
        /// <param name="idx">the position in the entry list</param>
        /// <returns>the tree entry; NULL if not found</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_tree_entry* git_tree_entry_byindex(
            git_tree* tree,
            UIntPtr idx);

        /// <summary>
        ///  Lookup a tree entry by SHA value.
        ///
        ///  <para>
        ///   This returns a git_tree_entry that is owned by the
        ///   git_tree. You don't have to free it, but you must not
        ///   use it after the git_tree is released.
        ///  </para>
        ///
        ///  <para>
        ///   Warning: this must examine every entry in the tree, so
        ///   it is not fast.
        ///  </para>
        /// </summary>
        /// <param name="tree">a previously loaded tree.</param>
        /// <param name="id">the sha being looked for</param>
        /// <returns>the tree entry; NULL if not found</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_tree_entry* git_tree_entry_byid(
            git_tree* tree,
            ref git_oid id);

        /// <summary>
        ///  Retrieve a tree entry contained in a tree or in any of its
        ///  subtrees, given its relative path.
        ///
        ///  <para>
        ///   Unlike the other lookup functions, the returned tree
        ///   entry is owned by the user and must be freed explicitly
        ///   with `git_tree_entry_free()`.
        ///  </para>
        /// </summary>
        /// <param name="_out">Pointer where to store the tree entry</param>
        /// <param name="root">
        ///  Previously loaded tree which is the root of the relative
        ///  path
        /// </param>
        /// <param name="path">Path to the contained entry</param>
        /// <returns>
        ///  0 on success; GIT_ENOTFOUND if the path does not exist
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_tree_entry_bypath(
            out git_tree_entry* _out,
            git_tree* root,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path);

        /// <summary>
        ///  Duplicate a tree entry
        ///
        ///  <para>
        ///   Create a copy of a tree entry. The returned copy is
        ///   owned by the user, and must be freed explicitly with
        ///   `git_tree_entry_free()`.
        ///  </para>
        /// </summary>
        /// <param name="dest">pointer where to store the copy</param>
        /// <param name="source">tree entry to duplicate</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_tree_entry_dup(
            git_tree_entry** dest,
            git_tree_entry* source);

        /// <summary>
        ///  Free a user-owned tree entry
        ///
        ///  <para>
        ///   IMPORTANT: This function is only needed for tree entries
        ///   owned by the user, such as the ones returned by
        ///   `git_tree_entry_dup()` or `git_tree_entry_bypath()`.
        ///  </para>
        /// </summary>
        /// <param name="entry">The entry to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_tree_entry_free(
            git_tree_entry* entry);

        /// <summary>
        ///  Get the filename of a tree entry
        /// </summary>
        /// <param name="entry">a tree entry</param>
        /// <returns>the name of the file</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_tree_entry_name(
            git_tree_entry* entry);

        /// <summary>
        ///  Get the id of the object pointed by the entry
        /// </summary>
        /// <param name="entry">a tree entry</param>
        /// <returns>the oid of the object</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_tree_entry_id(
            git_tree_entry* entry);

        /// <summary>
        ///  Get the type of the object pointed by the entry
        /// </summary>
        /// <param name="entry">a tree entry</param>
        /// <returns>the type of the pointed object</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_object_t git_tree_entry_type(
            git_tree_entry* entry);

        /// <summary>
        ///  Get the UNIX file attributes of a tree entry
        /// </summary>
        /// <param name="entry">a tree entry</param>
        /// <returns>filemode as an integer</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_filemode_t git_tree_entry_filemode(
            git_tree_entry* entry);

        /// <summary>
        ///  Get the raw UNIX file attributes of a tree entry
        ///
        ///  <para>
        ///   This function does not perform any normalization and is
        ///   only useful if you need to be able to recreate the
        ///   original tree object.
        ///  </para>
        /// </summary>
        /// <param name="entry">a tree entry</param>
        /// <returns>filemode as an integer</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_filemode_t git_tree_entry_filemode_raw(
            git_tree_entry* entry);

        /// <summary>
        ///  Compare two tree entries
        /// </summary>
        /// <param name="e1">first tree entry</param>
        /// <param name="e2">second tree entry</param>
        /// <returns>
        ///  < 0 if e1 is before e2, 0 if e1 == e2, >0 if e1 is after e2
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_tree_entry_cmp(
            git_tree_entry* e1,
            git_tree_entry* e2);

        /// <summary>
        ///  Convert a tree entry to the git_object it points to.
        ///
        ///  <para>
        ///   You must call `git_object_free()` on the object when you
        ///   are done with it.
        ///  </para>
        /// </summary>
        /// <param name="object_out">pointer to the converted object</param>
        /// <param name="repo">
        ///  repository where to lookup the pointed object
        /// </param>
        /// <param name="entry">a tree entry</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_tree_entry_to_object(
            git_object** object_out,
            git_repository* repo,
            git_tree_entry* entry);

        /// <summary>
        ///  Create a new tree builder.
        ///
        ///  <para>
        ///   The tree builder can be used to create or modify trees
        ///   in memory and write them as tree objects to the
        ///   database.
        ///  </para>
        ///
        ///  <para>
        ///   If the `source` parameter is not NULL, the tree builder
        ///   will be initialized with the entries of the given tree.
        ///  </para>
        ///
        ///  <para>
        ///   If the `source` parameter is NULL, the tree builder will
        ///   start with no entries and will have to be filled
        ///   manually.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Pointer where to store the tree builder
        /// </param>
        /// <param name="repo">
        ///  Repository in which to store the object
        /// </param>
        /// <param name="source">
        ///  Source tree to initialize the builder (optional)
        /// </param>
        /// <returns>0 on success; error code otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_treebuilder_new(
            out git_treebuilder* _out,
            git_repository* repo,
            git_tree* source);

        /// <summary>
        ///  Clear all the entries in the builder
        /// </summary>
        /// <param name="bld">Builder to clear</param>
        /// <returns>0 on success; error code otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_treebuilder_clear(
            git_treebuilder* bld);

        /// <summary>
        ///  Get the number of entries listed in a treebuilder
        /// </summary>
        /// <param name="bld">a previously loaded treebuilder.</param>
        /// <returns>the number of entries in the treebuilder</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_treebuilder_entrycount(
            git_treebuilder* bld);

        /// <summary>
        ///  Free a tree builder
        ///
        ///  <para>
        ///   This will clear all the entries and free to builder.
        ///   Failing to free the builder after you're done using it
        ///   will result in a memory leak
        ///  </para>
        /// </summary>
        /// <param name="bld">Builder to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_treebuilder_free(
            git_treebuilder* bld);

        /// <summary>
        ///  Get an entry from the builder from its filename
        ///
        ///  <para>
        ///   The returned entry is owned by the builder and should
        ///   not be freed manually.
        ///  </para>
        /// </summary>
        /// <param name="bld">Tree builder</param>
        /// <param name="filename">Name of the entry</param>
        /// <returns>pointer to the entry; NULL if not found</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_tree_entry* git_treebuilder_get(
            git_treebuilder* bld,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string filename);

        /// <summary>
        ///  Add or update an entry to the builder
        ///
        ///  <para>
        ///   Insert a new entry for `filename` in the builder with
        ///   the given attributes.
        ///  </para>
        ///
        ///  <para>
        ///   If an entry named `filename` already exists, its
        ///   attributes will be updated with the given ones.
        ///  </para>
        ///
        ///  <para>
        ///   The optional pointer `out` can be used to retrieve a
        ///   pointer to the newly created/updated entry. Pass NULL if
        ///   you do not need it. The pointer may not be valid past
        ///   the next operation in this builder. Duplicate the entry
        ///   if you want to keep it.
        ///  </para>
        ///
        ///  <para>
        ///   By default the entry that you are inserting will be
        ///   checked for validity; that it exists in the object
        ///   database and is of the correct type. If you do not want
        ///   this behavior, set the
        ///   `GIT_OPT_ENABLE_STRICT_OBJECT_CREATION` library option
        ///   to false.
        ///  </para>
        /// </summary>
        /// <param name="_out">Pointer to store the entry (optional)</param>
        /// <param name="bld">Tree builder</param>
        /// <param name="filename">Filename of the entry</param>
        /// <param name="id">SHA1 oid of the entry</param>
        /// <param name="filemode">
        ///  Folder attributes of the entry. This parameter must be
        ///  valued with one of the following entries: 0040000, 0100644,
        ///  0100755, 0120000 or 0160000.
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_treebuilder_insert(
            out git_tree_entry* _out,
            git_treebuilder* bld,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string filename,
            ref git_oid id,
            git_filemode_t filemode);

        /// <summary>
        ///  Remove an entry from the builder by its filename
        /// </summary>
        /// <param name="bld">Tree builder</param>
        /// <param name="filename">Filename of the entry to remove</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_treebuilder_remove(
            git_treebuilder* bld,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string filename);

        /// <summary>
        ///  Selectively remove entries in the tree
        ///
        ///  <para>
        ///   The `filter` callback will be called for each entry in
        ///   the tree with a pointer to the entry and the provided
        ///   `payload`; if the callback returns non-zero, the entry
        ///   will be filtered (removed from the builder).
        ///  </para>
        /// </summary>
        /// <param name="bld">Tree builder</param>
        /// <param name="filter">Callback to filter entries</param>
        /// <param name="payload">
        ///  Extra data to pass to filter callback
        /// </param>
        /// <returns>
        ///  0 on success, non-zero callback return value, or error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_treebuilder_filter(
            git_treebuilder* bld,
            git_treebuilder_filter_cb filter,
            IntPtr payload);

        /// <summary>
        ///  Write the contents of the tree builder as a tree object
        ///
        ///  <para>
        ///   The tree builder will be written to the given `repo`,
        ///   and its identifying SHA1 hash will be stored in the `id`
        ///   pointer.
        ///  </para>
        /// </summary>
        /// <param name="id">
        ///  Pointer to store the OID of the newly written tree
        /// </param>
        /// <param name="bld">Tree builder to write</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_treebuilder_write(
            ref git_oid id,
            git_treebuilder* bld);

        /// <summary>
        ///  Traverse the entries in a tree and its subtrees in post or
        ///  pre order.
        ///
        ///  <para>
        ///   The entries will be traversed in the specified order,
        ///   children subtrees will be automatically loaded as
        ///   required, and the `callback` will be called once per
        ///   entry with the current (relative) root for the entry and
        ///   the entry data itself.
        ///  </para>
        ///
        ///  <para>
        ///   If the callback returns a positive value, the passed
        ///   entry will be skipped on the traversal (in pre mode). A
        ///   negative value stops the walk.
        ///  </para>
        /// </summary>
        /// <param name="tree">The tree to walk</param>
        /// <param name="mode">Traversal mode (pre or post-order)</param>
        /// <param name="callback">
        ///  Function to call on each tree entry
        /// </param>
        /// <param name="payload">
        ///  Opaque pointer to be passed on each callback
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_tree_walk(
            git_tree* tree,
            git_treewalk_mode mode,
            git_treewalk_cb callback,
            IntPtr payload);

        /// <summary>
        ///  Create an in-memory copy of a tree. The copy must be
        ///  explicitly free'd or it will leak.
        /// </summary>
        /// <param name="_out">Pointer to store the copy of the tree</param>
        /// <param name="source">Original tree to copy</param>
        /// <returns>0</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_tree_dup(
            out git_tree* _out,
            git_tree* source);

        /// <summary>
        ///  Create a tree based on another one with the specified
        ///  modifications
        ///
        ///  <para>
        ///   Given the `baseline` perform the changes described in
        ///   the list of `updates` and create a new tree.
        ///  </para>
        ///
        ///  <para>
        ///   This function is optimized for common file/directory
        ///   addition, removal and replacement in trees. It is much
        ///   more efficient than reading the tree into a `git_index`
        ///   and modifying that, but in exchange it is not as
        ///   flexible.
        ///  </para>
        ///
        ///  <para>
        ///   Deleting and adding the same entry is undefined
        ///   behaviour, changing a tree to a blob or viceversa is not
        ///   supported.
        ///  </para>
        /// </summary>
        /// <param name="_out">id of the new tree</param>
        /// <param name="repo">
        ///  the repository in which to create the tree, must be the same
        ///  as for `baseline`
        /// </param>
        /// <param name="baseline">the tree to base these changes on</param>
        /// <param name="nupdates">
        ///  the number of elements in the update list
        /// </param>
        /// <param name="updates">the list of updates to perform</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_tree_create_updated(
            ref git_oid _out,
            git_repository* repo,
            git_tree* baseline,
            UIntPtr nupdates,
            ref git_tree_update updates);
    }
}
