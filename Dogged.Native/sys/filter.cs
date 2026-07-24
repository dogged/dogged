using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  A filter source represents a file/blob to be processed
    /// </summary>
    public struct git_filter_source
    {
    }

    /// <summary>
    ///  Initialize callback on filter
    ///
    ///  <para>
    ///   Specified as `filter.initialize`, this is an optional
    ///   callback invoked before a filter is first used. It will be
    ///   called once at most.
    ///  </para>
    ///
    ///  <para>
    ///   If non-NULL, the filter's `initialize` callback will be
    ///   invoked right before the first use of the filter, so you can
    ///   defer expensive initialization operations (in case libgit2
    ///   is being used in a way that doesn't need the filter).
    ///  </para>
    /// </summary>
    /// <param name="self">the filter to initialize</param>
    /// <returns>0 on success, negative number on failure</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_filter_init_fn(
        ref git_filter self);

    /// <summary>
    ///  Shutdown callback on filter
    ///
    ///  <para>
    ///   Specified as `filter.shutdown`, this is an optional callback
    ///   invoked when the filter is unregistered or when libgit2 is
    ///   shutting down. It will be called once at most and should
    ///   release resources as needed. This may be called even if the
    ///   `initialize` callback was not made.
    ///  </para>
    ///
    ///  <para>
    ///   Typically this function will free the `git_filter` object
    ///   itself.
    ///  </para>
    /// </summary>
    /// <param name="self">the filter to shutdown</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void git_filter_shutdown_fn(
        ref git_filter self);

    /// <summary>
    ///  Callback to decide if a given source needs this filter
    ///
    ///  <para>
    ///   Specified as `filter.check`, this is an optional callback
    ///   that checks if filtering is needed for a given source.
    ///  </para>
    ///
    ///  <para>
    ///   It should return 0 if the filter should be applied (i.e.
    ///   success), GIT_PASSTHROUGH if the filter should not be
    ///   applied, or an error code to fail out of the filter
    ///   processing pipeline and return to the caller.
    ///  </para>
    ///
    ///  <para>
    ///   The `attr_values` will be set to the values of any
    ///   attributes given in the filter definition. See `git_filter`
    ///   below for more detail.
    ///  </para>
    ///
    ///  <para>
    ///   The `payload` will be a pointer to a reference payload for
    ///   the filter. This will start as NULL, but `check` can assign
    ///   to this pointer for later use by the `stream` callback. Note
    ///   that the value should be heap allocated (not stack), so that
    ///   it doesn't go away before the `stream` callback can use it.
    ///   If a filter allocates and assigns a value to the `payload`,
    ///   it will need a `cleanup` callback to free the payload.
    ///  </para>
    /// </summary>
    /// <param name="self">the filter check</param>
    /// <param name="payload">a data for future filter functions</param>
    /// <param name="src">the filter source</param>
    /// <param name="attr_values">the attribute values</param>
    /// <returns>0 on success or a negative value on error</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_filter_check_fn(
        ref git_filter self,
        IntPtr payload,
        git_filter_source* src,
        char** attr_values);

    /// <summary>
    ///  Callback to actually perform the data filtering
    ///
    ///  <para>
    ///   Specified as `filter.apply`, this is the callback that
    ///   actually filters data. If it successfully writes the output,
    ///   it should return 0. Like `check`, it can return
    ///   GIT_PASSTHROUGH to indicate that the filter doesn't want to
    ///   run. Other error codes will stop filter processing and
    ///   return to the caller.
    ///  </para>
    ///
    ///  <para>
    ///   The `payload` value will refer to any payload that was set
    ///   by the `check` callback. It may be read from or written to
    ///   as needed.
    ///  </para>
    /// </summary>
    /// <param name="self">the filter check</param>
    /// <param name="payload">a data for future filter functions</param>
    /// <param name="to">the input buffer</param>
    /// <param name="from">the output buffer</param>
    /// <param name="src">the filter source</param>
    /// <returns>0 on success or a negative value on error</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_filter_apply_fn(
        ref git_filter self,
        IntPtr payload,
        ref git_buf to,
        ref git_buf from,
        git_filter_source* src);

    /// <summary>
    ///  Callback to perform the data filtering.
    ///
    ///  <para>
    ///   Specified as `filter.stream`, this is a callback that
    ///   filters data in a streaming manner. This function will
    ///   provide a `git_writestream` that will the original data will
    ///   be written to; with that data, the `git_writestream` will
    ///   then perform the filter translation and stream the filtered
    ///   data out to the `next` location.
    ///  </para>
    /// </summary>
    /// <param name="_out">the write stream</param>
    /// <param name="self">the filter</param>
    /// <param name="payload">a data for future filter functions</param>
    /// <param name="src">the filter source</param>
    /// <param name="next">the output stream</param>
    /// <returns>0 on success or a negative value on error</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_filter_stream_fn(
        out git_writestream* _out,
        ref git_filter self,
        IntPtr payload,
        git_filter_source* src,
        ref git_writestream next);

    /// <summary>
    ///  Callback to clean up after filtering has been applied
    ///
    ///  <para>
    ///   Specified as `filter.cleanup`, this is an optional callback
    ///   invoked after the filter has been applied. If the `check`,
    ///   `apply`, or `stream` callbacks allocated a `payload` to keep
    ///   per-source filter state, use this callback to free that
    ///   payload and release resources as required.
    ///  </para>
    /// </summary>
    /// <param name="self">the filter</param>
    /// <param name="payload">a data for future filter functions</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void git_filter_cleanup_fn(
        ref git_filter self,
        IntPtr payload);

    /// <summary>
    ///  Filter structure used to register custom filters.
    ///
    ///  <para>
    ///   To associate extra data with a filter, allocate extra data
    ///   and put the `git_filter` struct at the start of your data
    ///   buffer, then cast the `self` pointer to your larger
    ///   structure when your callback is invoked.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_filter
    {
        /// <summary>
        ///  The `version` field should be set to `GIT_FILTER_VERSION`.
        /// </summary>
        public uint version;

        /// <summary>
        ///  A whitespace-separated list of attribute names to check for
        ///  this filter (e.g. "eol crlf text"). If the attribute name is
        ///  bare, it will be simply loaded and passed to the `check`
        ///  callback. If it has a value (i.e. "name=value"), the
        ///  attribute must match that value for the filter to be
        ///  applied. The value may be a wildcard (eg, "name=*"), in
        ///  which case the filter will be invoked for any value for the
        ///  given attribute name. See the attribute parameter of the
        ///  `check` callback for the attribute value that was specified.
        /// </summary>
        public IntPtr attributes;

        /// <summary>
        ///  Called when the filter is first used for any file.
        /// </summary>
        public git_filter_init_fn initialize;

        /// <summary>
        ///  Called when the filter is removed or unregistered from the
        ///  system.
        /// </summary>
        public git_filter_shutdown_fn shutdown;

        /// <summary>
        ///  Called to determine whether the filter should be invoked for
        ///  a given file. If this function returns `GIT_PASSTHROUGH`
        ///  then the `stream` or `apply` functions will not be invoked
        ///  and the contents will be passed through unmodified.
        /// </summary>
        public git_filter_check_fn check;

        /// <summary>
        ///  Provided for backward compatibility; this will apply the
        ///  filter to the given contents in a `git_buf`. Callers should
        ///  provide a `stream` function instead.
        /// </summary>
        public git_filter_apply_fn apply;

        /// <summary>
        ///  Called to apply the filter, this function will provide a
        ///  `git_writestream` that will the original data will be
        ///  written to; with that data, the `git_writestream` will then
        ///  perform the filter translation and stream the filtered data
        ///  out to the `next` location.
        /// </summary>
        public git_filter_stream_fn stream;

        /// <summary>
        ///  Called when the system is done filtering for a file.
        /// </summary>
        public git_filter_cleanup_fn cleanup;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Look up a filter by name
        /// </summary>
        /// <param name="name">The name of the filter</param>
        /// <returns>
        ///  Pointer to the filter object or NULL if not found
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe IntPtr git_filter_lookup(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Create a new empty filter list
        ///
        ///  <para>
        ///   Normally you won't use this because
        ///   `git_filter_list_load` will create the filter list for
        ///   you, but you can use this in combination with the
        ///   `git_filter_lookup` and `git_filter_list_push` functions
        ///   to assemble your own chains of filters.
        ///  </para>
        /// </summary>
        /// <param name="_out">the filter list</param>
        /// <param name="repo">
        ///  the repository to use for configuration
        /// </param>
        /// <param name="mode">the filter mode (direction)</param>
        /// <param name="options">the options</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_filter_list_new(
            out git_filter_list* _out,
            git_repository* repo,
            git_filter_mode_t mode,
            uint options);

        /// <summary>
        ///  Add a filter to a filter list with the given payload.
        ///
        ///  <para>
        ///   Normally you won't have to do this because the filter
        ///   list is created by calling the "check" function on
        ///   registered filters when the filter attributes are set,
        ///   but this does allow more direct manipulation of filter
        ///   lists when desired.
        ///  </para>
        ///
        ///  <para>
        ///   Note that normally the "check" function can set up a
        ///   payload for the filter. Using this function, you can
        ///   either pass in a payload if you know the expected
        ///   payload format, or you can pass NULL. Some filters may
        ///   fail with a NULL payload. Good luck!
        ///  </para>
        /// </summary>
        /// <param name="fl">the filter list</param>
        /// <param name="filter">the filter to push</param>
        /// <param name="payload">the payload for the filter</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_filter_list_push(
            git_filter_list* fl,
            ref git_filter filter,
            IntPtr payload);

        /// <summary>
        ///  Look up how many filters are in the list
        ///
        ///  <para>
        ///   We will attempt to apply all of these filters to any
        ///   data passed in, but note that the filter apply action
        ///   still has the option of skipping data that is passed in
        ///   (for example, the CRLF filter will skip data that
        ///   appears to be binary).
        ///  </para>
        /// </summary>
        /// <param name="fl">A filter list</param>
        /// <returns>The number of filters in the list</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_filter_list_length(
            git_filter_list* fl);

        /// <summary>
        ///  Get the repository that the source data is coming from.
        /// </summary>
        /// <param name="src">the filter source</param>
        /// <returns>the repository for the filter information</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_repository* git_filter_source_repo(
            git_filter_source* src);

        /// <summary>
        ///  Get the path that the source data is coming from.
        /// </summary>
        /// <param name="src">the filter source</param>
        /// <returns>the path that is being filtered</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_filter_source_path(
            git_filter_source* src);

        /// <summary>
        ///  Get the file mode of the source file If the mode is unknown,
        ///  this will return 0
        /// </summary>
        /// <param name="src">the filter source</param>
        /// <returns>the file mode for the file being filtered</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe ushort git_filter_source_filemode(
            git_filter_source* src);

        /// <summary>
        ///  Get the OID of the source If the OID is unknown (often the
        ///  case with GIT_FILTER_CLEAN) then this will return NULL.
        /// </summary>
        /// <param name="src">the filter source</param>
        /// <returns>the object id of the file being filtered</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_filter_source_id(
            git_filter_source* src);

        /// <summary>
        ///  Get the git_filter_mode_t to be used
        /// </summary>
        /// <param name="src">the filter source</param>
        /// <returns>the mode (direction) of the filter</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_filter_mode_t git_filter_source_mode(
            git_filter_source* src);

        /// <summary>
        ///  Get the combination git_filter_flag_t options to be applied
        /// </summary>
        /// <param name="src">the filter source</param>
        /// <returns>the flags of the filter</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe uint git_filter_source_flags(
            git_filter_source* src);

        /// <summary>
        ///  Initializes a `git_filter` with default values. Equivalent
        ///  to creating an instance with GIT_FILTER_INIT.
        /// </summary>
        /// <param name="filter">
        ///  the `git_filter` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  Version the struct; pass `GIT_FILTER_VERSION`
        /// </param>
        /// <returns>0 on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_filter_init(
            ref git_filter filter,
            uint version);

        /// <summary>
        ///  Register a filter under a given name with a given priority.
        ///
        ///  <para>
        ///   As mentioned elsewhere, the initialize callback will not
        ///   be invoked immediately. It is deferred until the filter
        ///   is used in some way.
        ///  </para>
        ///
        ///  <para>
        ///   A filter's attribute checks and `check` and `stream` (or
        ///   `apply`) callbacks will be issued in order of `priority`
        ///   on smudge (to workdir), and in reverse order of
        ///   `priority` on clean (to odb).
        ///  </para>
        ///
        ///  <para>
        ///   Two filters are preregistered with libgit2: -
        ///   GIT_FILTER_CRLF with priority 0 - GIT_FILTER_IDENT with
        ///   priority 100
        ///  </para>
        ///
        ///  <para>
        ///   Currently the filter registry is not thread safe, so any
        ///   registering or deregistering of filters must be done
        ///   outside of any possible usage of the filters (i.e.
        ///   during application setup or shutdown).
        ///  </para>
        /// </summary>
        /// <param name="name">
        ///  A name by which the filter can be referenced. Attempting to
        ///  register with an in-use name will return GIT_EEXISTS.
        /// </param>
        /// <param name="filter">
        ///  The filter definition. This pointer will be stored as is by
        ///  libgit2 so it must be a durable allocation (either static or
        ///  on the heap).
        /// </param>
        /// <param name="priority">
        ///  The priority for filter application
        /// </param>
        /// <returns>
        ///  0 on successful registry, error code < 0 on failure
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_filter_register(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            ref git_filter filter,
            int priority);

        /// <summary>
        ///  Remove the filter with the given name
        ///
        ///  <para>
        ///   Attempting to remove the builtin libgit2 filters is not
        ///   permitted and will return an error.
        ///  </para>
        ///
        ///  <para>
        ///   Currently the filter registry is not thread safe, so any
        ///   registering or deregistering of filters must be done
        ///   outside of any possible usage of the filters (i.e.
        ///   during application setup or shutdown).
        ///  </para>
        /// </summary>
        /// <param name="name">
        ///  The name under which the filter was registered
        /// </param>
        /// <returns>0 on success, error code < 0 on failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_filter_unregister(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);
    }
}
