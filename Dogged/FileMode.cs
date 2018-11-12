using System;
using System.Collections;
using System.Collections.Generic;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// Valid modes for inde and tree entries.
    /// </summary>
    public enum FileMode
    {
        /// <summary>
        /// The file was unreadable or the index or tree was corrupt and
        /// a valid mode could not be read.
        /// </summary>
        Unreadable = 0,

        /// <summary>
        /// A tree object.
        /// </summary>
        Tree = 0x4000, // 040000

        /// <summary>
        /// A non-executable file.
        /// </summary>
        Blob = 0x81a4, // 0100644

        /// <summary>
        /// An executable file.
        /// </summary>
        ExecutableBlob = 0x81ed, // 0100755

        /// <summary>
        /// A symbolic link.
        /// </summary>
        Link = 0xa000, // 0120000

        /// <summary>
        /// A "gitlink", submodule or commit object.
        /// </summary>
        Commit = 0xe000, // 0160000
    }
}
