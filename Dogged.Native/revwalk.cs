using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    /// The sorting to use with revwalk.
    /// </summary>
    public enum git_sort_t
    {
        /// <summary>
        /// Sort the output with the same default method from `git`: reverse
	    /// chronological order. This is the default sorting for new walkers.
        /// </summary>
        GIT_SORT_NONE = 0,

        /// <summary>
	    /// Sort the repository contents in topological order (no parents before
	    /// all of its children are shown); this sorting mode can be combined
	    /// with time sorting to produce `git`'s `--date-order``.
        /// </summary>
        GIT_SORT_TOPOLOGICAL = (1 << 0),

        /// <summary>
        /// Sort the repository contents by commit time;
	    /// this sorting mode can be combined with
	    /// topological sorting.
        /// </summary>
        GIT_SORT_TIME = (1 << 1),

        /// <summary>
        /// Iterate through the repository contents in reverse
	     /// order; this sorting mode can be combined with
	     /// any of the above.
        /// </summary>
        GIT_SORT_REVERSE = (1 << 2)
    }

    /// <summary>
    /// Representation of an existing git revwalk.
    /// </summary>
    public struct git_revwalk { };
}
