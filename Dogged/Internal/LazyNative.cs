using System;
using System.Collections.Generic;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// Provides support for lazy initialization of values while respecting
    /// the native lifecycle.
    /// </summary>
    internal class LazyNative<T>
    {
        private readonly Func<T> valueFactory;
        private readonly NativeDisposable nativeObject;

        private T value;
        private bool hasValue = false;

        /// <summary>
        /// Creates a lazy value that will be created by the given
        /// <paramref name="valueFactory"/> upon first use, ensuring
        /// that the given <paramref name="nativeObject"/> is not disposed.
        /// Subsequent values will be returned without invoking the factory.
        /// </summary>
        /// <param name="valueFactory">The function to invoke to populate the value.</param>
        /// <param name="nativeObject">A native object to ensure is not disposed.</param>
        public LazyNative(Func<T> valueFactory, NativeDisposable nativeObject)
        {
            Ensure.ArgumentNotNull(valueFactory, "valueFactory");
            Ensure.ArgumentNotNull(nativeObject, "nativeObject");

            this.valueFactory = valueFactory;
            this.nativeObject = nativeObject;
        }

        /// <summary>
        /// Gets the value of this object, invoking the value factory if
        /// the value has not yet been read.
        /// </summary>
        public T Value
        {
            get
            {
                Ensure.NotDisposed(nativeObject);

                if (!hasValue)
                {
                    value = Ensure.NativeCall<T>(valueFactory, nativeObject);
                    hasValue = true;
                }

                return value;
            }
        }
    }
}
