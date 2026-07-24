using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Cherry-pick options
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_cherrypick_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_CHERRYPICK_OPTIONS_VERSION = 1;


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
        ///  Initialize git_cherrypick_options structure
        ///
        ///  <para>
        ///   Initializes a `git_cherrypick_options` with default
        ///   values. Equivalent to creating an instance with
        ///   GIT_CHERRYPICK_OPTIONS_INIT.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_cherrypick_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass `GIT_CHERRYPICK_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_cherrypick_options_init(
            ref git_cherrypick_options opts,
            uint version);

        /// <summary>
        ///  Cherry-picks the given commit against the given "our"
        ///  commit, producing an index that reflects the result of the
        ///  cherry-pick.
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
        /// <param name="cherrypick_commit">
        ///  the commit to cherry-pick
        /// </param>
        /// <param name="our_commit">
        ///  the commit to cherry-pick against (eg, HEAD)
        /// </param>
        /// <param name="mainline">
        ///  the parent of the `cherrypick_commit`, if it is a merge
        /// </param>
        /// <param name="merge_options">
        ///  the merge options (or null for defaults)
        /// </param>
        /// <returns>zero on success, -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_cherrypick_commit(
            out git_index* _out,
            git_repository* repo,
            git_commit* cherrypick_commit,
            git_commit* our_commit,
            uint mainline,
            ref git_merge_options merge_options);

        /// <summary>
        ///  Cherry-pick the given commit, producing changes in the index
        ///  and working directory.
        /// </summary>
        /// <param name="repo">the repository to cherry-pick</param>
        /// <param name="commit">the commit to cherry-pick</param>
        /// <param name="cherrypick_options">
        ///  the cherry-pick options (or null for defaults)
        /// </param>
        /// <returns>zero on success, -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_cherrypick(
            git_repository* repo,
            git_commit* commit,
            ref git_cherrypick_options cherrypick_options);
    }
}
