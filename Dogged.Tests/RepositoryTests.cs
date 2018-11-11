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
    }
}
