using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Representation of a generic object in a repository
    /// </summary>
    public struct git_object
    {
        /// <summary>
        ///  Maximum size of a git object
        /// </summary>
        public const ulong GIT_OBJECT_SIZE_MAX = ulong.MaxValue;
    }

    /// <summary>
    ///  Options for calculating object IDs from raw content.
    ///
    ///  <para>
    ///   Initialize with `GIT_OBJECT_ID_OPTIONS_INIT`. Alternatively,
    ///   you can use `git_object_id_options_init`.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_object_id_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_OBJECT_ID_OPTIONS_VERSION = 1;


        /// <summary>
        ///  version for the struct
        /// </summary>
        public uint version;

        /// <summary>
        ///  Object type of the raw content; if not specified, this
        ///  defaults to `GIT_OBJECT_BLOB`.
        /// </summary>
        public git_object_t object_type;

        /// <summary>
        ///  Object ID type to generate; if not specified, this defaults
        ///  to `GIT_OID_DEFAULT`.
        /// </summary>
        public git_oid_t oid_type;

        /// <summary>
        ///  Filters to mutate the raw data with; these are ignored
        ///  unless the given raw object data is a blob.
        /// </summary>
        public IntPtr filters;
    }

    /// <summary>
    ///  Basic type (loose or packed) of any Git object.
    /// </summary>
    public enum git_object_t
    {
        /// <summary>
        ///  Object can be any of the following
        /// </summary>
        GIT_OBJECT_ANY = -2,

        /// <summary>
        ///  Object is invalid.
        /// </summary>
        GIT_OBJECT_INVALID = -1,

        /// <summary>
        ///  A commit object.
        /// </summary>
        GIT_OBJECT_COMMIT = 1,

        /// <summary>
        ///  A tree (directory listing) object.
        /// </summary>
        GIT_OBJECT_TREE = 2,

        /// <summary>
        ///  A file revision object.
        /// </summary>
        GIT_OBJECT_BLOB = 3,

        /// <summary>
        ///  An annotated tag object.
        /// </summary>
        GIT_OBJECT_TAG = 4,
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Lookup a reference to one of the objects in a repository.
        ///
        ///  <para>
        ///   The generated reference is owned by the repository and
        ///   should be closed with the `git_object_free` method
        ///   instead of free'd manually.
        ///  </para>
        ///
        ///  <para>
        ///   The 'type' parameter must match the type of the object
        ///   in the odb; the method will fail otherwise. The special
        ///   value 'GIT_OBJECT_ANY' may be passed to let the method
        ///   guess the object's type.
        ///  </para>
        /// </summary>
        /// <param name="_object">pointer to the looked-up object</param>
        /// <param name="repo">the repository to look up the object</param>
        /// <param name="id">the unique identifier for the object</param>
        /// <param name="type">the type of the object</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_object_lookup(
            out git_object* _object,
            git_repository* repo,
            ref git_oid id,
            git_object_t type);

        /// <summary>
        ///  Lookup a reference to one of the objects in a repository,
        ///  given a prefix of its identifier (short id).
        ///
        ///  <para>
        ///   The object obtained will be so that its identifier
        ///   matches the first 'len' hexadecimal characters (packets
        ///   of 4 bits) of the given `id`. `len` must be at least
        ///   `GIT_OID_MINPREFIXLEN`, and long enough to identify a
        ///   unique object matching the prefix; otherwise the method
        ///   will fail.
        ///  </para>
        ///
        ///  <para>
        ///   The generated reference is owned by the repository and
        ///   should be closed with the `git_object_free` method
        ///   instead of free'd manually.
        ///  </para>
        ///
        ///  <para>
        ///   The `type` parameter must match the type of the object
        ///   in the odb; the method will fail otherwise. The special
        ///   value `GIT_OBJECT_ANY` may be passed to let the method
        ///   guess the object's type.
        ///  </para>
        /// </summary>
        /// <param name="object_out">
        ///  pointer where to store the looked-up object
        /// </param>
        /// <param name="repo">the repository to look up the object</param>
        /// <param name="id">a short identifier for the object</param>
        /// <param name="len">the length of the short identifier</param>
        /// <param name="type">the type of the object</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_object_lookup_prefix(
            out git_object* object_out,
            git_repository* repo,
            ref git_oid id,
            UIntPtr len,
            git_object_t type);

        /// <summary>
        ///  Lookup an object that represents a tree entry.
        /// </summary>
        /// <param name="_out">
        ///  buffer that receives a pointer to the object (which must be
        ///  freed by the caller)
        /// </param>
        /// <param name="treeish">
        ///  root object that can be peeled to a tree
        /// </param>
        /// <param name="path">
        ///  relative path from the root object to the desired object
        /// </param>
        /// <param name="type">type of object desired</param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_object_lookup_bypath(
            out git_object* _out,
            git_object* treeish,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            git_object_t type);

        /// <summary>
        ///  Get the id (SHA1) of a repository object
        /// </summary>
        /// <param name="obj">the repository object</param>
        /// <returns>the SHA1 id</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_object_id(
            git_object* obj);

        /// <summary>
        ///  Get a short abbreviated OID string for the object
        ///
        ///  <para>
        ///   This starts at the "core.abbrev" length (default 7
        ///   characters) and iteratively extends to a longer string
        ///   if that length is ambiguous. The result will be
        ///   unambiguous (at least until new objects are added to the
        ///   repository).
        ///  </para>
        /// </summary>
        /// <param name="_out">Buffer to write string into</param>
        /// <param name="obj">The object to get an ID for</param>
        /// <returns>0 on success, < 0 for error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_object_short_id(
            ref git_buf _out,
            git_object* obj);

        /// <summary>
        ///  Get the object type of an object
        /// </summary>
        /// <param name="obj">the repository object</param>
        /// <returns>the object's type</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_object_t git_object_type(
            git_object* obj);

        /// <summary>
        ///  Get the repository that owns this object
        ///
        ///  <para>
        ///   Freeing or calling `git_repository_close` on the
        ///   returned pointer will invalidate the actual object.
        ///  </para>
        ///
        ///  <para>
        ///   Any other operation may be run on the repository without
        ///   affecting the object.
        ///  </para>
        /// </summary>
        /// <param name="obj">the object</param>
        /// <returns>the repository who owns this object</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_repository* git_object_owner(
            git_object* obj);

        /// <summary>
        ///  Close an open object
        ///
        ///  <para>
        ///   This method instructs the library to close an existing
        ///   object; note that git_objects are owned and cached by
        ///   the repository so the object may or may not be freed
        ///   after this library call, depending on how aggressive is
        ///   the caching mechanism used by the repository.
        ///  </para>
        ///
        ///  <para>
        ///   IMPORTANT: It *is* necessary to call this method when
        ///   you stop using an object. Failure to do so will cause a
        ///   memory leak.
        ///  </para>
        /// </summary>
        /// <param name="_object">the object to close</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_object_free(
            git_object* _object);

        /// <summary>
        ///  Convert an object type to its string representation.
        ///
        ///  <para>
        ///   The result is a pointer to a string in static memory and
        ///   should not be free()'ed.
        ///  </para>
        /// </summary>
        /// <param name="type">object type to convert.</param>
        /// <returns>the corresponding string representation.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_object_type2string(
            git_object_t type);

        /// <summary>
        ///  Convert a string object type representation to it's
        ///  git_object_t.
        /// </summary>
        /// <param name="str">the string to convert.</param>
        /// <returns>the corresponding git_object_t.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_object_t git_object_string2type(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string str);

        /// <summary>
        ///  Determine if the given git_object_t is a valid object type.
        /// </summary>
        /// <param name="type">object type to test.</param>
        /// <returns>
        ///  1 if the type represents a valid loose object type, 0
        ///  otherwise
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_object_type_is_valid(
            git_object_t type);

        /// <summary>
        ///  Recursively peel an object until an object of the specified
        ///  type is met.
        ///
        ///  <para>
        ///   If the query cannot be satisfied due to the object
        ///   model, GIT_EINVALIDSPEC will be returned (e.g. trying to
        ///   peel a blob to a tree).
        ///  </para>
        ///
        ///  <para>
        ///   If you pass `GIT_OBJECT_ANY` as the target type, then
        ///   the object will be peeled until the type changes. A tag
        ///   will be peeled until the referenced object is no longer
        ///   a tag, and a commit will be peeled to a tree. Any other
        ///   object type will return GIT_EINVALIDSPEC.
        ///  </para>
        ///
        ///  <para>
        ///   If peeling a tag we discover an object which cannot be
        ///   peeled to the target type due to the object model,
        ///   GIT_EPEEL will be returned.
        ///  </para>
        ///
        ///  <para>
        ///   You must free the returned object.
        ///  </para>
        /// </summary>
        /// <param name="peeled">Pointer to the peeled git_object</param>
        /// <param name="_object">The object to be processed</param>
        /// <param name="target_type">
        ///  The type of the requested object (a GIT_OBJECT_ value)
        /// </param>
        /// <returns>
        ///  0 on success, GIT_EINVALIDSPEC, GIT_EPEEL, or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_object_peel(
            git_object** peeled,
            git_object* _object,
            git_object_t target_type);

        /// <summary>
        ///  Create an in-memory copy of a Git object. The copy must be
        ///  explicitly free'd or it will leak.
        /// </summary>
        /// <param name="dest">
        ///  Pointer to store the copy of the object
        /// </param>
        /// <param name="source">Original object to copy</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_object_dup(
            out git_object* dest,
            git_object* source);

        /// <summary>
        ///  Initialize `git_object_id_options` structure with default
        ///  values. Equivalent to creating an instance with
        ///  `GIT_WORKTREE_ADD_OPTIONS_INIT`.
        /// </summary>
        /// <param name="opts">
        ///  The `git_object_id_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass `GIT_OBJECT_ID_OPTIONS_INIT`.
        /// </param>
        /// <returns>0 on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_object_id_options_init(
            ref git_object_id_options opts,
            uint version);

        /// <summary>
        ///  Given the raw content of an object, determine the object ID.
        ///  This prepends the object header to the given data, and
        ///  hashes the results with the hash corresponding to the given
        ///  oid_type.
        /// </summary>
        /// <param name="oid_out">the resulting object id</param>
        /// <param name="buf">the raw object content</param>
        /// <param name="len">the length of the given buffer</param>
        /// <param name="opts">the options for id calculation</param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_object_id_from_buffer(
            out git_oid oid_out,
            IntPtr buf,
            UIntPtr len,
            ref git_object_id_options opts);

        /// <summary>
        ///  Given an on-disk file that contains the raw content of an
        ///  object, determine the object ID. This prepends the object
        ///  header to the given data, and hashes the results with the
        ///  hash corresponding to the given oid_type.
        ///
        ///  <para>
        ///   Note that this does not look at attributes or do any
        ///   on-disk filtering (like line ending translation), so
        ///   when used with blobs, it may not match the results for
        ///   adding to the repository. To compute the object ID for a
        ///   blob with filters, use `git_repository_hashfile`.
        ///  </para>
        /// </summary>
        /// <param name="oid_out">the resulting object id</param>
        /// <param name="path">
        ///  the on-disk path to the raw object content
        /// </param>
        /// <param name="opts">the options for id calculation</param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_object_id_from_file(
            out git_oid oid_out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            ref git_object_id_options opts);

        /// <summary>
        ///  Analyzes a buffer of raw object content and determines its
        ///  validity. Tree, commit, and tag objects will be parsed and
        ///  ensured that they are valid, parseable content. (Blobs are
        ///  always valid by definition.) An error message will be set
        ///  with an informative message if the object is not valid.
        /// </summary>
        /// <param name="valid">
        ///  Output pointer to set with validity of the object content
        /// </param>
        /// <param name="buf">The contents to validate</param>
        /// <param name="len">The length of the buffer</param>
        /// <param name="object_type">
        ///  The type of the object in the buffer
        /// </param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_object_rawcontent_is_valid(
            out int valid,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string buf,
            UIntPtr len,
            git_object_t object_type);
    }
}
