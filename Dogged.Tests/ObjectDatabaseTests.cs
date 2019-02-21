using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

using Dogged;

namespace Dogged.Tests
{
    /// <summary>
    /// Tests for working with object databases.
    /// </summary>
    public class ObjectDatabaseTests : TestBase
    {
        [Theory]
        [InlineData("a71586c1dfe8a71c6cbf6c129f404c5642ff31bd", 12, ObjectType.Blob)]
        [InlineData("45dd856fdd4d89b884c340ba0e047752d9b085d6", 155, ObjectType.Tree)]
        [InlineData("099fabac3a9ea935598528c27f866e34089c2eff", 224, ObjectType.Commit)]
        [InlineData("7b4384978d2493e851f9cca7858815fac9b10980", 156, ObjectType.Tag)]
        public void CanReadHeader(string id, long size, ObjectType type)
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (ObjectDatabase odb = repo.ObjectDatabase)
            {
                Assert.Equal((size, type), odb.ReadHeader(new ObjectId(id)));
            }
        }

        [Theory]
        [InlineData("a71586c1dfe8a71c6cbf6c129f404c5642ff31bd", "my new file\n", 12, ObjectType.Blob)]
        [InlineData("099fabac3a9ea935598528c27f866e34089c2eff", "tree 45dd856fdd4d89b884c340ba0e047752d9b085d6\nparent a65fedf39aefe402d3bb6e24df4d4f5fe4547750\nauthor Ben Straub <bstraub@github.com> 1342477396 -0700\ncommitter Ben Straub <bstraub@github.com> 1342477396 -0700\n\nAdd a symlink\n", 224, ObjectType.Commit)]
        public void CanRead(string id, string contents, long size, ObjectType type)
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (ObjectDatabase odb = repo.ObjectDatabase)
            using (ObjectDatabaseObject obj = odb.Read(new ObjectId(id)))
            {
                var contentBytes = Encoding.UTF8.GetBytes(contents);

                Assert.Equal(id, obj.Id.ToString());
                Assert.Equal(type, obj.Type);
                Assert.Equal(size, obj.Size);
                Assert.True(new ReadOnlySpan<byte>(contentBytes).SequenceEqual(obj.Data));
            }
        }

        [Fact]
        public void AttemptingToAccessDisposedObjectDatabaseObjecetThrows()
        {
            ObjectDatabaseObject obj;

            using (Repository repo = SandboxRepository("testrepo"))
            using (ObjectDatabase odb = repo.ObjectDatabase)
            using (obj = odb.Read(new ObjectId("a71586c1dfe8a71c6cbf6c129f404c5642ff31bd")))
            {
            }

            Assert.Throws<ObjectDisposedException>(() => obj.Id);
        }

        [Theory]
        [InlineData("Hello, world.\n", ObjectType.Blob, "f75ba05f340c51065cbea2e1fdbfe5fe13144c97")]
        [InlineData("tree 0a890bd10328d68f6d85efd2535e3a4c588ee8e6\nauthor Edward Thomson <ethomson@edwardthomson.com> 1550772904 +0000\ncommitter Edward Thomson <ethomson@edwardthomson.com> 1550772904 +0000\n\nfoo\n", ObjectType.Commit, "dbc2318bd976cee438f9a752286ff9a8b421df2e")]
        public void CanWrite(string contents, ObjectType type, string expectedId)
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (ObjectDatabase odb = repo.ObjectDatabase)
            {
                var id = odb.Write(Encoding.UTF8.GetBytes(contents), type);

                Assert.Equal(expectedId, id.ToString());
            }
        }

        [Fact]
        public void CanIterateObjects()
        {
            int count = 0;

            using (Repository repo = SandboxRepository("testrepo"))
            using (ObjectDatabase odb = repo.ObjectDatabase)
            {
                odb.ForEachObject((id) => {
                    count++;
                    return true;
                });
            }

            Assert.Equal(1703, count);
        }

        [Fact]
        public void CanStopObjectIteration()
        {
            int count = 0;

            using (Repository repo = SandboxRepository("testrepo"))
            using (ObjectDatabase odb = repo.ObjectDatabase)
            {
                odb.ForEachObject((id) => {
                    return (++count == 5) ? false : true;
                });
            }

            Assert.Equal(5, count);
        }

        [Fact]
        public void CanThrowDuringObjectIteration()
        {
            int count = 0;

            using (Repository repo = SandboxRepository("testrepo"))
            using (ObjectDatabase odb = repo.ObjectDatabase)
            {
                Assert.Throws<InvalidTimeZoneException>(() => {
                    odb.ForEachObject((id) => {
                        if (++count == 5)
                        {
                            throw new InvalidTimeZoneException();
                        }

                        return true;
                    });
                });
            }

            Assert.Equal(5, count);
        }

        // Ensure the object is not in the testrepo repository but can be
        // found by adding another repository's loose object directory as
        // a custom backend.
        [Fact]
        public void CanAddLooseBackend()
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (ObjectDatabase odb = repo.ObjectDatabase)
            {
                var oid = new ObjectId("e350052cc767cd1fcb37e84e9a89e701925be4ae");

                Assert.Throws<NotFoundException>(() => odb.ReadHeader(oid));

                var otherRepositoryPath = SandboxResource("submodules.git");
                var path = Path.Combine(otherRepositoryPath, "objects");

                var backend = ObjectDatabaseBackend.CreateLooseBackend(path);
                odb.AddBackend(backend, 100);

                Assert.Equal((195, ObjectType.Blob), odb.ReadHeader(oid));
            }
        }

        // Ensure the object is not in the testrepo repository but can be
        // found by adding another repository's packed object directory as
        // a custom backend.
        [Fact]
        public void CanAddPackBackend()
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (ObjectDatabase odb = repo.ObjectDatabase)
            {
                var oid1 = new ObjectId("739e3c4c51919baf6c4f55e7a74f5da2eba42ab0");
                var oid2 = new ObjectId("0ddeaded9502971eefe1e41e34d0e536853ae20f");

                Assert.Throws<NotFoundException>(() => odb.ReadHeader(oid1));
                Assert.Throws<NotFoundException>(() => odb.ReadHeader(oid2));

                var otherRepositoryPath = SandboxResource("duplicate.git");
                var path = Path.Combine(otherRepositoryPath, "objects");

                var backend = ObjectDatabaseBackend.CreatePackBackend(path);
                odb.AddBackend(backend, 100);

                Assert.Equal((7, ObjectType.Blob), odb.ReadHeader(oid1));
                Assert.Equal((7, ObjectType.Blob), odb.ReadHeader(oid2));
            }
        }

        // Ensure the object is not in the testrepo repository but can be
        // found by adding a single packfile from another repository as a
        // custom backend.
        [Fact]
        public void CanAddPackfileBackend()
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (ObjectDatabase odb = repo.ObjectDatabase)
            {
                var oid1 = new ObjectId("739e3c4c51919baf6c4f55e7a74f5da2eba42ab0");
                var oid2 = new ObjectId("0ddeaded9502971eefe1e41e34d0e536853ae20f");

                Assert.Throws<NotFoundException>(() => odb.ReadHeader(oid1));
                Assert.Throws<NotFoundException>(() => odb.ReadHeader(oid2));

                var otherRepositoryPath = SandboxResource("duplicate.git");
                var path = Path.Combine(otherRepositoryPath, "objects", "pack", "pack-b18eeacbd65cbd30a365d7564b45a468e8bd43d6.idx");

                var backend = ObjectDatabaseBackend.CreatePackfileBackend(path);
                odb.AddBackend(backend, 100);

                Assert.Throws<NotFoundException>(() => odb.ReadHeader(oid1));
                Assert.Equal((7, ObjectType.Blob), odb.ReadHeader(oid2));
            }
        }
    }
}
