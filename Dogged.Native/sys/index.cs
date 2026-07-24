using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Representation of a rename conflict entry in the index.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_index_name_entry
    {
        public IntPtr ancestor;

        public IntPtr ours;

        public IntPtr theirs;
    }

    /// <summary>
    ///  Representation of a resolve undo entry in the index.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_index_reuc_entry
    {
       public uint ancestor_mode;
       public uint our_mode;
       public uint their_mode;
    
       public git_oid ancestor_oid;
       public git_oid our_oid;
       public git_oid their_oid;
    
       public char* path;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Get the count of filename conflict entries currently in the
        ///  index.
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <returns>
        ///  integer of count of current filename conflict entries
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_index_name_entrycount(
            git_index* index);

        /// <summary>
        ///  Get a filename conflict entry from the index.
        ///
        ///  <para>
        ///   The returned entry is read-only and should not be
        ///   modified or freed by the caller.
        ///  </para>
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <param name="n">the position of the entry</param>
        /// <returns>
        ///  a pointer to the filename conflict entry; NULL if out of
        ///  bounds
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_index_name_entry* git_index_name_get_byindex(
            git_index* index,
            UIntPtr n);

        /// <summary>
        ///  Record the filenames involved in a rename conflict.
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <param name="ancestor">
        ///  the path of the file as it existed in the ancestor
        /// </param>
        /// <param name="ours">
        ///  the path of the file as it existed in our tree
        /// </param>
        /// <param name="theirs">
        ///  the path of the file as it existed in their tree
        /// </param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_name_add(
            git_index* index,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string ancestor,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string ours,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string theirs);

        /// <summary>
        ///  Remove all filename conflict entries.
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_name_clear(
            git_index* index);

        /// <summary>
        ///  Get the count of resolve undo entries currently in the
        ///  index.
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <returns>
        ///  integer of count of current resolve undo entries
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_index_reuc_entrycount(
            git_index* index);

        /// <summary>
        ///  Finds the resolve undo entry that points to the given path
        ///  in the Git index.
        /// </summary>
        /// <param name="at_pos">
        ///  the address to which the position of the reuc entry is
        ///  written (optional)
        /// </param>
        /// <param name="index">an existing index object</param>
        /// <param name="path">path to search</param>
        /// <returns>0 if found, < 0 otherwise (GIT_ENOTFOUND)</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_reuc_find(
            out UIntPtr at_pos,
            git_index* index,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path);

        /// <summary>
        ///  Get a resolve undo entry from the index.
        ///
        ///  <para>
        ///   The returned entry is read-only and should not be
        ///   modified or freed by the caller.
        ///  </para>
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <param name="path">path to search</param>
        /// <returns>the resolve undo entry; NULL if not found</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_index_reuc_entry* git_index_reuc_get_bypath(
            git_index* index,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path);

        /// <summary>
        ///  Get a resolve undo entry from the index.
        ///
        ///  <para>
        ///   The returned entry is read-only and should not be
        ///   modified or freed by the caller.
        ///  </para>
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <param name="n">the position of the entry</param>
        /// <returns>
        ///  a pointer to the resolve undo entry; NULL if out of bounds
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_index_reuc_entry* git_index_reuc_get_byindex(
            git_index* index,
            UIntPtr n);

        /// <summary>
        ///  Adds a resolve undo entry for a file based on the given
        ///  parameters.
        ///
        ///  <para>
        ///   The resolve undo entry contains the OIDs of files that
        ///   were involved in a merge conflict after the conflict has
        ///   been resolved. This allows conflicts to be re-resolved
        ///   later.
        ///  </para>
        ///
        ///  <para>
        ///   If there exists a resolve undo entry for the given path
        ///   in the index, it will be removed.
        ///  </para>
        ///
        ///  <para>
        ///   This method will fail in bare index instances.
        ///  </para>
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <param name="path">filename to add</param>
        /// <param name="ancestor_mode">mode of the ancestor file</param>
        /// <param name="ancestor_id">oid of the ancestor file</param>
        /// <param name="our_mode">mode of our file</param>
        /// <param name="our_id">oid of our file</param>
        /// <param name="their_mode">mode of their file</param>
        /// <param name="their_id">oid of their file</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_reuc_add(
            git_index* index,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            int ancestor_mode,
            ref git_oid ancestor_id,
            int our_mode,
            ref git_oid our_id,
            int their_mode,
            ref git_oid their_id);

        /// <summary>
        ///  Remove an resolve undo entry from the index
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <param name="n">
        ///  position of the resolve undo entry to remove
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_reuc_remove(
            git_index* index,
            UIntPtr n);

        /// <summary>
        ///  Remove all resolve undo entries from the index
        /// </summary>
        /// <param name="index">an existing index object</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_index_reuc_clear(
            git_index* index);
    }
}
