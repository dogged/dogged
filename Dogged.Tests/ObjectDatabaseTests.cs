using System;
using System.Collections.Generic;
using System.IO;
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
