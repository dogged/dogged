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
        public void AttemptsToAccessDisposedIndexThrow()
        {
            Index index;

            using (Repository repo = SandboxRepository("testrepo.git"))
            using (index = repo.Index)
            {
            }

            Assert.Throws<ObjectDisposedException>(() => index.Count);
            Assert.Throws<ObjectDisposedException>(() => index[42]);
        }
    }
}
