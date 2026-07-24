using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  The base structure for all credential types
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_credential
    {
        /// <summary>
        ///  A type of credential
        /// </summary>
        public git_credential_t credtype;

        /// <summary>
        ///  The deallocator for this type of credentials
        /// </summary>
        public IntPtr free;
    }

    /// <summary>
    ///  A plaintext username and password
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_credential_userpass_plaintext
    {
        /// <summary>
        ///  The parent credential
        /// </summary>
        public git_credential parent;

        /// <summary>
        ///  The username to authenticate as
        /// </summary>
        public IntPtr username;

        /// <summary>
        ///  The password to use
        /// </summary>
        public IntPtr password;
    }

    /// <summary>
    ///  Username-only credential information
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_credential_username
    {
        /// <summary>
        ///  The parent credential
        /// </summary>
        public git_credential parent;

        /// <summary>
        ///  The username to authenticate as
        /// </summary>
        public byte* username;
    }

    /// <summary>
    ///  A ssh key from disk
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_credential_ssh_key
    {
        /// <summary>
        ///  The parent credential
        /// </summary>
        public git_credential parent;

        /// <summary>
        ///  The username to authenticate as
        /// </summary>
        public IntPtr username;

        /// <summary>
        ///  The path to a public key
        /// </summary>
        public IntPtr publickey;

        /// <summary>
        ///  The path to a private key
        /// </summary>
        public IntPtr privatekey;

        /// <summary>
        ///  Passphrase to decrypt the private key
        /// </summary>
        public IntPtr passphrase;
    }

    /// <summary>
    ///  Keyboard-interactive based ssh authentication
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_credential_ssh_interactive
    {
        /// <summary>
        ///  The parent credential
        /// </summary>
        public git_credential parent;

        /// <summary>
        ///  The username to authenticate as
        /// </summary>
        public IntPtr username;

        /// <summary>
        ///  Callback used for authentication.
        /// </summary>
        public git_credential_ssh_interactive_cb prompt_callback;

        /// <summary>
        ///  Payload passed to prompt_callback
        /// </summary>
        public IntPtr payload;
    }

    /// <summary>
    ///  A key with a custom signature function
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_credential_ssh_custom
    {
        /// <summary>
        ///  The parent credential
        /// </summary>
        public git_credential parent;

        /// <summary>
        ///  The username to authenticate as
        /// </summary>
        public IntPtr username;

        /// <summary>
        ///  The public key data
        /// </summary>
        public IntPtr publickey;

        /// <summary>
        ///  Length of the public key
        /// </summary>
        public UIntPtr publickey_len;

        /// <summary>
        ///  Callback used to sign the data.
        /// </summary>
        public git_credential_sign_cb sign_callback;

        /// <summary>
        ///  Payload passed to prompt_callback
        /// </summary>
        public IntPtr payload;
    }
}
