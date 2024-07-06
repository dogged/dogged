using System;
using System.Collections.Generic;
using System.Diagnostics;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// Provides support for lazy initialization of values while respecting
    /// the native lifecycle.
    /// </summary>
    internal class LazyNative<T>
    {
        private readonly NativeDisposable nativeObject;

        private T value;
        private bool hasValue = false;

        /// <summary>
        /// Creates a lazy value that will be created on the first call to
        /// the getter, ensuring that the given <paramref name="nativeObject"/>
        /// is not disposed.
        /// <param name="nativeObject">A native object to ensure is not disposed.</param>
        public LazyNative(NativeDisposable nativeObject)
        {
            Ensure.ArgumentNotNull(nativeObject, "nativeObject");

            this.nativeObject = nativeObject;
        }

        /// <summary>
        /// Determines if the value has been set or not.
        /// </summary>
        protected bool HasValue
        {
            get
            {
                return hasValue;
            }
        }

        /// <summary>
        /// Gets the value of this object, invoking the value factory if
        /// the value has not yet been read.
        /// </summary>
        public virtual T Get(Func<T> valueFactory)
        {
            Ensure.NotDisposed(nativeObject);

            if (!hasValue)
            {
                value = Ensure.NativeCall<T>(valueFactory, nativeObject);
                hasValue = true;
            }

            return value;
        }

        /// <summary>
        /// Sets or clears the value of this object.
        /// </summary>
        public virtual void Set(T value)
        {
            Ensure.NotDisposed(nativeObject);

            if (value == null || value.Equals(default(T)))
            {
                hasValue = false;
                this.value = default(T);
            }
            else
            {
                hasValue = true;
                this.value = value;
            }
        }

        /// <summary>
        /// Takes the object; clearing it to the default value.
        /// </summary>
        public T Clear()
        {
            T ret = value;
            hasValue = false;
            value = default(T);
            return ret;
        }
    }
}
