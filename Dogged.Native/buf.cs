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
}
