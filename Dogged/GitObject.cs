using System;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// An object (blob, commit, tree, etc) in a Git repository.
    /// </summary>
    public abstract class GitObject : NativeDisposable
    {
        private unsafe ObjectHandle nativeObject;
        private ObjectId id;

        internal unsafe GitObject(ObjectHandle nativeObject, ObjectId id = null)
        {
            Ensure.NativeHandleIsValid(nativeObject);

            this.nativeObject = nativeObject;
            this.id = id;
        }

        /// <summary>
        /// Returns the type of the object.
        /// </summary>
        public abstract ObjectType Type { get; }

        /// <summary>
        /// Returns the object id.
        /// </summary>
        public unsafe ObjectId Id
        {
            get
            {
                Ensure.NotDisposed(this);

                if (id == null)
                {
                    git_oid* oid = libgit2.git_object_id(nativeObject);
                    Ensure.NativePointerNotNull(oid);

                    id = ObjectId.FromNative(*oid);
                }

                return id;
            }
        }

        internal unsafe static T FromNative<T>(ObjectHandle obj, ObjectId id = null) where T : GitObject
        {
            Ensure.NativeHandleIsValid(obj, nameof(obj));

            if (typeof(T) == typeof(Commit))
            {
                return (T)(object)Commit.FromNative(obj, id);
            }
            else if (typeof(T) == typeof(Tree))
            {
                return (T)(object)Tree.FromNative(obj, id);
            }
            else if (typeof(T) == typeof(Blob))
            {
                return (T)(object)Blob.FromNative(obj, id);
            }

            throw new InvalidOperationException("unknown object type");
        }

        internal unsafe static GitObject DuplicateNative(ObjectHandle obj, ObjectId id = null)
        {
            // git_object_dup will increase its internal refcount
            // on the object, so that the caller can dispose it
            // independently of this object.
            ObjectHandle dup = null;

            Ensure.NativeSuccess(libgit2.git_object_dup(out dup, obj));
            Ensure.NativeHandleIsValid(dup);

            try
            {
                return GitObject.FromNative(dup);
            }
            catch (Exception)
            {
                dup.Dispose();
                throw;
            }
        }

        internal unsafe static GitObject FromNative(ObjectHandle obj, ObjectId id = null)
        {
            switch (libgit2.git_object_type(obj))
            {
                case git_object_t.GIT_OBJECT_COMMIT:
                    return Commit.FromNative(obj, id);
                case git_object_t.GIT_OBJECT_TREE:
                    return Tree.FromNative(obj, id);
                case git_object_t.GIT_OBJECT_BLOB:
                    return Blob.FromNative(obj, id);
            }

            throw new InvalidOperationException("unknown object type");
        }

        internal static git_object_t GetType<T>() where T : GitObject
        {
            if (typeof(T) == typeof(Commit))
            {
                return git_object_t.GIT_OBJECT_COMMIT;
            }
            else if (typeof(T) == typeof(Tree))
            {
                return git_object_t.GIT_OBJECT_TREE;
            }
            else if (typeof(T) == typeof(Blob))
            {
                return git_object_t.GIT_OBJECT_BLOB;
            }

            return git_object_t.GIT_OBJECT_INVALID;
        }

        internal unsafe ObjectHandle NativeObject
        {
            get
            {
                return nativeObject;
            }
        }

        internal unsafe override bool IsDisposed
        {
            get
            {
                return (nativeObject == null);
            }
        }

        internal unsafe override void Dispose(bool disposing)
        {
            if (nativeObject != null)
            {
                nativeObject.Dispose();
                nativeObject = null;
            }
        }
    }
}
