using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Options structure for `git_midx_writer_options`.
    ///
    ///  <para>
    ///   Initialize with `GIT_MIDX_WRITER_OPTIONS_INIT`.
    ///   Alternatively, you can use `git_midx_writer_options_init`.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_midx_writer_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_MIDX_WRITER_OPTIONS_VERSION = 1;


        public uint version;

        /// <summary>
        ///  The object ID type that this commit graph contains.
        /// </summary>
        public git_oid_t oid_type;
    }

    /// <summary>
    ///  a writer for multi-pack-index files.
    /// </summary>
    public struct git_midx_writer
    {
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Initialize git_midx_writer_options structure
        ///
        ///  <para>
        ///   Initializes a `git_midx_writer_options` with default
        ///   values. Equivalent to creating an instance with
        ///   `GIT_MIDX_WRITER_OPTIONS_INIT`.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_midx_writer_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass `GIT_MIDX_WRITER_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_midx_writer_options_init(
            ref git_midx_writer_options opts,
            uint version);

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
            out git_midx_writer* _out,
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
            git_midx_writer* w);

        /// <summary>
        ///  Add an `.idx` file to the writer.
        /// </summary>
        /// <param name="w">the writer</param>
        /// <param name="idx_path">the path of an `.idx` file.</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_midx_writer_add(
            git_midx_writer* w,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string idx_path);

        /// <summary>
        ///  Write a `multi-pack-index` file to a file.
        /// </summary>
        /// <param name="w">the writer</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_midx_writer_commit(
            git_midx_writer* w);

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
            git_midx_writer* w);
    }
}
