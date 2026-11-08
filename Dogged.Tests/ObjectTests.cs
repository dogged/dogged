using System;
using System.IO;
using Xunit;

using Dogged;

namespace Dogged.Tests
{
    /// <summary>
    /// Tests for objects.
    /// </summary>
    public class ObjectTests : TestBase
    {
        [Fact]
        public void CanLookupHeadCommit()
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (Commit head = repo.Objects.Lookup<Commit>(repo.Head.Target))
            {
                Assert.NotNull(head);
                Assert.Equal(ObjectType.Commit, head.Type);
                Assert.Equal(new ObjectId("099fabac3a9ea935598528c27f866e34089c2eff", ObjectIdType.SHA1), head.Id);
            }
        }

        [Theory]
        [InlineData("099fabac3a9ea935598528c27f866e34089c2eff", ObjectIdType.SHA1, ObjectType.Commit)]
        [InlineData("45dd856fdd4d89b884c340ba0e047752d9b085d6", ObjectIdType.SHA1, ObjectType.Tree)]
        [InlineData("c0528fd6cc988c0a40ce0be11bc192fc8dc5346e", ObjectIdType.SHA1, ObjectType.Blob)]
        [InlineData("a8233120f6ad708f843d861ce2b7228ec4e3dec6", ObjectIdType.SHA1, ObjectType.Blob)]
        public void CanLookupObject(string hex, ObjectIdType idType, ObjectType objectType)
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (GitObject o = repo.Objects.Lookup(new ObjectId(hex, idType)))
            {
                Assert.NotNull(o);
                Assert.Equal(objectType, o.Type);
                Assert.Equal(new ObjectId(hex, idType), o.Id);
            }
        }

        [Theory]
        [InlineData("099fabac3a9ea935598528c27f866e34089c2eff", ObjectIdType.SHA1, ObjectType.Commit)]
        [InlineData("45dd856fdd4d89b884c340ba0e047752d9b085d6", ObjectIdType.SHA1, ObjectType.Tree)]
        [InlineData("c0528fd6cc988c0a40ce0be11bc192fc8dc5346e", ObjectIdType.SHA1, ObjectType.Blob)]
        [InlineData("a8233120f6ad708f843d861ce2b7228ec4e3dec6", ObjectIdType.SHA1, ObjectType.Blob)]
        public void CanLookupObjectWithGeneric(string hex, ObjectIdType idType, ObjectType objectType)
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (GitObject o = repo.Objects.Lookup<GitObject>(new ObjectId(hex, idType)))
            {
                Assert.NotNull(o);
                Assert.Equal(objectType, o.Type);
                Assert.Equal(new ObjectId(hex, idType), o.Id);
            }
        }

        [Theory]
        [InlineData("099fabac3a9ea935598528c27f866e34089c2eff", ObjectIdType.SHA1)]
        public void CanLookupCommit(string hex, ObjectIdType idType)
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (Commit c = repo.Objects.Lookup<Commit>(new ObjectId(hex, idType)))
            {
                Assert.NotNull(c);
                Assert.Equal(ObjectType.Commit, c.Type);
                Assert.Equal(new ObjectId(hex, idType), c.Id);
            }
        }

        [Theory]
        [InlineData("c0528fd6cc988c0a40ce0be11bc192fc8dc5346e", ObjectIdType.SHA1)]
        [InlineData("a8233120f6ad708f843d861ce2b7228ec4e3dec6", ObjectIdType.SHA1)]
        public void CanLookupBlob(string hex, ObjectIdType idType)
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (Blob b = repo.Objects.Lookup<Blob>(new ObjectId(hex, idType)))
            {
                Assert.NotNull(b);
                Assert.Equal(ObjectType.Blob, b.Type);
                Assert.Equal(new ObjectId(hex, idType), b.Id);
            }
        }

        [Theory]
        [InlineData("099fabac3a9ea935598528c27f866e34089c2eff", ObjectIdType.SHA1)]
        [InlineData("45dd856fdd4d89b884c340ba0e047752d9b085d6", ObjectIdType.SHA1)]
        [InlineData("a8233120f6ad708f843d861ce2b7228ec4e3dec6", ObjectIdType.SHA1)]
        public void AttemptsToAccessDisposedObjectThrow(string hex, ObjectIdType idType)
        {
            GitObject obj;

            using (Repository repo = SandboxRepository("testrepo"))
            using (obj = repo.Objects.Lookup(new ObjectId(hex, idType)))
            {
            }

            Assert.Throws<ObjectDisposedException>(() => obj.Id);
        }
    }
}
