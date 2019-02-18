using System;
using System.Collections.Generic;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// The representation of a Git repository, including its objects,
    /// references and the methods that operate on it.
    /// </summary>
    public class Repository : NativeDisposable
    {
        private unsafe git_repository* nativeRepository;

        private readonly LazyNative<bool> isBare;

        // Provide a strongly typed exception when a repository is not
        // found to open.
        private static readonly Dictionary<git_error_code, Func<string, Exception>> exceptionMap = new Dictionary<git_error_code, Func<string, Exception>>
        {
            { git_error_code.GIT_ENOTFOUND, (m) => new RepositoryNotFoundException(m) }
        };

        /// <summary>
        /// Opens the Git repository at the given <paramref name="path"/>.
        /// This can be either a path to the repository's working tree,
        /// or to the actual repository folder (the ".git" folder beneath
        /// a working directory or a bare repository).
        /// </summary>
        /// <param name="path">The path to the repository or a repository's working tree.</param>
        public unsafe Repository(string path) : this()
        {
            Ensure.NativeSuccess(libgit2.git_repository_open(out nativeRepository, path), exceptionMap);
        }

        private unsafe Repository(git_repository* nativeRepository) : this()
        {
            Ensure.NativePointerNotNull(nativeRepository);
            this.nativeRepository = nativeRepository;
        }

        private unsafe Repository()
        {
            isBare = new LazyNative<bool>(() => libgit2.git_repository_is_bare(this.nativeRepository) == 0 ? false : true, this);
        }

        /// <summary>
        /// Clone a remote repository.
        /// </summary>
        /// <param name="remotePath">The remote repository to clone</param>
        /// <param name="localPath">The local path to clone to</param>
        /// <returns>The newly cloned repository</returns>
        public unsafe static Repository Clone(string remotePath, string localPath)
        {
            Ensure.ArgumentNotNull(remotePath, "remotePath");
            Ensure.ArgumentNotNull(localPath, "localPath");

            git_repository* nativeRepository;
            Ensure.NativeSuccess(libgit2.git_clone(out nativeRepository, remotePath, localPath, null));

            return new Repository(nativeRepository);
        }

        /// <summary>
        /// Indicates whether the specified repository is bare.
        /// </summary>
        public unsafe bool IsBare
        {
            get
            {
                return isBare.Value;
            }
        }

        /// <summary>
        /// Indicates whether the HEAD is detached, meaning it points
        /// directly at an object id instead of a branch.
        /// </summary>
        public unsafe bool IsHeadDetached
        {
            get
            {
                int ret = Ensure.NativeCall<int>(() => libgit2.git_repository_head_detached(nativeRepository), this);
                return Ensure.NativeBoolean(ret);
            }
        }

        /// <summary>
        /// Retrieve and resolve the reference pointed to by `HEAD`.
        /// The repository HEAD will be peeled to a direct reference.
        /// </summary>
        public unsafe DirectReference Head
        {
            get
            {
                git_reference* reference = null;
                Ensure.NativeSuccess(() => libgit2.git_repository_head(out reference, nativeRepository), this);

                try
                {
                    var head = Reference.FromNative(reference);

                    if (!(head is DirectReference))
                    {
                        throw new DoggedException("unexpected reference type for HEAD");
                    }

                    return (DirectReference)head;
                }
                catch (Exception)
                {
                    libgit2.git_reference_free(reference);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get the commit pointed to by HEAD.
        /// </summary>
        public Commit HeadCommit
        {
            get
            {
                using (var head = Head)
                {
                    return Lookup<Commit>(head.Target);
                }
            }
        }

        /// <summary>
        /// Describes the repository's index.
        /// </summary>
        public unsafe Index Index
        {
            get
            {
                git_index* index = null;
                Ensure.NativeSuccess(() => libgit2.git_repository_index(out index, nativeRepository), this);
                return Index.FromNative(index);
            }
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

            Ensure.NativeSuccess(() => libgit2.git_object_lookup(out obj, nativeRepository, ref oid, git_object_t.GIT_OBJECT_ANY), this);
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
                Ensure.NativeSuccess(() => libgit2.git_commit_lookup(out obj, nativeRepository, ref oid), this);
                return (T)(object)Commit.FromNative(obj, id);
            }
            else if (typeof(T) == typeof(Tree))
            {
                git_tree* obj = null;
                Ensure.NativeSuccess(() => libgit2.git_tree_lookup(out obj, nativeRepository, ref oid), this);
                return (T)(object)Tree.FromNative(obj, id);
            }
            else if (typeof(T) == typeof(Blob))
            {
                git_blob* obj = null;
                Ensure.NativeSuccess(() => libgit2.git_blob_lookup(out obj, nativeRepository, ref oid), this);
                return (T)(object)Blob.FromNative(obj, id);
            }
            else if (typeof(T) == typeof(GitObject))
            {
                return (T)(object)Lookup(id);
            }

            throw new InvalidOperationException("unknown object type");
        }

        /// <summary>
        /// Get the object database ("ODB") for this repository.
        ///
        /// <para>
        /// If a custom ODB has not been set, the default database for the
        /// repository will be returned (the one located in `.git/objects`).
        /// </para>
        ///
        /// <para>
        /// The ODB must be freed once it's no longer being used by the user.
        /// </para>
        /// </summary>
        public unsafe ObjectDatabase ObjectDatabase
        {
            get
            {
                git_odb* odb = null;
                Ensure.NativeSuccess(() => libgit2.git_repository_odb(out odb, nativeRepository), this);
                return ObjectDatabase.FromNative(odb);
            }
        }

        internal unsafe override bool IsDisposed
        {
            get
            {
                return (nativeRepository == null);
            }
        }

        internal unsafe override void Dispose(bool disposing)
        {
            if (nativeRepository != null)
            {
                libgit2.git_repository_free(nativeRepository);
                nativeRepository = null;
            }
        }
    }

    /// <summary>
    /// This exception will be thrown when you attempt to open a repository,
    /// but it is not found at the specified path.
    /// </summary>
    public class RepositoryNotFoundException : DoggedException
    {
        public RepositoryNotFoundException(string message) : base(message) { }
    }
}
