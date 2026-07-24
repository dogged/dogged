using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
        /// <summary>
        ///  Create a new direct reference from an OID.
        /// </summary>
        /// <param name="name">the reference name</param>
        /// <param name="oid">the object id for a direct reference</param>
        /// <param name="peel">
        ///  the first non-tag object's OID, or NULL
        /// </param>
        /// <returns>the created git_reference or NULL on error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_reference* git_reference__alloc(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            ref git_oid oid,
            ref git_oid peel);

        /// <summary>
        ///  Create a new symbolic reference.
        /// </summary>
        /// <param name="name">the reference name</param>
        /// <param name="target">the target for a symbolic reference</param>
        /// <returns>the created git_reference or NULL on error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_reference* git_reference__alloc_symbolic(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string target);
    }
}
