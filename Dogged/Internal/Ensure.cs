using System;
using System.Collections.Generic;
using Dogged.Native;
using Dogged.Native.Services;

namespace Dogged
{
    /// <summary>
    /// Helper methods to provide well-typed managed exceptions on input
    /// validation failure or on native function errors.
    /// </summary>
    internal static class Ensure
    {
        // Default map of native error codes to managed exceptions.
        private static readonly Dictionary<git_error_code, Func<string, Exception>> defaultExceptions = new Dictionary<git_error_code, Func<string, Exception>>
        {
            { git_error_code.GIT_EUSER, (m) => new UserCancelledException(m) },
            { git_error_code.GIT_EBAREREPO, (m) => new BareRepositoryException(m) },
            { git_error_code.GIT_EEXISTS, (m) => new ItemExistsException(m) },
            { git_error_code.GIT_EINVALIDSPEC, (m) => new InvalidSpecificationException(m) },
            { git_error_code.GIT_EUNMERGED, (m) => new UnmergedIndexEntriesException(m) },
            { git_error_code.GIT_ENONFASTFORWARD, (m) => new NonFastForwardException(m) },
            { git_error_code.GIT_ECONFLICT, (m) => new CheckoutConflictException(m) },
            { git_error_code.GIT_ELOCKED, (m) => new LockedFileException(m) },
            { git_error_code.GIT_ENOTFOUND, (m) => new NotFoundException(m) },
            { git_error_code.GIT_EPEEL, (m) => new CannotBePeeledException(m) },
        };

