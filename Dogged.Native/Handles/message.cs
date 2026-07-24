using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
        /// <summary>
        ///  Clean's up any allocated memory in the
        ///  git_message_trailer_array filled by a call to
        ///  git_message_trailers.
        /// </summary>
        /// <param name="arr">The trailer to free.</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_message_trailer_array_free(
            IntPtr arr);
    }
}
