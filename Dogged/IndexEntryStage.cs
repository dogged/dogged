using System;
using System.Collections.Generic;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// The index entry stage level.
    /// </summary>
    public enum IndexEntryStage
    {
        /// <summary>
        /// Stage 0, the "main index"; a normal index entry.
        /// </summary>
        Main = 0,

        /// <summary>
        /// The "ours" side of a conflict index entry.
        /// </summary>
        Ours = 1,

        /// <summary>
        /// The "theirs" side of a conflict index entry.
        /// </summary>
        Theirs = 2,

        /// <summary>
        /// The common ancestor side of a conflict index entry.
        /// </summary>
        Ancestor = 3
    }
}
