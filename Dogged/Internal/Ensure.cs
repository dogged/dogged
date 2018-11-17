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
            string message;

            git_error* error = libgit2.git_error_last();

            if (error == null || error->message == null)
            {
                message = "Unknown error in libgit2";
            }
            else
            {
                message = Utf8Converter.FromNative(error->message);
            }

            if (customExceptions != null && customExceptions.ContainsKey(code))
            {
                exceptionBuilder = customExceptions[code];
            }
            else if (defaultExceptions.ContainsKey(code))
            {
                exceptionBuilder = defaultExceptions[code];
            }

            throw exceptionBuilder(message);
        }
    }
}
