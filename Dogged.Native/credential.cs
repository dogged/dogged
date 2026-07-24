using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Supported credential types
    ///
    ///  <para>
    ///   This represents the various types of authentication methods
    ///   supported by the library.
    ///  </para>
    /// </summary>
    public enum git_credential_t
    {
        /// <summary>
        ///  A vanilla user/password request
        /// </summary>
        GIT_CREDENTIAL_USERPASS_PLAINTEXT = 1,

        /// <summary>
        ///  An SSH key-based authentication request
        /// </summary>
        GIT_CREDENTIAL_SSH_KEY = 2,

        /// <summary>
        ///  An SSH key-based authentication request, with a custom
        ///  signature
        /// </summary>
        GIT_CREDENTIAL_SSH_CUSTOM = 4,

        /// <summary>
        ///  An NTLM/Negotiate-based authentication request.
        /// </summary>
        GIT_CREDENTIAL_DEFAULT = 8,

        /// <summary>
        ///  An SSH interactive authentication request
        /// </summary>
        GIT_CREDENTIAL_SSH_INTERACTIVE = 16,

        /// <summary>
        ///  Username-only authentication request
        ///
        ///  <para>
        ///   Used as a pre-authentication step if the underlying
        ///   transport (eg. SSH, with no username in its URL) does
        ///   not know which username to use.
        ///  </para>
        /// </summary>
        GIT_CREDENTIAL_USERNAME = 32,

        /// <summary>
        ///  An SSH key-based authentication request
        ///
        ///  <para>
        ///   Allows credentials to be read from memory instead of
        ///   files. Note that because of differences in crypto
        ///   backend support, it might not be functional.
        ///  </para>
        /// </summary>
        GIT_CREDENTIAL_SSH_MEMORY = 64,
    }

    /// <summary>
    ///  A key for NTLM/Kerberos "default" credentials
    /// </summary>
    public struct git_credential_default
    {
    }

    /// <summary>
    ///  Credential acquisition callback.
    ///
    ///  <para>
    ///   This callback is usually involved any time another system
    ///   might need authentication. As such, you are expected to
    ///   provide a valid git_credential object back, depending on
    ///   allowed_types (a git_credential_t bitmask).
    ///  </para>
    ///
    ///  <para>
    ///   Note that most authentication details are your
    ///   responsibility - this callback will be called until the
    ///   authentication succeeds, or you report an error. As such,
    ///   it's easy to get in a loop if you fail to stop providing the
    ///   same incorrect credentials.
    ///  </para>
    /// </summary>
    /// <param name="_out">The newly created credential object.</param>
    /// <param name="url">
    ///  The resource for which we are demanding a credential.
    /// </param>
    /// <param name="username_from_url">
    ///  The username that was embedded in a "user\@host" remote url, or
    ///  NULL if not included.
    /// </param>
    /// <param name="allowed_types">
    ///  A bitmask stating which credential types are OK to return.
    /// </param>
    /// <param name="payload">
    ///  The payload provided when specifying this callback.
    /// </param>
    /// <returns>
    ///  0 for success, < 0 to indicate an error, > 0 to indicate no
    ///  credential was acquired
    /// </returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_credential_acquire_cb(
        out git_credential* _out,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string url,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string username_from_url,
        uint allowed_types,
        IntPtr payload);

    public struct LIBSSH2_SESSION
    {
    }

    public struct LIBSSH2_USERAUTH_KBDINT_PROMPT
    {
    }

    public struct LIBSSH2_USERAUTH_KBDINT_RESPONSE
    {
    }

    /// <summary>
    ///  Callback for interactive SSH credentials.
    /// </summary>
    /// <param name="name">the name</param>
    /// <param name="name_len">the length of the name</param>
    /// <param name="instruction">the authentication instruction</param>
    /// <param name="instruction_len">the length of the instruction</param>
    /// <param name="num_prompts">the number of prompts</param>
    /// <param name="prompts">the prompts</param>
    /// <param name="responses">the responses</param>
    /// <param name="_abstract">the abstract</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void git_credential_ssh_interactive_cb(
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
        int name_len,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string instruction,
        int instruction_len,
        int num_prompts,
        LIBSSH2_USERAUTH_KBDINT_PROMPT* prompts,
        LIBSSH2_USERAUTH_KBDINT_RESPONSE* responses,
        IntPtr _abstract);

    /// <summary>
    ///  Callback for credential signing.
    /// </summary>
    /// <param name="session">the libssh2 session</param>
    /// <param name="sig">the signature</param>
    /// <param name="sig_len">the length of the signature</param>
    /// <param name="data">the data</param>
    /// <param name="data_len">the length of the data</param>
    /// <param name="_abstract">the abstract</param>
    /// <returns>
    ///  0 for success, < 0 to indicate an error, > 0 to indicate no
    ///  credential was acquired
    /// </returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_credential_sign_cb(
        LIBSSH2_SESSION* session,
        out byte* sig,
        out UIntPtr sig_len,
        byte* data,
        UIntPtr data_len,
        IntPtr _abstract);

    public static partial class libgit2
    {
        /// <summary>
        ///  Free a credential.
        ///
        ///  <para>
        ///   This is only necessary if you own the object; that is,
        ///   if you are a transport.
        ///  </para>
        /// </summary>
        /// <param name="cred">the object to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_credential_free(
            ref git_credential cred);

        /// <summary>
        ///  Check whether a credential object contains username
        ///  information.
        /// </summary>
        /// <param name="cred">object to check</param>
        /// <returns>
        ///  1 if the credential object has non-NULL username, 0
        ///  otherwise
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_credential_has_username(
            ref git_credential cred);

        /// <summary>
        ///  Return the username associated with a credential object.
        /// </summary>
        /// <param name="cred">object to check</param>
        /// <returns>
        ///  the credential username, or NULL if not applicable
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_credential_get_username(
            ref git_credential cred);

        /// <summary>
        ///  Create a new plain-text username and password credential
        ///  object. The supplied credential parameter will be internally
        ///  duplicated.
        /// </summary>
        /// <param name="_out">The newly created credential object.</param>
        /// <param name="username">The username of the credential.</param>
        /// <param name="password">The password of the credential.</param>
        /// <returns>0 for success or an error code for failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_credential_userpass_plaintext_new(
            out git_credential* _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string username,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string password);

        /// <summary>
        ///  Create a "default" credential usable for Negotiate
        ///  mechanisms like NTLM or Kerberos authentication.
        /// </summary>
        /// <param name="_out">The newly created credential object.</param>
        /// <returns>0 for success or an error code for failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_credential_default_new(
            out git_credential* _out);

        /// <summary>
        ///  Create a credential to specify a username.
        ///
        ///  <para>
        ///   This is used with ssh authentication to query for the
        ///   username if none is specified in the url.
        ///  </para>
        /// </summary>
        /// <param name="_out">The newly created credential object.</param>
        /// <param name="username">The username to authenticate with</param>
        /// <returns>0 for success or an error code for failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_credential_username_new(
            out git_credential* _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string username);

        /// <summary>
        ///  Create a new passphrase-protected ssh key credential object.
        ///  The supplied credential parameter will be internally
        ///  duplicated.
        /// </summary>
        /// <param name="_out">The newly created credential object.</param>
        /// <param name="username">username to use to authenticate</param>
        /// <param name="publickey">
        ///  The path to the public key of the credential.
        /// </param>
        /// <param name="privatekey">
        ///  The path to the private key of the credential.
        /// </param>
        /// <param name="passphrase">
        ///  The passphrase of the credential.
        /// </param>
        /// <returns>0 for success or an error code for failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_credential_ssh_key_new(
            out git_credential* _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string username,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string publickey,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string privatekey,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string passphrase);

        /// <summary>
        ///  Create a new ssh key credential object reading the keys from
        ///  memory.
        /// </summary>
        /// <param name="_out">The newly created credential object.</param>
        /// <param name="username">username to use to authenticate.</param>
        /// <param name="publickey">
        ///  The public key of the credential.
        /// </param>
        /// <param name="privatekey">
        ///  The private key of the credential.
        /// </param>
        /// <param name="passphrase">
        ///  The passphrase of the credential.
        /// </param>
        /// <returns>0 for success or an error code for failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_credential_ssh_key_memory_new(
            out git_credential* _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string username,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string publickey,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string privatekey,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string passphrase);

        /// <summary>
        ///  Create a new ssh keyboard-interactive based credential
        ///  object. The supplied credential parameter will be internally
        ///  duplicated.
        /// </summary>
        /// <param name="_out">The newly created credential object.</param>
        /// <param name="username">Username to use to authenticate.</param>
        /// <param name="prompt_callback">
        ///  The callback method used for prompts.
        /// </param>
        /// <param name="payload">
        ///  Additional data to pass to the callback.
        /// </param>
        /// <returns>0 for success or an error code for failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_credential_ssh_interactive_new(
            out git_credential* _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string username,
            git_credential_ssh_interactive_cb prompt_callback,
            IntPtr payload);

        /// <summary>
        ///  Create a new ssh key credential object used for querying an
        ///  ssh-agent. The supplied credential parameter will be
        ///  internally duplicated.
        /// </summary>
        /// <param name="_out">The newly created credential object.</param>
        /// <param name="username">username to use to authenticate</param>
        /// <returns>0 for success or an error code for failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_credential_ssh_key_from_agent(
            out git_credential* _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string username);

        /// <summary>
        ///  Create an ssh key credential with a custom signing function.
        ///
        ///  <para>
        ///   This lets you use your own function to sign the
        ///   challenge.
        ///  </para>
        ///
        ///  <para>
        ///   This function and its credential type is provided for
        ///   completeness and wraps `libssh2_userauth_publickey()`,
        ///   which is undocumented.
        ///  </para>
        ///
        ///  <para>
        ///   The supplied credential parameter will be internally
        ///   duplicated.
        ///  </para>
        /// </summary>
        /// <param name="_out">The newly created credential object.</param>
        /// <param name="username">username to use to authenticate</param>
        /// <param name="publickey">The bytes of the public key.</param>
        /// <param name="publickey_len">
        ///  The length of the public key in bytes.
        /// </param>
        /// <param name="sign_callback">
        ///  The callback method to sign the data during the challenge.
        /// </param>
        /// <param name="payload">
        ///  Additional data to pass to the callback.
        /// </param>
        /// <returns>0 for success or an error code for failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_credential_ssh_custom_new(
            out git_credential* _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string username,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string publickey,
            UIntPtr publickey_len,
            git_credential_sign_cb sign_callback,
            IntPtr payload);
    }
}
