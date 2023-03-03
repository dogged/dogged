using System;
using Xunit;

using Dogged;

namespace Dogged.Tests
{
    /// <summary>
    /// Test that we can operate on commits.
    /// </summary>
    public class CommitTests : TestBase
    {
        [Fact]
        public void CanLookupCommit()
        {
            ObjectId id = new ObjectId("be3563ae3f795b2b4353bcce3a527ad0a4f7f644");

            using (Repository repo = SandboxRepository("testrepo"))
            using (Commit c = repo.Objects.Lookup<Commit>(id))
            {
                Assert.NotNull(c);
                Assert.Equal(ObjectType.Commit, c.Type);
                Assert.Equal(id, c.Id);

                Assert.Equal("Scott Chacon", c.Author.Name);
                Assert.Equal("schacon@gmail.com", c.Author.Email);
                Assert.Equal(DateTimeOffset.FromUnixTimeSeconds(1274813907), c.Author.When);
                Assert.Equal("Merge branch 'br2'\n", c.Message);

                Assert.Equal(2, c.Parents.Count);
                Assert.Equal(new ObjectId("9fd738e8f7967c078dceed8190330fc8648ee56a"), c.Parents[0].Id);
                Assert.Equal(new ObjectId("c47800c7266a2be04c571c04d5a6614691ea99bd"), c.Parents[1].Id);

                Assert.Equal(new ObjectId("1810dff58d8a660512d4832e740f692884338ccd"), c.TreeId);
            }
        }

        [Fact]
        public void AttemptingToAccessDisposedCommitThrows()
        {
            Commit commit;

            using (Repository repo = SandboxRepository("testrepo"))
            using (commit = repo.Objects.Lookup<Commit>(new ObjectId("be3563ae3f795b2b4353bcce3a527ad0a4f7f644")))
            {
            }

            Assert.Throws<ObjectDisposedException>(() => commit.Id);
            Assert.Throws<ObjectDisposedException>(() => commit.Author);
            Assert.Throws<ObjectDisposedException>(() => commit.Committer);
            Assert.Throws<ObjectDisposedException>(() => commit.TreeId);
            Assert.Throws<ObjectDisposedException>(() => commit.Tree);
        }

        [Fact]
        public void CanUseTreeAfterDisposingCommit()
        {
            Tree tree;

            using (Repository repo = SandboxRepository("testrepo"))
            using (Commit commit = repo.Objects.Lookup<Commit>(new ObjectId("be3563ae3f795b2b4353bcce3a527ad0a4f7f644")))
            {
                tree = commit.Tree;
            }

            Assert.Equal(new ObjectId("1810dff58d8a660512d4832e740f692884338ccd"), tree.Id);
            tree.Dispose();
        }
    }
}
