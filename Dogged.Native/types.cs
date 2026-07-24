using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Time in a signature
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_time
    {
        /// <summary>
        ///  time in seconds from epoch
        /// </summary>
        public long time;

        /// <summary>
        ///  timezone offset, in minutes
        /// </summary>
        public int offset;

        /// <summary>
        ///  indicator for questionable '-0000' offsets in signature
        /// </summary>
        public char sign;
    }

    /// <summary>
    ///  A type to write in a streaming fashion, for example, for
    ///  filters.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_writestream
    {
        public IntPtr write;

        public IntPtr close;

        public IntPtr free;
    }
}
