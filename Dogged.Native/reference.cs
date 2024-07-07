using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    /// A reference is a pointer to a commit or another reference;
    /// reference types are branches and tags.
    /// </summary>
    public struct git_reference { };

    /// <summary>
    /// Basic types for a Git reference.
    /// </summary>
    public enum git_reference_t {
        /// <summary>
        /// An invalid reference.
        /// </summary>
        GIT_REFERENCE_INVALID = 0,

        /// <summary>
        /// A direct reference that points directly to an object id.
        /// </summary>
        GIT_REFERENCE_DIRECT = 1,

        /// <summary>
        /// A symbolic reference that points to another reference;
        /// for example, `HEAD`.
        /// </summary>
        GIT_REFERENCE_SYMBOLIC = 2,

        /// <summary>
        /// A value that includes all reference types.
        /// </summary>
        GIT_REFERENCE_ALL = GIT_REFERENCE_DIRECT | GIT_REFERENCE_SYMBOLIC
    };

    /// <summary>
    /// An object to contain the iteration of a reference collection.
    /// </summary>
    public struct git_reference_iterator { };

    public static partial class libgit2
    {
        /// <summary>
        /// Create an iterator for the repository's references.
        /// </summary>
        /// <param name="iterator">Pointer to the iterator that was created</param>
        /// <param name="index">The index to iterate over</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_iterator_new(out git_reference_iterator* iterator, git_repository* repository);

        /// <summary>
        /// Free the given reference iterator.
        /// </summary>
        /// <param name="iterator">The iterator to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_reference_iterator_free(git_reference_iterator* iterator);

        /// <summary>
        /// Free the given reference.
        /// </summary>
        /// <param name="reference">The reference to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_reference_free(git_reference* reference);

        /// <summary>
        /// Lookup a reference by name in a repository.  The returned
        /// reference must be freed by the user.
        /// </summary>
        /// <param name="reference">Pointer to the reference that was loaded from the repository</param>
        /// <param name="repo">The repository that contains the reference</param>
        /// <param name="name">The name of the reference to lookup</param>
        /// <returns>0 on success, GIT_ENOTFOUND if no reference exists by that name, GIT_EINVALIDSPEC if the given name is invalid or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_lookup(
            out git_reference* reference,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        /// Get the full name of a reference.
        /// </summary>
        /// <param name="reference">The reference to get the name of</param>
        /// <returns>The full name of the reference</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_reference_name(git_reference* reference);

        /// <summary>
        /// Get the next reference from the iterator.
        /// </summary>
        /// <param name="reference">Pointer to the reference that is next in iteration</param>
        /// <param name="iterator">The iterator for the repository's references</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_next(out git_reference* reference, git_reference_iterator* iterator);

        /// <summary>
        /// Recursively peel reference until object of the specified type
        /// is found.
        /// </summary>
        /// <param name="obj">Pointer to the object that was peeled</param>
        /// <param name="reference">The reference to peel</param>
        /// <param name="type">The type of object to peel to</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_peel(out git_object* obj, git_reference* reference, git_object_t type);

        /// <summary>
        /// Get the full name of the target of this reference.  This
        /// function is only supported for symbolic references.
        /// </summary>
        /// <param name="reference">The reference to get the target of</param>
        /// <returns>The target of the reference or NULL if the reference is not symbolic</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_reference_symbolic_target(git_reference* reference);

        /// <summary>
        /// Get the oid target of this reference.  This function is only
        /// supported for direct references.
        /// </summary>
        /// <param name="reference">The reference to get the target of</param>
        /// <returns>The target of the reference or NULL if the reference is not direct</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_reference_target(git_reference* reference);

        /// <summary>
        /// Get the type of this reference.
        /// </summary>
        /// <param name="reference">The reference to get the type of</param>
        /// <returns>The type of the reference</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_reference_t git_reference_type(git_reference* reference);
    }
}
