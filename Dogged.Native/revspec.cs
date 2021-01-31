using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    /// The type of a revision spec.
    /// </summary>
    public enum git_revspec_t
    {
        /// <summary>
        /// The spec targeted a single object.
        /// </summary>
        GIT_REVSPEC_SINGLE = (1 << 0),

        /// <summary>
        /// The spec targeted a range of commits.
        /// </summary>
        GIT_REVSPEC_RANGE = (1 << 1),

        /// <summary>
        /// The spec used the '...' operator, which invokes special semantics.
        /// </summary>
        GIT_REVSPEC_MERGE_BASE = (1 << 2)
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe class git_revspec
    {
        /// <summary>
        /// The left element of the revspec; must be freed by the user.
        /// </summary>
        public git_object* from;

        /// <summary>
        /// The right element of the revspec; must be freed by the user.
        /// </summary>
        public git_object* to;

        /// <summary>
        /// The intent of the revspec (i.e. `git_revspec_t` type)
        /// </summary>
        public uint flags;
    }
}
