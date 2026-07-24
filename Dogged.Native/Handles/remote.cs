using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public sealed class RemoteHandle : SafeHandle
    {
        public RemoteHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe RemoteHandle(git_remote* nativeRemote) :
            base((IntPtr)nativeRemote, true)
        {
        }

        public override bool IsInvalid
        {
            get
            {
                return handle == IntPtr.Zero;
            }
        }

        protected override unsafe bool ReleaseHandle()
        {
            libgit2.git_remote_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
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
            out RemoteHandle _out,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string url);

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
            out RemoteHandle _out,
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
            out RemoteHandle _out,
            RepositoryHandle repo,
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
            out RemoteHandle _out,
            RepositoryHandle repo,
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
            out RemoteHandle _out,
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
            out RemoteHandle _out,
            RepositoryHandle repo,
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
            RemoteHandle dest,
            RemoteHandle source);

        /// <summary>
        ///  Get the remote's repository
        /// </summary>
        /// <param name="remote">the remote</param>
        /// <returns>a pointer to the repository</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_repository* git_remote_owner(
            RemoteHandle remote);

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
            RemoteHandle remote);

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
            RemoteHandle remote);

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
            RemoteHandle remote);

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
            RepositoryHandle repo,
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
            RepositoryHandle repo,
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
            RemoteHandle remote,
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
            RemoteHandle remote,
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
            RepositoryHandle repo,
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
            RemoteHandle remote);

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
            RepositoryHandle repo,
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
            RemoteHandle remote);

        /// <summary>
        ///  Get the number of refspecs for a remote
        /// </summary>
        /// <param name="remote">the remote</param>
        /// <returns>
        ///  the amount of refspecs configured in this remote
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_remote_refspec_count(
            RemoteHandle remote);

        /// <summary>
        ///  Get a refspec from the remote
        /// </summary>
        /// <param name="remote">the remote to query</param>
        /// <param name="n">the refspec to get</param>
        /// <returns>the nth refspec</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_refspec* git_remote_get_refspec(
            RemoteHandle remote,
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
            RemoteHandle remote);

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
            RemoteHandle remote);

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
            RemoteHandle remote);

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
            RemoteHandle remote);

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
            RemoteHandle remote);

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
            IntPtr remote);

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
            RepositoryHandle repo);

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
            RemoteHandle remote,
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
            RemoteHandle remote,
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
            RemoteHandle remote,
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
            RemoteHandle remote,
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
            RemoteHandle remote,
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
            RemoteHandle remote,
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
            RemoteHandle remote,
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
            RemoteHandle remote,
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
            RemoteHandle remote);

        /// <summary>
        ///  Retrieve the tag auto-follow setting
        /// </summary>
        /// <param name="remote">the remote to query</param>
        /// <returns>the auto-follow setting</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_remote_autotag_option_t git_remote_autotag(
            RemoteHandle remote);

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
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string remote,
            git_remote_autotag_option_t value);

        /// <summary>
        ///  Retrieve the ref-prune setting
        /// </summary>
        /// <param name="remote">the remote to query</param>
        /// <returns>the ref-prune setting</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_remote_prune_refs(
            RemoteHandle remote);

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
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string new_name);

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
            RepositoryHandle repo,
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
            RemoteHandle remote);
    }
}
