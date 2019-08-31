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

        private readonly LazyNative<string> path;
        private readonly LazyNative<string> workdir;

        private readonly LazyNative<bool> isBare;

        private readonly Lazy<ObjectCollection> objects;
        private readonly Lazy<ReferenceCollection> references;

        // Provide a strongly typed exception when a repository is not
        // found to open.
        private static readonly Dictionary<git_error_code, Func<string, Exception>> exceptionMap = new Dictionary<git_error_code, Func<string, Exception>>
        {
            { git_error_code.GIT_ENOTFOUND, (m) => new RepositoryNotFoundException(m) }
        };

        private unsafe Repository(git_repository* nativeRepository)
        {
            Ensure.NativePointerNotNull(nativeRepository);
            this.nativeRepository = nativeRepository;

            path = new LazyNative<string>(() => libgit2.git_repository_path(this.nativeRepository), this);
            workdir = new LazyNative<string>(() => libgit2.git_repository_workdir(this.nativeRepository), this);

            isBare = new LazyNative<bool>(() => libgit2.git_repository_is_bare(this.nativeRepository) == 0 ? false : true, this);

            objects = new Lazy<ObjectCollection>(() => new ObjectCollection(this));
            references = new Lazy<ReferenceCollection>(() => new ReferenceCollection(this));
        }

        /// <summary>
        /// Opens the Git repository at the given <paramref name="path"/>.
        /// This can be either a path to the repository's working tree,
        /// or to the actual repository folder (the ".git" folder beneath
        /// a working directory or a bare repository).
        /// </summary>
        /// <param name="path">The path to the repository or a repository's working tree.</param>
        public unsafe static Repository Open(string path)
        {
            Ensure.ArgumentNotNull(path, "path");

            git_repository *nativeRepository;
            Ensure.NativeSuccess(libgit2.git_repository_open(out nativeRepository, path), exceptionMap);

            return new Repository(nativeRepository);
        }

        /// <summary>
        /// Creates a new Git repository in the given folder.
        /// </summary>
        /// <param name="path">The path to the repository</param>
        /// <param name="bare">If true, a Git repository without a working directory is created at the given path. If false, the provided path will be considered as the working directory into which the .git directory will be created.</param>
        /// <returns>The newly initialized repository</returns>
        public unsafe static Repository Init(string path, bool bare = false)
        {
            Ensure.ArgumentNotNull(path, "path");

            git_repository* nativeRepository;
            Ensure.NativeSuccess(libgit2.git_repository_init(out nativeRepository, path, bare ? (uint)1 : 0));

            return new Repository(nativeRepository);
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
        /// Create a custom repository that is not bound to an on-disk
        /// repository or working directory.  To use it, you can can configure
        /// custom backends.
        /// </summary>
        /// <returns>The newly created repository</returns>
        public unsafe static Repository CreateCustomRepository()
        {
            git_repository* nativeRepository;
            Ensure.NativeSuccess(libgit2.git_repository_new(out nativeRepository));

            return new Repository(nativeRepository);
        }

        internal unsafe git_repository* NativeRepository
        {
            get
            {
                return nativeRepository;
            }
        }

        public unsafe string Path
        {
            get
            {
                return path.Value;
            }
        }

        public unsafe string Workdir
        {
            get
            {
                return workdir.Value;
            }
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
                    return Objects.Lookup<Commit>(head.Target);
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
        /// An accessor for Git objects.
        /// </summary>
        public ObjectCollection Objects
        {
            get
            {
                return objects.Value;
            }
        }

        /// <summary>
        /// An accessor for references.
        /// </summary>
        public ReferenceCollection References
        {
            get
            {
                return references.Value;
            }
        }

        /// <summary>
        /// Get the object database ("ODB") for this repository.  This
        /// provides "raw" access to reading and writing object data;
        /// for most uses, you want the <see cref="Repository.Objects">
        /// collection.
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
            set
            {
                libgit2.git_repository_set_odb(nativeRepository, value.NativeOdb);
            }
        }

        public unsafe FilterList LoadFilterList(Blob blob, string path, FilterMode mode, FilterFlags flags)
        {
            Ensure.ArgumentNotNull(path, "path");
            git_filter_list* filters = null;

            Ensure.NativeSuccess(() => libgit2.git_filter_list_load(out filters, nativeRepository, (blob != null) ? (git_blob*)blob.NativeObject : null, path, (git_filter_mode_t)mode, (uint)flags), this);
            return (filters != null) ? FilterList.FromNative(this, filters) : null;
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
