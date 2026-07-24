using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public sealed class CommitGraphHandle : SafeHandle
    {
        public CommitGraphHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe CommitGraphHandle(git_commit_graph* nativeCommitGraph) :
            base((IntPtr)nativeCommitGraph, true)
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
            libgit2.git_commit_graph_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public sealed class CommitGraphWriterHandle : SafeHandle
    {
        public CommitGraphWriterHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe CommitGraphWriterHandle(git_commit_graph_writer* nativeCommitGraphWriter) :
            base((IntPtr)nativeCommitGraphWriter, true)
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
            libgit2.git_commit_graph_writer_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Opens a `git_commit_graph` from a path to an objects
        ///  directory.
        ///
        ///  <para>
        ///   This finds, opens, and validates the `commit-graph`
        ///   file.
        ///  </para>
        /// </summary>
        /// <param name="cgraph_out">
        ///  the `git_commit_graph` struct to initialize.
        /// </param>
        /// <param name="objects_dir">
        ///  the path to a git objects directory.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_graph_open(
            out CommitGraphHandle cgraph_out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string objects_dir);

        /// <summary>
        ///  Frees commit-graph data. This should only be called when
        ///  memory allocated using `git_commit_graph_open` is not
        ///  returned to libgit2 because it was not associated with the
        ///  ODB through a successful call to `git_odb_set_commit_graph`.
        /// </summary>
        /// <param name="cgraph">
        ///  the commit-graph object to free. If NULL, no action is
        ///  taken.
        /// </param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_commit_graph_free(
            IntPtr cgraph);

        /// <summary>
        ///  Create a new writer for `commit-graph` files.
        /// </summary>
        /// <param name="_out">Location to store the writer pointer.</param>
        /// <param name="objects_info_dir">
        ///  The `objects/info` directory. The `commit-graph` file will
        ///  be written in this directory.
        /// </param>
        /// <param name="options">
        ///  The options for the commit graph writer.
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_graph_writer_new(
            out CommitGraphWriterHandle _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string objects_info_dir,
            ref git_commit_graph_writer_options options);

        /// <summary>
        ///  Free the commit-graph writer and its resources.
        /// </summary>
        /// <param name="w">
        ///  The writer to free. If NULL no action is taken.
        /// </param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_commit_graph_writer_free(
            IntPtr w);

        /// <summary>
        ///  Add an `.idx` file (associated to a packfile) to the writer.
        /// </summary>
        /// <param name="w">The writer.</param>
        /// <param name="repo">
        ///  The repository that owns the `.idx` file.
        /// </param>
        /// <param name="idx_path">The path of an `.idx` file.</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_graph_writer_add_index_file(
            CommitGraphWriterHandle w,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string idx_path);

        /// <summary>
        ///  Add a revwalk to the writer. This will add all the commits
        ///  from the revwalk to the commit-graph.
        /// </summary>
        /// <param name="w">The writer.</param>
        /// <param name="walk">The git_revwalk.</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_graph_writer_add_revwalk(
            CommitGraphWriterHandle w,
            RevwalkHandle walk);

        /// <summary>
        ///  Write a `commit-graph` file to a file.
        /// </summary>
        /// <param name="w">The writer</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_graph_writer_commit(
            CommitGraphWriterHandle w);

        /// <summary>
        ///  Dump the contents of the `commit-graph` to an in-memory
        ///  buffer.
        /// </summary>
        /// <param name="buffer">
        ///  Buffer where to store the contents of the `commit-graph`.
        /// </param>
        /// <param name="w">The writer.</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_graph_writer_dump(
            out git_buf buffer,
            CommitGraphWriterHandle w);
    }
}
