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
        /// Determine if the blob content is binary or not.

        /// <para>
        /// The heuristic used to guess if a file is binary is taken from core git:
        /// Searching for NUL bytes and looking for a reasonable ratio of printable
        /// to non-printable characters among the first 8000 bytes.
        /// </para>
        /// </summary>
        /// <param name="blob">The blob to examine</param>
        /// <returns>1 if the content of the blob is detected as binary; 0 otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_blob_is_binary(git_blob* blob);

        /// <summary>
        /// Look up a blob from the repository.
        /// </summary>
        /// <param name="obj">Pointer to the blob that was loaded from the repository</param>
        /// <param name="repo">The repository that contains the blob</param>
        /// <param name="id">The id of the blob to lookup</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_blob_lookup(out git_blob* blob, git_repository* repo, ref git_oid id);

        /// <summary>
        /// Get a read-only buffer with the raw content of a blob.
        ///
        /// <para>
        /// A pointer to the raw content of a blob is returned;
        /// this pointer is owned internally by the object and shall
        /// not be free'd. The pointer may be invalidated at a later
        /// time.
        /// <para>
        /// </summary>
        /// <param name="blob">The blob to examine</param>
        /// <returns>The buffer.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe byte* git_blob_rawcontent(git_blob* blob);

        /// <summary>
        /// Get the size in bytes of the contents of a blob.
        /// </summary>
        /// <param name="blob">The blob to examine</param>
        /// <returns>Raw size of the blob in bytes</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe long git_blob_rawsize(git_blob* blob);

        /// <summary>
        /// Clone a remote repository.
        ///
        /// <para>
        /// By default this creates its repository and initial remote to match
        /// git's defaults. You can use the options in the callback to
        /// customize how these are created.
        /// </para>
        /// </summary>
        /// <param name="repo">Pointer to the repository that will be cloned.</param>
        /// <param name="remotePath">The remote repository to clone</param>
        /// <param name="localPath">The local path to clone to</param>
        /// <param name="options">Configuration options for the clone.  If null, defaults will be used.</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_clone(
            out git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string remotePath,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string localPath,
            git_clone_options* options);

        /// <summary>
        /// Get the signature for the commit's author.  This data is owned
        /// by the commit and should not be freed.
        /// </summary>
        /// <param name="commit">The commit to examine</param>
        /// <returns>A pointer to a signature for the author</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_signature* git_commit_author(git_commit* commit);

        /// <summary>
        /// Get the signature for the commit's committer.  This data is owned
        /// by the commit and should not be freed.
        /// </summary>
        /// <param name="commit">The commit to examine</param>
        /// <returns>A pointer to a signature for the committer</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_signature* git_commit_committer(git_commit* commit);

        /// <summary>
        /// Look up a commit from the repository.
        /// </summary>
        /// <param name="obj">Pointer to the commit that was loaded from the repository</param>
        /// <param name="repo">The repository that contains the commit</param>
        /// <param name="id">The id of the commit to lookup</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_lookup(out git_commit* obj, git_repository* repo, ref git_oid id);

        /// <summary>
        /// Get the tree that the given commit points to.  This tree must be
        /// freed when it is no longer needed.
        /// </summary>
        /// <param name="tree">A pointer to the tree</param>
        /// <param name="commit">The commit to examine</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_tree(out git_tree* tree, git_commit* commit);

        /// <summary>
        /// Get the object id of the tree that the given commit points to.
        /// This differs from <see cref="git_commit_tree"/> in that no
        /// attempts are made to lookup the tree or validate it.
        /// </summary>
        /// <param name="commit">The commit to examine</param>
        /// <returns>A pointer to the tree's object id</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_commit_tree_id(git_commit* commit);

        /// <summary>
        /// Returns the information (class and message) for the last error
        /// that occurred on the current thread.  This information is
        /// undefined if the last libgit2 function did not return an error.
        /// </summary>
        /// <returns>A pointer to a <see cref="git_error"/> that describes the error.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_error* git_error_last();

        /// <summary>
        /// Get a pointer to an entry in the index for the given path at the
        /// given stage level.
        ///
        /// <para>
        /// A "stage level" is a construct for handling conflicted files
        /// during a merge; generally, files are in stage level 0
        /// (sometimes called the "main index"); if a file is in conflict
        /// after a merge, there will be no entry at stage level 0, instead
        /// there will be entries at stages 1-3 representing the conflicting
        /// contents of the common ancestor, the file in "our" branch and
        /// the file in "their" branch.
        /// </para>
        ///
        /// <para>
        /// The entry is not modifiable and should not be freed.  Because the
        /// `git_index_entry` struct is a publicly defined struct, you should
        /// be able to make your own permanent copy of the data if necessary.
        /// </para>
        /// </summary>
        /// <param name="index">The index to read the entry from</param>
        /// <param name="path">The path for the index entry</param>
        /// <param name="stage">The stage level to query</param>
        /// <returns>A pointer to the entry or NULL if out of bounds</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_index_entry* git_index_get_bypath(
            git_index* index,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            int stage);

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
        /// Create a new index iterator; this will take a snapshot of the
        /// given index for iteration.
        /// </summary>
        /// <param name="iterator">Pointer to the iterator that was created</param>
        /// <param name="index">The index to iterate over</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_iterator_new(out git_index_iterator* iterator, git_index* index);

        /// <summary>
        /// Get the next index entry in the iterator.  This entry is owned
        /// by the iterator and should not be freed.
        /// </summary>
        /// <param name="entry">Pointer to the entry in the index</param>
        /// <param name="iterator">The iterator to query</param>
        /// <returns>0 on success, GIT_ITEROVER on successful completion of iteration, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_iterator_next(out git_index_entry* entry, git_index_iterator* iterator);

        /// <summary>
        /// Free an existing index iterator.
        /// </summary>
        /// <param name="iterator">The iterator to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_index_iterator_free(git_index_iterator* iterator);

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
        /// Free a git object.
        /// </summary>
        /// <param name="obj">The object to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_object_free(git_object* obj);

        /// <summary>
        /// Gets the object id of the given object.
        /// </summary>
        /// <param name="obj">The object to examine</param>
        /// <returns>A pointer to the object's id</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_object_id(git_object* obj);

        /// <summary>
        /// Look up an object from the repository.
        /// </summary>
        /// <param name="obj">Pointer to the object that was loaded from the repository</param>
        /// <param name="repo">The repository that contains the object</param>
        /// <param name="id">The id of the object to lookup</param>
        /// <param name="type">The type of the object to lookup</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_object_lookup(out git_object* obj, git_repository* repo, ref git_oid id, git_object_t type);

        /// <summary>
        /// Get the type of an object.
        /// </summary>
        /// <param name="obj">The object to query</param>
        /// <returns>The type of the object</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_object_t git_object_type(git_object* obj);

        /// <summary>
        /// Close an open database object.
        /// </summary>
        /// <param name="odb">The database to close.  If null, no action is taken.</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_odb_free(git_odb* odb);

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
        /// Creates a new Git repository in the given folder.
        /// </summary>
        /// <param name="repo">Pointer to the repository that will be created.</param>
        /// <param name="path">The path to the repository</param>
        /// <param name="bare">If non-zero, a Git repository without a working directory is created at the given path. If zero, the provided path will be considered as the working directory into which the .git directory will be created.</param>
        /// <returns>0 on success or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_init(
            out git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            uint bare);

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
        /// Get the object database ("ODB") for this repository.
        ///
        /// <para>
        /// If a custom ODB has not been set, the default database for the
        /// repository will be returned (the one located in `.git/objects`).
        /// </para>
        ///
        /// <para>
        /// The ODB must be freed once it's no longer being used by the user.
        /// </para>
        /// </summary>
        /// <param name="odb">Pointer to store the loaded ODB</param>
        /// <param name="repo">A repository object</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_repository_odb(out git_odb* odb, git_repository* repo);

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

        /// <summary>
        /// Get a pointer to an entry in the tree at the the given index.
        ///
        /// <para>
        /// The entry is not modifiable and should not be freed; it will be
        /// freed when the owning tree is freed.
        /// </para>
        /// </summary>
        /// <param name="tree">The tree to query</param>
        /// <param name="n">The index in the tree to return</param>
        /// <returns>A tree entry for the given index</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_tree_entry* git_tree_entry_byindex(git_tree* tree, UIntPtr n);

        /// <summary>
        /// Get a pointer to an entry in the tree for the the given filename.
        ///
        /// <para>
        /// The entry is not modifiable and should not be freed; it will be
        /// freed when the owning tree is freed.
        /// </para>
        /// </summary>
        /// <param name="tree">The tree to query</param>
        /// <param name="filename">The filename in the tree to return</param>
        /// <returns>A tree entry for the given index</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_tree_entry* git_tree_entry_byname(
            git_tree* tree,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string filename);

        /// <summary>
        /// Get the file mode of the given tree entry.
        /// </summary>
        /// <param name="entry">The tree entry to query</param>
        /// <returns>The file mode of the tree entry</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe uint git_tree_entry_filemode(git_tree_entry* entry);

        /// <summary>
        /// Get the object id of the given tree entry.
        /// </summary>
        /// <param name="entry">The tree entry to query</param>
        /// <returns>The object id of the tree entry</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_tree_entry_id(git_tree_entry* entry);

        /// <summary>
        /// Get the filename of the given tree entry.
        /// </summary>
        /// <param name="entry">The tree entry to query</param>
        /// <returns>The filename of the tree entry</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_tree_entry_name(git_tree_entry* entry);

        /// <summary>
        /// Get the number of entries in the tree.
        /// </summary>
        /// <param name="tree">The tree to query</param>
        /// <returns>The number of entries in the tree</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_tree_entrycount(git_tree* tree);

        /// <summary>
        /// Look up a tree from the repository.
        /// </summary>
        /// <param name="obj">Pointer to the tree that was loaded from the repository</param>
        /// <param name="repo">The repository that contains the tree</param>
        /// <param name="id">The id of the tree to lookup</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_tree_lookup(out git_tree* obj, git_repository* repo, ref git_oid id);
    }
}
