using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public sealed class OidShortenHandle : SafeHandle
    {
        public OidShortenHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe OidShortenHandle(git_oid_shorten* nativeOidShorten) :
            base((IntPtr)nativeOidShorten, true)
        {
        }

        public override bool IsInvalid
        {
            get
            {
                return handle == IntPtr.Zero;
            }
        }

        protected override unsafe bool ReleaseHandle()
        {
            libgit2.git_oid_shorten_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public static partial class libgit2
    {
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
            OidShortenHandle os,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string text_id);

        /// <summary>
        ///  Free an OID shortener instance
        /// </summary>
        /// <param name="os">a `git_oid_shorten` instance</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_oid_shorten_free(
            IntPtr os);
    }
}
