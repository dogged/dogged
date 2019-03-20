using System;
using System.Collections;
using System.Collections.Generic;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// Accessors for the Git references (branches, tags) in a repository.
    /// </summary>
    public class ReferenceCollection : IEnumerable<Reference>
    {
        private Repository repository;

        internal ReferenceCollection(Repository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Lookup and return the reference as specified by the given name.
        /// </summary>
        /// <param name="name">The reference to lookup</param>
        /// <returns>The reference specified by the given name</returns>
        public unsafe Reference Lookup(string name)
        {
            Ensure.ArgumentNotNull(name, "name");

            git_reference* reference = null;

            Ensure.NativeSuccess(() => libgit2.git_reference_lookup(out reference, repository.NativeRepository, name), repository);
            Ensure.NativePointerNotNull(reference);

            return Reference.FromNative(reference);
        }

        /// <summary>
        /// Returns an enumerator that iterates through references.
        ///
        /// <para>
        /// Note that the returned references are <see cref="IDisposable"/>
        /// and must be disposed properly.
        /// </para>
        /// </summary>
        /// <returns>An enumerator of <see cref="IndexEntry"/> objects</returns>
        public unsafe IEnumerator<Reference> GetEnumerator()
        {
            git_reference_iterator* iterator = null;
            Ensure.NativeSuccess(() => libgit2.git_reference_iterator_new(out iterator, repository.NativeRepository), repository);
            return ReferenceEnumerator.FromNative(iterator);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// An enumerator for <see cref="Reference"/> objects.
    /// </summary>
    public class ReferenceEnumerator : NativeDisposable, IEnumerator<Reference>
    {
        private unsafe git_reference_iterator* nativeIterator;

        private Reference current;

        private unsafe ReferenceEnumerator(git_reference_iterator* nativeIterator)
        {
            Ensure.ArgumentNotNull(nativeIterator, "iterator");
            this.nativeIterator = nativeIterator;
        }

        internal unsafe static ReferenceEnumerator FromNative(git_reference_iterator* nativeIterator)
        {
            return new ReferenceEnumerator(nativeIterator);
        }

        public void Reset()
        {
            /*
             * IEnumerator.Reset is not required to be implemented and may throw
             * a NotSupportedException.
             *
             * https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerator.reset
             */
            throw new NotSupportedException("cannot reset a reference iterator");
        }

        public unsafe bool MoveNext()
        {
            git_reference *reference = null;
            int ret = Ensure.NativeCall(() => libgit2.git_reference_next(out reference, nativeIterator), this);

            if (ret == (int)git_error_code.GIT_ITEROVER)
            {
                return false;
            }
            Ensure.NativeSuccess(ret);

            current = Reference.FromNative(reference);
            return true;
        }

        public Reference Current
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
                libgit2.git_reference_iterator_free(nativeIterator);
                nativeIterator = null;
            }
        }
    }
}
