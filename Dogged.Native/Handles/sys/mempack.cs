using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
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
            PackbuilderHandle pb);

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
            RepositoryHandle repo,
            ref git_odb_backend backend);
    }
}
