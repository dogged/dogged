using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    /// Valid modes for index and tree entries.
    /// </summary>
    public enum git_filemode_t
    {
        /// <summary>
        /// The file was unreadable or the index or tree was corrupt and
        /// a valid mode could not be read.
        /// </summary>
        GIT_FILEMODE_UNREADABLE = 0,

        /// <summary>
        /// A tree object.
        /// </summary>
        GIT_FILEMODE_TREE = 0x400, // 040000

        /// <summary>
        /// A non-executable file.
        /// </summary>
        GIT_FILEMODE_BLOB = 0x81a4, // 0100644

        /// <summary>
        /// An executable file.
        /// </summary>
        GIT_FILEMODE_BLOB_EXECUTABLE = 0x81ed, // 0100755

        /// <summary>
        /// A symbolic link.
        /// </summary>
        GIT_FILEMODE_LINK = 0xa000, // 0120000

        /// <summary>
        /// A "gitlink", submodule or commit object.
        /// </summary>
        GIT_FILEMODE_COMMIT = 0xe000, // 0160000
    }
}
