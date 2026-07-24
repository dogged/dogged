using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Parent type for `git_cert_hostkey` and `git_cert_x509`.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_cert
    {
        /// <summary>
        ///  Type of certificate. A `GIT_CERT_` value.
        /// </summary>
        public git_cert_t cert_type;
    }

    /// <summary>
    ///  Type of host certificate structure that is passed to the check
    ///  callback
    /// </summary>
    public enum git_cert_t
    {
        /// <summary>
        ///  No information about the certificate is available. This may
        ///  happen when using curl.
        /// </summary>
        GIT_CERT_NONE,

        /// <summary>
        ///  The `data` argument to the callback will be a pointer to the
        ///  DER-encoded data.
        /// </summary>
        GIT_CERT_X509,

        /// <summary>
        ///  The `data` argument to the callback will be a pointer to a
        ///  `git_cert_hostkey` structure.
        /// </summary>
        GIT_CERT_HOSTKEY_LIBSSH2,

        /// <summary>
        ///  The `data` argument to the callback will be a pointer to a
        ///  `git_strarray` with `name:content` strings containing
        ///  information about the certificate. This is used when using
        ///  curl.
        /// </summary>
        GIT_CERT_STRARRAY,
    }

    /// <summary>
    ///  Callback for the user's custom certificate checks.
    /// </summary>
    /// <param name="cert">The host certificate</param>
    /// <param name="valid">
    ///  Whether the libgit2 checks (OpenSSL or WinHTTP) think this
    ///  certificate is valid
    /// </param>
    /// <param name="host">Hostname of the host libgit2 connected to</param>
    /// <param name="payload">Payload provided by the caller</param>
    /// <returns>
    ///  0 to proceed with the connection, < 0 to fail the connection or
    ///  > 0 to indicate that the callback refused to act and that the
    ///  existing validity determination should be honored
    /// </returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_transport_certificate_check_cb(
        ref git_cert cert,
        int valid,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string host,
        IntPtr payload);

    /// <summary>
    ///  Type of SSH host fingerprint
    /// </summary>
    public enum git_cert_ssh_t
    {
        /// <summary>
        ///  MD5 is available
        /// </summary>
        GIT_CERT_SSH_MD5 = 1,

        /// <summary>
        ///  SHA-1 is available
        /// </summary>
        GIT_CERT_SSH_SHA1 = 2,

        /// <summary>
        ///  SHA-256 is available
        /// </summary>
        GIT_CERT_SSH_SHA256 = 4,

        /// <summary>
        ///  Raw hostkey is available
        /// </summary>
        GIT_CERT_SSH_RAW = 8,
    }

    public enum git_cert_ssh_raw_type_t
    {
        /// <summary>
        ///  The raw key is of an unknown type.
        /// </summary>
        GIT_CERT_SSH_RAW_TYPE_UNKNOWN = 0,

        /// <summary>
        ///  The raw key is an RSA key.
        /// </summary>
        GIT_CERT_SSH_RAW_TYPE_RSA = 1,

        /// <summary>
        ///  The raw key is a DSS key.
        /// </summary>
        GIT_CERT_SSH_RAW_TYPE_DSS = 2,

        /// <summary>
        ///  The raw key is a ECDSA 256 key.
        /// </summary>
        GIT_CERT_SSH_RAW_TYPE_KEY_ECDSA_256 = 3,

        /// <summary>
        ///  The raw key is a ECDSA 384 key.
        /// </summary>
        GIT_CERT_SSH_RAW_TYPE_KEY_ECDSA_384 = 4,

        /// <summary>
        ///  The raw key is a ECDSA 521 key.
        /// </summary>
        GIT_CERT_SSH_RAW_TYPE_KEY_ECDSA_521 = 5,

        /// <summary>
        ///  The raw key is a ED25519 key.
        /// </summary>
        GIT_CERT_SSH_RAW_TYPE_KEY_ED25519 = 6,
    }

    /// <summary>
    ///  Hostkey information taken from libssh2
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_cert_hostkey
    {
        /// <summary>
        ///  The parent cert
        /// </summary>
        public git_cert parent;

        /// <summary>
        ///  A bitmask containing the available fields.
        /// </summary>
        public git_cert_ssh_t type;

        /// <summary>
        ///  Hostkey hash. If `type` has `GIT_CERT_SSH_MD5` set, this
        ///  will have the MD5 hash of the hostkey.
        /// </summary>
        public byte* hash_md5;

        /// <summary>
        ///  Hostkey hash. If `type` has `GIT_CERT_SSH_SHA1` set, this
        ///  will have the SHA-1 hash of the hostkey.
        /// </summary>
        public byte* hash_sha1;

        /// <summary>
        ///  Hostkey hash. If `type` has `GIT_CERT_SSH_SHA256` set, this
        ///  will have the SHA-256 hash of the hostkey.
        /// </summary>
        public byte* hash_sha256;

        /// <summary>
        ///  Raw hostkey type. If `type` has `GIT_CERT_SSH_RAW` set, this
        ///  will have the type of the raw hostkey.
        /// </summary>
        public git_cert_ssh_raw_type_t raw_type;

        /// <summary>
        ///  Pointer to the raw hostkey. If `type` has `GIT_CERT_SSH_RAW`
        ///  set, this will have the raw contents of the hostkey.
        /// </summary>
        public IntPtr hostkey;

        /// <summary>
        ///  Raw hostkey length. If `type` has `GIT_CERT_SSH_RAW` set,
        ///  this will have the length of the raw contents of the
        ///  hostkey.
        /// </summary>
        public UIntPtr hostkey_len;
    }

    /// <summary>
    ///  X.509 certificate information
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_cert_x509
    {
        /// <summary>
        ///  The parent cert
        /// </summary>
        public git_cert parent;

        /// <summary>
        ///  Pointer to the X.509 certificate data
        /// </summary>
        public IntPtr data;

        /// <summary>
        ///  Length of the memory block pointed to by `data`.
        /// </summary>
        public UIntPtr len;
    }
}
