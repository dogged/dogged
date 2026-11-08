using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Options structure for `git_commit_graph_open_new`.
    ///
    ///  <para>
    ///   Initialize with `GIT_COMMIT_GRAPH_OPEN_OPTIONS_INIT`.
    ///   Alternatively, you can use
    ///   `git_commit_graph_open_options_init`.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_commit_graph_open_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_COMMIT_GRAPH_OPEN_OPTIONS_VERSION = 1;


        public uint version;

        /// <summary>
        ///  The object ID type that this commit graph contains.
        /// </summary>
        public git_oid_t oid_type;
    }

    /// <summary>
    ///  The strategy to use when adding a new set of commits to a
    ///  pre-existing commit-graph chain.
    /// </summary>
    public enum git_commit_graph_split_strategy_t
    {
        /// <summary>
        ///  Do not split commit-graph files. The other split
        ///  strategy-related option fields are ignored.
        /// </summary>
        GIT_COMMIT_GRAPH_SPLIT_STRATEGY_SINGLE_FILE = 0,
    }

    /// <summary>
    ///  Options structure for `git_commit_graph_writer_new`.
    ///
    ///  <para>
    ///   Initialize with `GIT_COMMIT_GRAPH_WRITER_OPTIONS_INIT`.
    ///   Alternatively, you can use
    ///   `git_commit_graph_writer_options_init`.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_commit_graph_writer_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_COMMIT_GRAPH_WRITER_OPTIONS_VERSION = 1;


        public uint version;

        /// <summary>
        ///  The object ID type that this commit graph contains.
        /// </summary>
        public git_oid_t oid_type;

        /// <summary>
        ///  The strategy to use when adding new commits to a
        ///  pre-existing commit-graph chain.
        /// </summary>
        public git_commit_graph_split_strategy_t split_strategy;

        /// <summary>
        ///  The number of commits in level N is less than X times the
        ///  number of commits in level N + 1. Default is 2.
        /// </summary>
        public float size_multiple;

        /// <summary>
        ///  The number of commits in level N + 1 is more than C commits.
        ///  Default is 64000.
        /// </summary>
        public UIntPtr max_commits;
    }

    /// <summary>
    ///  A git commit-graph
    /// </summary>
    public struct git_commit_graph
    {
    }

    /// <summary>
    ///  a writer for commit-graph files.
    /// </summary>
    public struct git_commit_graph_writer
    {
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Initialize git_commit_graph_open_options structure
        ///
        ///  <para>
        ///   Initializes a `git_commit_graph_open_options` with
        ///   default values. Equivalent to creating an instance with
        ///   `GIT_COMMIT_GRAPH_OPEN_OPTIONS_INIT`.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_commit_graph_open_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass
        ///  `GIT_COMMIT_GRAPH_OPEN_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_graph_open_options_init(
            ref git_commit_graph_open_options opts,
            uint version);

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
        /// <param name="options">options for opening the graph</param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_graph_open(
            out git_commit_graph* cgraph_out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string objects_dir,
            ref git_commit_graph_open_options options);

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
            git_commit_graph* cgraph);

        /// <summary>
        ///  Initialize git_commit_graph_writer_options structure
        ///
        ///  <para>
        ///   Initializes a `git_commit_graph_writer_options` with
        ///   default values. Equivalent to creating an instance with
        ///   `GIT_COMMIT_GRAPH_WRITER_OPTIONS_INIT`.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_commit_graph_writer_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass
        ///  `GIT_COMMIT_GRAPH_WRITER_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_graph_writer_options_init(
            ref git_commit_graph_writer_options opts,
            uint version);

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
            out git_commit_graph_writer* _out,
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
            git_commit_graph_writer* w);

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
            git_commit_graph_writer* w,
            git_repository* repo,
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
            git_commit_graph_writer* w,
            git_revwalk* walk);

        /// <summary>
        ///  Write a `commit-graph` file to a file.
        /// </summary>
        /// <param name="w">The writer</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_graph_writer_commit(
            git_commit_graph_writer* w);

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
            git_commit_graph_writer* w);
    }
}
