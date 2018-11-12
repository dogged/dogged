using System;

namespace Dogged.Native
{
    /// <summary>
    /// A reference is a pointer to a commit or another reference;
    /// reference types are branches and tags.
    /// </summary>
    public struct git_reference { };

    /// <summary>
    /// Basic types for a Git reference.
    /// </summary>
    public enum git_reference_t {
        /// <summary>
        /// An invalid reference.
        /// </summary>
        GIT_REFERENCE_INVALID = 0,

        /// <summary>
        /// A direct reference that points directly to an object id.
        /// </summary>
        GIT_REFERENCE_DIRECT = 1,

        /// <summary>
        /// A symbolic reference that points to another reference;
        /// for example, `HEAD`.
        /// </summary>
        GIT_REFERENCE_SYMBOLIC = 2,

        /// <summary>
        /// A value that includes all reference types.
        /// </summary>
        GIT_REFERENCE_ALL = GIT_REFERENCE_DIRECT | GIT_REFERENCE_SYMBOLIC
    };
}
