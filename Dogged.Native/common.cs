using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    /// The native libgit2 functions.  Upon the first invocation of any
    /// of these functions, the native library will be loaded and the
    /// <see cref="git_libgit2_init"/> function will be called to set up
    /// the native library.
    /// </summary>
    public static partial class libgit2
    {
#if WINDOWS
        public const string GIT_PATH_LIST_SEPARATOR = ";";
#else
        public const string GIT_PATH_LIST_SEPARATOR = ":";
#endif
    }
}
