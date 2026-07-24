using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public sealed class HashsigHandle : SafeHandle
    {
        public HashsigHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe HashsigHandle(git_hashsig* nativeHashsig) :
            base((IntPtr)nativeHashsig, true)
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
            libgit2.git_hashsig_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Compute a similarity signature for a text buffer
        ///
        ///  <para>
        ///   If you have passed the option
        ///   GIT_HASHSIG_IGNORE_WHITESPACE, then the whitespace will
        ///   be removed from the buffer while it is being processed,
        ///   modifying the buffer in place. Sorry about that!
        ///  </para>
        /// </summary>
        /// <param name="_out">The computed similarity signature.</param>
        /// <param name="buf">The input buffer.</param>
        /// <param name="buflen">The input buffer size.</param>
        /// <param name="opts">
        ///  The signature computation options (see above).
        /// </param>
        /// <returns>
        ///  0 on success, GIT_EBUFS if the buffer doesn't contain enough
        ///  data to compute a valid signature (unless
        ///  GIT_HASHSIG_ALLOW_SMALL_FILES is set), or error code.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_hashsig_create(
            out HashsigHandle _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string buf,
            UIntPtr buflen,
            git_hashsig_option_t opts);

        /// <summary>
        ///  Compute a similarity signature for a text file
        ///
        ///  <para>
        ///   This walks through the file, only loading a maximum of
        ///   4K of file data at a time. Otherwise, it acts just like
        ///   `git_hashsig_create`.
        ///  </para>
        /// </summary>
        /// <param name="_out">The computed similarity signature.</param>
        /// <param name="path">The path to the input file.</param>
        /// <param name="opts">
        ///  The signature computation options (see above).
        /// </param>
        /// <returns>
        ///  0 on success, GIT_EBUFS if the buffer doesn't contain enough
        ///  data to compute a valid signature (unless
        ///  GIT_HASHSIG_ALLOW_SMALL_FILES is set), or error code.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_hashsig_create_fromfile(
            out HashsigHandle _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            git_hashsig_option_t opts);

        /// <summary>
        ///  Release memory for a content similarity signature
        /// </summary>
        /// <param name="sig">The similarity signature to free.</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_hashsig_free(
            IntPtr sig);

        /// <summary>
        ///  Measure similarity score between two similarity signatures
        /// </summary>
        /// <param name="a">
        ///  The first similarity signature to compare.
        /// </param>
        /// <param name="b">
        ///  The second similarity signature to compare.
        /// </param>
        /// <returns>
        ///  [0 to 100] on success as the similarity score, or error
        ///  code.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_hashsig_compare(
            HashsigHandle a,
            HashsigHandle b);
    }
}
