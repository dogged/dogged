using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

using Dogged;

namespace Dogged.Tests
{
    /// <summary>
    /// Test that we can apply clean/smudge filters on data.
    /// </summary>
    public class FilterTests : TestBase
    {
        [Fact]
        public void NoFilterReturnedWhenNotConfigured()
        {
            var id = new ObjectId("a8233120f6ad708f843d861ce2b7228ec4e3dec6");
            var flags = FilterFlags.NoSystemAttributes;

            using (Repository repo = SandboxRepository("testrepo"))
            using (Blob b = repo.Objects.Lookup<Blob>(id))
            {
                Assert.Null(repo.LoadFilterList(b, "foo.txt", FilterMode.ToWorktree, flags));
                Assert.Null(repo.LoadFilterList(b, "foo.txt", FilterMode.ToObjectDatabase, flags));
            }
        }

        [Fact]
        public void CanFilterBufferToOdb()
        {
            var id = new ObjectId("a8233120f6ad708f843d861ce2b7228ec4e3dec6");
            var flags = FilterFlags.NoSystemAttributes;

            using (Repository repo = SandboxRepository("testrepo"))
            {
                string attributesPath = string.Format("{0}/.gitattributes", repo.Workdir);
                File.WriteAllLines(attributesPath, new string[] { "* text eol=crlf" });

                using (Blob b = repo.Objects.Lookup<Blob>(id))
                using (FilterList cleanFilter = repo.LoadFilterList(b, "foo.txt", FilterMode.ToObjectDatabase, flags))
                {
                    using (GitBuffer inputBuffer = b.GetFilteredContent("foo.txt"))
                    {
                        Assert.Equal(11, inputBuffer.Length);
                        Assert.True(inputBuffer.Content.SequenceEqual(new ReadOnlySpan<byte>(new byte[] { 0x68, 0x65, 0x79, 0x20, 0x74, 0x68, 0x65, 0x72, 0x65, 0x0d, 0x0a })));
                        Assert.Equal(11, inputBuffer.Content.Length);

                        using (GitBuffer outputBuffer = cleanFilter.Apply(inputBuffer))
                        {
                            Assert.Equal(10, outputBuffer.Length);
                            Assert.True(outputBuffer.Content.SequenceEqual(new ReadOnlySpan<byte>(new byte[] { 0x68, 0x65, 0x79, 0x20, 0x74, 0x68, 0x65, 0x72, 0x65, 0x0a })));
                            Assert.Equal(10, outputBuffer.Content.Length);
                        }
                    }
                }
            }
        }

        [Fact]
        public void CanFilterFileToOdb()
        {
            var id = new ObjectId("a8233120f6ad708f843d861ce2b7228ec4e3dec6");
            var flags = FilterFlags.NoSystemAttributes;

            using (Repository repo = SandboxRepository("testrepo"))
            {
                string attributesPath = string.Format("{0}/.gitattributes", repo.Workdir);
                File.WriteAllLines(attributesPath, new string[] { "* text eol=crlf" });

                string filePath = string.Format("{0}/foo.txt", repo.Workdir);
                File.WriteAllBytes(filePath, new byte[] { 0x68, 0x65, 0x79, 0x20, 0x74, 0x68, 0x65, 0x72, 0x65, 0x0d, 0x0a });

                using (Blob b = repo.Objects.Lookup<Blob>(id))
                using (FilterList cleanFilter = repo.LoadFilterList(b, "foo.txt", FilterMode.ToObjectDatabase, flags))
                {
                    using (GitBuffer outputBuffer = cleanFilter.Apply("foo.txt"))
                    {
                        Assert.Equal(10, outputBuffer.Length);
                        Assert.True(outputBuffer.Content.SequenceEqual(new ReadOnlySpan<byte>(new byte[] { 0x68, 0x65, 0x79, 0x20, 0x74, 0x68, 0x65, 0x72, 0x65, 0x0a })));
                        Assert.Equal(10, outputBuffer.Content.Length);
                    }
                }
            }
        }

