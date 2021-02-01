using System;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// The type of the revision spec.
    /// </summary>
    public enum RevisionSpecType
    {
        /// <summary>
        /// The spec targeted a single object.
        /// </summary>
        Single = (1 << 0),

        /// <summary>
    	/// The spec targeted a range of commits.
        /// </summary>
        Range = (1 << 1),

        /// <summary>
        /// The spec used the '...' operator, which invokes special semantics.
        /// </summary>
        MergeBase = (1 << 2)
    }

    /// <summary>
    /// A specifier for a revision or revision range.
    /// </summary>
    public class RevisionSpec : NativeDisposable
    {
        private git_revspec nativeObject = new git_revspec();
        private bool disposed = false;

        public unsafe static RevisionSpec Parse(Repository repository, string spec)
        {
            RevisionSpec obj = new RevisionSpec();

            Ensure.NativeSuccess(() => libgit2.git_revparse(obj.nativeObject, repository.NativeRepository, spec), repository);
            return obj;
        }

        public unsafe static GitObject ParseSingle(Repository repository, string spec)
        {
            git_object* obj = null;

            Ensure.NativeSuccess(() => libgit2.git_revparse_single(out obj, repository.NativeRepository, spec), repository);

            try
            {
                return GitObject.FromNative(obj);
            }
            catch (Exception)
            {
                libgit2.git_object_free(obj);
                throw;
            }
        }

        public unsafe GitObject From
        {
            get
            {
                if (nativeObject.from != null)
                {
                    return Ensure.NativeCall(() => GitObject.DuplicateNative(nativeObject.from), this);
                }

                return null;
            }
        }

        public unsafe GitObject To
        {
            get
            {
                if (nativeObject.to != null)
                {
                    return Ensure.NativeCall(() => GitObject.DuplicateNative(nativeObject.to), this);
                }

                return null;
            }
        }

        public RevisionSpecType Type
        {
            get
            {
                return (RevisionSpecType)((int)nativeObject.flags);
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
            if (!disposed) {
                if (nativeObject.from != null)
                {
                    libgit2.git_object_free(nativeObject.from);
                    nativeObject.from = null;
                }

                if (nativeObject.to != null)
                {
                    libgit2.git_object_free(nativeObject.to);
                    nativeObject.to = null;
                }

                disposed = true;
            }
        }
    }
}
