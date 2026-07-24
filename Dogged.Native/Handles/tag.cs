using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public sealed class TagHandle : SafeHandle
    {
        public TagHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe TagHandle(git_tag* nativeTag) :
            base((IntPtr)nativeTag, true)
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
            libgit2.git_tag_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Lookup a tag object from the repository.
        /// </summary>
        /// <param name="_out">pointer to the looked up tag</param>
        /// <param name="repo">
        ///  the repo to use when locating the tag.
        /// </param>
        /// <param name="id">identity of the tag to locate.</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_tag_lookup(
            out TagHandle _out,
            RepositoryHandle repo,
            ref git_oid id);

        /// <summary>
        ///  Lookup a tag object from the repository, given a prefix of
        ///  its identifier (short id).
        /// </summary>
        /// <param name="_out">pointer to the looked up tag</param>
        /// <param name="repo">
        ///  the repo to use when locating the tag.
        /// </param>
        /// <param name="id">identity of the tag to locate.</param>
        /// <param name="len">the length of the short identifier</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_tag_lookup_prefix(
            out TagHandle _out,
            RepositoryHandle repo,
            ref git_oid id,
            UIntPtr len);

        /// <summary>
        ///  Close an open tag
        ///
        ///  <para>
        ///   You can no longer use the git_tag pointer after this
        ///   call.
        ///  </para>
        ///
        ///  <para>
        ///   IMPORTANT: You MUST call this method when you are
        ///   through with a tag to release memory. Failure to do so
        ///   will cause a memory leak.
        ///  </para>
        /// </summary>
        /// <param name="tag">the tag to close</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_tag_free(
            IntPtr tag);

        /// <summary>
        ///  Get the id of a tag.
        /// </summary>
        /// <param name="tag">a previously loaded tag.</param>
        /// <returns>object identity for the tag.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_tag_id(
            TagHandle tag);

        /// <summary>
        ///  Get the repository that contains the tag.
        /// </summary>
        /// <param name="tag">A previously loaded tag.</param>
        /// <returns>Repository that contains this tag.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_repository* git_tag_owner(
            TagHandle tag);

        /// <summary>
        ///  Get the tagged object of a tag
        ///
        ///  <para>
        ///   This method performs a repository lookup for the given
        ///   object and returns it
        ///  </para>
        /// </summary>
        /// <param name="target_out">
        ///  pointer where to store the target
        /// </param>
        /// <param name="tag">a previously loaded tag.</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_tag_target(
            ObjectHandle target_out,
            TagHandle tag);

        /// <summary>
        ///  Get the OID of the tagged object of a tag
        /// </summary>
        /// <param name="tag">a previously loaded tag.</param>
        /// <returns>pointer to the OID</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_tag_target_id(
            TagHandle tag);

        /// <summary>
        ///  Get the type of a tag's tagged object
        /// </summary>
        /// <param name="tag">a previously loaded tag.</param>
        /// <returns>type of the tagged object</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_object_t git_tag_target_type(
            TagHandle tag);

        /// <summary>
        ///  Get the name of a tag
        /// </summary>
        /// <param name="tag">a previously loaded tag.</param>
        /// <returns>name of the tag</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_tag_name(
            TagHandle tag);

        /// <summary>
        ///  Get the tagger (author) of a tag
        /// </summary>
        /// <param name="tag">a previously loaded tag.</param>
        /// <returns>
        ///  reference to the tag's author or NULL when unspecified
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_signature* git_tag_tagger(
            TagHandle tag);

        /// <summary>
        ///  Get the message of a tag
        /// </summary>
        /// <param name="tag">a previously loaded tag.</param>
        /// <returns>message of the tag or NULL when unspecified</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_tag_message(
            TagHandle tag);

        /// <summary>
        ///  Create a new tag in the repository from an object
        ///
        ///  <para>
        ///   A new reference will also be created pointing to this
        ///   tag object. If `force` is true and a reference already
        ///   exists with the given name, it'll be replaced.
        ///  </para>
        ///
        ///  <para>
        ///   The message will not be cleaned up. This can be achieved
        ///   through `git_message_prettify()`.
        ///  </para>
        ///
        ///  <para>
        ///   The tag name will be checked for validity. You must
        ///   avoid the characters '~', '^', ':', ' \ ', '?', '[', and
        ///   '*', and the sequences ".." and "@{" which have special
        ///   meaning to revparse.
        ///  </para>
        /// </summary>
        /// <param name="oid">
        ///  Pointer where to store the OID of the newly created tag. If
        ///  the tag already exists, this parameter will be the oid of
        ///  the existing tag, and the function will return a GIT_EEXISTS
        ///  error code.
        /// </param>
        /// <param name="repo">Repository where to store the tag</param>
        /// <param name="tag_name">
        ///  Name for the tag; this name is validated for consistency. It
        ///  should also not conflict with an already existing tag name
        /// </param>
        /// <param name="target">
        ///  Object to which this tag points. This object must belong to
        ///  the given `repo`.
        /// </param>
        /// <param name="tagger">
        ///  Signature of the tagger for this tag, and of the tagging
        ///  time
        /// </param>
        /// <param name="message">Full message for this tag</param>
        /// <param name="force">Overwrite existing references</param>
        /// <returns>
        ///  0 on success, GIT_EINVALIDSPEC or an error code A tag object
        ///  is written to the ODB, and a proper reference is written in
        ///  the /refs/tags folder, pointing to it
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_tag_create(
            ref git_oid oid,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string tag_name,
            ObjectHandle target,
            ref git_signature tagger,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string message,
            int force);

        /// <summary>
        ///  Create a new tag in the object database pointing to a
        ///  git_object
        ///
        ///  <para>
        ///   The message will not be cleaned up. This can be achieved
        ///   through `git_message_prettify()`.
        ///  </para>
        /// </summary>
        /// <param name="oid">
        ///  Pointer where to store the OID of the newly created tag
        /// </param>
        /// <param name="repo">Repository where to store the tag</param>
        /// <param name="tag_name">Name for the tag</param>
        /// <param name="target">
        ///  Object to which this tag points. This object must belong to
        ///  the given `repo`.
        /// </param>
        /// <param name="tagger">
        ///  Signature of the tagger for this tag, and of the tagging
        ///  time
        /// </param>
        /// <param name="message">Full message for this tag</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_tag_annotation_create(
            ref git_oid oid,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string tag_name,
            ObjectHandle target,
            ref git_signature tagger,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string message);

        /// <summary>
        ///  Create a new tag in the repository from a buffer
        /// </summary>
        /// <param name="oid">
        ///  Pointer where to store the OID of the newly created tag
        /// </param>
        /// <param name="repo">Repository where to store the tag</param>
        /// <param name="buffer">Raw tag data</param>
        /// <param name="force">Overwrite existing tags</param>
        /// <returns>0 on success; error code otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_tag_create_from_buffer(
            ref git_oid oid,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string buffer,
            int force);

        /// <summary>
        ///  Create a new lightweight tag pointing at a target object
        ///
        ///  <para>
        ///   A new direct reference will be created pointing to this
        ///   target object. If `force` is true and a reference
        ///   already exists with the given name, it'll be replaced.
        ///  </para>
        ///
        ///  <para>
        ///   The tag name will be checked for validity. See
        ///   `git_tag_create()` for rules about valid names.
        ///  </para>
        /// </summary>
        /// <param name="oid">
        ///  Pointer where to store the OID of the provided target
        ///  object. If the tag already exists, this parameter will be
        ///  filled with the oid of the existing pointed object and the
        ///  function will return a GIT_EEXISTS error code.
        /// </param>
        /// <param name="repo">
        ///  Repository where to store the lightweight tag
        /// </param>
        /// <param name="tag_name">
        ///  Name for the tag; this name is validated for consistency. It
        ///  should also not conflict with an already existing tag name
        /// </param>
        /// <param name="target">
        ///  Object to which this tag points. This object must belong to
        ///  the given `repo`.
        /// </param>
        /// <param name="force">Overwrite existing references</param>
        /// <returns>
        ///  0 on success, GIT_EINVALIDSPEC or an error code A proper
        ///  reference is written in the /refs/tags folder, pointing to
        ///  the provided target object
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_tag_create_lightweight(
            ref git_oid oid,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string tag_name,
            ObjectHandle target,
            int force);

        /// <summary>
        ///  Delete an existing tag reference.
        ///
        ///  <para>
        ///   The tag name will be checked for validity. See
        ///   `git_tag_create()` for rules about valid names.
        ///  </para>
        /// </summary>
        /// <param name="repo">Repository where lives the tag</param>
        /// <param name="tag_name">
        ///  Name of the tag to be deleted; this name is validated for
        ///  consistency.
        /// </param>
        /// <returns>
        ///  0 on success, GIT_EINVALIDSPEC or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_tag_delete(
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string tag_name);

        /// <summary>
        ///  Fill a list with all the tags in the Repository
        ///
        ///  <para>
        ///   The string array will be filled with the names of the
        ///   matching tags; these values are owned by the user and
        ///   should be free'd manually when no longer needed, using
        ///   `git_strarray_free`.
        ///  </para>
        /// </summary>
        /// <param name="tag_names">
        ///  Pointer to a git_strarray structure where the tag names will
        ///  be stored
        /// </param>
        /// <param name="repo">Repository where to find the tags</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_tag_list(
            ref git_strarray tag_names,
            RepositoryHandle repo);

        /// <summary>
        ///  Fill a list with all the tags in the Repository which name
        ///  match a defined pattern
        ///
        ///  <para>
        ///   If an empty pattern is provided, all the tags will be
        ///   returned.
        ///  </para>
        ///
        ///  <para>
        ///   The string array will be filled with the names of the
        ///   matching tags; these values are owned by the user and
        ///   should be free'd manually when no longer needed, using
        ///   `git_strarray_free`.
        ///  </para>
        /// </summary>
        /// <param name="tag_names">
        ///  Pointer to a git_strarray structure where the tag names will
        ///  be stored
        /// </param>
        /// <param name="pattern">Standard fnmatch pattern</param>
        /// <param name="repo">Repository where to find the tags</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_tag_list_match(
            ref git_strarray tag_names,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string pattern,
            RepositoryHandle repo);

        /// <summary>
        ///  Call callback `cb' for each tag in the repository
        /// </summary>
        /// <param name="repo">Repository</param>
        /// <param name="callback">Callback function</param>
        /// <param name="payload">
        ///  Pointer to callback data (optional)
        /// </param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_tag_foreach(
            RepositoryHandle repo,
            git_tag_foreach_cb callback,
            IntPtr payload);

        /// <summary>
        ///  Recursively peel a tag until a non tag git_object is found
        ///
        ///  <para>
        ///   The retrieved `tag_target` object is owned by the
        ///   repository and should be closed with the
        ///   `git_object_free` method.
        ///  </para>
        /// </summary>
        /// <param name="tag_target_out">
        ///  Pointer to the peeled git_object
        /// </param>
        /// <param name="tag">The tag to be processed</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_tag_peel(
            ObjectHandle tag_target_out,
            TagHandle tag);

        /// <summary>
        ///  Create an in-memory copy of a tag. The copy must be
        ///  explicitly free'd or it will leak.
        /// </summary>
        /// <param name="_out">Pointer to store the copy of the tag</param>
        /// <param name="source">Original tag to copy</param>
        /// <returns>0</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_tag_dup(
            out TagHandle _out,
            TagHandle source);
    }
}
