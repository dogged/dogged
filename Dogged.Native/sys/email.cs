using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
        /// <summary>
        ///  Create a diff for a commit in mbox format for sending via
        ///  email.
        /// </summary>
        /// <param name="_out">buffer to store the e-mail patch in</param>
        /// <param name="diff">the changes to include in the email</param>
        /// <param name="patch_idx">the patch index</param>
        /// <param name="patch_count">
        ///  the total number of patches that will be included
        /// </param>
        /// <param name="commit_id">the commit id for this change</param>
        /// <param name="summary">the commit message for this change</param>
        /// <param name="body">
        ///  optional text to include above the diffstat
        /// </param>
        /// <param name="author">the person who authored this commit</param>
        /// <param name="opts">email creation options</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_email_create_from_diff(
            ref git_buf _out,
            git_diff* diff,
            UIntPtr patch_idx,
            UIntPtr patch_count,
            ref git_oid commit_id,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string summary,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string body,
            ref git_signature author,
            ref git_email_create_options opts);
    }
}
