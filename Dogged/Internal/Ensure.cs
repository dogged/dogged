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
