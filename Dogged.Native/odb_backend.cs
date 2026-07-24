using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Options for configuring a packfile object backend.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_odb_backend_pack_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_ODB_BACKEND_PACK_OPTIONS_VERSION = 1;


        /// <summary>
        ///  version for the struct
        /// </summary>
        public uint version;

        /// <summary>
        ///  Type of object IDs to use for this object database, or 0 for
        ///  default (currently SHA1).
        /// </summary>
        public git_oid_t oid_type;
    }

    public enum git_odb_backend_loose_flag_t
    {
        GIT_ODB_BACKEND_LOOSE_FSYNC = 1,
    }

    /// <summary>
    ///  Options for configuring a loose object backend.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_odb_backend_loose_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_ODB_BACKEND_LOOSE_OPTIONS_VERSION = 1;


        /// <summary>
        ///  version for the struct
        /// </summary>
        public uint version;

        /// <summary>
        ///  A combination of the `git_odb_backend_loose_flag_t` types.
        /// </summary>
        public uint flags;

        /// <summary>
        ///  zlib compression level to use (0-9), where 1 is the fastest
        ///  at the expense of larger files, and 9 produces the best
        ///  compression at the expense of speed. 0 indicates that no
        ///  compression should be performed. -1 is the default
        ///  (currently optimizing for speed).
        /// </summary>
        public int compression_level;

        /// <summary>
        ///  Permissions to use creating a directory or 0 for defaults
        /// </summary>
        public uint dir_mode;

        /// <summary>
        ///  Permissions to use creating a file or 0 for defaults
        /// </summary>
        public uint file_mode;

        /// <summary>
        ///  Type of object IDs to use for this object database, or 0 for
        ///  default (currently SHA1).
        /// </summary>
        public git_oid_t oid_type;
    }

    /// <summary>
    ///  Streaming mode
    /// </summary>
    public enum git_odb_stream_t
    {
        GIT_STREAM_RDONLY = 2,
        GIT_STREAM_WRONLY = 4,
        GIT_STREAM_RW = 6,
    }

    /// <summary>
    ///  A stream to read/write from a backend.
    ///
    ///  <para>
    ///   This represents a stream of data being written to or read
    ///   from a backend. When writing, the frontend functions take
    ///   care of calculating the object's id and all `finalize_write`
    ///   needs to do is store the object with the id it is passed.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_odb_stream
    {
        public IntPtr backend;

        public uint mode;

        public IntPtr hash_ctx;

        public long declared_size;

        public long received_bytes;

        /// <summary>
        ///  Write at most `len` bytes into `buffer` and advance the
        ///  stream.
        /// </summary>
        public IntPtr read;

        /// <summary>
        ///  Write `len` bytes from `buffer` into the stream.
        /// </summary>
        public IntPtr write;

        /// <summary>
        ///  Store the contents of the stream as an object with the id
        ///  specified in `oid`.
        ///
        ///  <para>
        ///   This method might not be invoked if: - an error occurs
        ///   earlier with the `write` callback, - the object referred
        ///   to by `oid` already exists in any backend, or - the
        ///   final number of received bytes differs from the size
        ///   declared with `git_odb_open_wstream()`
        ///  </para>
        /// </summary>
        public IntPtr finalize_write;

        /// <summary>
        ///  Free the stream's memory.
        ///
        ///  <para>
        ///   This method might be called without a call to
        ///   `finalize_write` if an error occurs or if the object is
        ///   already present in the ODB.
        ///  </para>
        /// </summary>
        public IntPtr free;
    }

    /// <summary>
    ///  A stream to write a pack file to the ODB
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_odb_writepack
    {
        public IntPtr backend;

        public IntPtr append;

        public IntPtr commit;

        public IntPtr free;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Initialize git_odb_backend_pack_options structure
        ///
        ///  <para>
        ///   Initialize a `git_odb_backend_pack_options` with default
        ///   values. Equivalent to creating an instance with
        ///   GIT_ODB_BACKEND_PACK_OPTIONS_INIT.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_odb_backend_pack_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass
        ///  `GIT_ODB_BACKEND_PACK_OPTIONS_VERSION`
        /// </param>
        /// <returns>0 on success or -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_backend_pack_options_init(
            ref git_odb_backend_pack_options opts,
            uint version);

        /// <summary>
        ///  Initialize git_odb_backend_loose_options structure
        ///
        ///  <para>
        ///   Initialize a `git_odb_backend_loose_options` with
        ///   default values. Equivalent to creating an instance with
        ///   GIT_ODB_BACKEND_LOOSE_OPTIONS_INIT.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_odb_backend_loose_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass
        ///  `GIT_ODB_BACKEND_LOOSE_OPTIONS_VERSION`
        /// </param>
        /// <returns>0 on success or -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_backend_loose_options_init(
            ref git_odb_backend_loose_options opts,
            uint version);

        /// <summary>
        ///  Create a backend for a directory containing packfiles.
        /// </summary>
        /// <param name="_out">
        ///  location to store the odb backend pointer
        /// </param>
        /// <param name="objects_dir">
        ///  the Git repository's objects directory
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_backend_pack(
            out git_odb_backend* _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string objects_dir);

        /// <summary>
        ///  Create a backend out of a single packfile
        ///
        ///  <para>
        ///   This can be useful for inspecting the contents of a
        ///   single packfile.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  location to store the odb backend pointer
        /// </param>
        /// <param name="index_file">
        ///  path to the packfile's .idx file
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_backend_one_pack(
            out git_odb_backend* _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string index_file);

        /// <summary>
        ///  Create a backend for loose objects
        /// </summary>
        /// <param name="_out">
        ///  location to store the odb backend pointer
        /// </param>
        /// <param name="objects_dir">
        ///  the Git repository's objects directory
        /// </param>
        /// <param name="compression_level">
        ///  zlib compression level (0-9), or -1 for the default
        /// </param>
        /// <param name="do_fsync">
        ///  if non-zero, perform an fsync on write
        /// </param>
        /// <param name="dir_mode">
        ///  permission to use when creating directories, or 0 for
        ///  default
        /// </param>
        /// <param name="file_mode">
        ///  permission to use when creating directories, or 0 for
        ///  default
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_backend_loose(
            out git_odb_backend* _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string objects_dir,
            int compression_level,
            int do_fsync,
            uint dir_mode,
            uint file_mode);
    }
}