        [Fact]
        public void CanFilterBlobToWorkdir()
        {
            var id = new ObjectId("a8233120f6ad708f843d861ce2b7228ec4e3dec6");
            var flags = FilterFlags.NoSystemAttributes;

            using (Repository repo = SandboxRepository("testrepo"))
            {
                string attributesPath = string.Format("{0}/.gitattributes", repo.Workdir);
                File.WriteAllLines(attributesPath, new string[] { "* text eol=crlf" });

                using (Blob b = repo.Objects.Lookup<Blob>(id))
                using (FilterList smudgeFilter = repo.LoadFilterList(b, "foo.txt", FilterMode.ToWorktree, flags))
                using (GitBuffer outputBuffer = smudgeFilter.Apply(b))
                {
                    Assert.Equal(11, outputBuffer.Length);
                    Assert.True(outputBuffer.Content.SequenceEqual(new ReadOnlySpan<byte>(new byte[] { 0x68, 0x65, 0x79, 0x20, 0x74, 0x68, 0x65, 0x72, 0x65, 0x0d, 0x0a })));
                    Assert.Equal(11, outputBuffer.Content.Length);
                }
            }
        }

        [Fact]
        public void CanFilterBufferToOdbInBareRepository()
        {
            var id = new ObjectId("799770d1cff46753a57db7a066159b5610da6e3a");
            var flags = FilterFlags.NoSystemAttributes | FilterFlags.AttributesFromHead;

            var blobFilterOptions = new BlobFilterOptions();
            blobFilterOptions.Flags = BlobFilterFlags.NoSystemAttributes | BlobFilterFlags.AttributesFromHead;

            using (Repository repo = SandboxRepository("crlf.git"))
            using (Blob b = repo.Objects.Lookup<Blob>(id))
            using (FilterList cleanFilter = repo.LoadFilterList(b, "file.crlf", FilterMode.ToObjectDatabase, flags))
            {
                using (GitBuffer inputBuffer = b.GetFilteredContent("file.crlf", blobFilterOptions))
                {
                    Assert.Equal(20, inputBuffer.Length);
                    Assert.True(inputBuffer.Content.SequenceEqual(new ReadOnlySpan<byte>(new byte[] { 0x6c, 0x66, 0x0d, 0x0a, 0x6c, 0x66, 0x0d, 0x0a, 0x6c, 0x66, 0x0d, 0x0a, 0x6c, 0x66, 0x0d, 0x0a, 0x6c, 0x66, 0x0d, 0x0a })));
                    Assert.Equal(20, inputBuffer.Content.Length);

                    // No filtering is configured
                    using (GitBuffer outputBuffer = cleanFilter.Apply(inputBuffer))
                    {
                        Assert.Equal(15, outputBuffer.Length);
                        Assert.True(outputBuffer.Content.SequenceEqual(new ReadOnlySpan<byte>(new byte[] { 0x6c, 0x66, 0x0a, 0x6c, 0x66, 0x0a, 0x6c, 0x66, 0x0a, 0x6c, 0x66, 0x0a, 0x6c, 0x66, 0x0a })));
                        Assert.Equal(15, outputBuffer.Content.Length);
                    }
                }
            }
        }

        [Fact]
        public void CanFilterBlobToWorkdirInBareRepository()
        {
            var id = new ObjectId("799770d1cff46753a57db7a066159b5610da6e3a");
            var flags = FilterFlags.NoSystemAttributes | FilterFlags.AttributesFromHead;

            using (Repository repo = SandboxRepository("crlf.git"))
            using (Blob b = repo.Objects.Lookup<Blob>(id))
            using (FilterList smudgeFilter = repo.LoadFilterList(b, "file.crlf", FilterMode.ToWorktree, flags))
            using (GitBuffer outputBuffer = smudgeFilter.Apply(b))
            {
                Assert.Equal(20, outputBuffer.Length);
                Assert.True(outputBuffer.Content.SequenceEqual(new ReadOnlySpan<byte>(new byte[] { 0x6c, 0x66, 0x0d, 0x0a, 0x6c, 0x66, 0x0d, 0x0a, 0x6c, 0x66, 0x0d, 0x0a, 0x6c, 0x66, 0x0d, 0x0a, 0x6c, 0x66, 0x0d, 0x0a })));
                Assert.Equal(20, outputBuffer.Content.Length);
            }
        }
    }
}
