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

        [Fact]
        public void CanMatchDiffExistingFile()
        {
            string repositoryPath = SandboxResource("testrepo");

            using (Repository repo = Repository.Open(repositoryPath))
            {
                Commit newCommit = repo.Objects.Lookup<Commit>(new ObjectId("a65fedf39aefe402d3bb6e24df4d4f5fe4547750"));
                Commit oldCommit = repo.Objects.Lookup<Commit>(new ObjectId("be3563ae3f795b2b4353bcce3a527ad0a4f7f644"));
                Diff diff = repo.Diff(oldCommit.Tree, newCommit.Tree);

                Assert.True(diff.IsMatch(PathSpec.Create("branch_file.txt")));
            }
        }

        [Fact]
        public void CanMatchDiffNonExistentFile()
        {
            string repositoryPath = SandboxResource("testrepo");

            using (Repository repo = Repository.Open(repositoryPath))
            {
                Commit newCommit = repo.Objects.Lookup<Commit>(new ObjectId("a65fedf39aefe402d3bb6e24df4d4f5fe4547750"));
                Commit oldCommit = repo.Objects.Lookup<Commit>(new ObjectId("be3563ae3f795b2b4353bcce3a527ad0a4f7f644"));
                Diff diff = repo.Diff(oldCommit.Tree, newCommit.Tree);

                Assert.False(diff.IsMatch(PathSpec.Create("nothing.txt")));
            }
        }

        [Fact]
        public void CanMatchDiffAnyFile()
        {
            string repositoryPath = SandboxResource("testrepo");

            using (Repository repo = Repository.Open(repositoryPath))
            {
                Commit newCommit = repo.Objects.Lookup<Commit>(new ObjectId("a65fedf39aefe402d3bb6e24df4d4f5fe4547750"));
                Commit oldCommit = repo.Objects.Lookup<Commit>(new ObjectId("be3563ae3f795b2b4353bcce3a527ad0a4f7f644"));
                Diff diff = repo.Diff(oldCommit.Tree, newCommit.Tree);

                Assert.True(diff.IsMatch(PathSpec.Create("branch_file.txt", "nothing.txt")));
            }
        }

        [Fact]
        public void CanMatchDiffGlobbing()
        {
            string repositoryPath = SandboxResource("testrepo");

            using (Repository repo = Repository.Open(repositoryPath))
            {
                Commit newCommit = repo.Objects.Lookup<Commit>(new ObjectId("a65fedf39aefe402d3bb6e24df4d4f5fe4547750"));
                Commit oldCommit = repo.Objects.Lookup<Commit>(new ObjectId("be3563ae3f795b2b4353bcce3a527ad0a4f7f644"));
                Diff diff = repo.Diff(oldCommit.Tree, newCommit.Tree);

                Assert.True(diff.IsMatch(PathSpec.Create("branch_file.*")));
                Assert.False(diff.IsMatch(PathSpec.Create("branch_file.*"), PathSpecFlags.NoGlob));
            }
        }
    }
}
