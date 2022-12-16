using Xunit;

namespace Dogged.Tests
{
    /// <summary>
    /// Tests for pathspec.
    /// </summary>
    public class PathSpecTests : TestBase
    {
        [Fact]
        public void CanMatchTreeExistingFile()
        {
            string repositoryPath = SandboxResource("testrepo");

            using (Repository repo = Repository.Open(repositoryPath))
            {
                Assert.True(repo.HeadCommit.Tree.IsMatch(PathSpec.Create("branch_file.txt")));
                Assert.True(repo.HeadCommit.Tree.IsMatch(PathSpec.Create("BRANCH_FILE.TXT"), PathSpecFlags.IgnoreCase));
                Assert.False(repo.HeadCommit.Tree.IsMatch(PathSpec.Create("BRANCH_FILE.TXT"), PathSpecFlags.UseCase));
            }
        }

        [Fact]
        public void CanMatchTreeNonExistentFile()
        {
            string repositoryPath = SandboxResource("testrepo");

            using (Repository repo = Repository.Open(repositoryPath))
            {
                Assert.False(repo.HeadCommit.Tree.IsMatch(PathSpec.Create("nothing.txt")));
                Assert.False(repo.HeadCommit.Tree.IsMatch(PathSpec.Create("NOTHING.TXT"), PathSpecFlags.IgnoreCase));
                Assert.False(repo.HeadCommit.Tree.IsMatch(PathSpec.Create("NOTHING.TXT"), PathSpecFlags.UseCase));
            }
        }

        [Fact]
        public void CanMatchTreeAnyFile()
        {
            string repositoryPath = SandboxResource("testrepo");

            using (Repository repo = Repository.Open(repositoryPath))
            {
                Assert.True(repo.HeadCommit.Tree.IsMatch(PathSpec.Create("branch_file.txt", "nothing.txt")));
            }
        }

        [Fact]
        public void CanMatchTreeGlobbing()
        {
            string repositoryPath = SandboxResource("testrepo");

            using (Repository repo = Repository.Open(repositoryPath))
            {
                Assert.True(repo.HeadCommit.Tree.IsMatch(PathSpec.Create("branch_file.*")));
                Assert.False(repo.HeadCommit.Tree.IsMatch(PathSpec.Create("branch_file.*"), PathSpecFlags.NoGlob));
            }
        }
    }
}
