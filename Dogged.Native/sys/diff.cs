using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Performance data from diffing
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_diff_perfdata
    {
        public uint version;

        /// <summary>
        ///  Number of stat() calls performed
        /// </summary>
        public UIntPtr stat_calls;

        /// <summary>
        ///  Number of ID calculations
        /// </summary>
        public UIntPtr oid_calculations;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Diff print callback that writes to a git_buf.
        ///
        ///  <para>
        ///   This function is provided not for you to call it
        ///   directly, but instead so you can use it as a function
        ///   pointer to the `git_diff_print` or `git_patch_print`
        ///   APIs. When using those APIs, you specify a callback to
        ///   actually handle the diff and/or patch data.
        ///  </para>
        ///
        ///  <para>
        ///   Use this callback to easily write that data to a
        ///   `git_buf` buffer. You must pass a `git_buf *` value as
        ///   the payload to the `git_diff_print` and/or
        ///   `git_patch_print` function. The data will be appended to
        ///   the buffer (after any existing content).
        ///  </para>
        /// </summary>
        /// <param name="delta">the delta being processed</param>
        /// <param name="hunk">the hunk being processed</param>
        /// <param name="line">the line being processed</param>
        /// <param name="payload">
        ///  the payload provided by the diff generator
        /// </param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_diff_print_callback__to_buf(
            ref git_diff_delta delta,
            ref git_diff_hunk hunk,
            ref git_diff_line line,
            IntPtr payload);

        /// <summary>
        ///  Diff print callback that writes to stdio FILE handle.
        ///
        ///  <para>
        ///   This function is provided not for you to call it
        ///   directly, but instead so you can use it as a function
        ///   pointer to the `git_diff_print` or `git_patch_print`
        ///   APIs. When using those APIs, you specify a callback to
        ///   actually handle the diff and/or patch data.
        ///  </para>
        ///
        ///  <para>
        ///   Use this callback to easily write that data to a stdio
        ///   FILE handle. You must pass a `FILE *` value (such as
        ///   `stdout` or `stderr` or the return value from `fopen()`)
        ///   as the payload to the `git_diff_print` and/or
        ///   `git_patch_print` function. If you pass NULL, this will
        ///   write data to `stdout`.
        ///  </para>
        /// </summary>
        /// <param name="delta">the delta being processed</param>
        /// <param name="hunk">the hunk being processed</param>
        /// <param name="line">the line being processed</param>
        /// <param name="payload">
        ///  the payload provided by the diff generator
        /// </param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_diff_print_callback__to_file_handle(
            ref git_diff_delta delta,
            ref git_diff_hunk hunk,
            ref git_diff_line line,
            IntPtr payload);

        /// <summary>
        ///  Get performance data for a diff object.
        /// </summary>
        /// <param name="_out">
        ///  Structure to be filled with diff performance data
        /// </param>
        /// <param name="diff">Diff to read performance data from</param>
        /// <returns>0 for success, < 0 for error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_diff_get_perfdata(
            ref git_diff_perfdata _out,
            git_diff* diff);

        /// <summary>
        ///  Get performance data for diffs from a git_status_list
        /// </summary>
        /// <param name="_out">
        ///  Structure to be filled with diff performance data
        /// </param>
        /// <param name="status">Diff to read performance data from</param>
        /// <returns>0 for success, < 0 for error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_status_list_get_perfdata(
            ref git_diff_perfdata _out,
            git_status_list* status);
    }
}
