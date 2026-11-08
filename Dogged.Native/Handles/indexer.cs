using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public sealed class IndexerHandle : SafeHandle
    {
        public IndexerHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe IndexerHandle(git_indexer* nativeIndexer) :
            base((IntPtr)nativeIndexer, true)
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
            libgit2.git_indexer_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Create a new indexer instance
        /// </summary>
        /// <param name="_out">where to store the indexer instance</param>
        /// <param name="path">
        ///  to the directory where the packfile should be stored
        /// </param>
        /// <param name="opts">
        ///  the options to create the indexer with
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_indexer_new(
            IndexerHandle _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
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
            IndexerHandle idx,
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
            IndexerHandle idx,
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
            IndexerHandle idx);

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
            IndexerHandle idx);

        /// <summary>
        ///  Free the indexer and its resources
        /// </summary>
        /// <param name="idx">the indexer to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_indexer_free(
            IntPtr idx);
    }
}
