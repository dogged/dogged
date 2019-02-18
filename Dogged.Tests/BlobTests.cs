using System;
using Xunit;

using Dogged;

namespace Dogged.Tests
{
    /// <summary>
    /// Test that we can operate on blobs.
    /// </summary>
    public class BlobTests : TestBase
    {
        [Fact]
        public void CanLookupBlob()
        {
            ObjectId id = new ObjectId("a8233120f6ad708f843d861ce2b7228ec4e3dec6");

            using (Repository repo = SandboxRepository("testrepo"))
            using (Blob b = repo.Lookup<Blob>(id))
            {
                Assert.NotNull(b);
                Assert.Equal(ObjectType.Blob, b.Type);
                Assert.Equal(id, b.Id);

                Assert.False(b.IsBinary);
                Assert.Equal(10, b.RawSize);
                Assert.True(b.RawContent.SequenceEqual(new ReadOnlySpan<byte>(new byte[] { 0x68, 0x65, 0x79, 0x20, 0x74, 0x68, 0x65, 0x72, 0x65, 0x0a })));
            }
        }

        [Fact]
        public void AttemptingToAccessDisposedBlobThrows()
        {
            Blob blob;

            using (Repository repo = SandboxRepository("testrepo"))
            using (blob = repo.Lookup<Blob>(new ObjectId("a8233120f6ad708f843d861ce2b7228ec4e3dec6")))
            {
            }

            Assert.Throws<ObjectDisposedException>(() => blob.Id);
            Assert.Throws<ObjectDisposedException>(() => blob.RawSize);
            Assert.Throws<ObjectDisposedException>(() => blob.RawContent.Length);
            Assert.Throws<ObjectDisposedException>(() => blob.IsBinary);
        }
    }
}
