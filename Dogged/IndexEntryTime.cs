using System;
using System.Collections.Generic;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// The modification or creation time of an index entry.
    /// </summary>
    public class IndexEntryTime
    {
        /// <summary>
        /// The unix epoch, midnight of January 1, 1970.
        /// </summary>
        public static IndexEntryTime Epoch = new IndexEntryTime();

        internal IndexEntryTime() { }

        /// <summary>
        /// Creates an index entry time.
        /// </summary>
        /// <param name="seconds">The number of seconds since the Unix epoch</param>
        /// <param name="nanoseconds">The nanoseconds portion of the timestamp</param>
        public IndexEntryTime(int seconds, long nanoseconds = 0)
        {
            Seconds = seconds;
            Nanoseconds = nanoseconds;
        }

        internal static IndexEntryTime FromNative(git_index_time time)
        {
            return new IndexEntryTime(time.seconds, time.nanoseconds);
        }

        /// <summary>
        /// Gets the number of seconds since the Unix epoch.
        /// </summary>
        public int Seconds { get; private set; }

        /// <summary>
        /// Gets the nanosecond portion of the timestamp if the index is
        /// using high-resolution times.
        /// </summary>
        public long Nanoseconds { get; private set; }
    }
}
