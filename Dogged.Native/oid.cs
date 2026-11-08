using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Unique identity of any object (commit, tree, blob, tag).
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_oid
    {
        /// <summary>
        ///  SHA1 is currently libgit2's default oid type.
        /// </summary>
        public const git_oid_t GIT_OID_DEFAULT = git_oid_t.GIT_OID_SHA1;

        /// <summary>
        ///  Size (in bytes) of a raw/binary sha1 oid
        /// </summary>
        public const int GIT_OID_SHA1_SIZE = 20;

        /// <summary>
        ///  Size (in bytes) of a hex formatted sha1 oid
        /// </summary>
        public const int GIT_OID_SHA1_HEXSIZE = (GIT_OID_SHA1_SIZE * 2);

        /// <summary>
        ///  Size (in bytes) of a raw/binary sha256 oid
        /// </summary>
        public const int GIT_OID_SHA256_SIZE = 32;

        /// <summary>
        ///  Size (in bytes) of a hex formatted sha256 oid
        /// </summary>
        public const int GIT_OID_SHA256_HEXSIZE = (GIT_OID_SHA256_SIZE * 2);

        /// <summary>
        ///  Maximum possible object ID size in raw format
        /// </summary>
        public const int GIT_OID_MAX_SIZE = GIT_OID_SHA256_SIZE;

        /// <summary>
        ///  Maximum possible object ID size in hex format
        /// </summary>
        public const int GIT_OID_MAX_HEXSIZE = GIT_OID_SHA256_HEXSIZE;

        /// <summary>
        ///  Minimum length (in number of hex characters, i.e. packets of
        ///  4 bits) of an oid prefix
        /// </summary>
        public const int GIT_OID_MINPREFIXLEN = 4;

        /// <summary>
        ///  type of object id
        /// </summary>
        public byte type;

        /// <summary>
        ///  raw binary formatted id
        /// </summary>
        public unsafe fixed byte id[GIT_OID_MAX_SIZE];
    }

    /// <summary>
    ///  The type of object id.
    /// </summary>
    public enum git_oid_t
    {
        /// <summary>
        ///  SHA1
        /// </summary>
        GIT_OID_SHA1 = 1,

        /// <summary>
        ///  SHA256
        /// </summary>
        GIT_OID_SHA256 = 2,
    }

    /// <summary>
    ///  OID Shortener object
    /// </summary>
    public struct git_oid_shorten
    {
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Parse a NUL terminated hex formatted object id string into a
        ///  `git_oid`.
        ///
        ///  <para>
        ///   The given string must be NUL terminated, and must be the
        ///   hex size of the given object ID type - 40 characters for
        ///   SHA1, 64 characters for SHA256.
        ///  </para>
        ///
        ///  <para>
        ///   To parse an incomplete object ID (an object ID prefix),
        ///   or a sequence of characters that is not NUL terminated,
        ///   use `git_oid_from_prefix`.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  oid structure the result is written into.
        /// </param>
        /// <param name="str">input hex string</param>
        /// <param name="type">object id type</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_oid_from_string(
            ref git_oid _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string str,
            git_oid_t type);

        /// <summary>
        ///  Parse the given number of characters out of a hex formatted
        ///  object id string into a `git_oid`.
        ///
        ///  <para>
        ///   The given length can be between 0 and the hex size for
        ///   the given object ID type - 40 characters for SHA1, 64
        ///   characters for SHA256. The remainder of the `git_oid`
        ///   will be set to zeros.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  oid structure the result is written into.
        /// </param>
        /// <param name="str">input hex prefix</param>
        /// <param name="len">the length of the given prefix</param>
        /// <param name="type">object id type</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_oid_from_prefix(
            ref git_oid _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string str,
            UIntPtr len,
            git_oid_t type);

        /// <summary>
        ///  Parse a raw object id into a `git_oid`.
        ///
        ///  <para>
        ///   The appropriate number of bytes for the given object ID
        ///   type will be read from the byte array - 20 bytes for
        ///   SHA1, 32 bytes for SHA256.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  oid structure the result is written into.
        /// </param>
        /// <param name="raw">raw object ID bytes</param>
        /// <param name="type">object id type</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_oid_from_raw(
            ref git_oid _out,
            byte* raw,
            git_oid_t type);

        /// <summary>
        ///  Parse a hex formatted object id into a git_oid.
        ///
        ///  <para>
        ///   The appropriate number of bytes for the given object ID
        ///   type will be read from the string - 40 bytes for SHA1,
        ///   64 bytes for SHA256. The given string need not be NUL
        ///   terminated.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  oid structure the result is written into.
        /// </param>
        /// <param name="str">
        ///  input hex string; must be pointing at the start of the hex
        ///  sequence and have at least the number of bytes needed for an
        ///  oid encoded in hex (40 bytes for sha1, 256 bytes for
        ///  sha256).
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_oid_fromstr(
            ref git_oid _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string str);

        /// <summary>
        ///  Parse a hex formatted NUL-terminated string into a git_oid.
        /// </summary>
        /// <param name="_out">
        ///  oid structure the result is written into.
        /// </param>
        /// <param name="str">
        ///  input hex string; must be null-terminated.
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_oid_fromstrp(
            ref git_oid _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string str);

        /// <summary>
        ///  Parse N characters of a hex formatted object id into a
        ///  git_oid.
        ///
        ///  <para>
        ///   If N is odd, the last byte's high nibble will be read in
        ///   and the low nibble set to zero.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  oid structure the result is written into.
        /// </param>
        /// <param name="str">
        ///  input hex string of at least size `length`
        /// </param>
        /// <param name="length">length of the input string</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_oid_fromstrn(
            ref git_oid _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string str,
            UIntPtr length);

        /// <summary>
        ///  Copy an already raw oid into a git_oid structure.
        /// </summary>
        /// <param name="_out">
        ///  oid structure the result is written into.
        /// </param>
        /// <param name="raw">the raw input bytes to be copied.</param>
        /// <returns>0 on success or error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_oid_fromraw(
            ref git_oid _out,
            byte* raw);

        /// <summary>
        ///  Format a git_oid into a hex string.
        /// </summary>
        /// <param name="_out">
        ///  output hex string; must be pointing at the start of the hex
        ///  sequence and have at least the number of bytes needed for an
        ///  oid encoded in hex (40 bytes for SHA1, 64 bytes for SHA256).
        ///  Only the oid digits are written; a ' \ 0' terminator must be
        ///  added by the caller if it is required.
        /// </param>
        /// <param name="id">oid structure to format.</param>
        /// <returns>0 on success or error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_oid_fmt(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string _out,
            ref git_oid id);

        /// <summary>
        ///  Format a git_oid into a partial hex string.
        /// </summary>
        /// <param name="_out">
        ///  output hex string; you say how many bytes to write. If the
        ///  number of bytes is > GIT_OID_SHA1_HEXSIZE, extra bytes will
        ///  be zeroed; if not, a ' \ 0' terminator is NOT added.
        /// </param>
        /// <param name="n">
        ///  number of characters to write into out string
        /// </param>
        /// <param name="id">oid structure to format.</param>
        /// <returns>0 on success or error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_oid_nfmt(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string _out,
            UIntPtr n,
            ref git_oid id);

        /// <summary>
        ///  Format a git_oid into a loose-object path string.
        ///
        ///  <para>
        ///   The resulting string is "aa/...", where "aa" is the
        ///   first two hex digits of the oid and "..." is the
        ///   remaining 38 digits.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  output hex string; must be pointing at the start of the hex
        ///  sequence and have at least the number of bytes needed for an
        ///  oid encoded in hex (41 bytes for SHA1, 65 bytes for SHA256).
        ///  Only the oid digits are written; a ' \ 0' terminator must be
        ///  added by the caller if it is required.
        /// </param>
        /// <param name="id">oid structure to format.</param>
        /// <returns>
        ///  0 on success, non-zero callback return value, or error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_oid_pathfmt(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string _out,
            ref git_oid id);

        /// <summary>
        ///  Format a git_oid into a statically allocated c-string.
        ///
        ///  <para>
        ///   The c-string is owned by the library and should not be
        ///   freed by the user. If libgit2 is built with thread
        ///   support, the string will be stored in TLS (i.e. one
        ///   buffer per thread) to allow for concurrent calls of the
        ///   function.
        ///  </para>
        /// </summary>
        /// <param name="oid">The oid structure to format</param>
        /// <returns>the c-string or NULL on failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe byte* git_oid_tostr_s(
            ref git_oid oid);

        /// <summary>
        ///  Format a git_oid into a buffer as a hex format c-string.
        ///
        ///  <para>
        ///   If the buffer is smaller than the size of a
        ///   hex-formatted oid string plus an additional byte
        ///   (GIT_OID_SHA_HEXSIZE + 1 for SHA1 or
        ///   GIT_OID_SHA256_HEXSIZE + 1 for SHA256), then the
        ///   resulting oid c-string will be truncated to n-1
        ///   characters (but will still be NUL-byte terminated).
        ///  </para>
        ///
        ///  <para>
        ///   If there are any input parameter errors (out == NULL, n
        ///   == 0, oid == NULL), then a pointer to an empty string is
        ///   returned, so that the return value can always be
        ///   printed.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  the buffer into which the oid string is output.
        /// </param>
        /// <param name="n">the size of the out buffer.</param>
        /// <param name="id">the oid structure to format.</param>
        /// <returns>
        ///  the out buffer pointer, assuming no input parameter errors,
        ///  otherwise a pointer to an empty string.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe byte* git_oid_tostr(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string _out,
            UIntPtr n,
            ref git_oid id);

        /// <summary>
        ///  Copy an oid from one structure to another.
        /// </summary>
        /// <param name="_out">
        ///  oid structure the result is written into.
        /// </param>
        /// <param name="src">oid structure to copy from.</param>
        /// <returns>0 on success or error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_oid_cpy(
            ref git_oid _out,
            ref git_oid src);

        /// <summary>
        ///  Compare two oid structures.
        /// </summary>
        /// <param name="a">first oid structure.</param>
        /// <param name="b">second oid structure.</param>
        /// <returns>< 0, 0, >0 if a < b, a == b, a > b.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_oid_cmp(
            ref git_oid a,
            ref git_oid b);

        /// <summary>
        ///  Compare two oid structures for equality
        /// </summary>
        /// <param name="a">first oid structure.</param>
        /// <param name="b">second oid structure.</param>
        /// <returns>true if equal, false otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_oid_equal(
            ref git_oid a,
            ref git_oid b);

        /// <summary>
        ///  Compare the first 'len' hexadecimal characters (packets of 4
        ///  bits) of two oid structures.
        /// </summary>
        /// <param name="a">first oid structure.</param>
        /// <param name="b">second oid structure.</param>
        /// <param name="len">the number of hex chars to compare</param>
        /// <returns>0 in case of a match</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_oid_ncmp(
            ref git_oid a,
            ref git_oid b,
            UIntPtr len);

        /// <summary>
        ///  Check if an oid equals an hex formatted object id.
        /// </summary>
        /// <param name="id">oid structure.</param>
        /// <param name="str">input hex string of an object id.</param>
        /// <returns>0 in case of a match, -1 otherwise.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_oid_streq(
            ref git_oid id,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string str);

        /// <summary>
        ///  Compare an oid to an hex formatted object id.
        /// </summary>
        /// <param name="id">oid structure.</param>
        /// <param name="str">input hex string of an object id.</param>
        /// <returns>
        ///  -1 if str is not valid, < 0 if id sorts before str, 0 if id
        ///  matches str, >0 if id sorts after str.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_oid_strcmp(
            ref git_oid id,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string str);

        /// <summary>
        ///  Check is an oid is all zeros.
        /// </summary>
        /// <param name="id">the object ID to check</param>
        /// <returns>1 if all zeros, 0 otherwise.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_oid_is_zero(
            ref git_oid id);

        /// <summary>
        ///  Create a new OID shortener.
        ///
        ///  <para>
        ///   The OID shortener is used to process a list of OIDs in
        ///   text form and return the shortest length that would
        ///   uniquely identify all of them.
        ///  </para>
        ///
        ///  <para>
        ///   E.g. look at the result of `git log --abbrev`.
        ///  </para>
        /// </summary>
        /// <param name="min_length">
        ///  The minimal length for all identifiers, which will be used
        ///  even if shorter OIDs would still be unique.
        /// </param>
        /// <returns>a `git_oid_shorten` instance, NULL if OOM</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid_shorten* git_oid_shorten_new(
            UIntPtr min_length);

        /// <summary>
        ///  Add a new OID to set of shortened OIDs and calculate the
        ///  minimal length to uniquely identify all the OIDs in the set.
        ///
        ///  <para>
        ///   The OID is expected to be a 40-char hexadecimal string.
        ///   The OID is owned by the user and will not be modified or
        ///   freed.
        ///  </para>
        ///
        ///  <para>
        ///   For performance reasons, there is a hard-limit of how
        ///   many OIDs can be added to a single set (around ~32000,
        ///   assuming a mostly randomized distribution), which should
        ///   be enough for any kind of program, and keeps the
        ///   algorithm fast and memory-efficient.
        ///  </para>
        ///
        ///  <para>
        ///   Attempting to add more than those OIDs will result in a
        ///   GIT_ERROR_INVALID error
        ///  </para>
        /// </summary>
        /// <param name="os">a `git_oid_shorten` instance</param>
        /// <param name="text_id">an OID in text form</param>
        /// <returns>
        ///  the minimal length to uniquely identify all OIDs added so
        ///  far to the set; or an error code ( < 0) if an error occurs.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_oid_shorten_add(
            git_oid_shorten* os,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string text_id);

        /// <summary>
        ///  Free an OID shortener instance
        /// </summary>
        /// <param name="os">a `git_oid_shorten` instance</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_oid_shorten_free(
            git_oid_shorten* os);
    }
}
