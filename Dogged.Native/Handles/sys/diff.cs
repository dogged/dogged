using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
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
            DiffHandle diff);

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
            StatusListHandle status);
    }
}
