using System;

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
}
