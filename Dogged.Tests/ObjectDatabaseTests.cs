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
    }
}
