using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

using Dogged;

namespace Dogged.Tests
{
    /// <summary>
    /// Tests for working with tree objects.
    /// </summary>
    public class TreeTests : TestBase
    {
        private class TreeEntryData
        {
            public TreeEntryData(FileMode mode, ObjectType type, ObjectId id, string name)
            {
                Mode = mode;
                Type = type;
                Id = id;
                Name = name;
            }

            public FileMode Mode { get; private set; }
            public ObjectType Type { get; private set; }
            public ObjectId Id { get; private set; }
            public string Name { get; private set; }
        };

        private TreeEntryData[] entries = {
            new TreeEntryData(FileMode.Blob, ObjectType.Blob, new ObjectId("a8233120f6ad708f843d861ce2b7228ec4e3dec6"), "README"),
            new TreeEntryData(FileMode.Blob, ObjectType.Blob, new ObjectId("3697d64be941a53d4ae8f6a271e4e3fa56b022cc"), "branch_file.txt"),
            new TreeEntryData(FileMode.Link, ObjectType.Blob, new ObjectId("c0528fd6cc988c0a40ce0be11bc192fc8dc5346e"), "link_to_new.txt"),
            new TreeEntryData(FileMode.Blob, ObjectType.Blob, new ObjectId("a71586c1dfe8a71c6cbf6c129f404c5642ff31bd"), "new.txt"),
        };

        [Theory]
        [InlineData("45dd856fdd4d89b884c340ba0e047752d9b085d6")]
        public void CanLookupTree(string hex)
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (Tree t = repo.Objects.Lookup<Tree>(new ObjectId(hex)))
            {
                Assert.NotNull(t);
                Assert.Equal(ObjectType.Tree, t.Type);
                Assert.Equal(new ObjectId(hex), t.Id);
            }
        }

        [Fact]
        public void CanGetTreeEntryByIndex()
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (Tree t = repo.Objects.Lookup<Tree>(new ObjectId("45dd856fdd4d89b884c340ba0e047752d9b085d6")))
            {
                Assert.Equal(4, t.Count);

                for(int i = 0; i < 4; i++)
                {
                    var expected = entries[i];
                    var entry = t[i];

                    Assert.Equal(expected.Mode, entry.Mode);
                    Assert.Equal(expected.Type, entry.Type);
                    Assert.Equal(expected.Id, entry.Id);
                    Assert.Equal(expected.Name, entry.Name);
                }
            }
        }

        [Fact]
        public void CanLookupTreeEntryByName()
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (Tree t = repo.Objects.Lookup<Tree>(new ObjectId("45dd856fdd4d89b884c340ba0e047752d9b085d6")))
            {
                foreach (var expected in entries)
                {
                    var entry = t[expected.Name];
                    Assert.NotNull(entry);

                    Assert.Equal(expected.Mode, entry.Mode);
                    Assert.Equal(expected.Type, entry.Type);
                    Assert.Equal(expected.Id, entry.Id);
                    Assert.Equal(expected.Name, entry.Name);
                }
            }
        }

        [Fact]
        public void CanIterateTreeEntries()
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (Tree t = repo.Objects.Lookup<Tree>(new ObjectId("45dd856fdd4d89b884c340ba0e047752d9b085d6")))
            {
                int i = 0;

                foreach (var entry in t)
                {
                    var expected = entries[i++];

                    Assert.Equal(expected.Mode, entry.Mode);
                    Assert.Equal(expected.Type, entry.Type);
                    Assert.Equal(expected.Id, entry.Id);
                    Assert.Equal(expected.Name, entry.Name);
                }
            }
        }

        [Fact]
        public void AttemptToGetInvalidTreeEntryThrows()
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (Tree t = repo.Objects.Lookup<Tree>(new ObjectId("45dd856fdd4d89b884c340ba0e047752d9b085d6")))
            {
                Assert.Throws<ArgumentException>(() => t[-1]);
                Assert.Throws<IndexOutOfRangeException>(() => t[4]);
            }
        }

        [Fact]
        public void AttemptingToAccessDisposedTreeThrows()
        {
            Tree tree;

            using (Repository repo = SandboxRepository("testrepo"))
            using (tree = repo.Objects.Lookup<Tree>(new ObjectId("45dd856fdd4d89b884c340ba0e047752d9b085d6")))
            {
            }

            Assert.Throws<ObjectDisposedException>(() => tree.Id);
            Assert.Throws<ObjectDisposedException>(() => tree[0]);
            Assert.Throws<ObjectDisposedException>(() => tree["README"]);
            Assert.Throws<ObjectDisposedException>(() => { foreach (var e in tree) { } });
        }

        [Fact]
        public void AttempingToAccessEntryOfDisposedTreeThrows()
        {
            List<TreeEntry> entries = new List<TreeEntry>();

            using (Repository repo = SandboxRepository("testrepo"))
            using (Tree t = repo.Objects.Lookup<Tree>(new ObjectId("45dd856fdd4d89b884c340ba0e047752d9b085d6")))
            {
                foreach (var entry in t)
                {
                    entries.Add(entry);
                }
            }

            foreach (var entry in entries)
            {
                Assert.Throws<ObjectDisposedException>(() => entry.Mode);
                Assert.Throws<ObjectDisposedException>(() => entry.Type);
                Assert.Throws<ObjectDisposedException>(() => entry.Id);
                Assert.Throws<ObjectDisposedException>(() => entry.Name);
            }
        }
    }
}
