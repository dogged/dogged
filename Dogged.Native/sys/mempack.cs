using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
        /// <summary>
        ///  Instantiate a new mempack backend.
        ///
        ///  <para>
        ///   The backend must be added to an existing ODB with the
        ///   highest priority.
        ///  </para>
        ///
        ///  <para>
        ///   git_mempack_new( &mempacker ); git_repository_odb( &odb
        ///   , repository); git_odb_add_backend(odb, mempacker, 999);
        ///  </para>
        ///
        ///  <para>
        ///   Once the backend has been loaded, all writes to the ODB
        ///   will instead be queued in memory, and can be finalized
        ///   with `git_mempack_dump`.
        ///  </para>
        ///
        ///  <para>
        ///   Subsequent reads will also be served from the in-memory
        ///   store to ensure consistency, until the memory store is
        ///   dumped.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Pointer where to store the ODB backend
        /// </param>
        /// <returns>0 on success; error code otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_mempack_new(
            out git_odb_backend* _out);

        /// <summary>
        ///  Write a thin packfile with the objects in the memory store.
        ///
        ///  <para>
        ///   A thin packfile is a packfile that does not contain its
        ///   transitive closure of references. This is useful for
        ///   efficiently distributing additions to a repository over
        ///   the network, but also finds use in the efficient bulk
        ///   addition of objects to a repository, locally.
        ///  </para>
        ///
        ///  <para>
        ///   This operation performs the (shallow) insert operations
        ///   into the `git_packbuilder`, but does not write the
        ///   packfile to disk; see `git_packbuilder_write_buf`.
        ///  </para>
        ///
        ///  <para>
        ///   It also does not reset the in-memory object database;
        ///   see `git_mempack_reset`.
        ///  </para>
        /// </summary>
        /// <param name="backend">The mempack backend</param>
        /// <param name="pb">
        ///  The packbuilder to use to write the packfile
        /// </param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_mempack_write_thin_pack(
            ref git_odb_backend backend,
            git_packbuilder* pb);

        /// <summary>
        ///  Dump all the queued in-memory writes to a packfile.
        ///
        ///  <para>
        ///   The contents of the packfile will be stored in the given
        ///   buffer. It is the caller's responsibility to ensure that
        ///   the generated packfile is available to the repository
        ///   (e.g. by writing it to disk, or doing something crazy
        ///   like distributing it across several copies of the
        ///   repository over a network).
        ///  </para>
        ///
        ///  <para>
        ///   Once the generated packfile is available to the
        ///   repository, call `git_mempack_reset` to cleanup the
        ///   memory store.
        ///  </para>
        ///
        ///  <para>
        ///   Calling `git_mempack_reset` before the packfile has been
        ///   written to disk will result in an inconsistent
        ///   repository (the objects in the memory store won't be
        ///   accessible).
        ///  </para>
        /// </summary>
        /// <param name="pack">
        ///  Buffer where to store the raw packfile
        /// </param>
        /// <param name="repo">
        ///  The active repository where the backend is loaded
        /// </param>
        /// <param name="backend">The mempack backend</param>
        /// <returns>0 on success; error code otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_mempack_dump(
            ref git_buf pack,
            git_repository* repo,
            ref git_odb_backend backend);

        /// <summary>
        ///  Reset the memory packer by clearing all the queued objects.
        ///
        ///  <para>
        ///   This assumes that `git_mempack_dump` has been called
        ///   before to store all the queued objects into a single
        ///   packfile.
        ///  </para>
        ///
        ///  <para>
        ///   Alternatively, call `reset` without a previous dump to
        ///   "undo" all the recently written objects, giving
        ///   transaction-like semantics to the Git repository.
        ///  </para>
        /// </summary>
        /// <param name="backend">The mempack backend</param>
        /// <returns>0 on success; error code otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_mempack_reset(
            ref git_odb_backend backend);

        /// <summary>
        ///  Get the total number of objects in mempack
        /// </summary>
        /// <param name="count">The count of objects in the mempack</param>
        /// <param name="backend">The mempack backend</param>
        /// <returns>0 on success, or -1 on error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_mempack_object_count(
            out UIntPtr count,
            ref git_odb_backend backend);
    }
}
