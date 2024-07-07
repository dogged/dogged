using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
        /// <summary>
        /// Get the signature for the commit's author.  This data is owned
        /// by the commit and should not be freed.
        /// </summary>
        /// <param name="commit">The commit to examine</param>
        /// <returns>A pointer to a signature for the author</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_signature* git_commit_author(git_commit* commit);

        /// <summary>
        /// Get the signature for the commit's committer.  This data is owned
        /// by the commit and should not be freed.
        /// </summary>
        /// <param name="commit">The commit to examine</param>
        /// <returns>A pointer to a signature for the committer</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_signature* git_commit_committer(git_commit* commit);

        /// <summary>
        /// Look up a commit from the repository.
        /// </summary>
        /// <param name="obj">Pointer to the commit that was loaded from the repository</param>
        /// <param name="repo">The repository that contains the commit</param>
        /// <param name="id">The id of the commit to lookup</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_lookup(out git_commit* obj, git_repository* repo, ref git_oid id);

        /// <summary>
        /// Get the tree that the given commit points to.  This tree must be
        /// freed when it is no longer needed.
        /// </summary>
        /// <param name="tree">A pointer to the tree</param>
        /// <param name="commit">The commit to examine</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_tree(out git_tree* tree, git_commit* commit);

        /// <summary>
        /// Get the object id of the tree that the given commit points to.
        /// This differs from <see cref="git_commit_tree"/> in that no
        /// attempts are made to lookup the tree or validate it.
        /// </summary>
        /// <param name="commit">The commit to examine</param>
        /// <returns>A pointer to the tree's object id</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_commit_tree_id(git_commit* commit);
    }
}
