using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
        /// <summary>
        ///  Clear the last library error that occurred for this thread.
        /// </summary>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_error_clear();

        /// <summary>
        ///  Set the error message string for this thread, using
        ///  `printf`-style formatting.
        ///
        ///  <para>
        ///   This function is public so that custom ODB backends and
        ///   the like can relay an error message through libgit2.
        ///   Most regular users of libgit2 will never need to call
        ///   this function -- actually, calling it in most
        ///   circumstances (for example, calling from within a
        ///   callback function) will just end up having the value
        ///   overwritten by libgit2 internals.
        ///  </para>
        ///
        ///  <para>
        ///   This error message is stored in thread-local storage and
        ///   only applies to the particular thread that this libgit2
        ///   call is made from.
        ///  </para>
        /// </summary>
        /// <param name="error_class">
        ///  One of the `git_error_t` enum above describing the general
        ///  subsystem that is responsible for the error.
        /// </param>
        /// <param name="fmt">
        ///  The `printf`-style format string; subsequent arguments must
        ///  be the arguments for the format string.
        /// </param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_error_set(
            int error_class,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string fmt);

        /// <summary>
        ///  Set the error message string for this thread. This function
        ///  is like `git_error_set` but takes a static string instead of
        ///  a `printf`-style format.
        /// </summary>
        /// <param name="error_class">
        ///  One of the `git_error_t` enum above describing the general
        ///  subsystem that is responsible for the error.
        /// </param>
        /// <param name="_string">The error message to keep</param>
        /// <returns>0 on success or -1 on failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_error_set_str(
            int error_class,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string _string);

        /// <summary>
        ///  Set the error message to a special value for memory
        ///  allocation failure.
        ///
        ///  <para>
        ///   The normal `git_error_set_str()` function attempts to
        ///   `strdup()` the string that is passed in. This is not a
        ///   good idea when the error in question is a memory
        ///   allocation failure. That circumstance has a special
        ///   setter function that sets the error string to a known
        ///   and statically allocated internal value.
        ///  </para>
        /// </summary>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_error_set_oom();
    }
}
