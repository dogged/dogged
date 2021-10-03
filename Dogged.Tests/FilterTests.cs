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
            var options = new FilterOptions { Flags = FilterFlags.NoSystemAttributes };

            using (Repository repo = SandboxRepository("testrepo"))
            using (Blob b = repo.Objects.Lookup<Blob>(id))
            {
                Assert.Null(FilterList.Load(repo, "foo.txt", b, FilterMode.ToWorktree, options));
                Assert.Null(FilterList.Load(repo, "foo.txt", b, FilterMode.ToObjectDatabase, options));
            }
        }

        [Fact]
        public void CanFilterBufferToOdb()
        {
            var id = new ObjectId("a8233120f6ad708f843d861ce2b7228ec4e3dec6");
            var options = new FilterOptions { Flags = FilterFlags.NoSystemAttributes };

            using (Repository repo = SandboxRepository("testrepo"))
            {
                string attributesPath = string.Format("{0}/.gitattributes", repo.Workdir);
                File.WriteAllLines(attributesPath, new string[] { "* text eol=crlf" });

                using (Blob b = repo.Objects.Lookup<Blob>(id))
                using (FilterList cleanFilter = FilterList.Load(repo, "foo.txt", b, FilterMode.ToObjectDatabase, options))
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
            var options = new FilterOptions { Flags = FilterFlags.NoSystemAttributes };

            using (Repository repo = SandboxRepository("testrepo"))
            {
                string attributesPath = string.Format("{0}/.gitattributes", repo.Workdir);
                File.WriteAllLines(attributesPath, new string[] { "* text eol=crlf" });

                string filePath = string.Format("{0}/foo.txt", repo.Workdir);
                File.WriteAllBytes(filePath, new byte[] { 0x68, 0x65, 0x79, 0x20, 0x74, 0x68, 0x65, 0x72, 0x65, 0x0d, 0x0a });

                using (Blob b = repo.Objects.Lookup<Blob>(id))
                using (FilterList cleanFilter = FilterList.Load(repo, "foo.txt", b, FilterMode.ToObjectDatabase, options))
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
            var options = new FilterOptions { Flags = FilterFlags.NoSystemAttributes };

            using (Repository repo = SandboxRepository("testrepo"))
            {
                string attributesPath = string.Format("{0}/.gitattributes", repo.Workdir);
                File.WriteAllLines(attributesPath, new string[] { "* text eol=crlf" });

                using (Blob b = repo.Objects.Lookup<Blob>(id))
                using (FilterList smudgeFilter = FilterList.Load(repo, "foo.txt", b, FilterMode.ToWorktree, options))
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
            var options = new FilterOptions { Flags = FilterFlags.NoSystemAttributes | FilterFlags.AttributesFromHead };

            var blobFilterOptions = new BlobFilterOptions();
            blobFilterOptions.Flags = BlobFilterFlags.NoSystemAttributes | BlobFilterFlags.AttributesFromHead;

            using (Repository repo = SandboxRepository("crlf.git"))
            using (Blob b = repo.Objects.Lookup<Blob>(id))
            using (FilterList cleanFilter = FilterList.Load(repo, "file.crlf", b, FilterMode.ToObjectDatabase, options))
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
            var options = new FilterOptions { Flags = FilterFlags.NoSystemAttributes | FilterFlags.AttributesFromHead };

            using (Repository repo = SandboxRepository("crlf.git"))
            using (Blob b = repo.Objects.Lookup<Blob>(id))
            using (FilterList smudgeFilter = FilterList.Load(repo, "file.crlf", b, FilterMode.ToWorktree, options))
            using (GitBuffer outputBuffer = smudgeFilter.Apply(b))
            {
                Assert.Equal(20, outputBuffer.Length);
                Assert.True(outputBuffer.Content.SequenceEqual(new ReadOnlySpan<byte>(new byte[] { 0x6c, 0x66, 0x0d, 0x0a, 0x6c, 0x66, 0x0d, 0x0a, 0x6c, 0x66, 0x0d, 0x0a, 0x6c, 0x66, 0x0d, 0x0a, 0x6c, 0x66, 0x0d, 0x0a })));
                Assert.Equal(20, outputBuffer.Content.Length);
            }
        }

        [Fact]
        public void CanFilterBlobAtSpecificCommit()
        {
            var id = new ObjectId("055c8729cdcc372500a08db659c045e16c4409fb");
            var options = new FilterOptions
            {
                Flags = FilterFlags.NoSystemAttributes | FilterFlags.AttributesFromCommit,
                AttributeCommitId = new ObjectId("b8986fec0f7bde90f78ac72706e782d82f24f2f0")
            };

            using (Repository repo = SandboxRepository("crlf.git"))
            using (Blob b = repo.Objects.Lookup<Blob>(id))
            using (FilterList smudgeFilter = FilterList.Load(repo, "ident.identlf", b, FilterMode.ToWorktree, options))
            using (GitBuffer outputBuffer = smudgeFilter.Apply(b))
            {
                Assert.Equal(48, outputBuffer.Length);
                Assert.True(outputBuffer.Content.SequenceEqual(new ReadOnlySpan<byte>(new byte[] { 0x24, 0x49, 0x64, 0x3a, 0x20, 0x30, 0x35, 0x35, 0x63, 0x38, 0x37, 0x32, 0x39, 0x63, 0x64, 0x63, 0x63, 0x33, 0x37, 0x32, 0x35, 0x30, 0x30, 0x61, 0x30, 0x38, 0x64, 0x62, 0x36, 0x35, 0x39, 0x63, 0x30, 0x34, 0x35, 0x65, 0x31, 0x36, 0x63, 0x34, 0x34, 0x30, 0x39, 0x66, 0x62, 0x20, 0x24, 0x0a })));
                Assert.Equal(48, outputBuffer.Content.Length);
            }
        }

        [Fact]
        public void CanFilterBlobAtSpecificCommitWithNoAttributesFile()
        {
            var id = new ObjectId("799770d1cff46753a57db7a066159b5610da6e3a");
            var options = new FilterOptions
            {
                Flags = FilterFlags.NoSystemAttributes | FilterFlags.AttributesFromCommit,
                AttributeCommitId = new ObjectId("5afb6a14a864e30787857dd92af837e8cdd2cb1b")
            };

            using (Repository repo = SandboxRepository("crlf.git"))
            using (Blob b = repo.Objects.Lookup<Blob>(id))
            {
                Assert.Null(FilterList.Load(repo, "file.bin", b, FilterMode.ToWorktree, options));
                Assert.Null(FilterList.Load(repo, "file.lf", b, FilterMode.ToWorktree, options));
                Assert.Null(FilterList.Load(repo, "file.crlf", b, FilterMode.ToWorktree, options));
            }
        }
    }
}
