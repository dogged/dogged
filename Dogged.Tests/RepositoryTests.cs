using System;
using System.IO;
using Xunit;

using Dogged;

namespace Dogged.Tests
{
    /// <summary>
    /// Test that we can operate on repositories.
    /// </summary>
    public class RepositoryTests : TestBase
    {
        [Fact]
        public void CanOpenRepository()
        {
            string repositoryPath = SandboxResource("testrepo");

            using (Repository repo = new Repository(repositoryPath))
            {
            }
        }

        [Fact]
        public void CanOpenBareRepository()
        {
            string repositoryPath = SandboxResource("testrepo.git");

            using (Repository repo = new Repository(repositoryPath))
            {
            }
        }

        [Fact]
        public void RepositoryNotFoundFailsWithWellTypedException()
        {
            string nonexistentPath = Path.Combine(TemporaryDirectory, "nonexistent");

            Assert.Throws<RepositoryNotFoundException>(() => {
                using (Repository repo = new Repository(nonexistentPath))
                {
                }
            });
        }

        [Fact]
        public void CanIdentifyNonBareRepository()
        {
            using (Repository repo = SandboxRepository("testrepo"))
            {
                Assert.False(repo.IsBare);
            }
        }

        [Fact]
        public void CanIdentifyBareRepository()
        {
            using (Repository repo = SandboxRepository("testrepo.git"))
            {
                Assert.True(repo.IsBare);
            }
        }

        [Fact]
        public void AttemptsToAccessDisposedRepositoryThrow()
        {
            Repository repo;

            using (repo = SandboxRepository("testrepo.git"))
            {
            }

            Assert.Throws<ObjectDisposedException>(() => repo.IsBare);
        }
    }
}
