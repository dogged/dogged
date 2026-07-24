using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  A git indexer object
    /// </summary>
    public struct git_indexer
    {
    }

    /// <summary>
    ///  This structure is used to provide callers information about the
    ///  progress of indexing a packfile, either directly or part of a
    ///  fetch or clone that downloads a packfile.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_indexer_progress
    {
        /// <summary>
        ///  number of objects in the packfile being indexed
        /// </summary>
        public uint total_objects;

        /// <summary>
        ///  received objects that have been hashed
        /// </summary>
        public uint indexed_objects;

        /// <summary>
        ///  received_objects: objects which have been downloaded
        /// </summary>
        public uint received_objects;

        /// <summary>
        ///  locally-available objects that have been injected in order
        ///  to fix a thin pack
        /// </summary>
        public uint local_objects;

        /// <summary>
        ///  number of deltas in the packfile being indexed
        /// </summary>
        public uint total_deltas;

        /// <summary>
        ///  received deltas that have been indexed
        /// </summary>
        public uint indexed_deltas;

        /// <summary>
        ///  size of the packfile received up to now
        /// </summary>
        public UIntPtr received_bytes;
    }

    /// <summary>
    ///  Type for progress callbacks during indexing. Return a value less
    ///  than zero to cancel the indexing or download.
    /// </summary>
    /// <param name="stats">
    ///  Structure containing information about the state of the transfer
    /// </param>
    /// <param name="payload">Payload provided by caller</param>
    /// <returns>0 on success or an error code</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_indexer_progress_cb(
        ref git_indexer_progress stats,
        IntPtr payload);

    /// <summary>
    ///  Options for indexer configuration
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_indexer_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_INDEXER_OPTIONS_VERSION = 1;


        public uint version;

        /// <summary>
        ///  progress_cb function to call with progress information
        /// </summary>
        public git_indexer_progress_cb progress_cb;

        /// <summary>
        ///  progress_cb_payload payload for the progress callback
        /// </summary>
        public IntPtr progress_cb_payload;

        /// <summary>
        ///  Do connectivity checks for the received pack
        /// </summary>
        public byte verify;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Initializes a `git_indexer_options` with default values.
        ///  Equivalent to creating an instance with
        ///  GIT_INDEXER_OPTIONS_INIT.
        /// </summary>
        /// <param name="opts">
        ///  the `git_indexer_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  Version of struct; pass `GIT_INDEXER_OPTIONS_VERSION`
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_indexer_options_init(
            ref git_indexer_options opts,
            uint version);

        /// <summary>
        ///  Create a new indexer instance
        /// </summary>
        /// <param name="_out">where to store the indexer instance</param>
        /// <param name="path">
        ///  to the directory where the packfile should be stored
        /// </param>
        /// <param name="mode">
        ///  permissions to use creating packfile or 0 for defaults
        /// </param>
        /// <param name="odb">
        ///  object database from which to read base objects when fixing
        ///  thin packs. Pass NULL if no thin pack is expected (an error
        ///  will be returned if there are bases missing)
        /// </param>
        /// <param name="opts">
        ///  Optional structure containing additional options. See
        ///  `git_indexer_options` above.
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_indexer_new(
            out git_indexer* _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            uint mode,
            git_odb* odb,
            ref git_indexer_options opts);

        /// <summary>
        ///  Add data to the indexer
        /// </summary>
        /// <param name="idx">the indexer</param>
        /// <param name="data">the data to add</param>
        /// <param name="size">the size of the data in bytes</param>
        /// <param name="stats">stat storage</param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_indexer_append(
            git_indexer* idx,
            IntPtr data,
            UIntPtr size,
            ref git_indexer_progress stats);

        /// <summary>
        ///  Finalize the pack and index
        ///
        ///  <para>
        ///   Resolve any pending deltas and write out the index file
        ///  </para>
        /// </summary>
        /// <param name="idx">the indexer</param>
        /// <param name="stats">Stat storage.</param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_indexer_commit(
            git_indexer* idx,
            ref git_indexer_progress stats);

        /// <summary>
        ///  Get the packfile's hash
        ///
        ///  <para>
        ///   A packfile's name is derived from the sorted hashing of
        ///   all object names. This is only correct after the index
        ///   has been finalized.
        ///  </para>
        /// </summary>
        /// <param name="idx">the indexer instance</param>
        /// <returns>the packfile's hash</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_indexer_hash(
            git_indexer* idx);

        /// <summary>
        ///  Get the unique name for the resulting packfile.
        ///
        ///  <para>
        ///   The packfile's name is derived from the packfile's
        ///   content. This is only correct after the index has been
        ///   finalized.
        ///  </para>
        /// </summary>
        /// <param name="idx">the indexer instance</param>
        /// <returns>a NUL terminated string for the packfile name</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_indexer_name(
            git_indexer* idx);

        /// <summary>
        ///  Free the indexer and its resources
        /// </summary>
        /// <param name="idx">the indexer to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_indexer_free(
            git_indexer* idx);
    }
}
