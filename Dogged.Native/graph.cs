using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
        /// <summary>
        ///  Count the number of unique commits between two commit
        ///  objects
        ///
        ///  <para>
        ///   There is no need for branches containing the commits to
        ///   have any upstream relationship, but it helps to think of
        ///   one as a branch and the other as its upstream, the
        ///   `ahead` and `behind` values will be what git would
        ///   report for the branches.
        ///  </para>
        /// </summary>
        /// <param name="ahead">
        ///  number of unique from commits in `upstream`
        /// </param>
        /// <param name="behind">
        ///  number of unique from commits in `local`
        /// </param>
        /// <param name="repo">
        ///  the repository where the commits exist
        /// </param>
        /// <param name="local">the commit for local</param>
        /// <param name="upstream">the commit for upstream</param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_graph_ahead_behind(
            out UIntPtr ahead,
            out UIntPtr behind,
            git_repository* repo,
            ref git_oid local,
            ref git_oid upstream);

        /// <summary>
        ///  Determine if a commit is the descendant of another commit.
        ///
        ///  <para>
        ///   Note that a commit is not considered a descendant of
        ///   itself, in contrast to `git merge-base --is-ancestor`.
        ///  </para>
        /// </summary>
        /// <param name="repo">
        ///  the repository where the commits exist
        /// </param>
        /// <param name="commit">a previously loaded commit</param>
        /// <param name="ancestor">a potential ancestor commit</param>
        /// <returns>
        ///  1 if the given commit is a descendant of the potential
        ///  ancestor, 0 if not, error code otherwise.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_graph_descendant_of(
            git_repository* repo,
            ref git_oid commit,
            ref git_oid ancestor);

        /// <summary>
        ///  Determine if a commit is reachable from any of a list of
        ///  commits by following parent edges.
        /// </summary>
        /// <param name="repo">
        ///  the repository where the commits exist
        /// </param>
        /// <param name="commit">a previously loaded commit</param>
        /// <param name="descendant_array">oids of the commits</param>
        /// <param name="length">
        ///  the number of commits in the provided `descendant_array`
        /// </param>
        /// <returns>
        ///  1 if the given commit is an ancestor of any of the given
        ///  potential descendants, 0 if not, error code otherwise.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_graph_reachable_from_any(
            git_repository* repo,
            ref git_oid commit,
            ref git_oid descendant_array,
            UIntPtr length);
    }
}
