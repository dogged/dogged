using System;
using System.Linq;
using Xunit;

namespace Dogged.Tests
{
    /// <summary>
    /// Tests for revision walker.
    /// </summary>
    public class RevisionWalkerTests : TestBase
    {
        [Fact]
        public void CanTraverseAll()
        {
            string repositoryPath = SandboxResource("testrepo");

            using (Repository repo = Repository.Open(repositoryPath))
            using (RevisionWalker walker = RevisionWalker.Create(repo))
            {
                walker.PushHead();
                Assert.Equal(8, walker.Count());
            }
        }

        [Fact]
        public void CanTraverseCommit()
        {
            string repositoryPath = SandboxResource("testrepo");

            using (Repository repo = Repository.Open(repositoryPath))
            using (RevisionWalker walker = RevisionWalker.Create(repo))
            {
                walker.PushCommit(new ObjectId("4a202b346bb0fb0db7eff3cffeb3c70babbd2045"));
                Assert.Equal(3, walker.Count());

                walker.PushHead();
                walker.HideCommit(new ObjectId("be3563ae3f795b2b4353bcce3a527ad0a4f7f644"));
                Assert.Equal(2, walker.Count());
            }
        }

        [Fact]
        public void CanTraverseGlob()
        {
            string repositoryPath = SandboxResource("testrepo");

            using (Repository repo = Repository.Open(repositoryPath))
            using (RevisionWalker walker = RevisionWalker.Create(repo))
            {
                walker.PushGlob("refs/heads/*");
                Assert.Equal(20, walker.Count());

                walker.Reset();
                walker.PushGlob("refs/heads/*");
                walker.HideHead();
                walker.HideGlob("refs/heads/b*");
                Assert.Equal(11, walker.Count());
            }
        }

        [Fact]
        public void CanTraverseRange()
        {
            string repositoryPath = SandboxResource("testrepo");

            using (Repository repo = Repository.Open(repositoryPath))
            using (RevisionWalker walker = RevisionWalker.Create(repo))
            {
                walker.PushRange("HEAD~3..HEAD");
                Assert.Equal(4, walker.Count());
            }
        }

        [Fact]
        public void CanTraverseReference()
        {
            string repositoryPath = SandboxResource("testrepo");

            using (Repository repo = Repository.Open(repositoryPath))
            using (RevisionWalker walker = RevisionWalker.Create(repo))
            {
                walker.PushReference(repo.References.Lookup("refs/heads/executable"));
                Assert.Equal(9, walker.Count());

                walker.Reset();
                walker.PushReference(repo.References.Lookup("refs/heads/dir"));
                walker.HideReference(repo.References.Lookup("refs/heads/br2"));
                Assert.Equal(2, walker.Count());
            }
        }

        [Fact]
        public void CanTraverseSorted()
        {
            string[] expectedCommits =
                {
                    "099fabac3a9ea935598528c27f866e34089c2eff",
                    "a65fedf39aefe402d3bb6e24df4d4f5fe4547750",
                    "be3563ae3f795b2b4353bcce3a527ad0a4f7f644",
                    "c47800c7266a2be04c571c04d5a6614691ea99bd",
                    "9fd738e8f7967c078dceed8190330fc8648ee56a",
                    "4a202b346bb0fb0db7eff3cffeb3c70babbd2045",
                    "5b5b025afb0b4c913b4c338a42934a3863bf3644",
                    "8496071c1b46c854b31185ea97743be6a8774479",
                };

            string repositoryPath = SandboxResource("testrepo");

            using (Repository repo = Repository.Open(repositoryPath))
            using (RevisionWalker walker = RevisionWalker.Create(repo))
            {
                walker.PushHead();
                Assert.Equal(
                    expectedCommits,
                    walker.Select(commit => commit.Id.ToString()).ToArray());

                walker.Reset();
                walker.PushHead();
                walker.SortFlags = SortFlags.Reverse;
                Assert.Equal(
                    expectedCommits.Reverse().ToArray(),
                    walker.Select(commit => commit.Id.ToString()).ToArray());
            }
        }
    }
}
