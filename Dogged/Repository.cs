using System;
using System.Collections.Generic;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// The representation of a Git repository, including its objects,
    /// references and the methods that operate on it.
    /// </summary>
    public class Repository : IDisposable
    {
        private unsafe git_repository* nativeRepository;

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
        }

        private unsafe void Dispose(bool disposing)
        {
            if (nativeRepository != null)
            {
                libgit2.git_repository_free(nativeRepository);
                nativeRepository = null;
            }
        }

        /// <summary>
        /// Frees the native resources associated with this repository.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Repository()
        {
            Dispose(false);
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
