using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public sealed class FilterListHandle : SafeHandle
    {
        public FilterListHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe FilterListHandle(git_filter_list* nativeFilterList) :
            base((IntPtr)nativeFilterList, true)
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
            libgit2.git_filter_list_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public static partial class libgit2
    {
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
            out FilterListHandle filters,
            RepositoryHandle repo,
            BlobHandle blob,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            git_filter_mode_t mode,
            git_filter_flag_t flags);

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
            out FilterListHandle filters,
            RepositoryHandle repo,
            BlobHandle blob,
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
            FilterListHandle filters,
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
            FilterListHandle filters,
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
            FilterListHandle filters,
            RepositoryHandle repo,
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
            FilterListHandle filters,
            BlobHandle blob);

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
            FilterListHandle filters,
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
            FilterListHandle filters,
            RepositoryHandle repo,
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
            FilterListHandle filters,
            BlobHandle blob,
            ref git_writestream target);

        /// <summary>
        ///  Free a git_filter_list
        /// </summary>
        /// <param name="filters">
        ///  A git_filter_list created by `git_filter_list_load`
        /// </param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_filter_list_free(
            IntPtr filters);
    }
}
