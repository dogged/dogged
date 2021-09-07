using System;
using System.Collections.Generic;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// An object id; a unique identifier for a Git object including a
    /// commit, tree, blob and tag.
    /// </summary>
    public class ObjectId
    {
        // Value to ASCII hexadigit map
        private static readonly char[] ToHex = {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'
        };

        // ASCII hexadigit (0-9a-f) to value map
        private static readonly sbyte[] FromHex = {
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, // 00
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, // 10
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, // 20
             0,  1,  2,  3,  4,  5,  6,  7,  8,  9, -1, -1, -1, -1, -1, -1, // 30
            -1, 10, 11, 12, 13, 14, 15, -1, -1, -1, -1, -1, -1, -1, -1, -1, // 40
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, // 50
            -1, 10, 11, 12, 13, 14, 15, -1, -1, -1, -1, -1, -1, -1, -1, -1, // 60
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, // 70
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, // 80
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, // 90
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, // a0
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, // b0
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, // c0
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, // d0
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, // e0
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, // f0
        };

        /// <summary>
        /// The "null oid" consisting of all zeros.
        /// </summary>
        public static ObjectId Zero = new ObjectId();

        private readonly git_oid oid = new git_oid();
        private string hex;

        private unsafe ObjectId(byte* id)
        {
            Ensure.ArgumentNotNull(id, "id");

            fixed(byte* dest = this.oid.id)
            {
                Buffer.MemoryCopy(id, dest, git_oid.GIT_OID_RAWSZ, git_oid.GIT_OID_RAWSZ);
            }
        }

        /// <summary>
        /// Create an Object ID from the given hexadecimal string.
        /// </summary>
        public unsafe ObjectId(string hex)
        {
            Ensure.ArgumentNotNull(hex, "hex");
            Ensure.ArgumentConformsTo(() => hex.Length == git_oid.GIT_OID_HEXSZ, "id length is invalid", "hex");

            for (int i = 0; i < hex.Length; i++)
            {
                sbyte b = FromHex[hex[i]];

                if (b < 0)
                {
                    throw new ArgumentException(string.Format("invalid hexadigit '{0}'", hex[i]), "hex");
                }

                fixed(byte* id = oid.id)
                {
                    id[i / 2] |= (byte)(b << (i % 2 == 0 ? 4 : 0));
                }
            }
        }

        private ObjectId() { }

        internal unsafe static ObjectId FromNative(git_oid nativeOid)
        {
            Ensure.ArgumentNotNull(nativeOid, "nativeOid");
            return new ObjectId(nativeOid.id);
        }

        internal unsafe static ObjectId FromNative(git_oid* nativeOid)
        {
            Ensure.ArgumentNotNull(nativeOid, "nativeOid");
            return new ObjectId(nativeOid->id);
        }

        internal git_oid ToNative()
        {
            return oid;
        }

        internal unsafe static void NativeCopy(git_oid* src, git_oid* dest)
        {
            Buffer.MemoryCopy(src->id, dest->id, git_oid.GIT_OID_RAWSZ, git_oid.GIT_OID_RAWSZ);
        }

        /// <summary>
        /// Return a hexadecimal string representation of the Object ID.
        /// </summary>
        public unsafe override string ToString()
        {
            if (hex == null)
            {
                char[] hexChars = new char[git_oid.GIT_OID_HEXSZ];

                fixed(byte* id = oid.id)
                {
                    for (int i = 0; i < git_oid.GIT_OID_RAWSZ; i++)
                    {
                        hexChars[i*2]     = ToHex[id[i] >> 4];
                        hexChars[(i*2)+1] = ToHex[id[i] & 0x0f];
                    }
                }

                hex = new String(hexChars);
            }

            return hex;
        }

        public unsafe override int GetHashCode()
        {
            fixed(byte* id = oid.id)
            {
                return (id[0] << 24) | (id[1] << 16) | (id[2] << 8) | id[3];
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ObjectId);
        }

        public unsafe bool Equals(ObjectId other)
        {
            if (other == null)
            {
                return false;
            }

            if (other == this)
            {
                return true;
            }

            // Pinning + loop unrolling inspired by
            // https://stackoverflow.com/a/38002854
            fixed(byte* b0 = this.oid.id)
            fixed(byte* b1 = other.oid.id)
            {
                if (*(ulong*)b0 != *(ulong*)b1) return false;
                if (*(ulong*)(b0 + 8) != *(ulong*)(b1 + 8)) return false;
                if (*(uint*)(b0 + 16) != *(uint*)(b1 + 16)) return false;
            }

            return true;
        }
    }
}
