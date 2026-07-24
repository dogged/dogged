using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Stages that are reported by the packbuilder progress callback.
    /// </summary>
    public enum git_packbuilder_stage_t
    {
        GIT_PACKBUILDER_ADDING_OBJECTS = 0,
        GIT_PACKBUILDER_DELTAFICATION = 1,
    }

    /// <summary>
    ///  Callback used to iterate over packed objects
    /// </summary>
    /// <param name="buf">A pointer to the object's data</param>
    /// <param name="size">The size of the underlying object</param>
    /// <param name="payload">
    ///  Payload passed to git_packbuilder_foreach
    /// </param>
    /// <returns>non-zero to terminate the iteration</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_packbuilder_foreach_cb(
        IntPtr buf,
        UIntPtr size,
        IntPtr payload);

    /// <summary>
    ///  Packbuilder progress notification function.
    /// </summary>
    /// <param name="stage">the stage of the packbuilder</param>
    /// <param name="current">the current object</param>
    /// <param name="total">the total number of objects</param>
    /// <param name="payload">the callback payload</param>
    /// <returns>0 on success or an error code</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_packbuilder_progress(
        int stage,
        uint current,
        uint total,
        IntPtr payload);

    /// <summary>
    ///  Representation of a git packbuilder
    /// </summary>
    public struct git_packbuilder
    {
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Initialize a new packbuilder
        /// </summary>
        /// <param name="_out">The new packbuilder object</param>
        /// <param name="repo">The repository</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_packbuilder_new(
            out git_packbuilder* _out,
            git_repository* repo);

        /// <summary>
        ///  Set number of threads to spawn
        ///
        ///  <para>
        ///   By default, libgit2 won't spawn any threads at all; when
        ///   set to 0, libgit2 will autodetect the number of CPUs.
        ///  </para>
        /// </summary>
        /// <param name="pb">The packbuilder</param>
        /// <param name="n">Number of threads to spawn</param>
        /// <returns>number of actual threads to be used</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe uint git_packbuilder_set_threads(
            git_packbuilder* pb,
            uint n);

        /// <summary>
        ///  Insert a single object
        ///
        ///  <para>
        ///   For an optimal pack it's mandatory to insert objects in
        ///   recency order, commits followed by trees and blobs.
        ///  </para>
        /// </summary>
        /// <param name="pb">The packbuilder</param>
        /// <param name="id">The oid of the commit</param>
        /// <param name="name">The name; might be NULL</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_packbuilder_insert(
            git_packbuilder* pb,
            ref git_oid id,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Insert a root tree object
        ///
        ///  <para>
        ///   This will add the tree as well as all referenced trees
        ///   and blobs.
        ///  </para>
        /// </summary>
        /// <param name="pb">The packbuilder</param>
        /// <param name="id">The oid of the root tree</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_packbuilder_insert_tree(
            git_packbuilder* pb,
            ref git_oid id);

        /// <summary>
        ///  Insert a commit object
        ///
        ///  <para>
        ///   This will add a commit as well as the completed
        ///   referenced tree.
        ///  </para>
        /// </summary>
        /// <param name="pb">The packbuilder</param>
        /// <param name="id">The oid of the commit</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_packbuilder_insert_commit(
            git_packbuilder* pb,
            ref git_oid id);

        /// <summary>
        ///  Insert objects as given by the walk
        ///
        ///  <para>
        ///   Those commits and all objects they reference will be
        ///   inserted into the packbuilder.
        ///  </para>
        /// </summary>
        /// <param name="pb">the packbuilder</param>
        /// <param name="walk">
        ///  the revwalk to use to fill the packbuilder
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_packbuilder_insert_walk(
            git_packbuilder* pb,
            git_revwalk* walk);

        /// <summary>
        ///  Recursively insert an object and its referenced objects
        ///
        ///  <para>
        ///   Insert the object as well as any object it references.
        ///  </para>
        /// </summary>
        /// <param name="pb">the packbuilder</param>
        /// <param name="id">the id of the root object to insert</param>
        /// <param name="name">optional name for the object</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_packbuilder_insert_recur(
            git_packbuilder* pb,
            ref git_oid id,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Write the contents of the packfile to an in-memory buffer
        ///
        ///  <para>
        ///   The contents of the buffer will become a valid packfile,
        ///   even though there will be no attached index
        ///  </para>
        /// </summary>
        /// <param name="buf">Buffer where to write the packfile</param>
        /// <param name="pb">The packbuilder</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_packbuilder_write_buf(
            ref git_buf buf,
            git_packbuilder* pb);

        /// <summary>
        ///  Write the new pack and corresponding index file to path.
        /// </summary>
        /// <param name="pb">The packbuilder</param>
        /// <param name="path">
        ///  Path to the directory where the packfile and index should be
        ///  stored, or NULL for default location
        /// </param>
        /// <param name="mode">
        ///  permissions to use creating a packfile or 0 for defaults
        /// </param>
        /// <param name="progress_cb">
        ///  function to call with progress information from the indexer
        ///  (optional)
        /// </param>
        /// <param name="progress_cb_payload">
        ///  payload for the progress callback (optional)
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_packbuilder_write(
            git_packbuilder* pb,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            uint mode,
            git_indexer_progress_cb progress_cb,
            IntPtr progress_cb_payload);

        /// <summary>
        ///  Get the packfile's hash
        ///
        ///  <para>
        ///   A packfile's name is derived from the sorted hashing of
        ///   all object names. This is only correct after the
        ///   packfile has been written.
        ///  </para>
        /// </summary>
        /// <param name="pb">The packbuilder object</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_packbuilder_hash(
            git_packbuilder* pb);

        /// <summary>
        ///  Get the unique name for the resulting packfile.
        ///
        ///  <para>
        ///   The packfile's name is derived from the packfile's
        ///   content. This is only correct after the packfile has
        ///   been written.
        ///  </para>
        /// </summary>
        /// <param name="pb">the packbuilder instance</param>
        /// <returns>a NUL terminated string for the packfile name</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_packbuilder_name(
            git_packbuilder* pb);

        /// <summary>
        ///  Create the new pack and pass each object to the callback
        /// </summary>
        /// <param name="pb">the packbuilder</param>
        /// <param name="cb">
        ///  the callback to call with each packed object's buffer
        /// </param>
        /// <param name="payload">the callback's data</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_packbuilder_foreach(
            git_packbuilder* pb,
            git_packbuilder_foreach_cb cb,
            IntPtr payload);

        /// <summary>
        ///  Get the total number of objects the packbuilder will write
        ///  out
        /// </summary>
        /// <param name="pb">the packbuilder</param>
        /// <returns>the number of objects in the packfile</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_packbuilder_object_count(
            git_packbuilder* pb);

        /// <summary>
        ///  Get the number of objects the packbuilder has already
        ///  written out
        /// </summary>
        /// <param name="pb">the packbuilder</param>
        /// <returns>
        ///  the number of objects which have already been written
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_packbuilder_written(
            git_packbuilder* pb);

        /// <summary>
        ///  Set the callbacks for a packbuilder
        /// </summary>
        /// <param name="pb">The packbuilder object</param>
        /// <param name="progress_cb">
        ///  Function to call with progress information during pack
        ///  building. Be aware that this is called inline with pack
        ///  building operations, so performance may be affected. When
        ///  progress_cb returns an error, the pack building process will
        ///  be aborted and the error will be returned from the invoked
        ///  function. `pb` must then be freed.
        /// </param>
        /// <param name="progress_cb_payload">
        ///  Payload for progress callback.
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_packbuilder_set_callbacks(
            git_packbuilder* pb,
            git_packbuilder_progress progress_cb,
            IntPtr progress_cb_payload);

        /// <summary>
        ///  Free the packbuilder and all associated data
        /// </summary>
        /// <param name="pb">The packbuilder</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_packbuilder_free(
            git_packbuilder* pb);
    }
}
