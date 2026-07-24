using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  An open refs database handle.
    /// </summary>
    public struct git_refdb
    {
    }

    /// <summary>
    ///  The type of the refdb as determined by "extensions.refStorage".
    /// </summary>
    public enum git_refdb_t
    {
        /// <summary>
        ///  Files backend using loose and packed refs.
        /// </summary>
        GIT_REFDB_FILES = 1,

        /// <summary>
        ///  Reftable backend.
        /// </summary>
        GIT_REFDB_REFTABLE = 2,
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Create a new reference database with no backends.
        ///
        ///  <para>
        ///   Before the Ref DB can be used for read/writing, a custom
        ///   database backend must be manually set using
        ///   `git_refdb_set_backend()`
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  location to store the database pointer, if opened. Set to
        ///  NULL if the open failed.
        /// </param>
        /// <param name="repo">the repository</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_refdb_new(
            out git_refdb* _out,
            git_repository* repo);

        /// <summary>
        ///  Create a new reference database and automatically add the
        ///  default backends:
        ///
        ///  <para>
        ///   - git_refdb_dir: read and write loose and packed refs
        ///   from disk, assuming the repository dir as the folder
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  location to store the database pointer, if opened. Set to
        ///  NULL if the open failed.
        /// </param>
        /// <param name="repo">the repository</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_refdb_open(
            out git_refdb* _out,
            git_repository* repo);

        /// <summary>
        ///  Suggests that the given refdb compress or optimize its
        ///  references. This mechanism is implementation specific. For
        ///  on-disk reference databases, for example, this may pack all
        ///  loose references.
        /// </summary>
        /// <param name="refdb">The reference database to optimize.</param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_refdb_compress(
            git_refdb* refdb);

        /// <summary>
        ///  Close an open reference database.
        /// </summary>
        /// <param name="refdb">reference database pointer or NULL</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_refdb_free(
            git_refdb* refdb);
    }
}
