using System;
using System.IO;
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
            using (Blob b = repo.Objects.Lookup<Blob>(id))
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
        public void CanGetFilteredContent()
        {
            ObjectId id = new ObjectId("a8233120f6ad708f843d861ce2b7228ec4e3dec6");

            using (Repository repo = SandboxRepository("testrepo"))
            using (Blob b = repo.Objects.Lookup<Blob>(id))
            {
                using (GitBuffer buf = b.GetFilteredContent("foo.txt"))
                {
                    Assert.Equal(10, buf.Length);
                    Assert.True(buf.Content.SequenceEqual(new ReadOnlySpan<byte>(new byte[] { 0x68, 0x65, 0x79, 0x20, 0x74, 0x68, 0x65, 0x72, 0x65, 0x0a })));
                }

                string attributesPath = string.Format("{0}/.gitattributes", repo.Workdir);
                Console.WriteLine(attributesPath);
                File.WriteAllLines(attributesPath, new string[] { "* text eol=crlf" });

                using (GitBuffer buf = b.GetFilteredContent("foo.txt"))
                {
                    Assert.Equal(11, buf.Length);
                    Assert.True(buf.Content.SequenceEqual(new ReadOnlySpan<byte>(new byte[] { 0x68, 0x65, 0x79, 0x20, 0x74, 0x68, 0x65, 0x72, 0x65, 0x0d, 0x0a })));
                }
            }
        }

        [Fact]
        public void AttemptingToAccessDisposedBlobThrows()
        {
            Blob blob;

            using (Repository repo = SandboxRepository("testrepo"))
            using (blob = repo.Objects.Lookup<Blob>(new ObjectId("a8233120f6ad708f843d861ce2b7228ec4e3dec6")))
            {
            }

            Assert.Throws<ObjectDisposedException>(() => blob.Id);
            Assert.Throws<ObjectDisposedException>(() => blob.RawSize);
            Assert.Throws<ObjectDisposedException>(() => blob.RawContent.Length);
            Assert.Throws<ObjectDisposedException>(() => blob.IsBinary);
        }
    }
}
