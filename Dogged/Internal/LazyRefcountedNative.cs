using System;
using System.Collections.Generic;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// Provides support for lazy initialization of values while respecting
    /// the native lifecycle.
    /// </summary>
    internal class LazyRefcountedNative<T> : LazyNative<T> where T : RefcountedDisposable
    {
        public LazyRefcountedNative(Func<T> valueFactory, NativeDisposable nativeObject) : base(valueFactory, nativeObject)
        {
        }

        /// <summary>
        /// Gets the value of this object, invoking the value factory if
        /// the value has not yet been read.
        /// </summary>
        public override T Value
        {
            get
            {
                T value = base.Value;
                value.Acquire();
                return value;
            }
        }

        public void Dispose()
        {
            if (HasValue)
            {
                T value = base.Clear();
                value.Dispose();
            }
        }
    }
}
