using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_odb_backend_read_cb(
        out byte* buffer,
        out UIntPtr len,
        out git_object_t type,
        git_odb_backend* backend,
        ref git_oid oid);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_odb_read_prefix_cb(
        ref git_oid oid,
        out byte* buffer,
        out UIntPtr len,
        out git_object_t type,
        git_odb_backend* backend,
        ref git_oid short_oid,
        UIntPtr short_oid_len);

    /// <summary>
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_odb_read_header_cb(
        out UIntPtr len,
        out git_object_t type,
        git_odb_backend* backend,
        ref git_oid oid);

    /// <summary>
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_odb_write_cb(
        git_odb_backend* backend,
        ref git_oid oid,
        byte* buffer,
        UIntPtr len,
        git_object_t type);

    /// <summary>
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_odb_writestream_cb(
        out git_odb_stream* stream,
        git_odb_backend* backend,
        Int64 len,
        git_object_t type);

    /// <summary>
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_odb_readstream_cb(
        out git_odb_stream* stream,
        out UIntPtr len,
        out git_object_t type,
        git_odb_backend* backend,
        ref git_oid oid);

    /// <summary>
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_odb_exists_cb(
        git_odb_backend* backend,
        ref git_oid oid);

    /// <summary>
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_odb_exists_prefix_cb(
        ref git_oid oid,
        git_odb_backend* backend,
        ref git_oid short_oid,
        UIntPtr short_oid_len);

    /// <summary>
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_odb_refresh_cb(
        git_odb_backend* backend);

    /// <summary>
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_odb_foreach_object_cb(
        git_odb_backend* backend,
        git_odb_foreach_cb cb,
        IntPtr payload);

    /// <summary>
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_odb_writepack_cb(
        out git_odb_writepack* writepack,
        git_odb_backend* backend,
        ref git_odb odb,
        git_indexer_progress_cb progress_cb,
        IntPtr progress_payload);

    /// <summary>
    /// "Freshens" an already existing object, updating its last-used
    /// time.  This occurs when `git_odb_write` was called, but the
    /// object already existed (and will not be re-written).  The
    /// underlying implementation may want to update last-used timestamps.
    ///
    /// If callers implement this, they should return `0` if the object
    /// exists and was freshened, and non-zero otherwise.
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_odb_freshen_cb(
        git_odb_backend* backend,
        ref git_oid oid);

    /// <summary>
    /// Frees any resources held by the odb (including the `git_odb_backend`
    /// itself). An odb backend implementation must provide this function.
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void git_odb_free_cb(
        git_odb_backend* backend);

    /// <summary>
    /// </summary>
    public unsafe struct git_odb_backend { }

    /// <summary>
    /// A custom backend in an object database.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_odb_backend_managed
    {
        /// <summary>
        /// </summary>
        public uint version;

        /// <summary>
        /// </summary>
        public readonly git_odb* odb;

        /// <summary>
        /// </summary>
        public git_odb_backend_read_cb read;

        /// <summary>
        /// </summary>
        public git_odb_read_prefix_cb read_prefix;

        /// <summary>
        /// </summary>
        public git_odb_read_header_cb read_header;

        /// <summary>
        /// </summary>
        public git_odb_write_cb write;

        /// <summary>
        /// </summary>
        public git_odb_writestream_cb writestream;

        /// <summary>
        /// </summary>
        public git_odb_readstream_cb readstream;

        /// <summary>
        /// </summary>
        public git_odb_exists_cb exists;

        /// <summary>
        /// </summary>
        public git_odb_exists_prefix_cb exists_prefix;

        /// <summary>
        /// </summary>
        public git_odb_refresh_cb refresh;

        /// <summary>
        /// </summary>
        public git_odb_foreach_object_cb foreach_object;

        /// <summary>
        /// </summary>
        public git_odb_writepack_cb writepack;

        /// <summary>
        /// "Freshens" an already existing object, updating its last-used
        /// time.  This occurs when `git_odb_write` was called, but the
        /// object already existed (and will not be re-written).  The
        /// underlying implementation may want to update last-used timestamps.
        ///
        /// If callers implement this, they should return `0` if the object
        /// exists and was freshened, and non-zero otherwise.
        /// </summary>
        public git_odb_freshen_cb freshen;

        /// <summary>
        /// Frees any resources held by the odb (including the `git_odb_backend`
        /// itself). An odb backend implementation must provide this function.
        /// </summary>
        public git_odb_free_cb free;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_odb_stream
    {
        git_odb_backend *backend;
        uint mode;
        void* hash_ctx;

        Int64 declared_size;
        Int64 received_bytes;

        /// <summary>
        /// Write at most `len` bytes into `buffer` and advance the stream.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate int read(
            ref git_odb_stream stream,
            byte* buffer,
            UIntPtr len);

        /// <summary>
        /// Write `len` bytes from `buffer` into the stream.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate int write(
            ref git_odb_stream stream,
            byte* buffer,
            UIntPtr len);

        /// <summary>
        /// Store the contents of the stream as an object with the id
        /// specified in `oid`.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate int finalize_write(
            git_odb_stream *stream,
            ref git_oid oid);

        /// <summary>
        /// Free the stream's memory.
        /// </summary>
        public unsafe delegate void free(
            git_odb_stream* stream);
    }

    /// <summary>
    /// A stream to write a pack file to the ODB.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_odb_writepack
    {
        /// <summary>
        /// </summary>
        public readonly git_odb_backend* odb;

        /// <summary>
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate int append(
            ref git_odb_writepack writepack,
            byte* data,
            UIntPtr len,
            ref git_indexer_progress stats);

        /// <summary>
        /// </summary>

        /// <summary>
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate int commit(
            ref git_odb_writepack writepack,
            ref git_indexer_progress stats);

        /// <summary>
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate int free(
            ref git_odb_writepack writepack);
    }
}
