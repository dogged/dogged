using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
        /// <summary>
        ///  Create a diff for a commit in mbox format for sending via
        ///  email. The commit must not be a merge commit.
        /// </summary>
        /// <param name="_out">buffer to store the e-mail patch in</param>
        /// <param name="commit">commit to create a patch for</param>
        /// <param name="opts">email creation options</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_email_create_from_commit(
            ref git_buf _out,
            CommitHandle commit,
            ref git_email_create_options opts);
    }
}
