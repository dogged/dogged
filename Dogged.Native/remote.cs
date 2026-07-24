using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Git's idea of a remote repository. A remote can be anonymous (in
    ///  which case it does not have backing configuration entries).
    /// </summary>
    public struct git_remote
    {
    }

    /// <summary>
    ///  Remote redirection settings; whether redirects to another host
    ///  are permitted. By default, git will follow a redirect on the
    ///  initial request (`/info/refs`), but not subsequent requests.
    /// </summary>
    public enum git_remote_redirect_t
    {
        /// <summary>
        ///  Do not follow any off-site redirects at any stage of the
        ///  fetch or push.
        /// </summary>
        GIT_REMOTE_REDIRECT_NONE = 1,

        /// <summary>
        ///  Allow off-site redirects only upon the initial request. This
        ///  is the default.
        /// </summary>
        GIT_REMOTE_REDIRECT_INITIAL = 2,

        /// <summary>
        ///  Allow redirects at any stage in the fetch or push.
        /// </summary>
        GIT_REMOTE_REDIRECT_ALL = 4,
    }

    /// <summary>
    ///  Remote creation options flags
    /// </summary>
    public enum git_remote_create_flags
    {
        /// <summary>
        ///  Ignore the repository apply.insteadOf configuration
        /// </summary>
        GIT_REMOTE_CREATE_SKIP_INSTEADOF = 1,

        /// <summary>
        ///  Don't build a fetchspec from the name if none is set
        /// </summary>
        GIT_REMOTE_CREATE_SKIP_DEFAULT_FETCHSPEC = 2,
    }

    /// <summary>
    ///  How to handle reference updates.
    /// </summary>
    public enum git_remote_update_flags
    {
        GIT_REMOTE_UPDATE_FETCHHEAD = 1,
        GIT_REMOTE_UPDATE_REPORT_UNCHANGED = 2,
    }

    /// <summary>
    ///  Remote creation options structure
    ///
    ///  <para>
    ///   Initialize with `GIT_REMOTE_CREATE_OPTIONS_INIT`.
    ///   Alternatively, you can use `git_remote_create_options_init`.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_remote_create_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_REMOTE_CREATE_OPTIONS_VERSION = 1;


        public uint version;

        /// <summary>
        ///  The repository that should own the remote. Setting this to
        ///  NULL results in a detached remote.
        /// </summary>
        public IntPtr repository;

        /// <summary>
        ///  The remote's name. Setting this to NULL results in an
        ///  in-memory/anonymous remote.
        /// </summary>
        public IntPtr name;

        /// <summary>
        ///  The fetchspec the remote should use.
        /// </summary>
        public IntPtr fetchspec;

        /// <summary>
        ///  Additional flags for the remote. See
        ///  git_remote_create_flags.
        /// </summary>
        public uint flags;
    }

    /// <summary>
    ///  Argument to the completion callback which tells it which
    ///  operation finished.
    /// </summary>
    public enum git_remote_completion_t
    {
        GIT_REMOTE_COMPLETION_DOWNLOAD,
        GIT_REMOTE_COMPLETION_INDEXING,
        GIT_REMOTE_COMPLETION_ERROR,
    }

    /// <summary>
    ///  Push network progress notification callback.
    /// </summary>
    /// <param name="current">The number of objects pushed so far</param>
    /// <param name="total">The total number of objects to push</param>
    /// <param name="bytes">The number of bytes pushed</param>
    /// <param name="payload">The user-specified payload callback</param>
    /// <returns>0 or an error code to stop the transfer</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_push_transfer_progress_cb(
        uint current,
        uint total,
        UIntPtr bytes,
        IntPtr payload);

    /// <summary>
    ///  Represents an update which will be performed on the remote
    ///  during push
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_push_update
    {
        /// <summary>
        ///  The source name of the reference
        /// </summary>
        public IntPtr src_refname;

        /// <summary>
        ///  The name of the reference to update on the server
        /// </summary>
        public IntPtr dst_refname;

        /// <summary>
        ///  The current target of the reference
        /// </summary>
        public git_oid src;

        /// <summary>
        ///  The new target for the reference
        /// </summary>
        public git_oid dst;
    }

    /// <summary>
    ///  Callback used to inform of upcoming updates.
    /// </summary>
    /// <param name="updates">
    ///  an array containing the updates which will be sent as commands
    ///  to the destination.
    /// </param>
    /// <param name="len">number of elements in `updates`</param>
    /// <param name="payload">Payload provided by the caller</param>
    /// <returns>0 or an error code to stop the push</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_push_negotiation(
        IntPtr updates,
        UIntPtr len,
        IntPtr payload);

    /// <summary>
    ///  Callback used to inform of the update status from the remote.
    ///
    ///  <para>
    ///   Called for each updated reference on push. If `status` is
    ///   not `NULL`, the update was rejected by the remote server and
    ///   `status` contains the reason given.
    ///  </para>
    /// </summary>
    /// <param name="refname">refname specifying to the remote ref</param>
    /// <param name="status">status message sent from the remote</param>
    /// <param name="data">data provided by the caller</param>
    /// <returns>0 on success, otherwise an error</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_push_update_reference_cb(
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string refname,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string status,
        IntPtr data);

    /// <summary>
    ///  Callback to resolve URLs before connecting to remote
    ///
    ///  <para>
    ///   If you return GIT_PASSTHROUGH, you don't need to write
    ///   anything to url_resolved.
    ///  </para>
    /// </summary>
    /// <param name="url_resolved">
    ///  The buffer to write the resolved URL to
    /// </param>
    /// <param name="url">The URL to resolve</param>
    /// <param name="direction">
    ///  GIT_DIRECTION_FETCH or GIT_DIRECTION_PUSH
    /// </param>
    /// <param name="payload">Payload provided by the caller</param>
    /// <returns>0 on success, GIT_PASSTHROUGH or an error</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_url_resolve_cb(
        ref git_buf url_resolved,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string url,
        int direction,
        IntPtr payload);

    /// <summary>
    ///  Callback invoked immediately before we attempt to connect to the
    ///  given url. Callers may change the URL before the connection by
    ///  calling `git_remote_set_instance_url` in the callback.
    /// </summary>
    /// <param name="remote">The remote to be connected</param>
    /// <param name="direction">
    ///  GIT_DIRECTION_FETCH or GIT_DIRECTION_PUSH
    /// </param>
    /// <param name="payload">Payload provided by the caller</param>
    /// <returns>0 on success, or an error</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_remote_ready_cb(
        git_remote* remote,
        int direction,
        IntPtr payload);

    /// <summary>
    ///  The callback settings structure
    ///
    ///  <para>
    ///   Set the callbacks to be called by the remote when informing
    ///   the user about the progress of the network operations.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_remote_callbacks
    {
        /// <summary>
        ///  The version
        /// </summary>
        public uint version;

        /// <summary>
        ///  Textual progress from the remote. Text send over the
        ///  progress side-band will be passed to this function (this is
        ///  the 'counting objects' output).
        /// </summary>
        public git_transport_message_cb sideband_progress;

        /// <summary>
        ///  Completion is called when different parts of the download
        ///  process are done (currently unused).
        /// </summary>
        public IntPtr completion;

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
        ///  During the download of new data, this will be regularly
        ///  called with the current count of progress done by the
        ///  indexer.
        /// </summary>
        public git_indexer_progress_cb transfer_progress;

        /// <summary>
        ///  Deprecated callback for reference updates, callers should
        ///  set `update_refs` instead. This is retained for backward
        ///  compatibility; if you specify both `update_refs` and
        ///  `update_tips`, then only the `update_refs` function will be
        ///  called.
        /// </summary>
        public IntPtr update_tips;

        /// <summary>
        ///  Function to call with progress information during pack
        ///  building. Be aware that this is called inline with pack
        ///  building operations, so performance may be affected.
        /// </summary>
        public git_packbuilder_progress pack_progress;

        /// <summary>
        ///  Function to call with progress information during the upload
        ///  portion of a push. Be aware that this is called inline with
        ///  pack building operations, so performance may be affected.
        /// </summary>
        public git_push_transfer_progress_cb push_transfer_progress;

        /// <summary>
        ///  See documentation of git_push_update_reference_cb
        /// </summary>
        public git_push_update_reference_cb push_update_reference;

        /// <summary>
        ///  Called once between the negotiation step and the upload. It
        ///  provides information about what updates will be performed.
        /// </summary>
        public git_push_negotiation push_negotiation;

        /// <summary>
        ///  Create the transport to use for this operation. Leave NULL
        ///  to auto-detect.
        /// </summary>
        public git_transport_cb transport;

        /// <summary>
        ///  Callback when the remote is ready to connect.
        /// </summary>
        public git_remote_ready_cb remote_ready;

        /// <summary>
        ///  This will be passed to each of the callbacks in this struct
        ///  as the last parameter.
        /// </summary>
        public IntPtr payload;

        /// <summary>
        ///  Resolve URL before connecting to remote. The returned URL
        ///  will be used to connect to the remote instead.
        ///
        ///  <para>
        ///   This callback is deprecated; users should use
        ///   git_remote_ready_cb and configure the instance URL
        ///   instead.
        ///  </para>
        /// </summary>
        public git_url_resolve_cb resolve_url;

        /// <summary>
        ///  Each time a reference is updated locally, this function will
        ///  be called with information about it. This should be
        ///  preferred over the `update_tips` callback in this structure.
        /// </summary>
        public IntPtr update_refs;
    }

    /// <summary>
    ///  Acceptable prune settings when fetching
    /// </summary>
    public enum git_fetch_prune_t
    {
        /// <summary>
        ///  Use the setting from the configuration
        /// </summary>
        GIT_FETCH_PRUNE_UNSPECIFIED,

        /// <summary>
        ///  Force pruning on
        /// </summary>
        GIT_FETCH_PRUNE,

        /// <summary>
        ///  Force pruning off
        /// </summary>
        GIT_FETCH_NO_PRUNE,
    }

    /// <summary>
    ///  Automatic tag following option
    ///
    ///  <para>
    ///   Lets us select the --tags option to use.
    ///  </para>
    /// </summary>
    public enum git_remote_autotag_option_t
    {
        /// <summary>
        ///  Use the setting from the configuration.
        /// </summary>
        GIT_REMOTE_DOWNLOAD_TAGS_UNSPECIFIED = 0,

        /// <summary>
        ///  Ask the server for tags pointing to objects we're already
        ///  downloading.
        /// </summary>
        GIT_REMOTE_DOWNLOAD_TAGS_AUTO,

        /// <summary>
        ///  Don't ask for any tags beyond the refspecs.
        /// </summary>
        GIT_REMOTE_DOWNLOAD_TAGS_NONE,

        /// <summary>
        ///  Ask for all the tags.
        /// </summary>
        GIT_REMOTE_DOWNLOAD_TAGS_ALL,
    }

    /// <summary>
    ///  Constants for fetch depth (shallowness of fetch).
    /// </summary>
    public enum git_fetch_depth_t
    {
        /// <summary>
        ///  The fetch is "full" (not shallow). This is the default.
        /// </summary>
        GIT_FETCH_DEPTH_FULL = 0,

        /// <summary>
        ///  The fetch should "unshallow" and fetch missing data.
        /// </summary>
        GIT_FETCH_DEPTH_UNSHALLOW = 2147483647,
    }

    /// <summary>
    ///  Fetch options structure.
    ///
    ///  <para>
    ///   Initialize with `GIT_FETCH_OPTIONS_INIT` macro to correctly
    ///   set the `version` field. e.g. git_fetch_options opts =
    ///   GIT_FETCH_OPTIONS_INIT;
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_fetch_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_FETCH_OPTIONS_VERSION = 1;


        public int version;

        /// <summary>
        ///  Callbacks to use for this fetch operation
        /// </summary>
        public git_remote_callbacks callbacks;

        /// <summary>
        ///  Whether to perform a prune after the fetch
        /// </summary>
        public git_fetch_prune_t prune;

        /// <summary>
        ///  How to handle reference updates; see
        ///  `git_remote_update_flags`.
        /// </summary>
        public uint update_fetchhead;

        /// <summary>
        ///  Determines how to behave regarding tags on the remote, such
        ///  as auto-downloading tags for objects we're downloading or
        ///  downloading all of them.
        ///
        ///  <para>
        ///   The default is to auto-follow tags.
        ///  </para>
        /// </summary>
        public git_remote_autotag_option_t download_tags;

        /// <summary>
        ///  Proxy options to use, by default no proxy is used.
        /// </summary>
        public git_proxy_options proxy_opts;

        /// <summary>
        ///  Depth of the fetch to perform, or `GIT_FETCH_DEPTH_FULL` (or
        ///  `0`) for full history, or `GIT_FETCH_DEPTH_UNSHALLOW` to
        ///  "unshallow" a shallow repository.
        ///
        ///  <para>
        ///   The default is full (`GIT_FETCH_DEPTH_FULL` or `0`).
        ///  </para>
        /// </summary>
        public int depth;

        /// <summary>
        ///  Whether to allow off-site redirects. If this is not
        ///  specified, the `http.followRedirects` configuration setting
        ///  will be consulted.
        /// </summary>
        public git_remote_redirect_t follow_redirects;

        /// <summary>
        ///  Extra headers for this fetch operation
        /// </summary>
        public git_strarray custom_headers;
    }

    /// <summary>
    ///  Controls the behavior of a git_push object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_push_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_PUSH_OPTIONS_VERSION = 1;


        public uint version;

        /// <summary>
        ///  If the transport being used to push to the remote requires
        ///  the creation of a pack file, this controls the number of
        ///  worker threads used by the packbuilder when creating that
        ///  pack file to be sent to the remote.
        ///
        ///  <para>
        ///   If set to 0, the packbuilder will auto-detect the number
        ///   of threads to create. The default value is 1.
        ///  </para>
        /// </summary>
        public uint pb_parallelism;

        /// <summary>
        ///  Callbacks to use for this push operation
        /// </summary>
        public git_remote_callbacks callbacks;

        /// <summary>
        ///  Proxy options to use, by default no proxy is used.
        /// </summary>
        public git_proxy_options proxy_opts;

        /// <summary>
        ///  Whether to allow off-site redirects. If this is not
        ///  specified, the `http.followRedirects` configuration setting
        ///  will be consulted.
        /// </summary>
        public git_remote_redirect_t follow_redirects;

        /// <summary>
        ///  Extra headers for this push operation
        /// </summary>
        public git_strarray custom_headers;

        /// <summary>
        ///  "Push options" to deliver to the remote.
        /// </summary>
        public git_strarray remote_push_options;
    }

    /// <summary>
    ///  Remote creation options structure
    ///
    ///  <para>
    ///   Initialize with `GIT_REMOTE_CREATE_OPTIONS_INIT`.
    ///   Alternatively, you can use `git_remote_create_options_init`.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_remote_connect_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_REMOTE_CONNECT_OPTIONS_VERSION = 1;


        public uint version;

        /// <summary>
        ///  Callbacks to use for this connection
        /// </summary>
        public git_remote_callbacks callbacks;

        /// <summary>
        ///  HTTP Proxy settings
        /// </summary>
        public git_proxy_options proxy_opts;

        /// <summary>
        ///  Whether to allow off-site redirects. If this is not
        ///  specified, the `http.followRedirects` configuration setting
        ///  will be consulted.
        /// </summary>
        public git_remote_redirect_t follow_redirects;

        /// <summary>
        ///  Extra HTTP headers to use in this connection
        /// </summary>
        public git_strarray custom_headers;
    }

    /// <summary>
    ///  Preparation for a push operation. Can be used to configure what
    ///  to push and the level of parallelism of the packfile builder.
    /// </summary>
    public struct git_push
    {
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Add a remote with the default fetch refspec to the
        ///  repository's configuration.
        /// </summary>
        /// <param name="_out">the resulting remote</param>
        /// <param name="repo">
        ///  the repository in which to create the remote
        /// </param>
        /// <param name="name">the remote's name</param>
        /// <param name="url">the remote's url</param>
        /// <returns>
        ///  0, GIT_EINVALIDSPEC, GIT_EEXISTS or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_create(
            out git_remote* _out,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string url);

        /// <summary>
        ///  Initialize git_remote_create_options structure
        ///
        ///  <para>
        ///   Initializes a `git_remote_create_options` with default
        ///   values. Equivalent to creating an instance with
        ///   `GIT_REMOTE_CREATE_OPTIONS_INIT`.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_remote_create_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass
        ///  `GIT_REMOTE_CREATE_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_create_options_init(
            ref git_remote_create_options opts,
            uint version);

        /// <summary>
        ///  Create a remote, with options.
        ///
        ///  <para>
        ///   This function allows more fine-grained control over the
        ///   remote creation.
        ///  </para>
        ///
        ///  <para>
        ///   Passing NULL as the opts argument will result in a
        ///   detached remote.
        ///  </para>
        /// </summary>
        /// <param name="_out">the resulting remote</param>
        /// <param name="url">the remote's url</param>
        /// <param name="opts">the remote creation options</param>
        /// <returns>
        ///  0, GIT_EINVALIDSPEC, GIT_EEXISTS or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_create_with_opts(
            out git_remote* _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string url,
            ref git_remote_create_options opts);

        /// <summary>
        ///  Add a remote with the provided fetch refspec (or default if
        ///  NULL) to the repository's configuration.
        /// </summary>
        /// <param name="_out">the resulting remote</param>
        /// <param name="repo">
        ///  the repository in which to create the remote
        /// </param>
        /// <param name="name">the remote's name</param>
        /// <param name="url">the remote's url</param>
        /// <param name="fetch">the remote fetch value</param>
        /// <returns>
        ///  0, GIT_EINVALIDSPEC, GIT_EEXISTS or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_create_with_fetchspec(
            out git_remote* _out,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string url,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string fetch);

        /// <summary>
        ///  Create an anonymous remote
        ///
        ///  <para>
        ///   Create a remote with the given url in-memory. You can
        ///   use this when you have a URL instead of a remote's name.
        ///  </para>
        /// </summary>
        /// <param name="_out">pointer to the new remote objects</param>
        /// <param name="repo">the associated repository</param>
        /// <param name="url">the remote repository's URL</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_create_anonymous(
            out git_remote* _out,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string url);

        /// <summary>
        ///  Create a remote without a connected local repo
        ///
        ///  <para>
        ///   Create a remote with the given url in-memory. You can
        ///   use this when you have a URL instead of a remote's name.
        ///  </para>
        ///
        ///  <para>
        ///   Contrasted with git_remote_create_anonymous, a detached
        ///   remote will not consider any repo configuration values
        ///   (such as insteadof url substitutions).
        ///  </para>
        /// </summary>
        /// <param name="_out">pointer to the new remote objects</param>
        /// <param name="url">the remote repository's URL</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_create_detached(
            out git_remote* _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string url);

        /// <summary>
        ///  Get the information for a particular remote
        ///
        ///  <para>
        ///   The name will be checked for validity. See
        ///   `git_tag_create()` for rules about valid names.
        ///  </para>
        /// </summary>
        /// <param name="_out">pointer to the new remote object</param>
        /// <param name="repo">the associated repository</param>
        /// <param name="name">the remote's name</param>
        /// <returns>
        ///  0, GIT_ENOTFOUND, GIT_EINVALIDSPEC or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_lookup(
            out git_remote* _out,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Create a copy of an existing remote. All internal strings
        ///  are also duplicated. Callbacks are not duplicated.
        ///
        ///  <para>
        ///   Call `git_remote_free` to free the data.
        ///  </para>
        /// </summary>
        /// <param name="dest">pointer where to store the copy</param>
        /// <param name="source">object to copy</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_dup(
            git_remote** dest,
            git_remote* source);

        /// <summary>
        ///  Get the remote's repository
        /// </summary>
        /// <param name="remote">the remote</param>
        /// <returns>a pointer to the repository</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_repository* git_remote_owner(
            git_remote* remote);

        /// <summary>
        ///  Get the remote's name
        /// </summary>
        /// <param name="remote">the remote</param>
        /// <returns>
        ///  a pointer to the name or NULL for in-memory remotes
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_remote_name(
            git_remote* remote);

        /// <summary>
        ///  Get the remote's url
        ///
        ///  <para>
        ///   If url.*.insteadOf has been configured for this URL, it
        ///   will return the modified URL. This function does not
        ///   consider if a push url has been configured for this
        ///   remote (use `git_remote_pushurl` if needed).
        ///  </para>
        /// </summary>
        /// <param name="remote">the remote</param>
        /// <returns>a pointer to the url</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_remote_url(
            git_remote* remote);

        /// <summary>
        ///  Get the remote's url for pushing.
        ///
        ///  <para>
        ///   If url.*.pushInsteadOf has been configured for this URL,
        ///   it will return the modified URL. If
        ///   `git_remote_set_instance_pushurl` has been called for
        ///   this remote, then that URL will be returned.
        ///  </para>
        /// </summary>
        /// <param name="remote">the remote</param>
        /// <returns>
        ///  a pointer to the url or NULL if no special url for pushing
        ///  is set
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_remote_pushurl(
            git_remote* remote);

        /// <summary>
        ///  Set the remote's url in the configuration
        ///
        ///  <para>
        ///   Remote objects already in memory will not be affected.
        ///   This assumes the common case of a single-url remote and
        ///   will otherwise return an error.
        ///  </para>
        /// </summary>
        /// <param name="repo">
        ///  the repository in which to perform the change
        /// </param>
        /// <param name="remote">the remote's name</param>
        /// <param name="url">the url to set</param>
        /// <returns>0 or an error value</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_set_url(
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string remote,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string url);

        /// <summary>
        ///  Set the remote's url for pushing in the configuration.
        ///
        ///  <para>
        ///   Remote objects already in memory will not be affected.
        ///   This assumes the common case of a single-url remote and
        ///   will otherwise return an error.
        ///  </para>
        /// </summary>
        /// <param name="repo">
        ///  the repository in which to perform the change
        /// </param>
        /// <param name="remote">the remote's name</param>
        /// <param name="url">the url to set</param>
        /// <returns>0, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_set_pushurl(
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string remote,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string url);

        /// <summary>
        ///  Set the url for this particular url instance. The URL in the
        ///  configuration will be ignored, and will not be changed.
        /// </summary>
        /// <param name="remote">the remote's name</param>
        /// <param name="url">the url to set</param>
        /// <returns>0 or an error value</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_set_instance_url(
            git_remote* remote,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string url);

        /// <summary>
        ///  Set the push url for this particular url instance. The URL
        ///  in the configuration will be ignored, and will not be
        ///  changed.
        /// </summary>
        /// <param name="remote">the remote's name</param>
        /// <param name="url">the url to set</param>
        /// <returns>0 or an error value</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_set_instance_pushurl(
            git_remote* remote,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string url);

        /// <summary>
        ///  Add a fetch refspec to the remote's configuration
        ///
        ///  <para>
        ///   Add the given refspec to the fetch list in the
        ///   configuration. No loaded remote instances will be
        ///   affected.
        ///  </para>
        /// </summary>
        /// <param name="repo">
        ///  the repository in which to change the configuration
        /// </param>
        /// <param name="remote">the name of the remote to change</param>
        /// <param name="refspec">the new fetch refspec</param>
        /// <returns>
        ///  0, GIT_EINVALIDSPEC if refspec is invalid or an error value
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_add_fetch(
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string remote,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string refspec);

        /// <summary>
        ///  Get the remote's list of fetch refspecs
        ///
        ///  <para>
        ///   The memory is owned by the user and should be freed with
        ///   `git_strarray_free`.
        ///  </para>
        /// </summary>
        /// <param name="array">
        ///  pointer to the array in which to store the strings
        /// </param>
        /// <param name="remote">the remote to query</param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_get_fetch_refspecs(
            ref git_strarray array,
            git_remote* remote);

        /// <summary>
        ///  Add a push refspec to the remote's configuration
        ///
        ///  <para>
        ///   Add the given refspec to the push list in the
        ///   configuration. No loaded remote instances will be
        ///   affected.
        ///  </para>
        /// </summary>
        /// <param name="repo">
        ///  the repository in which to change the configuration
        /// </param>
        /// <param name="remote">the name of the remote to change</param>
        /// <param name="refspec">the new push refspec</param>
        /// <returns>
        ///  0, GIT_EINVALIDSPEC if refspec is invalid or an error value
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_add_push(
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string remote,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string refspec);

        /// <summary>
        ///  Get the remote's list of push refspecs
        ///
        ///  <para>
        ///   The memory is owned by the user and should be freed with
        ///   `git_strarray_free`.
        ///  </para>
        /// </summary>
        /// <param name="array">
        ///  pointer to the array in which to store the strings
        /// </param>
        /// <param name="remote">the remote to query</param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_get_push_refspecs(
            ref git_strarray array,
            git_remote* remote);

        /// <summary>
        ///  Get the number of refspecs for a remote
        /// </summary>
        /// <param name="remote">the remote</param>
        /// <returns>
        ///  the amount of refspecs configured in this remote
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_remote_refspec_count(
            git_remote* remote);

        /// <summary>
        ///  Get a refspec from the remote
        /// </summary>
        /// <param name="remote">the remote to query</param>
        /// <param name="n">the refspec to get</param>
        /// <returns>the nth refspec</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_refspec* git_remote_get_refspec(
            git_remote* remote,
            UIntPtr n);

        /// <summary>
        ///  Get the remote repository's reference advertisement list
        ///
        ///  <para>
        ///   Get the list of references with which the server
        ///   responds to a new connection.
        ///  </para>
        ///
        ///  <para>
        ///   The remote (or more exactly its transport) must have
        ///   connected to the remote repository. This list is
        ///   available as soon as the connection to the remote is
        ///   initiated and it remains available after disconnecting.
        ///  </para>
        ///
        ///  <para>
        ///   The memory belongs to the remote. The pointer will be
        ///   valid as long as a new connection is not initiated, but
        ///   it is recommended that you make a copy in order to make
        ///   use of the data.
        ///  </para>
        /// </summary>
        /// <param name="_out">pointer to the array</param>
        /// <param name="size">the number of remote heads</param>
        /// <param name="remote">the remote</param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_ls(
            out IntPtr _out,
            out UIntPtr size,
            git_remote* remote);

        /// <summary>
        ///  Check whether the remote is connected
        ///
        ///  <para>
        ///   Check whether the remote's underlying transport is
        ///   connected to the remote host.
        ///  </para>
        /// </summary>
        /// <param name="remote">the remote</param>
        /// <returns>1 if it's connected, 0 otherwise.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_connected(
            git_remote* remote);

        /// <summary>
        ///  Get the remote repository's object format.
        ///
        ///  <para>
        ///   The remote (or more exactly its transport) must have
        ///   connected to the remote repository. This format is
        ///   available as soon as the connection to the remote is
        ///   initiated and stays connected.
        ///  </para>
        /// </summary>
        /// <param name="_out">the resulting object format type</param>
        /// <param name="remote">the remote</param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_oid_type(
            git_oid_t* _out,
            git_remote* remote);

        /// <summary>
        ///  Cancel the operation
        ///
        ///  <para>
        ///   At certain points in its operation, the network code
        ///   checks whether the operation has been cancelled and if
        ///   so stops the operation.
        ///  </para>
        /// </summary>
        /// <param name="remote">the remote</param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_stop(
            git_remote* remote);

        /// <summary>
        ///  Disconnect from the remote
        ///
        ///  <para>
        ///   Close the connection to the remote.
        ///  </para>
        /// </summary>
        /// <param name="remote">the remote to disconnect from</param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_disconnect(
            git_remote* remote);

        /// <summary>
        ///  Free the memory associated with a remote
        ///
        ///  <para>
        ///   This also disconnects from the remote, if the connection
        ///   has not been closed yet (using git_remote_disconnect).
        ///  </para>
        /// </summary>
        /// <param name="remote">the remote to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_remote_free(
            git_remote* remote);

        /// <summary>
        ///  Get a list of the configured remotes for a repo
        ///
        ///  <para>
        ///   The string array must be freed by the user.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  a string array which receives the names of the remotes
        /// </param>
        /// <param name="repo">the repository to query</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_list(
            ref git_strarray _out,
            git_repository* repo);

        /// <summary>
        ///  Initializes a `git_remote_callbacks` with default values.
        ///  Equivalent to creating an instance with
        ///  GIT_REMOTE_CALLBACKS_INIT.
        /// </summary>
        /// <param name="opts">
        ///  the `git_remote_callbacks` struct to initialize
        /// </param>
        /// <param name="version">
        ///  Version of struct; pass `GIT_REMOTE_CALLBACKS_VERSION`
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_init_callbacks(
            ref git_remote_callbacks opts,
            uint version);

        /// <summary>
        ///  Initialize git_fetch_options structure
        ///
        ///  <para>
        ///   Initializes a `git_fetch_options` with default values.
        ///   Equivalent to creating an instance with
        ///   `GIT_FETCH_OPTIONS_INIT`.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_fetch_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass `GIT_FETCH_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_fetch_options_init(
            ref git_fetch_options opts,
            uint version);

        /// <summary>
        ///  Initialize git_push_options structure
        ///
        ///  <para>
        ///   Initializes a `git_push_options` with default values.
        ///   Equivalent to creating an instance with
        ///   `GIT_PUSH_OPTIONS_INIT`.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_push_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass `GIT_PUSH_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_push_options_init(
            ref git_push_options opts,
            uint version);

        /// <summary>
        ///  Initialize git_remote_connect_options structure.
        ///
        ///  <para>
        ///   Initializes a `git_remote_connect_options` with default
        ///   values. Equivalent to creating an instance with
        ///   `GIT_REMOTE_CONNECT_OPTIONS_INIT`.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_remote_connect_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass
        ///  `GIT_REMOTE_CONNECT_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_connect_options_init(
            ref git_remote_connect_options opts,
            uint version);

        /// <summary>
        ///  Open a connection to a remote.
        ///
        ///  <para>
        ///   The transport is selected based on the URL; the
        ///   direction argument is due to a limitation of the git
        ///   protocol which starts up a specific binary which can
        ///   only do the one or the other.
        ///  </para>
        /// </summary>
        /// <param name="remote">the remote to connect to</param>
        /// <param name="direction">
        ///  GIT_DIRECTION_FETCH if you want to fetch or
        ///  GIT_DIRECTION_PUSH if you want to push
        /// </param>
        /// <param name="callbacks">
        ///  the callbacks to use for this connection
        /// </param>
        /// <param name="proxy_opts">proxy settings</param>
        /// <param name="custom_headers">
        ///  extra HTTP headers to use in this connection
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_connect(
            git_remote* remote,
            git_direction direction,
            ref git_remote_callbacks callbacks,
            ref git_proxy_options proxy_opts,
            ref git_strarray custom_headers);

        /// <summary>
        ///  Open a connection to a remote with extended options.
        ///
        ///  <para>
        ///   The transport is selected based on the URL; the
        ///   direction argument is due to a limitation of the git
        ///   protocol which starts up a specific binary which can
        ///   only do the one or the other.
        ///  </para>
        ///
        ///  <para>
        ///   The given options structure will form the defaults for
        ///   connection options and callback setup. Callers may
        ///   override these defaults by specifying
        ///   `git_fetch_options` or `git_push_options` in subsequent
        ///   calls.
        ///  </para>
        /// </summary>
        /// <param name="remote">the remote to connect to</param>
        /// <param name="direction">
        ///  GIT_DIRECTION_FETCH if you want to fetch or
        ///  GIT_DIRECTION_PUSH if you want to push
        /// </param>
        /// <param name="opts">the remote connection options</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_connect_ext(
            git_remote* remote,
            git_direction direction,
            ref git_remote_connect_options opts);

        /// <summary>
        ///  Download and index the packfile.
        ///
        ///  <para>
        ///   Connect to the remote if it hasn't been done yet,
        ///   negotiate with the remote git which objects are missing,
        ///   download and index the packfile.
        ///  </para>
        ///
        ///  <para>
        ///   The .idx file will be created and both it and the
        ///   packfile with be renamed to their final name.
        ///  </para>
        ///
        ///  <para>
        ///   If options are specified and this remote is already
        ///   connected then the existing remote connection options
        ///   will be discarded and the remote will now use the new
        ///   options.
        ///  </para>
        /// </summary>
        /// <param name="remote">the remote</param>
        /// <param name="refspecs">
        ///  the refspecs to use for this negotiation and download. Use
        ///  NULL or an empty array to use the base refspecs
        /// </param>
        /// <param name="opts">
        ///  the options to use for this fetch or NULL
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_download(
            git_remote* remote,
            ref git_strarray refspecs,
            ref git_fetch_options opts);

        /// <summary>
        ///  Create a packfile and send it to the server
        ///
        ///  <para>
        ///   Connect to the remote if it hasn't been done yet,
        ///   negotiate with the remote git which objects are missing,
        ///   create a packfile with the missing objects and send it.
        ///  </para>
        ///
        ///  <para>
        ///   If options are specified and this remote is already
        ///   connected then the existing remote connection options
        ///   will be discarded and the remote will now use the new
        ///   options.
        ///  </para>
        /// </summary>
        /// <param name="remote">the remote</param>
        /// <param name="refspecs">
        ///  the refspecs to use for this negotiation and upload. Use
        ///  NULL or an empty array to use the base refspecs
        /// </param>
        /// <param name="opts">the options to use for this push</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_upload(
            git_remote* remote,
            ref git_strarray refspecs,
            ref git_push_options opts);

        /// <summary>
        ///  Update the tips to the new state.
        ///
        ///  <para>
        ///   If callbacks are not specified then the callbacks
        ///   specified to `git_remote_connect` will be used (if it
        ///   was called).
        ///  </para>
        /// </summary>
        /// <param name="remote">the remote to update</param>
        /// <param name="callbacks">
        ///  pointer to the callback structure to use or NULL
        /// </param>
        /// <param name="update_flags">
        ///  the git_remote_update_flags for these tips.
        /// </param>
        /// <param name="download_tags">
        ///  what the behaviour for downloading tags is for this fetch.
        ///  This is ignored for push. This must be the same value passed
        ///  to `git_remote_download()`.
        /// </param>
        /// <param name="reflog_message">
        ///  The message to insert into the reflogs. If NULL and
        ///  fetching, the default is "fetch <name>", where <name> is the
        ///  name of the remote (or its url, for in-memory remotes). This
        ///  parameter is ignored when pushing.
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_update_tips(
            git_remote* remote,
            ref git_remote_callbacks callbacks,
            uint update_flags,
            git_remote_autotag_option_t download_tags,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string reflog_message);

        /// <summary>
        ///  Download new data and update tips.
        ///
        ///  <para>
        ///   Convenience function to connect to a remote, download
        ///   the data, disconnect and update the remote-tracking
        ///   branches.
        ///  </para>
        ///
        ///  <para>
        ///   If options are specified and this remote is already
        ///   connected then the existing remote connection options
        ///   will be discarded and the remote will now use the new
        ///   options.
        ///  </para>
        /// </summary>
        /// <param name="remote">the remote to fetch from</param>
        /// <param name="refspecs">
        ///  the refspecs to use for this fetch. Pass NULL or an empty
        ///  array to use the base refspecs.
        /// </param>
        /// <param name="opts">options to use for this fetch or NULL</param>
        /// <param name="reflog_message">
        ///  The message to insert into the reflogs. If NULL, the default
        ///  is "fetch"
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_fetch(
            git_remote* remote,
            ref git_strarray refspecs,
            ref git_fetch_options opts,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string reflog_message);

        /// <summary>
        ///  Prune tracking refs that are no longer present on remote.
        ///
        ///  <para>
        ///   If callbacks are not specified then the callbacks
        ///   specified to `git_remote_connect` will be used (if it
        ///   was called).
        ///  </para>
        /// </summary>
        /// <param name="remote">the remote to prune</param>
        /// <param name="callbacks">callbacks to use for this prune</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_prune(
            git_remote* remote,
            ref git_remote_callbacks callbacks);

        /// <summary>
        ///  Perform a push.
        ///
        ///  <para>
        ///   If options are specified and this remote is already
        ///   connected then the existing remote connection options
        ///   will be discarded and the remote will now use the new
        ///   options.
        ///  </para>
        /// </summary>
        /// <param name="remote">the remote to push to</param>
        /// <param name="refspecs">
        ///  the refspecs to use for pushing. If NULL or an empty array,
        ///  the configured refspecs will be used
        /// </param>
        /// <param name="opts">options to use for this push</param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_push(
            git_remote* remote,
            ref git_strarray refspecs,
            ref git_push_options opts);

        /// <summary>
        ///  Get the statistics structure that is filled in by the fetch
        ///  operation.
        /// </summary>
        /// <param name="remote">the remote to get statistics for</param>
        /// <returns>the git_indexer_progress for the remote</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_indexer_progress* git_remote_stats(
            git_remote* remote);

        /// <summary>
        ///  Retrieve the tag auto-follow setting
        /// </summary>
        /// <param name="remote">the remote to query</param>
        /// <returns>the auto-follow setting</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_remote_autotag_option_t git_remote_autotag(
            git_remote* remote);

        /// <summary>
        ///  Set the remote's tag following setting.
        ///
        ///  <para>
        ///   The change will be made in the configuration. No loaded
        ///   remotes will be affected.
        ///  </para>
        /// </summary>
        /// <param name="repo">
        ///  the repository in which to make the change
        /// </param>
        /// <param name="remote">the name of the remote</param>
        /// <param name="value">the new value to take.</param>
        /// <returns>0, or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_set_autotag(
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string remote,
            git_remote_autotag_option_t value);

        /// <summary>
        ///  Retrieve the ref-prune setting
        /// </summary>
        /// <param name="remote">the remote to query</param>
        /// <returns>the ref-prune setting</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_prune_refs(
            git_remote* remote);

        /// <summary>
        ///  Give the remote a new name
        ///
        ///  <para>
        ///   All remote-tracking branches and configuration settings
        ///   for the remote are updated.
        ///  </para>
        ///
        ///  <para>
        ///   The new name will be checked for validity. See
        ///   `git_tag_create()` for rules about valid names.
        ///  </para>
        ///
        ///  <para>
        ///   No loaded instances of a the remote with the old name
        ///   will change their name or their list of refspecs.
        ///  </para>
        /// </summary>
        /// <param name="problems">
        ///  non-default refspecs cannot be renamed and will be stored
        ///  here for further processing by the caller. Always free this
        ///  strarray on successful return.
        /// </param>
        /// <param name="repo">the repository in which to rename</param>
        /// <param name="name">the current name of the remote</param>
        /// <param name="new_name">
        ///  the new name the remote should bear
        /// </param>
        /// <returns>
        ///  0, GIT_EINVALIDSPEC, GIT_EEXISTS or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_rename(
            ref git_strarray problems,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string new_name);

        /// <summary>
        ///  Ensure the remote name is well-formed.
        /// </summary>
        /// <param name="valid">
        ///  output pointer to set with validity of given remote name
        /// </param>
        /// <param name="remote_name">name to be checked.</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_name_is_valid(
            int* valid,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string remote_name);

        /// <summary>
        ///  Delete an existing persisted remote.
        ///
        ///  <para>
        ///   All remote-tracking branches and configuration settings
        ///   for the remote will be removed.
        ///  </para>
        /// </summary>
        /// <param name="repo">the repository in which to act</param>
        /// <param name="name">the name of the remote to delete</param>
        /// <returns>0 on success, or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_delete(
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Retrieve the name of the remote's default branch
        ///
        ///  <para>
        ///   The default branch of a repository is the branch which
        ///   HEAD points to. If the remote does not support reporting
        ///   this information directly, it performs the guess as git
        ///   does; that is, if there are multiple branches which
        ///   point to the same commit, the first one is chosen. If
        ///   the master branch is a candidate, it wins.
        ///  </para>
        ///
        ///  <para>
        ///   This function must only be called after connecting.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  the buffer in which to store the reference name
        /// </param>
        /// <param name="remote">the remote</param>
        /// <returns>
        ///  0, GIT_ENOTFOUND if the remote does not have any references
        ///  or none of them point to HEAD's commit, or an error message.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_default_branch(
            ref git_buf _out,
            git_remote* remote);
    }
}
