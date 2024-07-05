using System;
using System.Collections.Generic;
using System.Diagnostics;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// Disposable pattern for objects that manage the lifecycle of
    /// native resources, with refcounting.
    /// </summary>
    public abstract class RefcountedDisposable : KnownDisposable
    {
        private int count = 0;

        internal abstract void Dispose(bool disposing);

        public void Acquire()
        {
            count++;
        }

        /// <summary>
        /// Disposes the native resources associated with this object.
        /// </summary>
        public override void Dispose()
        {
            Debug.Assert(count > 0);

            if (--count == 0)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        ~RefcountedDisposable()
        {
            if (count == 1)
            {
                Dispose(false);
                count = 0;
            }

            Debug.Assert(count == 0);
        }
    }
}
