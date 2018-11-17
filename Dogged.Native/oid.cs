using System;

namespace Dogged.Native
{
    /// <summary>
    /// An object id; a unique identifier for a Git object including a
    /// commit, tree, blob and tag.
    /// </summary>
    public struct git_oid
    {
        /// <summary>
        /// Size in bytes of a raw (binary) object id.
        /// </summary>
        public const int GIT_OID_RAWSZ = 20;

        /// <summary>
        /// Size in bytes of a hexadecimal formatted object id string.
        /// </summary>
        public const int GIT_OID_HEXSZ = (GIT_OID_RAWSZ * 2);

        /// <summary>
        /// The raw (binary) object id.
        /// </summary>
        public unsafe fixed byte id[GIT_OID_RAWSZ];
    }
}
