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
        public unsafe Repository(string path)
        {
            Ensure.NativeSuccess(libgit2.git_repository_open(out nativeRepository, path), exceptionMap);

            isBare = new LazyNative<bool>(() => libgit2.git_repository_is_bare(nativeRepository) == 0 ? false : true, this);
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
