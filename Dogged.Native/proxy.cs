using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  The type of proxy to use.
    /// </summary>
    public enum git_proxy_t
    {
        /// <summary>
        ///  Do not attempt to connect through a proxy
        ///
        ///  <para>
        ///   If built against libcurl, it itself may attempt to
        ///   connect to a proxy if the environment variables specify
        ///   it.
        ///  </para>
        /// </summary>
        GIT_PROXY_NONE,

        /// <summary>
        ///  Try to auto-detect the proxy from the git configuration.
        /// </summary>
        GIT_PROXY_AUTO,

        /// <summary>
        ///  Connect via the URL given in the options
        /// </summary>
        GIT_PROXY_SPECIFIED,
    }

    /// <summary>
    ///  Options for connecting through a proxy
    ///
    ///  <para>
    ///   Note that not all types may be supported, depending on the
    ///   platform and compilation options.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_proxy_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_PROXY_OPTIONS_VERSION = 1;


        public uint version;

        /// <summary>
        ///  The type of proxy to use, by URL, auto-detect.
        /// </summary>
        public git_proxy_t type;

        /// <summary>
        ///  The URL of the proxy.
        /// </summary>
        public IntPtr url;

        /// <summary>
        ///  This will be called if the remote host requires
        ///  authentication in order to connect to it.
        ///
        ///  <para>
        ///   Returning GIT_PASSTHROUGH will make libgit2 behave as
        ///   though this field isn't set.
        ///  </para>
        /// </summary>
        public git_credential_acquire_cb credentials;

        /// <summary>
        ///  This will be called to let the user make the final decision
        ///  of whether to allow the connection to proceed. Returns 0 to
        ///  allow the connection or a negative value to indicate an
        ///  error.
        /// </summary>
        public git_transport_certificate_check_cb certificate_check;

        /// <summary>
        ///  Payload to be provided to the credentials and certificate
        ///  check callbacks.
        /// </summary>
        public IntPtr payload;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Initialize git_proxy_options structure
        ///
        ///  <para>
        ///   Initializes a `git_proxy_options` with default values.
        ///   Equivalent to creating an instance with
        ///   `GIT_PROXY_OPTIONS_INIT`.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_proxy_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass `GIT_PROXY_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_proxy_options_init(
            ref git_proxy_options opts,
            uint version);
    }
}
