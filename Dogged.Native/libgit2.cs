using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    /// The native libgit2 functions.  Upon the first invocation of any
    /// of these functions, the native library will be loaded and the
    /// <see cref="git_libgit2_init"/> function will be called to set up
    /// the native library.
    /// </summary>
    public static class libgit2
    {
        private const string libgit2_dll = NativeLibrary.Filename;

        private static NativeInitializer nativeInitializer;

        /// <summary>
        /// Load and initialize the native library.
        /// </summary>
        static libgit2()
        {
            nativeInitializer = new NativeInitializer();
        }

        /// <summary>
        /// Returns the information (class and message) for the last error
        /// that occurred on the current thread.  This information is
        /// undefined if the last libgit2 function did not return an error.
        /// </summary>
        /// <returns>A pointer to a <see cref="git_error"/> that describes the error.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_error* git_error_last();

        /// <summary>
        /// Get a pointer to an entry in the index at the the given position.
        ///
        /// <para>
        /// The entry is not modifiable and should not be freed.  Because the
        /// `git_index_entry` struct is a publicly defined struct, you should
        /// be able to make your own permanent copy of the data if necessary.
        /// </para>
        /// </summary>
        /// <param name="index">The index to read the entry from</param>
        /// <param name="n">The position of the index entry</param>
        /// <returns>A pointer to the entry or NULL if out of bounds</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_index_entry* git_index_get_byindex(git_index* index, UIntPtr n);

        /// <summary>
        /// Get the count of entries currently in the index.
        /// </summary>
        /// <param name="index">The index to read the entry from</param>
        /// <returns>Count of the current entries</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_index_entrycount(git_index* index);

        /// <summary>
        /// Free an existing index object.
        /// </summary>
        /// <param name="index">The index to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_index_free(git_index* index);

        /// <summary>
        /// Query compile time options for libgit2.  This will show the
        /// functionality that is built in to the library.
        /// </summary>
        /// <returns>A combination of <see cref="git_feature_t"/> values.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern git_feature_t git_libgit2_features();

        /// <summary>
        /// Initialize the native library's global state.
        ///
        /// <para>
        /// This function must be called before any other libgit2
        /// function in order to set up global state and threading.
        /// </para>
        ///
        /// <para>
        /// This function may be called multiple times - it will return
        /// the number of times the initialization has been called (including
        /// this one) that have not subsequently been shutdown.
        /// </para>
        /// </summary>
        /// <returns>The number of initializations of the library, or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_libgit2_init();

        /// <summary>
        /// Shutdown the global state.
        ///
        /// <para>
        /// Clean up the global state and threading context after calling
        /// it as many times as <see cref="git_libgit2_init"/> was called -
        /// it will return the number of remainining initializations that
        /// have not been shutdown (after this one).
        /// </para>
        /// </summary>
        /// <returns>The number of remaining initializations of the library, or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_libgit2_shutdown();

        /// <summary>
        /// Retrieve and resolve the reference pointed to by HEAD.
        ///
        /// <para>
        /// The returned <see cref="git_reference"/> will be owned by the
        /// caller and <see cref="git_reference_free"/> must be called when
        /// done to release the allocated memory.
        /// </para>
        /// </summary>
        /// <param name="reference">Pointer to the reference that will be retrieved</param>
        /// <param name="repo">The repository object</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_head(
            out git_reference* reference, git_repository* repo);

        /// <summary>
        /// Indicates if the repository's HEAD is detached.  A repository's
        /// HEAD is detached when it points directly to a commit instead
        /// of a branch.
        /// </summary>
        /// <param name="repo">The repository object</param>
        /// <returns>1 if the HEAD is detached, 0 if it's not, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_head_detached(
            git_repository* repo);

        /// <summary>
        /// Checks if a repository is bare.
        /// </summary>
        /// <param name="repo">Repository to test</param>
        /// <returns>1 if the repository is bare, 0 otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_is_bare(
            git_repository* repo);

        /// <summary>
        /// Get the index file for this repository.
        ///
        /// <para>
        /// If a custom index has not been set, the default index for the
        /// repository will be returned (the one located in `.git/index`).
        /// </para>
        ///
        /// <para>
        /// The index must be freed once it's no longer being used by the
        /// user.
        /// </para>
        /// </summary>
        /// <param name="index">Pointer to the index that will be opened</param>
        /// <param name="repo">The repository object</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_index(out git_index* index, git_repository* repo);

        /// <summary>
        /// Open a git repository.
        ///
        /// <para>
        /// The <paramref name="path"/> argument must point to either a Git
        /// repository folder, or an existing working tree.
        /// </para>
        ///
        /// <para>
        /// The method will automatically detect if <paramref name="path"/>
        /// is a working tree or a bare repository.  It will fail if the
        /// given path is neither.
        /// </para>
        /// </summary>
        /// <param name="repo">Pointer to the repository that will be opened.</param>
        /// <param name="path">The path to the repository</param>
        /// <returns>0 on success or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_open(
            out git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path);

        /// <summary>
        /// Close a previously opened repository and free any native memory
        /// that was allocated.
        ///
        /// <para>
        /// Note that after a repository is free'd, all the objects it
        /// has spawned will still exist until they are manually closed
        /// by the user with `git_object_free`, but accessing any of
        /// the attributes of an object without a backing repository
        /// will result in undefined behavior.
        /// </para>
        /// </summary>
        /// <param name="repo">The repository handle to close.</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_repository_free(git_repository* repo);

        /// <summary>
        /// Free the given reference.
        /// </summary>
        /// <param name="reference">The reference to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_reference_free(git_reference* reference);

        /// <summary>
        /// Get the full name of a reference.
        /// </summary>
        /// <param name="reference">The reference to get the name of</param>
        /// <returns>The full name of the reference</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_reference_name(git_reference* reference);

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
