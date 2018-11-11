using System;
using System.Collections.Generic;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// Disposable pattern for objects that manage the lifecycle of
    /// native resources.
    /// </summary>
    public abstract class NativeDisposable : IDisposable
    {
        internal abstract bool IsDisposed { get; }
        internal abstract void Dispose(bool disposing);

        /// <summary>
        /// Disposes the native resources associated with this object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~NativeDisposable()
        {
            Dispose(false);
        }
    }
}
