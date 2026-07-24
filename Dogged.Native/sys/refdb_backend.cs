using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Every backend's iterator must have a pointer to itself as the
    ///  first element, so the API can talk to it. You'd define your
    ///  iterator as
    ///
    ///  <para>
    ///   struct my_iterator { git_reference_iterator parent; ... }
    ///  </para>
    ///
    ///  <para>
    ///   and assign `iter->parent.backend` to your
    ///   `git_refdb_backend`.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_reference_iterator
    {
        public IntPtr db;

        /// <summary>
        ///  Return the current reference and advance the iterator.
        /// </summary>
        public IntPtr next;

        /// <summary>
        ///  Return the name of the current reference and advance the
        ///  iterator
        /// </summary>
        public IntPtr next_name;

        /// <summary>
        ///  Free the iterator
        /// </summary>
        public IntPtr free;
    }

    public enum git_refdb_backend_init_flag_t
    {
        /// <summary>
        ///  The refdb that is to be initialized is for a worktree.
        /// </summary>
        GIT_REFDB_BACKEND_INIT_IS_WORKTREE = 1,

        /// <summary>
        ///  The refdb that is to be initialized is for a worktree.
        /// </summary>
        GIT_REFDB_BACKEND_INIT_FORCE_HEAD = 2,
    }

    /// <summary>
    ///  An instance for a custom backend
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_refdb_backend
    {
        /// <summary>
        ///  The backend API version
        /// </summary>
        public uint version;

        /// <summary>
        ///  Create a new refdb and initialize its structures.
        ///
        ///  <para>
        ///   A refdb implementation may provide this function; if it
        ///   is not provided, no data structures will be initialized
        ///   for the refdb when a new repository is created.
        ///  </para>
        /// </summary>
        public IntPtr init;

        /// <summary>
        ///  Queries the refdb backend for the existence of a reference.
        ///
        ///  <para>
        ///   A refdb implementation must provide this function.
        ///  </para>
        /// </summary>
        public IntPtr exists;

        /// <summary>
        ///  Queries the refdb backend for a given reference.
        ///
        ///  <para>
        ///   A refdb implementation must provide this function.
        ///  </para>
        /// </summary>
        public IntPtr lookup;

        /// <summary>
        ///  Allocate an iterator object for the backend.
        ///
        ///  <para>
        ///   A refdb implementation must provide this function.
        ///  </para>
        /// </summary>
        public IntPtr iterator;

        /// <summary>
        ///  Writes the given reference to the refdb.
        ///
        ///  <para>
        ///   A refdb implementation must provide this function.
        ///  </para>
        /// </summary>
        public IntPtr write;

        /// <summary>
        ///  Rename a reference in the refdb.
        ///
        ///  <para>
        ///   A refdb implementation must provide this function.
        ///  </para>
        /// </summary>
        public IntPtr rename;

        /// <summary>
        ///  Deletes the given reference from the refdb.
        ///
        ///  <para>
        ///   If it exists, its reflog should be deleted as well.
        ///  </para>
        ///
        ///  <para>
        ///   A refdb implementation must provide this function.
        ///  </para>
        /// </summary>
        public IntPtr del;

        /// <summary>
        ///  Suggests that the given refdb compress or optimize its
        ///  references.
        ///
        ///  <para>
        ///   This mechanism is implementation specific. For on-disk
        ///   reference databases, this may pack all loose references.
        ///  </para>
        ///
        ///  <para>
        ///   A refdb implementation may provide this function; if it
        ///   is not provided, nothing will be done.
        ///  </para>
        /// </summary>
        public IntPtr compress;

        /// <summary>
        ///  Query whether a particular reference has a log (may be
        ///  empty)
        ///
        ///  <para>
        ///   Shall return 1 if it has a reflog, 0 it it doesn't and
        ///   negative in case an error occurred.
        ///  </para>
        ///
        ///  <para>
        ///   A refdb implementation must provide this function.
        ///  </para>
        /// </summary>
        public IntPtr has_log;

        /// <summary>
        ///  Make sure a particular reference will have a reflog which
        ///  will be appended to on writes.
        ///
        ///  <para>
        ///   A refdb implementation must provide this function.
        ///  </para>
        /// </summary>
        public IntPtr ensure_log;

        /// <summary>
        ///  Frees any resources held by the refdb (including the
        ///  `git_refdb_backend` itself).
        ///
        ///  <para>
        ///   A refdb backend implementation must provide this
        ///   function.
        ///  </para>
        /// </summary>
        public IntPtr free;

        /// <summary>
        ///  Read the reflog for the given reference name.
        ///
        ///  <para>
        ///   A refdb implementation must provide this function.
        ///  </para>
        /// </summary>
        public IntPtr reflog_read;

        /// <summary>
        ///  Write a reflog to disk.
        ///
        ///  <para>
        ///   A refdb implementation must provide this function.
        ///  </para>
        /// </summary>
        public IntPtr reflog_write;

        /// <summary>
        ///  Rename a reflog.
        ///
        ///  <para>
        ///   A refdb implementation must provide this function.
        ///  </para>
        /// </summary>
        public IntPtr reflog_rename;

        /// <summary>
        ///  Remove a reflog.
        ///
        ///  <para>
        ///   A refdb implementation must provide this function.
        ///  </para>
        /// </summary>
        public IntPtr reflog_delete;

        /// <summary>
        ///  Lock a reference.
        ///
        ///  <para>
        ///   A refdb implementation may provide this function; if it
        ///   is not provided, the transaction API will fail to work.
        ///  </para>
        /// </summary>
        public IntPtr _lock;

        /// <summary>
        ///  Unlock a reference.
        ///
        ///  <para>
        ///   Only one of target or symbolic_target will be set.
        ///   `success` will be true if the reference should be
        ///   update, false if the lock must be discarded.
        ///  </para>
        ///
        ///  <para>
        ///   A refdb implementation must provide this function if a
        ///   `lock` implementation is provided.
        ///  </para>
        /// </summary>
        public IntPtr unlock;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Initializes a `git_refdb_backend` with default values.
        ///  Equivalent to creating an instance with
        ///  GIT_REFDB_BACKEND_INIT.
        /// </summary>
        /// <param name="backend">
        ///  the `git_refdb_backend` struct to initialize
        /// </param>
        /// <param name="version">
        ///  Version of struct; pass `GIT_REFDB_BACKEND_VERSION`
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_refdb_init_backend(
            ref git_refdb_backend backend,
            uint version);

        /// <summary>
        ///  Constructors for default filesystem-based refdb backend
        ///
        ///  <para>
        ///   Under normal usage, this is called for you when the
        ///   repository is opened / created, but you can use this to
        ///   explicitly construct a filesystem refdb backend for a
        ///   repository.
        ///  </para>
        /// </summary>
        /// <param name="backend_out">
        ///  Output pointer to the git_refdb_backend object
        /// </param>
        /// <param name="repo">Git repository to access</param>
        /// <returns>0 on success, < 0 error code on failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_refdb_backend_fs(
            out git_refdb_backend* backend_out,
            git_repository* repo);

        /// <summary>
        ///  Constructor for the reftable-based refdb backend
        ///
        ///  <para>
        ///   Under normal usage, this is called for you when the
        ///   repository is opened / created that uses the reftable
        ///   format, but you can use this to explicitly construct a
        ///   reftable refdb backend for a repository.
        ///  </para>
        /// </summary>
        /// <param name="backend_out">
        ///  Output pointer to the git_refdb_backend object
        /// </param>
        /// <param name="repo">Git repository to access</param>
        /// <returns>0 on success, < 0 error code on failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_refdb_backend_reftable(
            out git_refdb_backend* backend_out,
            git_repository* repo);

        /// <summary>
        ///  Sets the custom backend to an existing reference DB
        ///
        ///  <para>
        ///   The `git_refdb` will take ownership of the
        ///   `git_refdb_backend` so you should NOT free it after
        ///   calling this function.
        ///  </para>
        /// </summary>
        /// <param name="refdb">database to add the backend to</param>
        /// <param name="backend">
        ///  pointer to a git_refdb_backend instance
        /// </param>
        /// <returns>0 on success; error code otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_refdb_set_backend(
            git_refdb* refdb,
            ref git_refdb_backend backend);
    }
}
