using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Array of object ids
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_oidarray
    {
        public IntPtr ids;

        public UIntPtr count;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Free the object IDs contained in an oid_array. This method
        ///  should be called on `git_oidarray` objects that were
        ///  provided by the library. Not doing so will result in a
        ///  memory leak.
        ///
        ///  <para>
        ///   This does not free the `git_oidarray` itself, since the
        ///   library will never allocate that object directly itself.
        ///  </para>
        /// </summary>
        /// <param name="array">
        ///  git_oidarray from which to free oid data
        /// </param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_oidarray_dispose(
            ref git_oidarray array);
    }
}
