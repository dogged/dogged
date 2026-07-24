using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public sealed class OdbHandle : SafeHandle
    {
        public OdbHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe OdbHandle(git_odb* nativeOdb) :
            base((IntPtr)nativeOdb, true)
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
            libgit2.git_odb_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public sealed class OdbObjectHandle : SafeHandle
    {
        public OdbObjectHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe OdbObjectHandle(git_odb_object* nativeOdbObject) :
            base((IntPtr)nativeOdbObject, true)
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
            libgit2.git_odb_object_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Create a new object database with no backends.
        ///
        ///  <para>
        ///   Before the ODB can be used for read/writing, a custom
        ///   database backend must be manually added using
        ///   `git_odb_add_backend()`
        ///  </para>
        /// </summary>
        /// <param name="odb">
        ///  location to store the database pointer, if opened.
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_new(
            out OdbHandle odb);

        /// <summary>
        ///  Create a new object database and automatically add the two
        ///  default backends:
        ///
        ///  <para>
        ///   - git_odb_backend_loose: read and write loose object
        ///   files from disk, assuming `objects_dir` as the Objects
        ///   folder
        ///  </para>
        ///
        ///  <para>
        ///   - git_odb_backend_pack: read objects from packfiles,
        ///   assuming `objects_dir` as the Objects folder which
        ///   contains a 'pack/' folder with the corresponding data
        ///  </para>
        /// </summary>
        /// <param name="odb_out">
        ///  location to store the database pointer, if opened. Set to
        ///  NULL if the open failed.
        /// </param>
        /// <param name="objects_dir">
        ///  path of the backends' "objects" directory.
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_open(
            out OdbHandle odb_out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string objects_dir);

        /// <summary>
        ///  Add an on-disk alternate to an existing Object DB.
        ///
        ///  <para>
        ///   Note that the added path must point to an `objects`, not
        ///   to a full repository, to use it as an alternate store.
        ///  </para>
        ///
        ///  <para>
        ///   Alternate backends are always checked for objects
        ///   *after* all the main backends have been exhausted.
        ///  </para>
        ///
        ///  <para>
        ///   Writing is disabled on alternate backends.
        ///  </para>
        /// </summary>
        /// <param name="odb">database to add the backend to</param>
        /// <param name="path">
        ///  path to the objects folder for the alternate
        /// </param>
        /// <returns>0 on success, error code otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_add_disk_alternate(
            OdbHandle odb,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path);

        /// <summary>
        ///  Close an open object database.
        /// </summary>
        /// <param name="db">
        ///  database pointer to close. If NULL no action is taken.
        /// </param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_odb_free(
            IntPtr db);

        /// <summary>
        ///  Read an object from the database.
        ///
        ///  <para>
        ///   This method queries all available ODB backends trying to
        ///   read the given OID.
        ///  </para>
        ///
        ///  <para>
        ///   The returned object is reference counted and internally
        ///   cached, so it should be closed by the user once it's no
        ///   longer in use.
        ///  </para>
        /// </summary>
        /// <param name="obj">pointer where to store the read object</param>
        /// <param name="db">database to search for the object in.</param>
        /// <param name="id">identity of the object to read.</param>
        /// <returns>
        ///  0 if the object was read, GIT_ENOTFOUND if the object is not
        ///  in the database.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_read(
            out OdbObjectHandle obj,
            OdbHandle db,
            ref git_oid id);

        /// <summary>
        ///  Read an object from the database, given a prefix of its
        ///  identifier.
        ///
        ///  <para>
        ///   This method queries all available ODB backends trying to
        ///   match the 'len' first hexadecimal characters of the
        ///   'short_id'. The remaining (GIT_OID_SHA1_HEXSIZE-len)*4
        ///   bits of 'short_id' must be 0s. 'len' must be at least
        ///   GIT_OID_MINPREFIXLEN, and the prefix must be long enough
        ///   to identify a unique object in all the backends; the
        ///   method will fail otherwise.
        ///  </para>
        ///
        ///  <para>
        ///   The returned object is reference counted and internally
        ///   cached, so it should be closed by the user once it's no
        ///   longer in use.
        ///  </para>
        /// </summary>
        /// <param name="obj">pointer where to store the read object</param>
        /// <param name="db">database to search for the object in.</param>
        /// <param name="short_id">
        ///  a prefix of the id of the object to read.
        /// </param>
        /// <param name="len">the length of the prefix</param>
        /// <returns>
        ///  0 if the object was read, GIT_ENOTFOUND if the object is not
        ///  in the database. GIT_EAMBIGUOUS if the prefix is ambiguous
        ///  (several objects match the prefix)
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_read_prefix(
            out OdbObjectHandle obj,
            OdbHandle db,
            ref git_oid short_id,
            UIntPtr len);

        /// <summary>
        ///  Read the header of an object from the database, without
        ///  reading its full contents.
        ///
        ///  <para>
        ///   The header includes the length and the type of an
        ///   object.
        ///  </para>
        ///
        ///  <para>
        ///   Note that most backends do not support reading only the
        ///   header of an object, so the whole object will be read
        ///   and then the header will be returned.
        ///  </para>
        /// </summary>
        /// <param name="len_out">pointer where to store the length</param>
        /// <param name="type_out">pointer where to store the type</param>
        /// <param name="db">database to search for the object in.</param>
        /// <param name="id">identity of the object to read.</param>
        /// <returns>
        ///  0 if the object was read, GIT_ENOTFOUND if the object is not
        ///  in the database.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_read_header(
            out UIntPtr len_out,
            out git_object_t type_out,
            OdbHandle db,
            ref git_oid id);

        /// <summary>
        ///  Determine if the given object can be found in the object
        ///  database.
        /// </summary>
        /// <param name="db">
        ///  database to be searched for the given object.
        /// </param>
        /// <param name="id">the object to search for.</param>
        /// <returns>1 if the object was found, 0 otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_exists(
            OdbHandle db,
            ref git_oid id);

        /// <summary>
        ///  Determine if the given object can be found in the object
        ///  database, with extended options.
        /// </summary>
        /// <param name="db">
        ///  database to be searched for the given object.
        /// </param>
        /// <param name="id">the object to search for.</param>
        /// <param name="flags">
        ///  flags affecting the lookup (see `git_odb_lookup_flags_t`)
        /// </param>
        /// <returns>1 if the object was found, 0 otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_exists_ext(
            OdbHandle db,
            ref git_oid id,
            uint flags);

        /// <summary>
        ///  Determine if an object can be found in the object database
        ///  by an abbreviated object ID.
        /// </summary>
        /// <param name="_out">
        ///  The full OID of the found object if just one is found.
        /// </param>
        /// <param name="db">
        ///  The database to be searched for the given object.
        /// </param>
        /// <param name="short_id">
        ///  A prefix of the id of the object to read.
        /// </param>
        /// <param name="len">The length of the prefix.</param>
        /// <returns>
        ///  0 if found, GIT_ENOTFOUND if not found, GIT_EAMBIGUOUS if
        ///  multiple matches were found, other value < 0 if there was a
        ///  read error.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_exists_prefix(
            ref git_oid _out,
            OdbHandle db,
            ref git_oid short_id,
            UIntPtr len);

        /// <summary>
        ///  Determine if one or more objects can be found in the object
        ///  database by their abbreviated object ID and type.
        ///
        ///  <para>
        ///   The given array will be updated in place: for each
        ///   abbreviated ID that is unique in the database, and of
        ///   the given type (if specified), the full object ID,
        ///   object ID length (`GIT_OID_SHA1_HEXSIZE`) and type will
        ///   be written back to the array. For IDs that are not found
        ///   (or are ambiguous), the array entry will be zeroed.
        ///  </para>
        ///
        ///  <para>
        ///   Note that since this function operates on multiple
        ///   objects, the underlying database will not be asked to be
        ///   reloaded if an object is not found (which is unlike
        ///   other object database operations.)
        ///  </para>
        /// </summary>
        /// <param name="db">
        ///  The database to be searched for the given objects.
        /// </param>
        /// <param name="ids">
        ///  An array of short object IDs to search for
        /// </param>
        /// <param name="count">The length of the `ids` array</param>
        /// <returns>0 on success or an error code on failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_expand_ids(
            OdbHandle db,
            ref git_odb_expand_id ids,
            UIntPtr count);

        /// <summary>
        ///  Refresh the object database to load newly added files.
        ///
        ///  <para>
        ///   If the object databases have changed on disk while the
        ///   library is running, this function will force a reload of
        ///   the underlying indexes.
        ///  </para>
        ///
        ///  <para>
        ///   Use this function when you're confident that an external
        ///   application has tampered with the ODB.
        ///  </para>
        ///
        ///  <para>
        ///   NOTE that it is not necessary to call this function at
        ///   all. The library will automatically attempt to refresh
        ///   the ODB when a lookup fails, to see if the looked up
        ///   object exists on disk but hasn't been loaded yet.
        ///  </para>
        /// </summary>
        /// <param name="db">database to refresh</param>
        /// <returns>0 on success, error code otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_refresh(
            OdbHandle db);

        /// <summary>
        ///  List all objects available in the database
        ///
        ///  <para>
        ///   The callback will be called for each object available in
        ///   the database. Note that the objects are likely to be
        ///   returned in the index order, which would make accessing
        ///   the objects in that order inefficient. Return a non-zero
        ///   value from the callback to stop looping.
        ///  </para>
        /// </summary>
        /// <param name="db">database to use</param>
        /// <param name="cb">the callback to call for each object</param>
        /// <param name="payload">data to pass to the callback</param>
        /// <returns>
        ///  0 on success, non-zero callback return value, or error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_foreach(
            OdbHandle db,
            git_odb_foreach_cb cb,
            IntPtr payload);

        /// <summary>
        ///  Write an object directly into the ODB
        ///
        ///  <para>
        ///   This method writes a full object straight into the ODB.
        ///   For most cases, it is preferred to write objects through
        ///   a write stream, which is both faster and less memory
        ///   intensive, specially for big objects.
        ///  </para>
        ///
        ///  <para>
        ///   This method is provided for compatibility with custom
        ///   backends which are not able to support streaming writes
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  pointer to store the OID result of the write
        /// </param>
        /// <param name="odb">
        ///  object database where to store the object
        /// </param>
        /// <param name="data">buffer with the data to store</param>
        /// <param name="len">size of the buffer</param>
        /// <param name="type">type of the data to store</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_write(
            ref git_oid _out,
            OdbHandle odb,
            byte* data,
            UIntPtr len,
            git_object_t type);

        /// <summary>
        ///  Open a stream to write an object into the ODB
        ///
        ///  <para>
        ///   The type and final length of the object must be
        ///   specified when opening the stream.
        ///  </para>
        ///
        ///  <para>
        ///   The returned stream will be of type `GIT_STREAM_WRONLY`,
        ///   and it won't be effective until
        ///   `git_odb_stream_finalize_write` is called and returns
        ///   without an error
        ///  </para>
        ///
        ///  <para>
        ///   The stream must always be freed when done with
        ///   `git_odb_stream_free` or will leak memory.
        ///  </para>
        /// </summary>
        /// <param name="_out">pointer where to store the stream</param>
        /// <param name="db">
        ///  object database where the stream will write
        /// </param>
        /// <param name="size">
        ///  final size of the object that will be written
        /// </param>
        /// <param name="type">
        ///  type of the object that will be written
        /// </param>
        /// <returns>
        ///  0 if the stream was created; error code otherwise
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_open_wstream(
            out git_odb_stream* _out,
            OdbHandle db,
            long size,
            git_object_t type);

        /// <summary>
        ///  Free an odb stream
        /// </summary>
        /// <param name="stream">the stream to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_odb_stream_free(
            IntPtr stream);

        /// <summary>
        ///  Open a stream to read an object from the ODB
        ///
        ///  <para>
        ///   Note that most backends do *not* support streaming reads
        ///   because they store their objects as compressed/delta'ed
        ///   blobs.
        ///  </para>
        ///
        ///  <para>
        ///   It's recommended to use `git_odb_read` instead, which is
        ///   assured to work on all backends.
        ///  </para>
        ///
        ///  <para>
        ///   The returned stream will be of type `GIT_STREAM_RDONLY`
        ///   and will have the following methods:
        ///  </para>
        ///
        ///  <para>
        ///   - stream->read: read `n` bytes from the stream -
        ///   stream->free: free the stream
        ///  </para>
        ///
        ///  <para>
        ///   The stream must always be free'd or will leak memory.
        ///  </para>
        /// </summary>
        /// <param name="_out">pointer where to store the stream</param>
        /// <param name="len">
        ///  pointer where to store the length of the object
        /// </param>
        /// <param name="type">
        ///  pointer where to store the type of the object
        /// </param>
        /// <param name="db">
        ///  object database where the stream will read from
        /// </param>
        /// <param name="oid">
        ///  oid of the object the stream will read from
        /// </param>
        /// <returns>
        ///  0 if the stream was created, error code otherwise
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_open_rstream(
            out git_odb_stream* _out,
            out UIntPtr len,
            git_object_t* type,
            OdbHandle db,
            ref git_oid oid);

        /// <summary>
        ///  Open a stream for writing a pack file to the ODB.
        ///
        ///  <para>
        ///   If the ODB layer understands pack files, then the given
        ///   packfile will likely be streamed directly to disk (and a
        ///   corresponding index created). If the ODB layer does not
        ///   understand pack files, the objects will be stored in
        ///   whatever format the ODB layer uses.
        ///  </para>
        /// </summary>
        /// <param name="_out">pointer to the writepack functions</param>
        /// <param name="db">
        ///  object database where the stream will read from
        /// </param>
        /// <param name="progress_cb">
        ///  function to call with progress information. Be aware that
        ///  this is called inline with network and indexing operations,
        ///  so performance may be affected.
        /// </param>
        /// <param name="progress_payload">
        ///  payload for the progress callback
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_write_pack(
            out git_odb_writepack* _out,
            OdbHandle db,
            git_indexer_progress_cb progress_cb,
            IntPtr progress_payload);

        /// <summary>
        ///  Write a `multi-pack-index` file from all the `.pack` files
        ///  in the ODB.
        ///
        ///  <para>
        ///   If the ODB layer understands pack files, then this will
        ///   create a file called `multi-pack-index` next to the
        ///   `.pack` and `.idx` files, which will contain an index of
        ///   all objects stored in `.pack` files. This will allow for
        ///   O(log n) lookup for n objects (regardless of how many
        ///   packfiles there exist).
        ///  </para>
        /// </summary>
        /// <param name="db">
        ///  object database where the `multi-pack-index` file will be
        ///  written.
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_write_multi_pack_index(
            OdbHandle db);

        /// <summary>
        ///  Create a copy of an odb_object
        ///
        ///  <para>
        ///   The returned copy must be manually freed with
        ///   `git_odb_object_free`. Note that because of an
        ///   implementation detail, the returned copy will be the
        ///   same pointer as `source`: the object is internally
        ///   refcounted, so the copy still needs to be freed twice.
        ///  </para>
        /// </summary>
        /// <param name="dest">pointer where to store the copy</param>
        /// <param name="source">object to copy</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_object_dup(
            OdbObjectHandle dest,
            OdbObjectHandle source);

        /// <summary>
        ///  Close an ODB object
        ///
        ///  <para>
        ///   This method must always be called once a
        ///   `git_odb_object` is no longer needed, otherwise memory
        ///   will leak.
        ///  </para>
        /// </summary>
        /// <param name="_object">object to close</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_odb_object_free(
            IntPtr _object);

        /// <summary>
        ///  Return the OID of an ODB object
        ///
        ///  <para>
        ///   This is the OID from which the object was read from
        ///  </para>
        /// </summary>
        /// <param name="_object">the object</param>
        /// <returns>a pointer to the OID</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_odb_object_id(
            OdbObjectHandle _object);

        /// <summary>
        ///  Return the data of an ODB object
        ///
        ///  <para>
        ///   This is the uncompressed, raw data as read from the ODB,
        ///   without the leading header.
        ///  </para>
        ///
        ///  <para>
        ///   This pointer is owned by the object and shall not be
        ///   free'd.
        ///  </para>
        /// </summary>
        /// <param name="_object">the object</param>
        /// <returns>a pointer to the data</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe byte* git_odb_object_data(
            OdbObjectHandle _object);

        /// <summary>
        ///  Return the size of an ODB object
        ///
        ///  <para>
        ///   This is the real size of the `data` buffer, not the
        ///   actual size of the object.
        ///  </para>
        /// </summary>
        /// <param name="_object">the object</param>
        /// <returns>the size</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_odb_object_size(
            OdbObjectHandle _object);

        /// <summary>
        ///  Return the type of an ODB object
        /// </summary>
        /// <param name="_object">the object</param>
        /// <returns>the type</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_object_t git_odb_object_type(
            OdbObjectHandle _object);

        /// <summary>
        ///  Add a custom backend to an existing Object DB
        ///
        ///  <para>
        ///   The backends are checked in relative ordering, based on
        ///   the value of the `priority` parameter.
        ///  </para>
        ///
        ///  <para>
        ///   Read <sys /odb_backend.h> for more information.
        ///  </para>
        /// </summary>
        /// <param name="odb">database to add the backend to</param>
        /// <param name="backend">
        ///  pointer to a git_odb_backend instance
        /// </param>
        /// <param name="priority">
        ///  Value for ordering the backends queue
        /// </param>
        /// <returns>0 on success, error code otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_add_backend(
            OdbHandle odb,
            ref git_odb_backend backend,
            int priority);

        /// <summary>
        ///  Add a custom backend to an existing Object DB; this backend
        ///  will work as an alternate.
        ///
        ///  <para>
        ///   Alternate backends are always checked for objects
        ///   *after* all the main backends have been exhausted.
        ///  </para>
        ///
        ///  <para>
        ///   The backends are checked in relative ordering, based on
        ///   the value of the `priority` parameter.
        ///  </para>
        ///
        ///  <para>
        ///   Writing is disabled on alternate backends.
        ///  </para>
        ///
        ///  <para>
        ///   Read <sys /odb_backend.h> for more information.
        ///  </para>
        /// </summary>
        /// <param name="odb">database to add the backend to</param>
        /// <param name="backend">
        ///  pointer to a git_odb_backend instance
        /// </param>
        /// <param name="priority">
        ///  Value for ordering the backends queue
        /// </param>
        /// <returns>0 on success, error code otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_add_alternate(
            OdbHandle odb,
            ref git_odb_backend backend,
            int priority);

        /// <summary>
        ///  Get the number of ODB backend objects
        /// </summary>
        /// <param name="odb">object database</param>
        /// <returns>number of backends in the ODB</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_odb_num_backends(
            OdbHandle odb);

        /// <summary>
        ///  Lookup an ODB backend object by index
        /// </summary>
        /// <param name="_out">output pointer to ODB backend at pos</param>
        /// <param name="odb">object database</param>
        /// <param name="pos">
        ///  index into object database backend list
        /// </param>
        /// <returns>
        ///  0 on success, GIT_ENOTFOUND if pos is invalid, other errors
        ///  < 0
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_get_backend(
            out git_odb_backend* _out,
            OdbHandle odb,
            UIntPtr pos);

        /// <summary>
        ///  Set the git commit-graph for the ODB.
        ///
        ///  <para>
        ///   After a successful call, the ownership of the cgraph
        ///   parameter will be transferred to libgit2, and the caller
        ///   should not free it.
        ///  </para>
        ///
        ///  <para>
        ///   The commit-graph can also be unset by explicitly passing
        ///   NULL as the cgraph parameter.
        ///  </para>
        /// </summary>
        /// <param name="odb">object database</param>
        /// <param name="cgraph">the git commit-graph</param>
        /// <returns>0 on success; error code otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_set_commit_graph(
            OdbHandle odb,
            CommitGraphHandle cgraph);
    }
}
