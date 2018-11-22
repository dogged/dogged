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
    public class Index : NativeDisposable, IEnumerable<IndexEntry>
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

        /// <summary>
        /// Returns an enumerate that iterates through index entries.  This
        /// will iterate over a snapshot of the index, so the underlying
        /// index can safely be mutated during iteration.
        /// </summary>
        /// <returns>An enumerator of <see cref="IndexEntry"/> objects</returns>
        public unsafe IEnumerator<IndexEntry> GetEnumerator()
        {
            git_index_iterator* iterator = null;
            Ensure.NativeSuccess(() => libgit2.git_index_iterator_new(out iterator, nativeIndex), this);
            return IndexEnumerator.FromNative(iterator);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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

    /// <summary>
    /// An enumerator for <see cref="IndexEntry"/> objects.  This will
    /// iterate over a snapshot of the index, so the underlying index
    /// can safely be mutated during iteration.
    /// </summary>
    public class IndexEnumerator : NativeDisposable, IEnumerator<IndexEntry>
    {
        private unsafe git_index_iterator* nativeIterator;

        private IndexEntry current;

        private unsafe IndexEnumerator(git_index_iterator* nativeIterator)
        {
            Ensure.ArgumentNotNull(nativeIterator, "iterator");
            this.nativeIterator = nativeIterator;
        }

        internal unsafe static IndexEnumerator FromNative(git_index_iterator* nativeIterator)
        {
            return new IndexEnumerator(nativeIterator);
        }

        public void Reset()
        {
            /*
             * IEnumerator.Reset is not required to be implemented and may throw
             * a NotSupportedException.
             *
             * https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerator.reset
             */
            throw new NotSupportedException("cannot reset an index iterator");
        }

        public unsafe bool MoveNext()
        {
            git_index_entry *entry = null;
            int ret = Ensure.NativeCall(() => libgit2.git_index_iterator_next(out entry, nativeIterator), this);

            if (ret == (int)git_error_code.GIT_ITEROVER)
            {
                return false;
            }
            Ensure.NativeSuccess(ret);

            current = IndexEntry.FromNative(entry);
            return true;
        }

        public IndexEntry Current
        {
            get
            {
                return current;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        internal unsafe override bool IsDisposed
        {
            get
            {
                return (nativeIterator == null);
            }
        }

        internal unsafe override void Dispose(bool disposing)
        {
            if (nativeIterator != null)
            {
                libgit2.git_index_iterator_free(nativeIterator);
                nativeIterator = null;
            }
        }
    }
}
