using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
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
            IntPtr backend,
            IntPtr data);
    }
}
