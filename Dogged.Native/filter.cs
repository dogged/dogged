using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Filters are applied in one of two directions: smudging - which
    ///  is exporting a file from the Git object database to the working
    ///  directory, and cleaning - which is importing a file from the
    ///  working directory to the Git object database. These values
    ///  control which direction of change is being applied.
    /// </summary>
    public enum git_filter_mode_t
    {
        GIT_FILTER_TO_WORKTREE = 0,
        GIT_FILTER_SMUDGE = 0,
        GIT_FILTER_TO_ODB = 1,
        GIT_FILTER_CLEAN = 1,
    }

    /// <summary>
    ///  Filter option flags.
    /// </summary>
    [Flags]
    public enum git_filter_flag_t
    {
        GIT_FILTER_DEFAULT = 0,

        /// <summary>
        ///  Don't error for `safecrlf` violations, allow them to
        ///  continue.
        /// </summary>
        GIT_FILTER_ALLOW_UNSAFE = 1,

        /// <summary>
        ///  Don't load `/etc/gitattributes` (or the system equivalent)
        /// </summary>
        GIT_FILTER_NO_SYSTEM_ATTRIBUTES = 2,

        /// <summary>
        ///  Load attributes from `.gitattributes` in the root of HEAD
        /// </summary>
        GIT_FILTER_ATTRIBUTES_FROM_HEAD = 4,

        /// <summary>
        ///  Load attributes from `.gitattributes` in a given commit.
        ///  This can only be specified in a `git_filter_options`.
        /// </summary>
        GIT_FILTER_ATTRIBUTES_FROM_COMMIT = 8,
    }

    /// <summary>
    ///  Filtering options
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_filter_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_FILTER_OPTIONS_VERSION = 1;


        public uint version;

        /// <summary>
        ///  See `git_filter_flag_t` above
        /// </summary>
        public git_filter_flag_t flags;

        public IntPtr commit_id;

        /// <summary>
        ///  The commit to load attributes from, when
        ///  `GIT_FILTER_ATTRIBUTES_FROM_COMMIT` is specified.
        /// </summary>
        public git_oid attr_commit_id;
    }

    /// <summary>
    ///  List of filters to be applied
    ///
    ///  <para>
    ///   This represents a list of filters to be applied to a file /
    ///   blob. You can build the list with one call, apply it with
    ///   another, and dispose it with a third. In typical usage,
    ///   there are not many occasions where a git_filter_list is
    ///   needed directly since the library will generally handle
    ///   conversions for you, but it can be convenient to be able to
    ///   build and apply the list sometimes.
    ///  </para>
    /// </summary>
    public struct git_filter_list
    {
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Initialize git_filter_options structure
        ///
        ///  <para>
        ///   Initialize a `git_filter_options` with default values.
        ///   Equivalent to creating an instance with
        ///   GIT_FILTER_OPTIONS_VERSION.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_filter_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass `GIT_FILTER_OPTIONS_VERSION`
        /// </param>
        /// <returns>0 on success or -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_filter_options_init(
            ref git_filter_options opts,
            uint version);

        /// <summary>
        ///  Load the filter list for a given path.
        ///
        ///  <para>
        ///   This will return 0 (success) but set the output
        ///   git_filter_list to NULL if no filters are requested for
        ///   the given file.
        ///  </para>
        /// </summary>
        /// <param name="filters">
        ///  Output newly created git_filter_list (or NULL)
        /// </param>
        /// <param name="repo">
        ///  Repository object that contains `path`
        /// </param>
        /// <param name="blob">
        ///  The blob to which the filter will be applied (if known)
        /// </param>
        /// <param name="path">
        ///  Relative path of the file to be filtered
        /// </param>
        /// <param name="mode">
        ///  Filtering direction (WT->ODB or ODB->WT)
        /// </param>
        /// <param name="flags">
        ///  Combination of `git_filter_flag_t` flags
        /// </param>
        /// <returns>
        ///  0 on success (which could still return NULL if no filters
        ///  are needed for the requested file), < 0 on error
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_filter_list_load(
            out git_filter_list* filters,
            git_repository* repo,
            git_blob* blob,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            git_filter_mode_t mode,
            uint flags);

        /// <summary>
        ///  Load the filter list for a given path.
        ///
        ///  <para>
        ///   This will return 0 (success) but set the output
        ///   git_filter_list to NULL if no filters are requested for
        ///   the given file.
        ///  </para>
        /// </summary>
        /// <param name="filters">
        ///  Output newly created git_filter_list (or NULL)
        /// </param>
        /// <param name="repo">
        ///  Repository object that contains `path`
        /// </param>
        /// <param name="blob">
        ///  The blob to which the filter will be applied (if known)
        /// </param>
        /// <param name="path">
        ///  Relative path of the file to be filtered
        /// </param>
        /// <param name="mode">
        ///  Filtering direction (WT->ODB or ODB->WT)
        /// </param>
        /// <param name="opts">
        ///  The `git_filter_options` to use when loading filters
        /// </param>
        /// <returns>
        ///  0 on success (which could still return NULL if no filters
        ///  are needed for the requested file), < 0 on error
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_filter_list_load_ext(
            out git_filter_list* filters,
            git_repository* repo,
            git_blob* blob,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            git_filter_mode_t mode,
            ref git_filter_options opts);

        /// <summary>
        ///  Query the filter list to see if a given filter (by name)
        ///  will run. The built-in filters "crlf" and "ident" can be
        ///  queried, otherwise this is the name of the filter specified
        ///  by the filter attribute.
        ///
        ///  <para>
        ///   This will return 0 if the given filter is not in the
        ///   list, or 1 if the filter will be applied.
        ///  </para>
        /// </summary>
        /// <param name="filters">A loaded git_filter_list (or NULL)</param>
        /// <param name="name">The name of the filter to query</param>
        /// <returns>1 if the filter is in the list, 0 otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_filter_list_contains(
            git_filter_list* filters,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Apply filter list to a data buffer.
        /// </summary>
        /// <param name="_out">
        ///  Buffer to store the result of the filtering
        /// </param>
        /// <param name="filters">A loaded git_filter_list (or NULL)</param>
        /// <param name="_in">Buffer containing the data to filter</param>
        /// <param name="in_len">The length of the input buffer</param>
        /// <returns>0 on success, an error code otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_filter_list_apply_to_buffer(
            ref git_buf _out,
            git_filter_list* filters,
            byte* _in,
            UIntPtr in_len);

        /// <summary>
        ///  Apply a filter list to the contents of a file on disk
        /// </summary>
        /// <param name="_out">
        ///  buffer into which to store the filtered file
        /// </param>
        /// <param name="filters">the list of filters to apply</param>
        /// <param name="repo">
        ///  the repository in which to perform the filtering
        /// </param>
        /// <param name="path">
        ///  the path of the file to filter, a relative path will be
        ///  taken as relative to the workdir
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_filter_list_apply_to_file(
            ref git_buf _out,
            git_filter_list* filters,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path);

        /// <summary>
        ///  Apply a filter list to the contents of a blob
        /// </summary>
        /// <param name="_out">
        ///  buffer into which to store the filtered file
        /// </param>
        /// <param name="filters">the list of filters to apply</param>
        /// <param name="blob">the blob to filter</param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_filter_list_apply_to_blob(
            ref git_buf _out,
            git_filter_list* filters,
            git_blob* blob);

        /// <summary>
        ///  Apply a filter list to an arbitrary buffer as a stream
        /// </summary>
        /// <param name="filters">the list of filters to apply</param>
        /// <param name="buffer">the buffer to filter</param>
        /// <param name="len">the size of the buffer</param>
        /// <param name="target">
        ///  the stream into which the data will be written
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_filter_list_stream_buffer(
            git_filter_list* filters,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string buffer,
            UIntPtr len,
            ref git_writestream target);

        /// <summary>
        ///  Apply a filter list to a file as a stream
        /// </summary>
        /// <param name="filters">the list of filters to apply</param>
        /// <param name="repo">
        ///  the repository in which to perform the filtering
        /// </param>
        /// <param name="path">
        ///  the path of the file to filter, a relative path will be
        ///  taken as relative to the workdir
        /// </param>
        /// <param name="target">
        ///  the stream into which the data will be written
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_filter_list_stream_file(
            git_filter_list* filters,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            ref git_writestream target);

        /// <summary>
        ///  Apply a filter list to a blob as a stream
        /// </summary>
        /// <param name="filters">the list of filters to apply</param>
        /// <param name="blob">the blob to filter</param>
        /// <param name="target">
        ///  the stream into which the data will be written
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_filter_list_stream_blob(
            git_filter_list* filters,
            git_blob* blob,
            ref git_writestream target);

        /// <summary>
        ///  Free a git_filter_list
        /// </summary>
        /// <param name="filters">
        ///  A git_filter_list created by `git_filter_list_load`
        /// </param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_filter_list_free(
            git_filter_list* filters);
    }
}
