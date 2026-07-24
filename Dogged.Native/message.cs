using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Represents a single git message trailer.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_message_trailer
    {
        public IntPtr key;

        public IntPtr value;
    }

    /// <summary>
    ///  Represents an array of git message trailers.
    ///
    ///  <para>
    ///   Struct members under the private comment are private,
    ///   subject to change and should not be used by callers.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_message_trailer_array
    {
        public IntPtr trailers;

        public UIntPtr count;

        public IntPtr _trailer_block;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Clean up excess whitespace and make sure there is a trailing
        ///  newline in the message.
        ///
        ///  <para>
        ///   Optionally, it can remove lines which start with the
        ///   comment character.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  The user-allocated git_buf which will be filled with the
        ///  cleaned up message.
        /// </param>
        /// <param name="message">The message to be prettified.</param>
        /// <param name="strip_comments">
        ///  Non-zero to remove comment lines, 0 to leave them in.
        /// </param>
        /// <param name="comment_char">
        ///  Comment character. Lines starting with this character are
        ///  considered to be comments and removed if `strip_comments` is
        ///  non-zero.
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_message_prettify(
            ref git_buf _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string message,
            int strip_comments,
            char comment_char);

        /// <summary>
        ///  Parse trailers out of a message, filling the array pointed
        ///  to by +arr+.
        ///
        ///  <para>
        ///   Trailers are key/value pairs in the last paragraph of a
        ///   message, not including any patches or conflicts that may
        ///   be present.
        ///  </para>
        /// </summary>
        /// <param name="arr">
        ///  A pre-allocated git_message_trailer_array struct to be
        ///  filled in with any trailers found during parsing.
        /// </param>
        /// <param name="message">The message to be parsed</param>
        /// <returns>0 on success, or non-zero on error.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_message_trailers(
            ref git_message_trailer_array arr,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string message);

        /// <summary>
        ///  Clean's up any allocated memory in the
        ///  git_message_trailer_array filled by a call to
        ///  git_message_trailers.
        /// </summary>
        /// <param name="arr">The trailer to free.</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_message_trailer_array_free(
            ref git_message_trailer_array arr);
    }
}
