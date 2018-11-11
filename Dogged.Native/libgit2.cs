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
    public static class libgit2
    {
        private const string libgit2_dll = NativeLibrary.Filename;

        private static NativeInitializer nativeInitializer;

        /// <summary>
        /// Load and initialize the native library.
        /// </summary>
        static libgit2()
        {
            nativeInitializer = new NativeInitializer();
        }

        /// <summary>
        /// Query compile time options for libgit2.  This will show the
        /// functionality that is built in to the library.
        /// </summary>
        /// <returns>A combination of <see cref="git_feature_t"/> values.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern git_feature_t git_libgit2_features();

        /// <summary>
        /// Initialize the native library's global state.
        ///
        /// <para>
        /// This function must be called before any other libgit2
        /// function in order to set up global state and threading.
        /// </para>
        ///
        /// <para>
        /// This function may be called multiple times - it will return
        /// the number of times the initialization has been called (including
        /// this one) that have not subsequently been shutdown.
        /// </para>
        /// </summary>
        /// <returns>The number of initializations of the library, or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_libgit2_init();

        /// <summary>
        /// Shutdown the global state.
        ///
        /// <para>
        /// Clean up the global state and threading context after calling
        /// it as many times as <see cref="git_libgit2_init"/> was called -
        /// it will return the number of remainining initializations that
        /// have not been shutdown (after this one).
        /// </para>
        /// </summary>
        /// <returns>The number of remaining initializations of the library, or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_libgit2_shutdown();
    }
}
