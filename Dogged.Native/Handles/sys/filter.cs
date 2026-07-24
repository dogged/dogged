using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
        /// <summary>
        ///  Create a new empty filter list
        ///
        ///  <para>
        ///   Normally you won't use this because
        ///   `git_filter_list_load` will create the filter list for
        ///   you, but you can use this in combination with the
        ///   `git_filter_lookup` and `git_filter_list_push` functions
        ///   to assemble your own chains of filters.
        ///  </para>
        /// </summary>
        /// <param name="_out">the filter list</param>
        /// <param name="repo">
        ///  the repository to use for configuration
        /// </param>
        /// <param name="mode">the filter mode (direction)</param>
        /// <param name="options">the options</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_filter_list_new(
            out FilterListHandle _out,
            RepositoryHandle repo,
            git_filter_mode_t mode,
            uint options);

        /// <summary>
        ///  Add a filter to a filter list with the given payload.
        ///
        ///  <para>
        ///   Normally you won't have to do this because the filter
        ///   list is created by calling the "check" function on
        ///   registered filters when the filter attributes are set,
        ///   but this does allow more direct manipulation of filter
        ///   lists when desired.
        ///  </para>
        ///
        ///  <para>
        ///   Note that normally the "check" function can set up a
        ///   payload for the filter. Using this function, you can
        ///   either pass in a payload if you know the expected
        ///   payload format, or you can pass NULL. Some filters may
        ///   fail with a NULL payload. Good luck!
        ///  </para>
        /// </summary>
        /// <param name="fl">the filter list</param>
        /// <param name="filter">the filter to push</param>
        /// <param name="payload">the payload for the filter</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_filter_list_push(
            FilterListHandle fl,
            ref git_filter filter,
            IntPtr payload);

        /// <summary>
        ///  Look up how many filters are in the list
        ///
        ///  <para>
        ///   We will attempt to apply all of these filters to any
        ///   data passed in, but note that the filter apply action
        ///   still has the option of skipping data that is passed in
        ///   (for example, the CRLF filter will skip data that
        ///   appears to be binary).
        ///  </para>
        /// </summary>
        /// <param name="fl">A filter list</param>
        /// <returns>The number of filters in the list</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_filter_list_length(
            FilterListHandle fl);
    }
}
