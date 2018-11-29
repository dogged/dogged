using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    /// The signature portion of a git commit, containing the author or
    /// committer information including name, email address and time.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_signature
    {
        /// <summary>
        /// The full name of the author or committer.
        /// </summary>
        public byte* name;

        /// <summary>
        /// The email address of the author or committer.
        /// </summary>
        public byte* email;

        /// <summary>
        /// The time the commit was authored or committed.
        /// </summary>
        public git_time when;
    }
}
