using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Available tracing levels. When tracing is set to a particular
    ///  level, callers will be provided tracing at the given level and
    ///  all lower levels.
    /// </summary>
    public enum git_trace_level_t
    {
        /// <summary>
        ///  No tracing will be performed.
        /// </summary>
        GIT_TRACE_NONE = 0,

        /// <summary>
        ///  Severe errors that may impact the program's execution
        /// </summary>
        GIT_TRACE_FATAL = 1,

        /// <summary>
        ///  Errors that do not impact the program's execution
        /// </summary>
        GIT_TRACE_ERROR = 2,

        /// <summary>
        ///  Warnings that suggest abnormal data
        /// </summary>
        GIT_TRACE_WARN = 3,

        /// <summary>
        ///  Informational messages about program execution
        /// </summary>
        GIT_TRACE_INFO = 4,

        /// <summary>
        ///  Detailed data that allows for debugging
        /// </summary>
        GIT_TRACE_DEBUG = 5,

        /// <summary>
        ///  Exceptionally detailed debugging data
        /// </summary>
        GIT_TRACE_TRACE = 6,
    }

    /// <summary>
    ///  An instance for a tracing function
    /// </summary>
    /// <param name="level">the trace level</param>
    /// <param name="msg">the trace message</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void git_trace_cb(
        git_trace_level_t level,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string msg);

    public static partial class libgit2
    {
        /// <summary>
        ///  Sets the system tracing configuration to the specified level
        ///  with the specified callback. When system events occur at a
        ///  level equal to, or lower than, the given level they will be
        ///  reported to the given callback.
        /// </summary>
        /// <param name="level">Level to set tracing to</param>
        /// <param name="cb">Function to call with trace data</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_trace_set(
            git_trace_level_t level,
            git_trace_cb cb);
    }
}
