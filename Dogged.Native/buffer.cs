using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  A data buffer for exporting data from libgit2
    ///
    ///  <para>
    ///   Sometimes libgit2 wants to return an allocated data buffer
    ///   to the caller and have the caller take responsibility for
    ///   freeing that memory. To make ownership clear in these cases,
    ///   libgit2 uses `git_buf` to return this data. Callers should
    ///   use `git_buf_dispose()` to release the memory when they are
    ///   done.
    ///  </para>
    ///
    ///  <para>
    ///   A `git_buf` contains a pointer to a NUL-terminated C string,
    ///   and the length of the string (not including the NUL
    ///   terminator).
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_buf
    {
        /// <summary>
        ///  The buffer contents. `ptr` points to the start of the buffer
        ///  being returned. The buffer's length (in bytes) is specified
        ///  by the `size` member of the structure, and contains a NUL
        ///  terminator at position `(size + 1)`.
        /// </summary>
        public byte* ptr;

        /// <summary>
        ///  This field is reserved and unused.
        /// </summary>
        public UIntPtr reserved;

        /// <summary>
        ///  The length (in bytes) of the buffer pointed to by `ptr`, not
        ///  including a NUL terminator.
        /// </summary>
        public UIntPtr size;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Free the memory referred to by the git_buf.
        ///
        ///  <para>
        ///   Note that this does not free the `git_buf` itself, just
        ///   the memory pointed to by `buffer->ptr`.
        ///  </para>
        /// </summary>
        /// <param name="buffer">The buffer to deallocate</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_buf_dispose(
            ref git_buf buffer);
    }
}
