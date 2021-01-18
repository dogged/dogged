using System;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// A reference is a pointer to a commit or another reference;
    /// reference types are branches and tags.
    /// </summary>
    public abstract class Reference : NativeDisposable
    {
        private readonly LazyNative<string> name;

        internal unsafe Reference(git_reference *nativeReference)
        {
            Ensure.ArgumentNotNull(nativeReference, "reference");
            NativeReference = nativeReference;

            name = new LazyNative<string>(() => {
                return Ensure.NativeObjectNotNull(libgit2.git_reference_name(NativeReference));
            }, this);
        }

        protected Reference() { }

        internal unsafe static Reference FromNative(git_reference* nativeReference)
        {
            switch (libgit2.git_reference_type(nativeReference))
            {
                case git_reference_t.GIT_REFERENCE_DIRECT:
                    return DirectReference.FromNative(nativeReference);
                case git_reference_t.GIT_REFERENCE_SYMBOLIC:
                    return SymbolicReference.FromNative(nativeReference);
            }

            libgit2.git_reference_free(nativeReference);
            throw new InvalidOperationException("unknown reference type");
        }

        internal unsafe git_reference* NativeReference { get; private set; }

        /// <summary>
        /// Gets the fully-qualified name of the reference.  For branches,
        /// this will be prefixed with "refs/heads"; for tags, this will
        /// be prefixed with "refs/tags".
        /// </summary>
        public unsafe string Name
        {
            get
            {
                return name.Value;
            }
        }

        public unsafe GitObject Peel()
        {
            Ensure.NotDisposed(NativeReference, "reference");

            git_object* obj = null;

            Ensure.NativeSuccess(() => libgit2.git_reference_peel(out obj, NativeReference, git_object_t.GIT_OBJECT_ANY), this);
            Ensure.NativePointerNotNull(obj);

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

        public unsafe T Peel<T>() where T : GitObject
        {
            Ensure.NotDisposed(NativeReference, "reference");

            git_object_t type = GitObject.GetType<T>();
            git_object* obj = null;

            if (type == git_object_t.GIT_OBJECT_INVALID)
            {
                throw new InvalidOperationException("unknown object type");
            }

            Ensure.NativeSuccess(() => libgit2.git_reference_peel(out obj, NativeReference, type), this);
            Ensure.NativePointerNotNull(obj);

            try
            {
                return GitObject.FromNative<T>(obj);
            }
            catch (Exception)
            {
                libgit2.git_object_free(obj);
                throw;
            }
        }

        internal unsafe override bool IsDisposed
        {
            get
            {
                return (NativeReference == null);
            }
        }

        internal unsafe override void Dispose(bool disposing)
        {
            if (NativeReference != null)
            {
                libgit2.git_reference_free(NativeReference);
                NativeReference = null;
            }
        }
    }

    /// <summary>
    /// A direct reference is a reference that points directly to an
    /// object id.  This is the most common type of reference.
    /// </summary>
    public class DirectReference : Reference
    {
        private readonly LazyNative<ObjectId> target;

        private unsafe DirectReference(git_reference* nativeReference) : base(nativeReference)
        {
            target = new LazyNative<ObjectId>(() => {
                git_oid* oid = libgit2.git_reference_target(NativeReference);
                Ensure.NativePointerNotNull(oid);
                return ObjectId.FromNative(*oid);
            }, this);
        }

        /// <summary>
        /// Gets the target object id that this reference points to.
        /// </summary>
        public unsafe ObjectId Target
        {
            get
            {
                return target.Value;
            }
        }

        internal unsafe static new DirectReference FromNative(git_reference* nativeReference)
        {
            return new DirectReference(nativeReference);
        }
    }

    /// <summary>
    /// A symbolic reference is a reference that points ot another
    /// reference.  Typically, the only symbolic reference in your
    /// repository is `HEAD`.
    /// </summary>
    public class SymbolicReference : Reference
    {
        private readonly LazyNative<string> target;

        private unsafe SymbolicReference(git_reference* nativeReference) : base(nativeReference)
        {
            target = new LazyNative<string>(() => {
                return Ensure.NativeObjectNotNull(libgit2.git_reference_symbolic_target(NativeReference));
            }, this);
        }

        /// <summary>
        /// Gets the target reference name that this reference points to.
        /// </summary>
        public unsafe string Target
        {
            get
            {
                return target.Value;
            }
        }

        internal unsafe static new SymbolicReference FromNative(git_reference* nativeReference)
        {
            return new SymbolicReference(nativeReference);
        }
    }
}
