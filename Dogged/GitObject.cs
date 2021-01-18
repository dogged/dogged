using System;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// An object (blob, commit, tree, etc) in a Git repository.
    /// </summary>
    public abstract class GitObject : NativeDisposable
    {
        private unsafe git_object* nativeObject;
        private ObjectId id;

        internal unsafe GitObject(git_object* nativeObject, ObjectId id = null)
        {
            Ensure.NativePointerNotNull(nativeObject);

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

        internal unsafe static T FromNative<T>(git_object* obj, ObjectId id = null) where T : GitObject
        {
            Ensure.ArgumentNotNull(obj, nameof(obj));

            if (typeof(T) == typeof(Commit))
            {
                return (T)(object)Commit.FromNative((git_commit*)obj, id);
            }
            else if (typeof(T) == typeof(Tree))
            {
                return (T)(object)Tree.FromNative((git_tree*)obj, id);
            }
            else if (typeof(T) == typeof(Blob))
            {
                return (T)(object)Blob.FromNative((git_blob*)obj, id);
            }

            throw new InvalidOperationException("unknown object type");
        }

        internal unsafe static GitObject FromNative(git_object* obj, ObjectId id = null)
        {
            switch (libgit2.git_object_type(obj))
            {
                case git_object_t.GIT_OBJECT_COMMIT:
                    return Commit.FromNative((git_commit*)obj, id);
                case git_object_t.GIT_OBJECT_TREE:
                    return Tree.FromNative((git_tree*)obj, id);
                case git_object_t.GIT_OBJECT_BLOB:
                    return Blob.FromNative((git_blob*)obj, id);
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

        internal unsafe git_object* NativeObject
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
                libgit2.git_object_free(nativeObject);
                nativeObject = null;
            }
        }
    }
}
