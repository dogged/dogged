using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace Dogged.Native
{
    /// <summary>
    /// An object with critical finalization which can be loaded during
    /// the first use of a libgit2 function.  Upon initialization, it will
    /// invoke the necessary native library initializers and upon garbage
    /// collection, its finalizer will invoke the necessary shutdown.
    /// <summary>
    internal class NativeInitializer : CriticalFinalizerObject
    {
        /// <summary>
        /// Initialize the native library.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public NativeInitializer()
        {
            libgit2.git_libgit2_init();
        }

        /// <summary>
        /// Shutdown the native library.
        /// </summary>
        ~NativeInitializer()
        {
            libgit2.git_libgit2_shutdown();
        }
    }
}
