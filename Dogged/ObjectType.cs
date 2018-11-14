using System;

namespace Dogged
{
    /// <summary>
    /// The type of a Git object.
    /// </summary>
    public enum ObjectType
    {
        /// <summary>
        /// A commit object.
        /// </summary>
        Commit = 1,

        /// <summary>
        /// A tree (directory listing) object.
        /// </summary>
        Tree = 2,

        /// <summary>
        /// A blob (file) object.
        /// </summary>
        Blob = 3
    }
}
