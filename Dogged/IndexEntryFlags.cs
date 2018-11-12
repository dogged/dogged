using System;
using System.Collections.Generic;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// The flags for an index entry.
    /// </summary>
    public enum IndexEntryFlags
    {
        /// <summary>
        /// The index entry is a normal file.
        /// </summary>
        None = 0,

        /// <summary>
        /// The working tree data should be assumed to be valid, and
        /// should not be compared to the index; the working tree
        /// should be assumed to be unchanged.
        /// </summary>
        Valid = (1 << 0),

        /// <summary>
        /// This path is intended to be added later.
        /// </summary>
        IntentToAdd = (1 << 1),

        /// <summary>
        /// The working tree version is assumed to be up-to-date and
        /// the index contents should be used regardless of what's on
        /// disk.
        /// </summary>
        SkipWorktree = (1 << 2),
    }
}
