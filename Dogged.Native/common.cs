using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Configurable features of libgit2; either optional settings (like
    ///  threading), or features that can be enabled by one of a number
    ///  of different backend "providers" (like HTTPS, which can be
    ///  provided by OpenSSL, mbedTLS, or system libraries).
    /// </summary>
    public enum git_feature_t
    {
        /// <summary>
        ///  libgit2 is thread-aware and can be used from multiple
        ///  threads (as described in the documentation).
        /// </summary>
        GIT_FEATURE_THREADS = 1,

        /// <summary>
        ///  HTTPS remotes
        /// </summary>
        GIT_FEATURE_HTTPS = 2,

        /// <summary>
        ///  SSH remotes
        /// </summary>
        GIT_FEATURE_SSH = 4,

        /// <summary>
        ///  Sub-second resolution in index timestamps
        /// </summary>
        GIT_FEATURE_NSEC = 8,

        /// <summary>
        ///  HTTP parsing; always available
        /// </summary>
        GIT_FEATURE_HTTP_PARSER = 16,

        /// <summary>
        ///  Regular expression support; always available
        /// </summary>
        GIT_FEATURE_REGEX = 32,

        /// <summary>
        ///  Internationalization support for filename translation
        /// </summary>
        GIT_FEATURE_I18N = 64,

        /// <summary>
        ///  NTLM support over HTTPS
        /// </summary>
        GIT_FEATURE_AUTH_NTLM = 128,

        /// <summary>
        ///  Kerberos (SPNEGO) authentication support over HTTPS
        /// </summary>
        GIT_FEATURE_AUTH_NEGOTIATE = 256,

        /// <summary>
        ///  zlib support; always available
        /// </summary>
        GIT_FEATURE_COMPRESSION = 512,

        /// <summary>
        ///  SHA1 object support; always available
        /// </summary>
        GIT_FEATURE_SHA1 = 1024,

        /// <summary>
        ///  SHA256 object support
        /// </summary>
        GIT_FEATURE_SHA256 = 2048,

        /// <summary>
        ///  HTTP remotes
        /// </summary>
        GIT_FEATURE_HTTP = 4096,
    }

    /// <summary>
    ///  Global library options
    ///
    ///  <para>
    ///   These are used to select which global option to set or get
    ///   and are used in `git_libgit2_opts()`.
    ///  </para>
    /// </summary>
    public enum git_libgit2_opt_t
    {
        GIT_OPT_GET_MWINDOW_SIZE,
        GIT_OPT_SET_MWINDOW_SIZE,
        GIT_OPT_GET_MWINDOW_MAPPED_LIMIT,
        GIT_OPT_SET_MWINDOW_MAPPED_LIMIT,
        GIT_OPT_GET_SEARCH_PATH,
        GIT_OPT_SET_SEARCH_PATH,
        GIT_OPT_SET_CACHE_OBJECT_LIMIT,
        GIT_OPT_SET_CACHE_MAX_SIZE,
        GIT_OPT_ENABLE_CACHING,
        GIT_OPT_GET_CACHED_MEMORY,
        GIT_OPT_GET_TEMPLATE_PATH,
        GIT_OPT_SET_TEMPLATE_PATH,
        GIT_OPT_SET_SSL_CERT_LOCATIONS,
        GIT_OPT_SET_USER_AGENT,
        GIT_OPT_ENABLE_STRICT_OBJECT_CREATION,
        GIT_OPT_ENABLE_STRICT_SYMBOLIC_REF_CREATION,
        GIT_OPT_SET_SSL_CIPHERS,
        GIT_OPT_GET_USER_AGENT,
        GIT_OPT_ENABLE_OFS_DELTA,
        GIT_OPT_ENABLE_FSYNC_GITDIR,
        GIT_OPT_GET_WINDOWS_SHAREMODE,
        GIT_OPT_SET_WINDOWS_SHAREMODE,
        GIT_OPT_ENABLE_STRICT_HASH_VERIFICATION,
        GIT_OPT_SET_ALLOCATOR,
        GIT_OPT_ENABLE_UNSAVED_INDEX_SAFETY,
        GIT_OPT_GET_PACK_MAX_OBJECTS,
        GIT_OPT_SET_PACK_MAX_OBJECTS,
        GIT_OPT_DISABLE_PACK_KEEP_FILE_CHECKS,
        GIT_OPT_ENABLE_HTTP_EXPECT_CONTINUE,
        GIT_OPT_GET_MWINDOW_FILE_LIMIT,
        GIT_OPT_SET_MWINDOW_FILE_LIMIT,
        GIT_OPT_SET_ODB_PACKED_PRIORITY,
        GIT_OPT_SET_ODB_LOOSE_PRIORITY,
        GIT_OPT_GET_EXTENSIONS,
        GIT_OPT_SET_EXTENSIONS,
        GIT_OPT_GET_OWNER_VALIDATION,
        GIT_OPT_SET_OWNER_VALIDATION,
        GIT_OPT_GET_HOMEDIR,
        GIT_OPT_SET_HOMEDIR,
        GIT_OPT_SET_SERVER_CONNECT_TIMEOUT,
        GIT_OPT_GET_SERVER_CONNECT_TIMEOUT,
        GIT_OPT_SET_SERVER_TIMEOUT,
        GIT_OPT_GET_SERVER_TIMEOUT,
        GIT_OPT_SET_USER_AGENT_PRODUCT,
        GIT_OPT_GET_USER_AGENT_PRODUCT,
        GIT_OPT_ADD_SSL_X509_CERT,
        GIT_OPT_GET_PACK_MAX_OBJECT_SIZE,
        GIT_OPT_SET_PACK_MAX_OBJECT_SIZE,
    }

    /// <summary>
    ///  Build information items to query. This is the information type
    ///  passed to `git_libgit2_buildinfo` to get particular information
    ///  about the libgit2 build.
    /// </summary>
    public enum git_buildinfo_t
    {
        /// <summary>
        ///  The CPU type that libgit2 was built for.
        /// </summary>
        GIT_BUILDINFO_CPU = 1,

        /// <summary>
        ///  The commit that libgit2 was built from.
        /// </summary>
        GIT_BUILDINFO_COMMIT,
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Return the version of the libgit2 library being currently
        ///  used.
        /// </summary>
        /// <param name="major">Store the major version number</param>
        /// <param name="minor">Store the minor version number</param>
        /// <param name="rev">Store the revision (patch) number</param>
        /// <returns>0 on success or an error code on failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_libgit2_version(
            int* major,
            int* minor,
            int* rev);

        /// <summary>
        ///  Return the prerelease state of the libgit2 library currently
        ///  being used. For nightly builds during active development,
        ///  this will be "alpha". Releases may have a "beta" or release
        ///  candidate ("rc1", "rc2", etc) prerelease. For a final
        ///  release, this function returns NULL.
        /// </summary>
        /// <returns>the name of the prerelease state or NULL</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_libgit2_prerelease();

        /// <summary>
        ///  Query compile time options for libgit2.
        /// </summary>
        /// <returns>A combination of GIT_FEATURE_* values.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_feature_t git_libgit2_features();

        /// <summary>
        ///  Query the backend details for the compile-time feature in
        ///  libgit2.
        ///
        ///  <para>
        ///   This will return the "backend" for the feature, which is
        ///   useful for things like HTTPS or SSH support, that can
        ///   have multiple backends that could be compiled in.
        ///  </para>
        ///
        ///  <para>
        ///   For example, when libgit2 is compiled with dynamic
        ///   OpenSSL support, the feature backend will be
        ///   `openssl-dynamic`. The feature backend names reflect the
        ///   compilation options specified to the build system
        ///   (though in all lower case). The backend _may_ be
        ///   "builtin" for features that are provided by libgit2
        ///   itself.
        ///  </para>
        ///
        ///  <para>
        ///   If the feature is not supported by the library, this API
        ///   returns `NULL`.
        ///  </para>
        /// </summary>
        /// <param name="feature">the feature to query details for</param>
        /// <returns>
        ///  the provider details, or NULL if the feature is not
        ///  supported
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_libgit2_feature_backend(
            git_feature_t feature);

        /// <summary>
        /// Get or set a global configuration option for libgit2.
        /// </summary>
        /// <param name="option">The option value to get or set</param>
        /// <param name="...">The options to set</param>
        /// <returns>0 on success or an error code</returns>
        
        // macOS arm64 requires padding before varargs, we'll set up the
        // PInvoke methods with the extra padding as private, then use a
        // public C# method to proxy to them. This gives us a consistent
        // API between macOS arm64 and everything else.
        // see https://github.com/dotnet/runtime/issues/48796
        #if MACOS_ARM64
           [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
           private static extern unsafe int git_libgit2_opts(git_libgit2_opt_t option, IntPtr padding1, IntPtr padding2, IntPtr padding3, IntPtr padding4, IntPtr padding5, IntPtr padding6, IntPtr padding7, git_config_level_t level, ref git_buf buf);
        
           public static int git_libgit2_opts(git_libgit2_opt_t option, git_config_level_t level, ref git_buf buf)
           {
               return git_libgit2_opts(option, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, level, ref buf);
           }
        
           [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
           private static extern unsafe int git_libgit2_opts(git_libgit2_opt_t option, IntPtr padding1, IntPtr padding2, IntPtr padding3, IntPtr padding4, IntPtr padding5, IntPtr padding6, IntPtr padding7, git_config_level_t level, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path);
        
           public static int git_libgit2_opts(git_libgit2_opt_t option, git_config_level_t level, string path)
           {
               return git_libgit2_opts(option, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, level, path);
           }
        #else
           [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
           public static extern unsafe int git_libgit2_opts(git_libgit2_opt_t option, git_config_level_t level, ref git_buf buf);
        
           [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
           public static extern unsafe int git_libgit2_opts(git_libgit2_opt_t option, git_config_level_t level, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path);
        #endif


        /// <summary>
        ///  Query information about the compile-time information about
        ///  libgit2.
        /// </summary>
        /// <param name="info">the build information to query</param>
        /// <returns>the requested information, or `NULL` on error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_libgit2_buildinfo(
            git_buildinfo_t info);
    }
}
