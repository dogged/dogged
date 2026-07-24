using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  In-memory representation of a blob object.
    /// </summary>
    public struct git_blob
    {
    }

    /// <summary>
    ///  Flags to control the functionality of `git_blob_filter`.
    /// </summary>
    [Flags]
    public enum git_blob_filter_flag_t
    {
        /// <summary>
        ///  When set, filters will not be applied to binary files.
        /// </summary>
        GIT_BLOB_FILTER_CHECK_FOR_BINARY = 1,

        /// <summary>
        ///  When set, filters will not load configuration from the
        ///  system-wide `gitattributes` in `/etc` (or system
        ///  equivalent).
        /// </summary>
        GIT_BLOB_FILTER_NO_SYSTEM_ATTRIBUTES = 2,

        /// <summary>
        ///  When set, filters will be loaded from a `.gitattributes`
        ///  file in the HEAD commit.
        /// </summary>
        GIT_BLOB_FILTER_ATTRIBUTES_FROM_HEAD = 4,

        /// <summary>
        ///  When set, filters will be loaded from a `.gitattributes`
        ///  file in the specified commit.
        /// </summary>
        GIT_BLOB_FILTER_ATTRIBUTES_FROM_COMMIT = 8,
    }

    /// <summary>
    ///  The options used when applying filter options to a file.
    ///
    ///  <para>
    ///   Initialize with `GIT_BLOB_FILTER_OPTIONS_INIT`.
    ///   Alternatively, you can use `git_blob_filter_options_init`.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_blob_filter_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_BLOB_FILTER_OPTIONS_VERSION = 1;


        /// <summary>
        ///  Version number of the options structure.
        /// </summary>
        public int version;

        /// <summary>
        ///  Flags to control the filtering process, see
        ///  `git_blob_filter_flag_t` above.
        /// </summary>
        public git_blob_filter_flag_t flags;

        /// <summary>
        ///  This value is unused and reserved for API compatibility.
        /// </summary>
        public IntPtr commit_id;

        /// <summary>
        ///  The commit to load attributes from, when
        ///  `GIT_BLOB_FILTER_ATTRIBUTES_FROM_COMMIT` is specified.
        /// </summary>
        public git_oid attr_commit_id;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Lookup a blob object from a repository.
        /// </summary>
        /// <param name="blob">pointer to the looked up blob</param>
        /// <param name="repo">
        ///  the repo to use when locating the blob.
        /// </param>
        /// <param name="id">identity of the blob to locate.</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_blob_lookup(
            out git_blob* blob,
            git_repository* repo,
            ref git_oid id);

        /// <summary>
        ///  Lookup a blob object from a repository, given a prefix of
        ///  its identifier (short id).
        /// </summary>
        /// <param name="blob">pointer to the looked up blob</param>
        /// <param name="repo">
        ///  the repo to use when locating the blob.
        /// </param>
        /// <param name="id">identity of the blob to locate.</param>
        /// <param name="len">the length of the short identifier</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_blob_lookup_prefix(
            out git_blob* blob,
            git_repository* repo,
            ref git_oid id,
            UIntPtr len);

        /// <summary>
        ///  Close an open blob
        ///
        ///  <para>
        ///   This is a wrapper around git_object_free()
        ///  </para>
        ///
        ///  <para>
        ///   IMPORTANT: It *is* necessary to call this method when
        ///   you stop using a blob. Failure to do so will cause a
        ///   memory leak.
        ///  </para>
        /// </summary>
        /// <param name="blob">the blob to close</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_blob_free(
            git_blob* blob);

        /// <summary>
        ///  Get the id of a blob.
        /// </summary>
        /// <param name="blob">a previously loaded blob.</param>
        /// <returns>SHA1 hash for this blob.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_blob_id(
            git_blob* blob);

        /// <summary>
        ///  Get the repository that contains the blob.
        /// </summary>
        /// <param name="blob">A previously loaded blob.</param>
        /// <returns>Repository that contains this blob.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_repository* git_blob_owner(
            git_blob* blob);

        /// <summary>
        ///  Get a read-only buffer with the raw content of a blob.
        ///
        ///  <para>
        ///   A pointer to the raw content of a blob is returned; this
        ///   pointer is owned internally by the object and shall not
        ///   be free'd. The pointer may be invalidated at a later
        ///   time.
        ///  </para>
        /// </summary>
        /// <param name="blob">pointer to the blob</param>
        /// <returns>the pointer, or NULL on error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe byte* git_blob_rawcontent(
            git_blob* blob);

        /// <summary>
        ///  Get the size in bytes of the contents of a blob
        /// </summary>
        /// <param name="blob">pointer to the blob</param>
        /// <returns>size in bytes</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe long git_blob_rawsize(
            git_blob* blob);

        /// <summary>
        ///  Initialize git_blob_filter_options structure
        ///
        ///  <para>
        ///   Initializes a `git_blob_filter_options` with default
        ///   values. Equivalent to creating an instance with
        ///   `GIT_BLOB_FILTER_OPTIONS_INIT`.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_blob_filter_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass GIT_BLOB_FILTER_OPTIONS_VERSION
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_blob_filter_options_init(
            ref git_blob_filter_options opts,
            uint version);

        /// <summary>
        ///  Get a buffer with the filtered content of a blob.
        ///
        ///  <para>
        ///   This applies filters as if the blob was being checked
        ///   out to the working directory under the specified
        ///   filename. This may apply CRLF filtering or other types
        ///   of changes depending on the file attributes set for the
        ///   blob and the content detected in it.
        ///  </para>
        ///
        ///  <para>
        ///   The output is written into a `git_buf` which the caller
        ///   must dispose when done (via `git_buf_dispose`).
        ///  </para>
        ///
        ///  <para>
        ///   If no filters need to be applied, then the `out` buffer
        ///   will just be populated with a pointer to the raw content
        ///   of the blob. In that case, be careful to *not* free the
        ///   blob until done with the buffer or copy it into memory
        ///   you own.
        ///  </para>
        /// </summary>
        /// <param name="_out">The git_buf to be filled in</param>
        /// <param name="blob">Pointer to the blob</param>
        /// <param name="as_path">
        ///  Path used for file attribute lookups, etc.
        /// </param>
        /// <param name="opts">Options to use for filtering the blob</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_blob_filter(
            ref git_buf _out,
            git_blob* blob,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string as_path,
            ref git_blob_filter_options opts);

        /// <summary>
        ///  Read a file from the working folder of a repository and
        ///  write it to the object database.
        /// </summary>
        /// <param name="id">return the id of the written blob</param>
        /// <param name="repo">
        ///  repository where the blob will be written. this repository
        ///  cannot be bare
        /// </param>
        /// <param name="relative_path">
        ///  file from which the blob will be created, relative to the
        ///  repository's working dir
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_blob_create_from_workdir(
            out git_oid id,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string relative_path);

        /// <summary>
        ///  Read a file from the filesystem (not necessarily inside the
        ///  working folder of the repository) and write it to the object
        ///  database.
        /// </summary>
        /// <param name="id">return the id of the written blob</param>
        /// <param name="repo">
        ///  repository where the blob will be written. this repository
        ///  can be bare or not
        /// </param>
        /// <param name="path">
        ///  file from which the blob will be created
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_blob_create_from_disk(
            out git_oid id,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path);

        /// <summary>
        ///  Create a stream to write a new blob into the object
        ///  database.
        ///
        ///  <para>
        ///   This function may need to buffer the data on disk and
        ///   will in general not be the right choice if you know the
        ///   size of the data to write. If you have data in memory,
        ///   use `git_blob_create_from_buffer()`. If you do not, but
        ///   know the size of the contents (and don't want/need to
        ///   perform filtering), use `git_odb_open_wstream()`.
        ///  </para>
        ///
        ///  <para>
        ///   Don't close this stream yourself but pass it to
        ///   `git_blob_create_from_stream_commit()` to commit the
        ///   write to the object db and get the object id.
        ///  </para>
        ///
        ///  <para>
        ///   If the `hintpath` parameter is filled, it will be used
        ///   to determine what git filters should be applied to the
        ///   object before it is written to the object database.
        ///  </para>
        /// </summary>
        /// <param name="_out">the stream into which to write</param>
        /// <param name="repo">
        ///  Repository where the blob will be written. This repository
        ///  can be bare or not.
        /// </param>
        /// <param name="hintpath">
        ///  If not NULL, will be used to select data filters to apply
        ///  onto the content of the blob to be created.
        /// </param>
        /// <returns>0 or error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_blob_create_from_stream(
            out git_writestream* _out,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string hintpath);

        /// <summary>
        ///  Close the stream and finalize writing the blob to the object
        ///  database.
        ///
        ///  <para>
        ///   The stream will be closed and freed.
        ///  </para>
        /// </summary>
        /// <param name="_out">the id of the new blob</param>
        /// <param name="stream">the stream to close</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_blob_create_from_stream_commit(
            out git_oid _out,
            ref git_writestream stream);

        /// <summary>
        ///  Write an in-memory buffer to the object database as a blob.
        /// </summary>
        /// <param name="id">return the id of the written blob</param>
        /// <param name="repo">
        ///  repository where the blob will be written
        /// </param>
        /// <param name="buffer">data to be written into the blob</param>
        /// <param name="len">length of the data</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_blob_create_from_buffer(
            out git_oid id,
            git_repository* repo,
            IntPtr buffer,
            UIntPtr len);

        /// <summary>
        ///  Determine if the blob content is most likely binary or not.
        ///
        ///  <para>
        ///   The heuristic used to guess if a file is binary is taken
        ///   from core git: Searching for NUL bytes and looking for a
        ///   reasonable ratio of printable to non-printable
        ///   characters among the first 8000 bytes.
        ///  </para>
        /// </summary>
        /// <param name="blob">
        ///  The blob which content should be analyzed
        /// </param>
        /// <returns>
        ///  1 if the content of the blob is detected as binary; 0
        ///  otherwise.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_blob_is_binary(
            git_blob* blob);

        /// <summary>
        ///  Determine if the given content is most certainly binary or
        ///  not; this is the same mechanism used by `git_blob_is_binary`
        ///  but only looking at raw data.
        /// </summary>
        /// <param name="data">
        ///  The blob data which content should be analyzed
        /// </param>
        /// <param name="len">The length of the data</param>
        /// <returns>
        ///  1 if the content of the blob is detected as binary; 0
        ///  otherwise.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_blob_data_is_binary(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string data,
            UIntPtr len);

        /// <summary>
        ///  Create an in-memory copy of a blob. The copy must be
        ///  explicitly free'd or it will leak.
        /// </summary>
        /// <param name="_out">
        ///  Pointer to store the copy of the object
        /// </param>
        /// <param name="source">Original object to copy</param>
        /// <returns>0.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_blob_dup(
            out git_blob* _out,
            git_blob* source);
    }
}
