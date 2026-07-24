using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  An instance for a custom backend
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_odb_backend
    {
        public uint version;

        public IntPtr odb;

        public IntPtr read;

        public IntPtr read_prefix;

        public IntPtr read_header;

        /// <summary>
        ///  Write an object into the backend. The id of the object has
        ///  already been calculated and is passed in.
        /// </summary>
        public IntPtr write;

        public IntPtr writestream;

        public IntPtr readstream;

        public IntPtr exists;

        public IntPtr exists_prefix;

        /// <summary>
        ///  If the backend implements a refreshing mechanism, it should
        ///  be exposed through this endpoint. Each call to
        ///  `git_odb_refresh()` will invoke it.
        ///
        ///  <para>
        ///   The odb layer will automatically call this when needed
        ///   on failed lookups (ie. `exists()`, `read()`,
        ///   `read_header()`).
        ///  </para>
        /// </summary>
        public IntPtr refresh;

        public IntPtr _foreach;

        public IntPtr writepack;

        /// <summary>
        ///  If the backend supports pack files, this will create a
        ///  `multi-pack-index` file which will contain an index of all
        ///  objects across all the `.pack` files.
        /// </summary>
        public IntPtr writemidx;

        /// <summary>
        ///  "Freshens" an already existing object, updating its
        ///  last-used time. This occurs when `git_odb_write` was called,
        ///  but the object already existed (and will not be re-written).
        ///  The underlying implementation may want to update last-used
        ///  timestamps.
        ///
        ///  <para>
        ///   If callers implement this, they should return `0` if the
        ///   object exists and was freshened, and non-zero otherwise.
        ///  </para>
        /// </summary>
        public IntPtr freshen;

        /// <summary>
        ///  Frees any resources held by the odb (including the
        ///  `git_odb_backend` itself). An odb backend implementation
        ///  must provide this function.
        /// </summary>
        public IntPtr free;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Initializes a `git_odb_backend` with default values.
        ///  Equivalent to creating an instance with
        ///  GIT_ODB_BACKEND_INIT.
        /// </summary>
        /// <param name="backend">
        ///  the `git_odb_backend` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  Version the struct; pass `GIT_ODB_BACKEND_VERSION`
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_odb_init_backend(
            ref git_odb_backend backend,
            uint version);

        /// <summary>
        ///  Allocate data for an ODB object. Custom ODB backends may use
        ///  this to provide data back to the ODB from their read
        ///  function. This memory should not be freed once it is
        ///  returned to libgit2. If a custom ODB uses this function but
        ///  encounters an error and does not return this data to
        ///  libgit2, then they should use the corresponding
        ///  git_odb_backend_data_free function.
        /// </summary>
        /// <param name="backend">
        ///  the ODB backend that is allocating this memory
        /// </param>
        /// <param name="len">the number of bytes to allocate</param>
        /// <returns>
        ///  the allocated buffer on success or NULL if out of memory
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe IntPtr git_odb_backend_data_alloc(
            ref git_odb_backend backend,
            UIntPtr len);

        /// <summary>
        ///  Frees custom allocated ODB data. This should only be called
        ///  when memory allocated using git_odb_backend_data_alloc is
        ///  not returned to libgit2 because the backend encountered an
        ///  error in the read function after allocation and did not
        ///  return this data to libgit2.
        /// </summary>
        /// <param name="backend">
        ///  the ODB backend that is freeing this memory
        /// </param>
        /// <param name="data">the buffer to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_odb_backend_data_free(
            ref git_odb_backend backend,
            IntPtr data);

        /// <summary>
        ///  Allocate memory for an ODB object from a custom backend.
        ///  This is an alias of `git_odb_backend_data_alloc` and is
        ///  preserved for backward compatibility.
        ///
        ///  <para>
        ///   This function is deprecated, but there is no plan to
        ///   remove this function at this time.
        ///  </para>
        /// </summary>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe IntPtr git_odb_backend_malloc(
            ref git_odb_backend backend,
            UIntPtr len);
    }
}
