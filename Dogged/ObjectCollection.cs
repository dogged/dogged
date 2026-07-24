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
            ObjectHandle nativeObject;

            Ensure.NativeSuccess(libgit2.git_object_lookup(out nativeObject, repository.NativeRepository, ref oid, git_object_t.GIT_OBJECT_ANY));
            Ensure.NativeHandleIsValid(nativeObject);

            try
            {
                return GitObject.FromNative(nativeObject, id);
            }
            catch (Exception)
            {
                nativeObject.Dispose();
                throw;
            }
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
                CommitHandle nativeCommit;
                Ensure.NativeSuccess(libgit2.git_commit_lookup(out nativeCommit, repository.NativeRepository, ref oid));
                return (T)(GitObject)Commit.FromNative(nativeCommit, id);
            }
            else if (typeof(T) == typeof(Tree))
            {
                TreeHandle nativeTree;
                Ensure.NativeSuccess(libgit2.git_tree_lookup(out nativeTree, repository.NativeRepository, ref oid));
                return (T)(GitObject)Tree.FromNative(nativeTree, id);
            }
            else if (typeof(T) == typeof(Blob))
            {
                BlobHandle nativeBlob;
                Ensure.NativeSuccess(libgit2.git_blob_lookup(out nativeBlob, repository.NativeRepository, ref oid));
                return (T)(GitObject)Blob.FromNative(nativeBlob, id);
            }
            else if (typeof(T) == typeof(GitObject))
            {
                return (T)(GitObject)Lookup(id);
            }

            throw new InvalidOperationException("unknown object type");
        }
    }
}
