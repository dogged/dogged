using System;
using System.Runtime.InteropServices;

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

    public static partial class libgit2
    {
        /// <summary>
        /// Copy an oid from one structure to another.
        /// </summary>
        ///
        /// <param name="dst">The oid structure the result is written into.</param>
        /// <param name="src">The oid structure to copy from.</param>
        /// <returns>0 on success or error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_oid_cpy(ref git_oid dst, ref git_oid src);
    }
}
