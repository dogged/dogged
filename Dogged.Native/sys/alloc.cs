using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  An instance for a custom memory allocator
    ///
    ///  <para>
    ///   Setting the pointers of this structure allows the developer
    ///   to implement custom memory allocators. The global memory
    ///   allocator can be set by using "GIT_OPT_SET_ALLOCATOR" with
    ///   the `git_libgit2_opts` function. Keep in mind that all
    ///   fields need to be set to a proper function.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_allocator
    {
        /// <summary>
        ///  Allocate `n` bytes of memory
        /// </summary>
        public IntPtr gmalloc;

        /// <summary>
        ///  This function shall deallocate the old object `ptr` and
        ///  return a pointer to a new object that has the size specified
        ///  by `size`. In case `ptr` is `NULL`, a new array shall be
        ///  allocated.
        /// </summary>
        public IntPtr grealloc;

        /// <summary>
        ///  This function shall free the memory pointed to by `ptr`. In
        ///  case `ptr` is `NULL`, this shall be a no-op.
        /// </summary>
        public IntPtr gfree;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Initialize the allocator structure to use the `stdalloc`
        ///  pointer.
        ///
        ///  <para>
        ///   Set up the structure so that all of its members are
        ///   using the standard "stdalloc" allocator functions. The
        ///   structure can then be used with `git_allocator_setup`.
        ///  </para>
        /// </summary>
        /// <param name="allocator">
        ///  The allocator that is to be initialized.
        /// </param>
        /// <returns>An error code or 0.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_stdalloc_init_allocator(
            ref git_allocator allocator);

        /// <summary>
        ///  Initialize the allocator structure to use the `crtdbg`
        ///  pointer.
        ///
        ///  <para>
        ///   Set up the structure so that all of its members are
        ///   using the "crtdbg" allocator functions. Note that this
        ///   allocator is only available on Windows platforms and
        ///   only if libgit2 is being compiled with "-DMSVC_CRTDBG".
        ///  </para>
        /// </summary>
        /// <param name="allocator">
        ///  The allocator that is to be initialized.
        /// </param>
        /// <returns>An error code or 0.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_win32_crtdbg_init_allocator(
            ref git_allocator allocator);
    }
}
