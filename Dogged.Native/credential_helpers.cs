using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Payload for git_credential_userpass_plaintext.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_credential_userpass_payload
    {
        public IntPtr username;

        public IntPtr password;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Stock callback usable as a git_credential_acquire_cb. This
        ///  calls git_cred_userpass_plaintext_new unless the protocol
        ///  has not specified `GIT_CREDENTIAL_USERPASS_PLAINTEXT` as an
        ///  allowed type.
        /// </summary>
        /// <param name="_out">The newly created credential object.</param>
        /// <param name="url">
        ///  The resource for which we are demanding a credential.
        /// </param>
        /// <param name="user_from_url">
        ///  The username that was embedded in a "user\@host" remote url,
        ///  or NULL if not included.
        /// </param>
        /// <param name="allowed_types">
        ///  A bitmask stating which credential types are OK to return.
        /// </param>
        /// <param name="payload">
        ///  The payload provided when specifying this callback. (This is
        ///  interpreted as a `git_credential_userpass_payload*`.)
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_credential_userpass(
            out git_credential* _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string url,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string user_from_url,
            uint allowed_types,
            IntPtr payload);
    }
}
