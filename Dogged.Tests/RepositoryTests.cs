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

            using (Repository repo = Repository.Open(repositoryPath))
            {
            }
        }

        [Fact]
        public void CanOpenBareRepository()
        {
            string repositoryPath = SandboxResource("testrepo.git");

            using (Repository repo = Repository.Open(repositoryPath))
            {
            }
        }

        [Fact]
        public void RepositoryNotFoundFailsWithWellTypedException()
        {
            string nonexistentPath = Path.Combine(TemporaryDirectory, "nonexistent");

            Assert.Throws<RepositoryNotFoundException>(() => {
                using (Repository repo = Repository.Open(nonexistentPath))
                {
                }
            });
        }

        [Fact]
        public void CanInitializeRepository()
        {
            string newPath = Path.Combine(TemporaryDirectory, "newrepo");

            using (Repository repo = Repository.Init(newPath))
            {
                Assert.True(Directory.Exists(newPath));
                Assert.True(Directory.Exists(Path.Combine(newPath, ".git")));
            }
        }

        [Fact]
        public void CanInitializeBareRepository()
        {
            string newPath = Path.Combine(TemporaryDirectory, "newrepo.git");

            using (Repository repo = Repository.Init(newPath, true))
            {
                Assert.True(Directory.Exists(newPath));
                Assert.True(File.Exists(Path.Combine(newPath, "HEAD")));
            }
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
        public void CanGetRepositoryHead()
        {
            using (Repository repo = SandboxRepository("testrepo"))
            {
                // HEAD is not detached but git_repository_head gives us a peeled reference
                Assert.False(repo.IsHeadDetached);

                using (var head = repo.Head)
                {
                    Assert.NotNull(head);
                    Assert.IsType<DirectReference>(head);
                    Assert.Equal("refs/heads/master", head.Name);
                    Assert.Equal(new ObjectId("099fabac3a9ea935598528c27f866e34089c2eff"), ((DirectReference)head).Target);
                }
            }
        }

        [Fact]
        public void CanGetRepositoryHeadAsCommit()
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (Commit commit = repo.HeadCommit)
            {
                Assert.Equal(new ObjectId("099fabac3a9ea935598528c27f866e34089c2eff"), commit.Id);
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
            Assert.Throws<ObjectDisposedException>(() => repo.Index);
            Assert.Throws<ObjectDisposedException>(() => repo.Head);
        }
    }
}
