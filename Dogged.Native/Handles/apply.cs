using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
        /// <summary>
        ///  Apply a `git_diff` to a `git_tree`, and return the resulting
        ///  image as an index.
        /// </summary>
        /// <param name="_out">the postimage of the application</param>
        /// <param name="repo">the repository to apply</param>
        /// <param name="preimage">the tree to apply the diff to</param>
        /// <param name="diff">the diff to apply</param>
        /// <param name="options">
        ///  the options for the apply (or null for defaults)
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_apply_to_tree(
            out IndexHandle _out,
            RepositoryHandle repo,
            TreeHandle preimage,
            DiffHandle diff,
            ref git_apply_options options);

        /// <summary>
        ///  Apply a `git_diff` to the given repository, making changes
        ///  directly in the working directory, the index, or both.
        /// </summary>
        /// <param name="repo">the repository to apply to</param>
        /// <param name="diff">the diff to apply</param>
        /// <param name="location">
        ///  the location to apply (workdir, index or both)
        /// </param>
        /// <param name="options">
        ///  the options for the apply (or null for defaults)
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_apply(
            RepositoryHandle repo,
            DiffHandle diff,
            git_apply_location_t location,
            ref git_apply_options options);
    }
}
