using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Array of strings
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_strarray
    {
        public char** strings;

        public UIntPtr count;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Free the strings contained in a string array. This method
        ///  should be called on `git_strarray` objects that were
        ///  provided by the library. Not doing so, will result in a
        ///  memory leak.
        ///
        ///  <para>
        ///   This does not free the `git_strarray` itself, since the
        ///   library will never allocate that object directly itself.
        ///  </para>
        /// </summary>
        /// <param name="array">
        ///  The git_strarray that contains strings to free
        /// </param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_strarray_dispose(
            ref git_strarray array);
    }
}