        /// <summary>
        /// Ensure that the given <paramref name="object"/> value is not null.
        /// Used to validate method parameter values provided by callers.
        /// </summary>
        /// <param name="o">The object to validate.</param>
        /// <param name="name">The name of the object to use in messages.</param>
        public static void ArgumentNotNull(object o, string name)
        {
            if (o == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        /// <summary>
        /// Ensure that the given pointer is not null. Used to validate
        /// values used or provided by native methods.
        /// </summary>
        /// <param name="o">The object to validate.</param>
        /// <param name="name">The name of the object to use in messages.</param>
        public unsafe static void ArgumentNotNull(void* o, string name)
        {
            if (o == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        /// <summary>
        /// Ensures that the given argument conforms to a format specified
        /// by a <paramref name="validation"/> function.
        /// </summary>
        /// <param name="validation">The validation function to apply</param>
        /// <param name="message">The message used when the argument does not conform</param>
        public static void ArgumentConformsTo(Func<bool> validation, string name, string message)
        {
            if (!validation())
            {
                throw new ArgumentException(name, message);
            }
        }

        /// <summary>
        /// Ensures that the given unsigned long can be cast to an integer
        /// without losing data.  Used to ensure that values from native
        /// functions can be represented in managed int types.
        /// </summary>
        /// <param value="value">The value to test</param>
        /// <param value="name">The name of the value to use for error messages</param>
        public static int CastToInt(ulong value, string name)
        {
            if (value > int.MaxValue)
            {
                throw new InvalidCastException(string.Format("{0} is too large for an int", name));
            }

            return (int)value;
        }

        /// <summary>
        /// Ensures that the given UIntPtr can be cast to an integer without
        /// losing data.  Used to ensure that values from native functions
        /// can be represented in managed int types.
        /// </summary>
        /// <param value="value">The value to test</param>
        /// <param value="name">The name of the value to use for error messages</param>
        public static int CastToInt(UIntPtr value, string name)
        {
            if ((ulong)value > int.MaxValue)
            {
                throw new InvalidCastException(string.Format("{0} is too large for an int", name));
            }

            return (int)value;
        }

        /// <summary>
        /// Ensures that the given UIntPtr can be cast to a long without
        /// losing data.  Used to ensure that values from native functions
        /// can be represented in managed long types.
        /// </summary>
        /// <param value="value">The value to test</param>
        /// <param value="name">The name of the value to use for error messages</param>
        public static long CastToLong(UIntPtr value, string name)
        {
            if ((ulong)value > long.MaxValue)
            {
                throw new InvalidCastException(string.Format("{0} is too large for a long", name));
            }

            return (long)value;
        }

        /// <summary>
        /// Ensures that the given long can be cast to a UInt without losing
        /// data.  Used to ensure that values from callers can be represented
        /// by native unsigned int types.
        /// </summary>
        /// <param value="value">The value to test</param>
        /// <param value="name">The name of the value to use for error messages</param>
        public static uint CastToUInt(long value, string name)
        {
            if (value < 0 || value > uint.MaxValue)
            {
                throw new InvalidCastException(string.Format("{0} is out of range", name));
            }

            return (uint)value;
        }

        public static void EnumDefined(Type type, object value, string name)
        {
            if (!Enum.IsDefined(type, value))
            {
                throw new InvalidOperationException(string.Format("value {0} is out of range for {1}", value, type.Name));
            }
        }

        /// <summary>
        /// Ensure that the given native object <paramref name="wrapper"/>
        /// has not been disposed.
        /// </summary>
        /// <param name="wrapper">The native object wrapper to validate.</param>
        public unsafe static void NotDisposed(NativeDisposable wrapper)
        {
            if (wrapper.IsDisposed)
            {
                throw new ObjectDisposedException(wrapper.GetType().Name);
            }
        }

        /// <summary>
        /// Ensure that the given <paramref name="nativeObject"/> is not null,
        /// indicating that it has not been disposed.
        /// </summary>
        /// <param name="nativeObject">The object to validate.</param>
        /// <param name="name">The name of the object to use in messages.</param>
        public unsafe static void NotDisposed(void* nativeObject, string name)
        {
            if (nativeObject == null)
            {
                throw new ObjectDisposedException(name);
            }
        }

        /// <summary>
        /// Safely executes the given native function call for a given
        /// object, ensuring that the object is not disposed and that it
        /// will be kept alive until the end of the call.
        /// </summary>
        /// <param name="call">The function to invoke.</param>
        /// <param name="obj">The object to validate and keep alive.</param>
        public static T NativeCall<T>(Func<T> call, NativeDisposable obj)
        {
            Ensure.NotDisposed(obj);
            T ret = call();
            GC.KeepAlive(obj);
            return ret;
        }

        /// <summary>
        /// Ensures that the given native return code corresponds to a
        /// boolean.  If the return code is negative, indicating native
        /// function call failure, an exception will be thrown.  Otherwise,
        /// the native return code will be converted to a managed boolean.
        /// </summary>
        /// <param name="nativeReturnCode">The result of a native function call</param>
        /// <returns>A bool corresponding to the given native return code</returns>
        public static bool NativeBoolean(int nativeReturnCode)
        {
            if (nativeReturnCode < 0)
            {
                HandleNativeError(nativeReturnCode, null);
            }

            return (nativeReturnCode != 0) ? true : false;
        }

        /// <summary>
        /// Ensures that the given native pointer is not null.  This is
        /// is useful for validating that a native function succeeds.
        /// </summary>
        /// <param name="nativeResult">The result of a native function call</param>
        public static unsafe void NativePointerNotNull(void* nativeResult)
        {
            if (nativeResult == null)
            {
                throw new DoggedException(GetNativeErrorMessage());
            }
        }

        /// <summary>
        /// Ensures that the given native object is not null, returning
        /// the value.  This is useful for validating that a native
        /// function succeeds.
        /// </summary>
        /// <param name="nativeResult">The result of a native function call</param>
        /// <returns>The native result provided</returns>
        public static unsafe T NativeObjectNotNull<T>(T nativeResult)
        {
            if (nativeResult == null)
            {
                throw new DoggedException(GetNativeErrorMessage());
            }

            return nativeResult;
        }

        /// <summary>
        /// Ensure that the given return code from a native function indicates
        /// success; ie that it is non-negative.
        /// <param name="nativeReturnCode">The return code from a libgit2 function.</param>
        /// </summary>
        public static void NativeSuccess(int nativeReturnCode)
        {
            NativeSuccess(nativeReturnCode, null);
        }

        /// <summary>
        /// Ensure that the given return code from a native function indicates
        /// success; ie that it is non-negative.
        /// </summary>
        /// <param name="nativeReturnCode">The return code from a libgit2 function.</param>
        /// <param name="customExceptions">Any custom exceptions that should be thrown for a particular libgit2 error code.</param>
        public static void NativeSuccess(int nativeReturnCode, Dictionary<git_error_code, Func<string, Exception>> customExceptions)
        {
            if (nativeReturnCode < 0)
            {
                HandleNativeError(nativeReturnCode, customExceptions);
            }
        }

        /// <summary>
        /// Invoke the given function and ensure that its return code
        /// indicates success; ie that it is non-negative.
        /// </summary>
        /// <param name="call">The function call to invoke.</param>
        /// <param name="obj">The object to validate and keep alive.</param>
        public static void NativeSuccess(Func<int> call, NativeDisposable obj)
        {
            NativeSuccess(NativeCall<int>(call, obj));
        }

        /// <summary>
        /// Identify the libgit2 error and throw an error code.
        /// </summary>
        /// <param name="nativeReturnCode">The return code from a libgit2 function.</param>
        /// <param name="customExceptions">Any custom exceptions that should be thrown for a particular libgit2 error code.</param>
        private static unsafe void HandleNativeError(int nativeReturnCode, Dictionary<git_error_code, Func<string, Exception>> customExceptions)
        {
            Func<string, Exception> exceptionBuilder = (m) => new DoggedException(m);
            git_error_code code = (git_error_code)nativeReturnCode;

            if (customExceptions != null && customExceptions.ContainsKey(code))
            {
                exceptionBuilder = customExceptions[code];
            }
            else if (defaultExceptions.ContainsKey(code))
            {
                exceptionBuilder = defaultExceptions[code];
            }

            throw exceptionBuilder(GetNativeErrorMessage());
        }

        private static unsafe string GetNativeErrorMessage()
        {
            git_error* error = libgit2.git_error_last();

            if (error == null || error->message == null)
            {
                return "Unknown error in libgit2";
            }
            else
            {
               return Utf8Converter.FromNative(error->message);
            }
        }
    }
}
