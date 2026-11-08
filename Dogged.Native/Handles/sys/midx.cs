using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public sealed class MidxWriterHandle : SafeHandle
    {
        public MidxWriterHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe MidxWriterHandle(git_midx_writer* nativeMidxWriter) :
            base((IntPtr)nativeMidxWriter, true)
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
            libgit2.git_midx_writer_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Create a new writer for `multi-pack-index` files.
        /// </summary>
        /// <param name="_out">location to store the writer pointer.</param>
        /// <param name="pack_dir">
        ///  the directory where the `.pack` and `.idx` files are. The
        ///  `multi-pack-index` file will be written in this directory,
        ///  too.
        /// </param>
        /// <param name="options">
        ///  the options to create the writer with
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_midx_writer_new(
            out MidxWriterHandle _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string pack_dir,
            ref git_midx_writer_options options);

        /// <summary>
        ///  Free the multi-pack-index writer and its resources.
        /// </summary>
        /// <param name="w">
        ///  the writer to free. If NULL no action is taken.
        /// </param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_midx_writer_free(
            IntPtr w);

        /// <summary>
        ///  Add an `.idx` file to the writer.
        /// </summary>
        /// <param name="w">the writer</param>
        /// <param name="idx_path">the path of an `.idx` file.</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_midx_writer_add(
            MidxWriterHandle w,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string idx_path);

        /// <summary>
        ///  Write a `multi-pack-index` file to a file.
        /// </summary>
        /// <param name="w">the writer</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_midx_writer_commit(
            MidxWriterHandle w);

        /// <summary>
        ///  Dump the contents of the `multi-pack-index` to an in-memory
        ///  buffer.
        /// </summary>
        /// <param name="midx">
        ///  Buffer where to store the contents of the
        ///  `multi-pack-index`.
        /// </param>
        /// <param name="w">the writer</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_midx_writer_dump(
            ref git_buf midx,
            MidxWriterHandle w);
    }
}
