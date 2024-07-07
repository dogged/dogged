using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
        /// <summary>
        /// Get a pointer to an entry in the tree at the the given index.
        ///
        /// <para>
        /// The entry is not modifiable and should not be freed; it will be
        /// freed when the owning tree is freed.
        /// </para>
        /// </summary>
        /// <param name="tree">The tree to query</param>
        /// <param name="n">The index in the tree to return</param>
        /// <returns>A tree entry for the given index</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_tree_entry* git_tree_entry_byindex(git_tree* tree, UIntPtr n);

        /// <summary>
        /// Get a pointer to an entry in the tree for the the given filename.
        ///
        /// <para>
        /// The entry is not modifiable and should not be freed; it will be
        /// freed when the owning tree is freed.
        /// </para>
        /// </summary>
        /// <param name="tree">The tree to query</param>
        /// <param name="filename">The filename in the tree to return</param>
        /// <returns>A tree entry for the given index</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_tree_entry* git_tree_entry_byname(
            git_tree* tree,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string filename);

        /// <summary>
        /// Get the file mode of the given tree entry.
        /// </summary>
        /// <param name="entry">The tree entry to query</param>
        /// <returns>The file mode of the tree entry</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe uint git_tree_entry_filemode(git_tree_entry* entry);

        /// <summary>
        /// Get the object id of the given tree entry.
        /// </summary>
        /// <param name="entry">The tree entry to query</param>
        /// <returns>The object id of the tree entry</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_tree_entry_id(git_tree_entry* entry);

        /// <summary>
        /// Get the filename of the given tree entry.
        /// </summary>
        /// <param name="entry">The tree entry to query</param>
        /// <returns>The filename of the tree entry</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_tree_entry_name(git_tree_entry* entry);

        /// <summary>
        /// Get the number of entries in the tree.
        /// </summary>
        /// <param name="tree">The tree to query</param>
        /// <returns>The number of entries in the tree</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_tree_entrycount(git_tree* tree);

        /// <summary>
        /// Look up a tree from the repository.
        /// </summary>
        /// <param name="obj">Pointer to the tree that was loaded from the repository</param>
        /// <param name="repo">The repository that contains the tree</param>
        /// <param name="id">The id of the tree to lookup</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_tree_lookup(out git_tree* obj, git_repository* repo, ref git_oid id);
    }
}
