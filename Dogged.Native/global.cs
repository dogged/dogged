using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
        /// <summary>
        ///  Init the global state
        ///
        ///  <para>
        ///   This function must be called before any other libgit2
        ///   function in order to set up global state and threading.
        ///  </para>
        ///
        ///  <para>
        ///   This function may be called multiple times - it will
        ///   return the number of times the initialization has been
        ///   called (including this one) that have not subsequently
        ///   been shutdown.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  the number of initializations of the library, or an error
        ///  code.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_libgit2_init();

        /// <summary>
        ///  Shutdown the global state
        ///
        ///  <para>
        ///   Clean up the global state and threading context after
        ///   calling it as many times as `git_libgit2_init()` was
        ///   called - it will return the number of remainining
        ///   initializations that have not been shutdown (after this
        ///   one).
        ///  </para>
        /// </summary>
        /// <returns>
        ///  the number of remaining initializations of the library, or
        ///  an error code.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_libgit2_shutdown();
    }
}
