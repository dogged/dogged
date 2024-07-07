using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    /// Function declaration for callbacks from the git_odb_foreach function.
    /// </summary>
    /// <param name="id">The object ID contained in the database</param>
    /// <param name="payload">The payload provided to the git_odb_foreach function</param>
    /// <returns>0 on success or non-zero to stop iteration</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_odb_foreach_cb(
        git_oid* id,
        UIntPtr payload);

    /// <summary>
    /// An object database stores the objects (commit, trees, blobs, tags,
    /// etc) for a repository.
    /// </summary>
    public struct git_odb
    {
    }

    /// <summary>
    /// An object that was read from the object database.
    /// </summary>
    public struct git_odb_object
    {
    }

    public static partial class libgit2
    {
        /// <summary>
        /// Add a custom backend to an existing object database.
        ///
        /// <para>
        /// The backends are checked in relative ordering, based on the
        /// value of the `priority` parameter.
        /// </para>
        /// </summary>
        /// <param name="odb">Database to add the backend to</param>
        /// <param name="backend">The object database backend to add</param>
        /// <param name="priority">Value for ordering the backends queue</param>
        /// <returns>0 on success or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_add_backend(
            git_odb* odb,
            git_odb_backend* backend,
            int priority);

        /// <summary>
        /// Create a backend out of a single packfile.  This can be useful for
        /// inspecting the contents of a single packfile.
        /// </summary>
        /// <param name="backend">Pointer to the odb backend that will be created.</param>
        /// <param name="index_file">The path to the packfile's .idx file</param>
        /// <returns>0 on success or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_backend_one_pack(
            out git_odb_backend* backend,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string index_file);

        /// <summary>
        /// Create a backend for packfiles within a typical git object
        /// directory that contains a "pack" directory that will be consulted
        /// to find the packfiles to load.  Each packfile within that "pack"
        /// directory will be consulted as storage for the object database.
        /// </summary>
        /// <param name="backend">Pointer to the odb backend that will be created.</param>
        /// <param name="objects_dir">The path to the directory containing packfiles</param>
        /// <returns>0 on success or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_backend_pack(
            out git_odb_backend* backend,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string objects_dir);

        /// <summary>
        /// Create a backend for loose object files stored within a typical
        /// git object directory containing a "fanout" of subdirectories,
        /// named with the first two characters of the object's ID and
        /// objects stored in files beneath that, each named with the
        /// remaining 38 characters of their ID.
        /// </summary>
        /// <param name="backend">Pointer to the odb backend that will be created.</param>
        /// <param name="objects_dir">The path to the directory containing packfiles</param>
        /// <param name="compression_level">zlib compression level to use</param>
        /// <param name="do_fsync">If non-zero, call fsync after writing</param>
        /// <param name="dir_mode">POSIX open(2)-style permissions to use creating a directory or 0 for defaults</param>
        /// <param name="file_mode">POSIX open(2)-style permissions to use creating a file or 0 for defaults</param>
        /// <returns>0 on success or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_backend_loose(
            out git_odb_backend* backend,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string objects_dir,
            int compression_level,
            int do_fsync,
            uint dir_mode,
            uint file_mode);

        /// <summary>
        /// List all objects available in the database.
        ///
        /// <para>
        /// The callback will be called for each object available in the
        /// database. Note that the objects are likely to be returned in the index
        /// order, which would make accessing the objects in that order inefficient.
        /// Return a non-zero value from the callback to stop looping.
        /// </para>
        /// </summary>
        /// <param name="odb">The database to iterate.</param>
        /// <param name="cb">The callback to invoke for each object</param>
        /// <param name="payload">Custom data to pass back to the callback</param>
        /// <returns>0 on success, non-zero callback return value, or error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_foreach(git_odb* odb, git_odb_foreach_cb cb, IntPtr payload);

        /// <summary>
        /// Close an open database object.
        /// </summary>
        /// <param name="odb">The database to close.  If null, no action is taken.</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_odb_free(git_odb* odb);

        /// <summary>
        /// Create a new object database with no backends.
        ///
        /// <para>
        /// Before the ODB can be used for read/writing, a custom database
        /// backend must be manually added using `git_odb_add_backend()`
        /// </para>
        /// </summary>
        /// <param name="odb">Pointer to the object database that will be created.</param>
        /// <returns>0 on success or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_new(out git_odb *odb);

        /// <summary>
        /// Return the data of an ODB object.  This is the uncompressed,
        /// raw data as read from the ODB, without the leading header.
        /// </summary>
        /// <param name="obj">The object to read its contents</param>
        /// <returns>The raw content of the object</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe byte* git_odb_object_data(git_odb_object *obj);

        /// <summary>
        /// Return the OID of an ODB object; this is the OID from which the
        // object was read from.
        /// </summary>
        /// <param name="object">The object to lookup the ID of</param>
        /// <returns>A pointer to the object ID</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_odb_object_id(git_odb_object* obj);

        /// <summary>
        /// Return the size of an ODB object.  This is the real size of the
        /// data buffer, not the actual size of the object.
        /// </summary>
        /// <param name="obj">The object to query</param>
        /// <returns>The size of the object</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_odb_object_size(git_odb_object* obj);

        /// <summary>
        /// Return the type of an ODB object.
        /// </summary>
        /// <param name="obj">The object to query</param>
        /// <returns>The type of object</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_object_t git_odb_object_type(git_odb_object* obj);

        /// <summary>
        /// Free an ODB object.  This method must always be called once a
        /// git_odb_object is no longer needed, otherwise memory will leak.
        /// </summary>
        /// <param name="obj">Object to close</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_odb_object_free(git_odb_object* obj);

        /// <summary>
        /// Read an object from the database.
        ///
        /// <para>
        /// This method queries all available ODB backends
        /// trying to read the given OID.
        /// </para>
        /// </summary>
        /// <param name="obj">Point to store the read object</param>
        /// <param name="odb">The database to read from</param>
        /// <param name="id">The object ID to read</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_read(out git_odb_object* obj, git_odb* odb, ref git_oid id);

        /// <summary>
        /// Read the header of an object from the database, without reading
        /// its full contents.
        ///
        /// <para>
        /// The header includes the length and the type of an object.
        /// </para>
        ///
        /// <para>
        /// Note that most backends do not support reading only the header
        /// of an object, so the whole object will be read and then the
        /// header will be returned.
        /// </para>
        /// </summary>
        /// <param name="len_out">Pointer to store the length</param>
        /// <param name="type_out">Pointer to store the type</param>
        /// <param name="odb">Database to search for the object in</param>
        /// <param name="id">ID of the objecft to read</param>
        /// <returns>0 on success, GIT_ENOTFOUND if the object is not in the database, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_read_header(out UIntPtr len_out, out git_object_t type, git_odb* odb, ref git_oid id);

        /// <summary>
        /// Write an object directly into the object database.
        ///
        /// <para>
        /// This method writes a full object straight into the ODB.
        /// For most cases, it is preferred to write objects through a write
        /// stream, which is both faster and less memory intensive, specially
        /// for big objects.
        /// </para>
        ///
        /// <para>
        /// This method is provided for compatibility with custom backends
        /// which are not able to support streaming writes
        /// </para>
        /// </summary>
        /// <param name="id">Pointer to store the OID result of the write</param>
        /// <param name="odb">Object database to store the object</param>
        /// <param name="data">Buffer with the data to store</param>
        /// <param name="len">Size of the buffer</param>
        /// <param name="type">Type of the data to store</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_write(ref git_oid id, git_odb* odb, byte* data, UIntPtr len, git_object_t type);
    }
}
