using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  A merge driver source represents the file to be merged
    /// </summary>
    public struct git_merge_driver_source
    {
    }

    /// <summary>
    ///  Initialize callback on merge driver
    ///
    ///  <para>
    ///   Specified as `driver.initialize`, this is an optional
    ///   callback invoked before a merge driver is first used. It
    ///   will be called once at most per library lifetime.
    ///  </para>
    ///
    ///  <para>
    ///   If non-NULL, the merge driver's `initialize` callback will
    ///   be invoked right before the first use of the driver, so you
    ///   can defer expensive initialization operations (in case
    ///   libgit2 is being used in a way that doesn't need the merge
    ///   driver).
    ///  </para>
    /// </summary>
    /// <param name="self">the merge driver to initialize</param>
    /// <returns>0 on success, or a negative number on failure</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_merge_driver_init_fn(
        ref git_merge_driver self);

    /// <summary>
    ///  Shutdown callback on merge driver
    ///
    ///  <para>
    ///   Specified as `driver.shutdown`, this is an optional callback
    ///   invoked when the merge driver is unregistered or when
    ///   libgit2 is shutting down. It will be called once at most and
    ///   should release resources as needed. This may be called even
    ///   if the `initialize` callback was not made.
    ///  </para>
    ///
    ///  <para>
    ///   Typically this function will free the `git_merge_driver`
    ///   object itself.
    ///  </para>
    /// </summary>
    /// <param name="self">the merge driver to shutdown</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void git_merge_driver_shutdown_fn(
        ref git_merge_driver self);

    /// <summary>
    ///  Callback to perform the merge.
    ///
    ///  <para>
    ///   Specified as `driver.apply`, this is the callback that
    ///   actually does the merge. If it can successfully perform a
    ///   merge, it should populate `path_out` with a pointer to the
    ///   filename to accept, `mode_out` with the resultant mode, and
    ///   `merged_out` with the buffer of the merged file and then
    ///   return 0. If the driver returns `GIT_PASSTHROUGH`, then the
    ///   default merge driver should instead be run. It can also
    ///   return `GIT_EMERGECONFLICT` if the driver is not able to
    ///   produce a merge result, and the file will remain conflicted.
    ///   Any other errors will fail and return to the caller.
    ///  </para>
    ///
    ///  <para>
    ///   The `filter_name` contains the name of the filter that was
    ///   invoked, as specified by the file's attributes.
    ///  </para>
    ///
    ///  <para>
    ///   The `src` contains the data about the file to be merged.
    ///  </para>
    /// </summary>
    /// <param name="self">the merge driver</param>
    /// <param name="path_out">the resolved path</param>
    /// <param name="mode_out">the resolved mode</param>
    /// <param name="merged_out">the merged output contents</param>
    /// <param name="filter_name">the filter that was invoked</param>
    /// <param name="src">the data about the unmerged file</param>
    /// <returns>0 on success, or an error code</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_merge_driver_apply_fn(
        ref git_merge_driver self,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))] out string path_out,
        out uint mode_out,
        ref git_buf merged_out,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string filter_name,
        git_merge_driver_source* src);

    /// <summary>
    ///  Merge driver structure used to register custom merge drivers.
    ///
    ///  <para>
    ///   To associate extra data with a driver, allocate extra data
    ///   and put the `git_merge_driver` struct at the start of your
    ///   data buffer, then cast the `self` pointer to your larger
    ///   structure when your callback is invoked.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_merge_driver
    {
        /// <summary>
        ///  The `version` should be set to `GIT_MERGE_DRIVER_VERSION`.
        /// </summary>
        public uint version;

        /// <summary>
        ///  Called when the merge driver is first used for any file.
        /// </summary>
        public git_merge_driver_init_fn initialize;

        /// <summary>
        ///  Called when the merge driver is unregistered from the
        ///  system.
        /// </summary>
        public git_merge_driver_shutdown_fn shutdown;

        /// <summary>
        ///  Called to merge the contents of a conflict. If this function
        ///  returns `GIT_PASSTHROUGH` then the default (`text`) merge
        ///  driver will instead be invoked. If this function returns
        ///  `GIT_EMERGECONFLICT` then the file will remain conflicted.
        /// </summary>
        public git_merge_driver_apply_fn apply;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Look up a merge driver by name
        /// </summary>
        /// <param name="name">The name of the merge driver</param>
        /// <returns>
        ///  Pointer to the merge driver object or NULL if not found
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe IntPtr git_merge_driver_lookup(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Get the repository that the source data is coming from.
        /// </summary>
        /// <param name="src">the merge driver source</param>
        /// <returns>the repository</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_repository* git_merge_driver_source_repo(
            git_merge_driver_source* src);

        /// <summary>
        ///  Gets the ancestor of the file to merge.
        /// </summary>
        /// <param name="src">the merge driver source</param>
        /// <returns>the ancestor or NULL if there was no ancestor</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_index_entry* git_merge_driver_source_ancestor(
            git_merge_driver_source* src);

        /// <summary>
        ///  Gets the ours side of the file to merge.
        /// </summary>
        /// <param name="src">the merge driver source</param>
        /// <returns>
        ///  the ours side or NULL if there was no ours side
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_index_entry* git_merge_driver_source_ours(
            git_merge_driver_source* src);

        /// <summary>
        ///  Gets the theirs side of the file to merge.
        /// </summary>
        /// <param name="src">the merge driver source</param>
        /// <returns>
        ///  the theirs side or NULL if there was no theirs side
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_index_entry* git_merge_driver_source_theirs(
            git_merge_driver_source* src);

        /// <summary>
        ///  Gets the merge file options that the merge was invoked with.
        /// </summary>
        /// <param name="src">the merge driver source</param>
        /// <returns>the options</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_merge_file_options* git_merge_driver_source_file_options(
            git_merge_driver_source* src);

        /// <summary>
        ///  Register a merge driver under a given name.
        ///
        ///  <para>
        ///   As mentioned elsewhere, the initialize callback will not
        ///   be invoked immediately. It is deferred until the driver
        ///   is used in some way.
        ///  </para>
        ///
        ///  <para>
        ///   Currently the merge driver registry is not thread safe,
        ///   so any registering or deregistering of merge drivers
        ///   must be done outside of any possible usage of the
        ///   drivers (i.e. during application setup or shutdown).
        ///  </para>
        /// </summary>
        /// <param name="name">
        ///  The name of this driver to match an attribute. Attempting to
        ///  register with an in-use name will return GIT_EEXISTS.
        /// </param>
        /// <param name="driver">
        ///  The merge driver definition. This pointer will be stored as
        ///  is by libgit2 so it must be a durable allocation (either
        ///  static or on the heap).
        /// </param>
        /// <returns>
        ///  0 on successful registry, error code < 0 on failure
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_merge_driver_register(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            ref git_merge_driver driver);

        /// <summary>
        ///  Remove the merge driver with the given name.
        ///
        ///  <para>
        ///   Attempting to remove the builtin libgit2 merge drivers
        ///   is not permitted and will return an error.
        ///  </para>
        ///
        ///  <para>
        ///   Currently the merge driver registry is not thread safe,
        ///   so any registering or deregistering of drivers must be
        ///   done outside of any possible usage of the drivers (i.e.
        ///   during application setup or shutdown).
        ///  </para>
        /// </summary>
        /// <param name="name">
        ///  The name under which the merge driver was registered
        /// </param>
        /// <returns>0 on success, error code < 0 on failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_merge_driver_unregister(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);
    }
}
