using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

using Dogged;

namespace Dogged.Tests
{
    /// <summary>
    /// Test that we can operate on indexes.
    /// </summary>
    public class IndexTests : TestBase
    {
        [Fact]
        public void CanGetIndexFromRepository()
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (Index index = repo.Index)
            {
            }
        }

        [Fact]
        public void GetIndexEntryCount()
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (Index index = repo.Index)
            {
                Assert.Equal(109, index.Count);
            }
        }

        [Fact]
        public void CanGetIndexEntryByPosition()
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (Index index = repo.Index)
            {
                IndexEntry entry = index[0];

                Assert.Equal(FileMode.Blob, entry.Mode);
                Assert.Equal(new ObjectId("fd8430bc864cfcd5f10e5590f8a447e01b942bfe"), entry.Id);
                Assert.Equal(".HEADER", entry.Path);
            }
        }

        [Fact]
        public void AccessingAnOutOfRangePositionThrows()
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (Index index = repo.Index)
            {
                Assert.Throws<ArgumentException>(() => index[-1]);
                Assert.Throws<ArgumentException>(() => index[int.MinValue]);

                Assert.Throws<IndexOutOfRangeException>(() => index[110]);
                Assert.Throws<IndexOutOfRangeException>(() => index[int.MaxValue]);
            }
        }

        [Fact]
        public void CanGetIndexEntryByPath()
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (Index index = repo.Index)
            {
                IndexEntry entry = index[".HEADER"];

                Assert.Equal(FileMode.Blob, entry.Mode);
                Assert.Equal(new ObjectId("fd8430bc864cfcd5f10e5590f8a447e01b942bfe"), entry.Id);
                Assert.Equal(".HEADER", entry.Path);
            }
        }

        [Fact]
        public void AccessingAnInvalidPathThrows()
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (Index index = repo.Index)
            {
                Assert.Throws<ArgumentNullException>(() => index[null]);

                Assert.Throws<KeyNotFoundException>(() => index["foo"]);
                Assert.Throws<KeyNotFoundException>(() => index["bar"]);
            }
        }

        [Fact]
        public void EnumerateIndexEntries()
        {
            IndexEntry[] expected = new[] {
                new IndexEntry(".gitmodules", FileMode.Blob, new ObjectId("51589c218bf77a8da9e9d8dbc097d76a742726c4")),
                new IndexEntry("sub", FileMode.Commit, new ObjectId("b7a59b3f4ea13b985f8a1e0d3757d5cd3331add8"))
            };

            using (Repository repo = SandboxRepository("super"))
            using (Index index = repo.Index)
            {
                Assert.Equal(expected[0], repo.Index.First());
                Assert.Equal(expected[1], repo.Index.Last());
            }
        }

        [Fact]
        public void AttemptsToResetIndexEnumeratorThrows()
        {
            IndexEntry[] expected = new[] {
                new IndexEntry(".gitmodules", FileMode.Blob, new ObjectId("51589c218bf77a8da9e9d8dbc097d76a742726c4")),
                new IndexEntry("sub", FileMode.Commit, new ObjectId("b7a59b3f4ea13b985f8a1e0d3757d5cd3331add8"))
            };

            using (Repository repo = SandboxRepository("super"))
            using (Index index = repo.Index)
            using (IEnumerator<IndexEntry> enumerator = index.GetEnumerator())
            {
                Assert.True(enumerator.MoveNext());
                Assert.Equal(expected[0], enumerator.Current);
                Assert.True(enumerator.MoveNext());

                Assert.Throws<NotSupportedException>(() => enumerator.Reset());
            }
        }

        [Fact]
        public void AttemptsToAccessDisposedIndexThrow()
        {
            Index index;

            using (Repository repo = SandboxRepository("testrepo.git"))
            using (index = repo.Index)
            {
            }

            Assert.Throws<ObjectDisposedException>(() => index.Count);
            Assert.Throws<ObjectDisposedException>(() => index[42]);
            Assert.Throws<ObjectDisposedException>(() => index.First());
        }
    }
}
