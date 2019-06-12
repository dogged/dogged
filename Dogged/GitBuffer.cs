using System;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// A memory buffer that represents data in a Git repository.
    /// </summary>
    public class GitBuffer : NativeDisposable
    {
        private git_buf nativeObject = new git_buf();
        private bool disposed = false;

        internal unsafe GitBuffer()
        {
        }

        internal git_buf NativeBuffer
        {
            get
            {
                return nativeObject;
            }
        }

        public long Length
        {
            get
            {
                Ensure.NotDisposed(this);
                Ensure.CastToLong(nativeObject.size, "size");
                return (long)nativeObject.size;
            }
        }

        public unsafe ReadOnlySpan<byte> Content
        {
            get
            {
                Ensure.NotDisposed(this);

                var size = Ensure.CastToInt(nativeObject.size, "size");
                return new ReadOnlySpan<byte>(nativeObject.ptr, size);
            }
        }

        internal unsafe override bool IsDisposed
        {
            get
            {
                return disposed;
            }
        }

        internal unsafe override void Dispose(bool disposing)
        {
            libgit2.git_buf_dispose(nativeObject);
            disposed = true;
        }
    }
}
