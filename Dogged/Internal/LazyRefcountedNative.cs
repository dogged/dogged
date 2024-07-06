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
        public LazyRefcountedNative(NativeDisposable nativeObject) : base(nativeObject) { }

        public override T Get(Func<T> valueFactory)
        {
            T value = base.Get(valueFactory);
            value.Acquire();
            return value;
        }

        public override void Set(T value)
        {
            Dispose();

            if (value != null)
            {
                value.Acquire();
                base.Set(value);
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
