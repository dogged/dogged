using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    /// The timestamp for a commit.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_time
    {
        /// <summary>
        /// The time in the number of seconds since the Unix epoch
        /// (midnight, January 1, 1970).
        /// </summary>
        public long time;

        /// <summary>
        /// The timezone offset in minutes.
        /// </summary>
        public int offset;
    }
}
