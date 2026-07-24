using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public class ObjectHandle : SafeHandle
    {
        public ObjectHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe ObjectHandle(git_object* nativeObject) :
            base((IntPtr)nativeObject, true)
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
            libgit2.git_object_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
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
            out ObjectHandle _object,
            RepositoryHandle repo,
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
            out ObjectHandle object_out,
            RepositoryHandle repo,
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
            out ObjectHandle _out,
            ObjectHandle treeish,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            git_object_t type);

        /// <summary>
        ///  Get the id (SHA1) of a repository object
        /// </summary>
        /// <param name="obj">the repository object</param>
        /// <returns>the SHA1 id</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_object_id(
            ObjectHandle obj);

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
            ObjectHandle obj);

        /// <summary>
        ///  Get the object type of an object
        /// </summary>
        /// <param name="obj">the repository object</param>
        /// <returns>the object's type</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_object_t git_object_type(
            ObjectHandle obj);

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
            ObjectHandle obj);

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
            IntPtr _object);

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
            ObjectHandle peeled,
            ObjectHandle _object,
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
            out ObjectHandle dest,
            ObjectHandle source);
    }
}
