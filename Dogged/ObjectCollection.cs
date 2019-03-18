using System;
using System.Collections;
using System.Collections.Generic;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// Accessors for the Git objects (commits, trees, blobs, tags, etc)
    /// in a repository.
    /// </summary>
    public class ObjectCollection
    {
        private Repository repository;

        internal ObjectCollection(Repository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Lookup and return the git object in the repository as specified
        /// by the given ID.
        /// </summary>
        /// <param name="id">The object id to lookup</param>
        /// <returns>The Git object specified by the given ID</returns>
        public unsafe GitObject Lookup(ObjectId id)
        {
            Ensure.ArgumentNotNull(id, "id");

            git_oid oid = id.ToNative();
            git_object* obj = null;

            Ensure.NativeSuccess(() => libgit2.git_object_lookup(out obj, repository.NativeRepository, ref oid, git_object_t.GIT_OBJECT_ANY), repository);
            Ensure.NativePointerNotNull(obj);

            switch (libgit2.git_object_type(obj))
            {
                case git_object_t.GIT_OBJECT_COMMIT:
                    return Commit.FromNative((git_commit*)obj, id);
                case git_object_t.GIT_OBJECT_TREE:
                    return Tree.FromNative((git_tree*)obj, id);
                case git_object_t.GIT_OBJECT_BLOB:
                    return Blob.FromNative((git_blob*)obj, id);
            }

            libgit2.git_object_free(obj);
            throw new InvalidOperationException("unknown object type");
        }

        /// <summary>
        /// Lookup and return the git object in the repository as specified
        /// by the given ID.
        /// </summary>
        /// <param name="id">The object id to lookup</param>
        /// <returns>The Git object specified by the given ID</returns>
        public unsafe T Lookup<T>(ObjectId id) where T : GitObject
        {
            Ensure.ArgumentNotNull(id, "id");

            git_oid oid = id.ToNative();

            if (typeof(T) == typeof(Commit))
            {
                git_commit* obj = null;
                Ensure.NativeSuccess(() => libgit2.git_commit_lookup(out obj, repository.NativeRepository, ref oid), repository);
                return (T)(object)Commit.FromNative(obj, id);
            }
            else if (typeof(T) == typeof(Tree))
            {
                git_tree* obj = null;
                Ensure.NativeSuccess(() => libgit2.git_tree_lookup(out obj, repository.NativeRepository, ref oid), repository);
                return (T)(object)Tree.FromNative(obj, id);
            }
            else if (typeof(T) == typeof(Blob))
            {
                git_blob* obj = null;
                Ensure.NativeSuccess(() => libgit2.git_blob_lookup(out obj, repository.NativeRepository, ref oid), repository);
                return (T)(object)Blob.FromNative(obj, id);
            }
            else if (typeof(T) == typeof(GitObject))
            {
                return (T)(object)Lookup(id);
            }

            throw new InvalidOperationException("unknown object type");
        }
    }
}
