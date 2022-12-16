using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    /// Representation of string array.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct git_strarray
    {
        public unsafe IntPtr* strings;
        public UIntPtr count;
    }
}
