using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public sealed class DescribeResultHandle : SafeHandle
    {
        public DescribeResultHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe DescribeResultHandle(git_describe_result* nativeDescribeResult) :
            base((IntPtr)nativeDescribeResult, true)
        {
        }

        public override bool IsInvalid
        {
            get
            {
                return handle == IntPtr.Zero;
            }
        }

        protected override unsafe bool ReleaseHandle()
        {
            libgit2.git_describe_result_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Describe a commit
        ///
        ///  <para>
        ///   Perform the describe operation on the given committish
        ///   object.
        ///  </para>
        /// </summary>
        /// <param name="result">
        ///  pointer to store the result. You must free this once you're
        ///  done with it.
        /// </param>
        /// <param name="committish">a committish to describe</param>
        /// <param name="opts">
        ///  the lookup options (or NULL for defaults)
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_describe_commit(
            out DescribeResultHandle result,
            ObjectHandle committish,
            ref git_describe_options opts);

        /// <summary>
        ///  Describe a commit
        ///
        ///  <para>
        ///   Perform the describe operation on the current commit and
        ///   the worktree. After performing describe on HEAD, a
        ///   status is run and the description is considered to be
        ///   dirty if there are.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  pointer to store the result. You must free this once you're
        ///  done with it.
        /// </param>
        /// <param name="repo">
        ///  the repository in which to perform the describe
        /// </param>
        /// <param name="opts">
        ///  the lookup options (or NULL for defaults)
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_describe_workdir(
            out DescribeResultHandle _out,
            RepositoryHandle repo,
            ref git_describe_options opts);

        /// <summary>
        ///  Print the describe result to a buffer
        /// </summary>
        /// <param name="_out">The buffer to store the result</param>
        /// <param name="result">
        ///  the result from `git_describe_commit()` or
        ///  `git_describe_workdir()`.
        /// </param>
        /// <param name="opts">
        ///  the formatting options (or NULL for defaults)
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_describe_format(
            ref git_buf _out,
            DescribeResultHandle result,
            ref git_describe_format_options opts);

        /// <summary>
        ///  Free the describe result.
        /// </summary>
        /// <param name="result">The result to free.</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_describe_result_free(
            IntPtr result);
    }
}
