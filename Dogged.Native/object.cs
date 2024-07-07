using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    /// The type of a Git object.
    /// </summary>
    public enum git_object_t
    {
        /// <summary>
        /// When looking up an object, search for objects of any type.
        /// </summary>
        GIT_OBJECT_ANY = -2,

        /// <summary>
        /// The object is invalid.
        /// </summary>
        GIT_OBJECT_INVALID = -1,

        /// <summary>
        /// A commit object.
        /// </summary>
        GIT_OBJECT_COMMIT = 1,

        /// <summary>
        /// A tree object (directory listing).
        /// </summary>
        GIT_OBJECT_TREE = 2,

        /// <summary>
        /// A blob (file) object.
        /// </summary>
        GIT_OBJECT_BLOB = 3,

        /// <summary>
        /// An annotated tag object.
        /// </summary>
        GIT_OBJECT_TAG = 4,

        /// <summary>
        /// A delta base; the base is given by an offset.
        /// </summary>
        GIT_OBJECT_OFS_DELTA = 6,

        /// <summary>
        /// A delta base; the base is given by a reference.
        /// </summary>
        GIT_OBJECT_REF_DELTA = 7,
    };

    /// <summary>
    /// A blob (file) object.
    /// </summary>
    public struct git_blob { };

    /// <summary>
    /// A commit object.
    /// </summary>
    public struct git_commit { };

    /// <summary>
    /// An object (blob, commit, tree, etc) in a Git repository.
    /// </summary>
    public struct git_object { };

    /// <summary>
    /// A tree (directory listing) object.
    /// </summary>
    public struct git_tree { };

    /// <summary>
    /// An entry in a tree.
    /// </summary>
    public struct git_tree_entry { };

    public static partial class libgit2
    {
        /// <summary>
        /// Create an in-memory copy of a Git object.  This copy must be
        /// explicitly free'd or it will leak.
        /// </summary>
        /// <param name="dest">Pointer to store the copy of the object</param>
        /// <param name="source">Original object to copy</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_object_dup(out git_object* dest, git_object* source);

        /// <summary>
        /// Free a git object.
        /// </summary>
        /// <param name="obj">The object to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_object_free(git_object* obj);

        /// <summary>
        /// Gets the object id of the given object.
        /// </summary>
        /// <param name="obj">The object to examine</param>
        /// <returns>A pointer to the object's id</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_object_id(git_object* obj);

        /// <summary>
        /// Look up an object from the repository.
        /// </summary>
        /// <param name="obj">Pointer to the object that was loaded from the repository</param>
        /// <param name="repo">The repository that contains the object</param>
        /// <param name="id">The id of the object to lookup</param>
        /// <param name="type">The type of the object to lookup</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_object_lookup(out git_object* obj, git_repository* repo, ref git_oid id, git_object_t type);

        /// <summary>
        /// Get the type of an object.
        /// </summary>
        /// <param name="obj">The object to query</param>
        /// <returns>The type of the object</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_object_t git_object_type(git_object* obj);
    }
}
