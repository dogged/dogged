using System;
using System.Collections;
using System.Collections.Generic;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// An index is a map of paths in the repository to object contents.
    /// The repository has an index which acts as the staging area for
    /// changes to be committed and a cache of in-working tree files.
    /// </summary>
    public class Index : NativeDisposable
    {
        private unsafe git_index* nativeIndex;

        private unsafe Index(git_index* nativeIndex)
        {
            Ensure.ArgumentNotNull(nativeIndex, "index");
            this.nativeIndex = nativeIndex;
        }

        internal unsafe static Index FromNative(git_index* nativeIndex)
        {
            return new Index(nativeIndex);
        }

        /// <summary>
        /// Gets the number of entries in the index.
        /// </summary>
        public unsafe int Count
        {
            get
            {
                UIntPtr count = Ensure.NativeCall<UIntPtr>(() => libgit2.git_index_entrycount(nativeIndex), this);
                return Ensure.CastToInt(count, "count");
            }
        }

        /// <summary>
        /// Gets the index entry at the specified index.
        /// </summary>
        public unsafe IndexEntry this[int position]
        {
            get
            {
                Ensure.NotDisposed(nativeIndex, "index");
                Ensure.ArgumentConformsTo(() => position >= 0, "position", "position must not be negative");

                git_index_entry* entry = libgit2.git_index_get_byindex(nativeIndex, (UIntPtr)position);
                GC.KeepAlive(this);

                if (entry == null)
                {
                    throw new IndexOutOfRangeException(string.Format("there is no index entry at position {0}", position));
                }

                return IndexEntry.FromNative(entry);
            }
        }

        internal unsafe override bool IsDisposed
        {
            get
            {
                return (nativeIndex == null);
            }
        }

        internal unsafe override void Dispose(bool disposing)
        {
            if (nativeIndex != null)
            {
                libgit2.git_index_free(nativeIndex);
                nativeIndex = null;
            }
        }
    }
}
