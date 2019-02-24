using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    /// The progress of the indexer.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_indexer_progress
    {
        /// <summary>
        /// Number of objects in the packfile being indexed
        /// </summary>
        public uint total_objects;

        /// <summary>
        /// Number of received objects that have been hashed
        /// </summary>
        public uint indexed_objects;

        /// <summary>
        /// Number of objects which have been downloaded
        /// </summary>
        public uint received_objects;

        /// <summary>
        /// Number of locally-available objects that have been injected
        /// in order to fix a thin pack
        /// </summary>
        public uint local_objects;

        /// <summary>
        /// Number of deltas in the packfile being indexed
        /// </summary>
        public uint total_deltas;

        /// <summary>
        /// Number of received deltas that have been indexed
        /// </summary>
        public uint indexed_deltas;

        /// <summary>
        /// Size of the packfile received up to now
        /// </summary>
        public UIntPtr received_bytes;
    }

    /// <summary>
    /// Type for progress callbacks during indexing.  Return a value less
    /// than zero to cancel the indexing or download.
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int git_indexer_progress_cb(
        ref git_indexer_progress stats,
        UIntPtr payload);
}
