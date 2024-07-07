using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe class git_buf
    {
        public byte* ptr;
        public UIntPtr asize;
        public UIntPtr size;
    }

    public static partial class libgit2
    {
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_buf_dispose(git_buf buf);
    }
}
