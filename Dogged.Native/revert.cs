using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Options for revert
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_revert_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_REVERT_OPTIONS_VERSION = 1;


        public uint version;

        /// <summary>
        ///  For merge commits, the "mainline" is treated as the parent.
        /// </summary>
        public uint mainline;

        /// <summary>
        ///  Options for the merging
        /// </summary>
        public git_merge_options merge_opts;

        /// <summary>
        ///  Options for the checkout
        /// </summary>
        public git_checkout_options checkout_opts;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Initialize git_revert_options structure
        ///
        ///  <para>
        ///   Initializes a `git_revert_options` with default values.
        ///   Equivalent to creating an instance with
        ///   `GIT_REVERT_OPTIONS_INIT`.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_revert_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass `GIT_REVERT_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_revert_options_init(
            ref git_revert_options opts,
            uint version);

        /// <summary>
        ///  Reverts the given commit against the given "our" commit,
        ///  producing an index that reflects the result of the revert.
        ///
        ///  <para>
        ///   The returned index must be freed explicitly with
        ///   `git_index_free`.
        ///  </para>
        /// </summary>
        /// <param name="_out">pointer to store the index result in</param>
        /// <param name="repo">
        ///  the repository that contains the given commits
        /// </param>
        /// <param name="revert_commit">the commit to revert</param>
        /// <param name="our_commit">
        ///  the commit to revert against (eg, HEAD)
        /// </param>
        /// <param name="mainline">
        ///  the parent of the revert commit, if it is a merge
        /// </param>
        /// <param name="merge_options">
        ///  the merge options (or null for defaults)
        /// </param>
        /// <returns>zero on success, -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_revert_commit(
            out git_index* _out,
            git_repository* repo,
            git_commit* revert_commit,
            git_commit* our_commit,
            uint mainline,
            ref git_merge_options merge_options);

        /// <summary>
        ///  Reverts the given commit, producing changes in the index and
        ///  working directory.
        /// </summary>
        /// <param name="repo">the repository to revert</param>
        /// <param name="commit">the commit to revert</param>
        /// <param name="given_opts">
        ///  the revert options (or null for defaults)
        /// </param>
        /// <returns>zero on success, -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_revert(
            git_repository* repo,
            git_commit* commit,
            ref git_revert_options given_opts);
    }
}
