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
                    Assert.Equal(10, buf.Content.Length);
                }

                string attributesPath = string.Format("{0}/.gitattributes", repo.Workdir);
                File.WriteAllLines(attributesPath, new string[] { "* text eol=crlf" });

                using (GitBuffer buf = b.GetFilteredContent("foo.txt"))
                {
                    Assert.Equal(11, buf.Length);
                    Assert.True(buf.Content.SequenceEqual(new ReadOnlySpan<byte>(new byte[] { 0x68, 0x65, 0x79, 0x20, 0x74, 0x68, 0x65, 0x72, 0x65, 0x0d, 0x0a })));
                    Assert.Equal(11, buf.Content.Length);
                }
            }
        }

        [Fact]
        public void CanGetFilteredContentInBareRepo()
        {
            ObjectId id = new ObjectId("799770d1cff46753a57db7a066159b5610da6e3a");
            BlobFilterOptions options = new BlobFilterOptions() {
                Flags = BlobFilterFlags.CheckForBinary |
                        BlobFilterFlags.NoSystemAttributes |
                        BlobFilterFlags.AttributesFromHead
            };

            using (Repository repo = SandboxRepository("crlf.git"))
            using (Blob b = repo.Objects.Lookup<Blob>(id))
            {
                using (GitBuffer buf = b.GetFilteredContent("file.bin", options))
                {
                    Assert.Equal(15, buf.Length);
                    Assert.True(buf.Content.SequenceEqual(new ReadOnlySpan<byte>(new byte[] { 0x6c, 0x66, 0x0a, 0x6c, 0x66, 0x0a, 0x6c, 0x66, 0x0a, 0x6c, 0x66, 0x0a, 0x6c, 0x66, 0x0a })));
                    Assert.Equal(15, buf.Content.Length);
                }

                using (GitBuffer buf = b.GetFilteredContent("file.crlf", options))
                {
                    Assert.Equal(20, buf.Length);
                    Assert.True(buf.Content.SequenceEqual(new ReadOnlySpan<byte>(new byte[] { 0x6c, 0x66, 0x0d, 0x0a, 0x6c, 0x66, 0x0d, 0x0a, 0x6c, 0x66, 0x0d, 0x0a, 0x6c, 0x66, 0x0d, 0x0a, 0x6c, 0x66, 0x0d, 0x0a })));
                    Assert.Equal(20, buf.Content.Length);
                }

                using (GitBuffer buf = b.GetFilteredContent("file.lf", options))
                {
                    Assert.Equal(15, buf.Length);
                    Assert.True(buf.Content.SequenceEqual(new ReadOnlySpan<byte>(new byte[] { 0x6c, 0x66, 0x0a, 0x6c, 0x66, 0x0a, 0x6c, 0x66, 0x0a, 0x6c, 0x66, 0x0a, 0x6c, 0x66, 0x0a })));
                    Assert.Equal(15, buf.Content.Length);
                }
            }
        }

        [Fact]
        public void CanGetFilteredContentInBareRepoBySpecificCommit()
        {
            ObjectId id = new ObjectId("799770d1cff46753a57db7a066159b5610da6e3a");
            BlobFilterOptions options = new BlobFilterOptions() {
                Flags = BlobFilterFlags.CheckForBinary |
                        BlobFilterFlags.NoSystemAttributes |
                        BlobFilterFlags.AttributesFromCommit,
                CommitId = new ObjectId("5afb6a14a864e30787857dd92af837e8cdd2cb1b")
            };

            using (Repository repo = SandboxRepository("crlf.git"))
            using (Blob b = repo.Objects.Lookup<Blob>(id))
            {
                using (GitBuffer buf = b.GetFilteredContent("file.bin", options))
                {
                    Assert.Equal(15, buf.Length);
                    Assert.True(buf.Content.SequenceEqual(new ReadOnlySpan<byte>(new byte[] { 0x6c, 0x66, 0x0a, 0x6c, 0x66, 0x0a, 0x6c, 0x66, 0x0a, 0x6c, 0x66, 0x0a, 0x6c, 0x66, 0x0a })));
                    Assert.Equal(15, buf.Content.Length);
                }

                using (GitBuffer buf = b.GetFilteredContent("file.crlf", options))
                {
                    Assert.Equal(15, buf.Length);
                    Assert.True(buf.Content.SequenceEqual(new ReadOnlySpan<byte>(new byte[] { 0x6c, 0x66, 0x0a, 0x6c, 0x66, 0x0a, 0x6c, 0x66, 0x0a, 0x6c, 0x66, 0x0a, 0x6c, 0x66, 0x0a })));
                    Assert.Equal(15, buf.Content.Length);
                }

                using (GitBuffer buf = b.GetFilteredContent("file.lf", options))
                {
                    Assert.Equal(15, buf.Length);
                    Assert.True(buf.Content.SequenceEqual(new ReadOnlySpan<byte>(new byte[] { 0x6c, 0x66, 0x0a, 0x6c, 0x66, 0x0a, 0x6c, 0x66, 0x0a, 0x6c, 0x66, 0x0a, 0x6c, 0x66, 0x0a })));
                    Assert.Equal(15, buf.Content.Length);
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
