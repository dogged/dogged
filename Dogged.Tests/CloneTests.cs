using System;
using System.IO;
using Xunit;

using Dogged;

namespace Dogged.Tests
{
    /// <summary>
    /// Test that we can clone repositories locally.
    /// </summary>
    public class CloneTests : TestBase
    {
        [Fact]
        public void CanClone()
        {
            string remotePath = SandboxResource("testrepo");
            string localPath = Path.Combine(TemporaryDirectory, "clone");

            using (Repository repo = Repository.Clone(remotePath, localPath))
            {
                Assert.False(repo.IsBare);
                using (Commit commit = repo.HeadCommit)
                {
                    Assert.Equal(new ObjectId("099fabac3a9ea935598528c27f866e34089c2eff"), commit.Id);
                }
            }
        }
    }
}
