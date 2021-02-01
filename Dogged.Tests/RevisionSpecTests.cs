using System;
using System.Collections.Generic;
using Xunit;

using Dogged;

namespace Dogged.Tests
{
    /// <summary>
    /// Tests for revision specs.
    /// </summary>
    public class RevisionSpecTests : TestBase
    {
        [Theory]
        [InlineData("HEAD", "a65fedf39aefe402d3bb6e24df4d4f5fe4547750", null, RevisionSpecType.Single)]
        [InlineData("be3563a", "be3563ae3f795b2b4353bcce3a527ad0a4f7f644", null, RevisionSpecType.Single)]
        [InlineData("HEAD~3..HEAD", "4a202b346bb0fb0db7eff3cffeb3c70babbd2045", "a65fedf39aefe402d3bb6e24df4d4f5fe4547750", RevisionSpecType.Range)]
        [InlineData("HEAD~3...HEAD", "4a202b346bb0fb0db7eff3cffeb3c70babbd2045", "a65fedf39aefe402d3bb6e24df4d4f5fe4547750", RevisionSpecType.Range | RevisionSpecType.MergeBase)]
        [InlineData("HEAD~3..", "4a202b346bb0fb0db7eff3cffeb3c70babbd2045", "a65fedf39aefe402d3bb6e24df4d4f5fe4547750", RevisionSpecType.Range)]
        [InlineData("HEAD~3...", "4a202b346bb0fb0db7eff3cffeb3c70babbd2045", "a65fedf39aefe402d3bb6e24df4d4f5fe4547750", RevisionSpecType.Range | RevisionSpecType.MergeBase)]
        [InlineData("..HEAD~3", "a65fedf39aefe402d3bb6e24df4d4f5fe4547750", "4a202b346bb0fb0db7eff3cffeb3c70babbd2045", RevisionSpecType.Range)]
        [InlineData("...HEAD~3", "a65fedf39aefe402d3bb6e24df4d4f5fe4547750", "4a202b346bb0fb0db7eff3cffeb3c70babbd2045", RevisionSpecType.Range | RevisionSpecType.MergeBase)]
        [InlineData("...", "a65fedf39aefe402d3bb6e24df4d4f5fe4547750", "a65fedf39aefe402d3bb6e24df4d4f5fe4547750", RevisionSpecType.Range | RevisionSpecType.MergeBase)]
        [InlineData("be3563a^1..be3563a", "9fd738e8f7967c078dceed8190330fc8648ee56a", "be3563ae3f795b2b4353bcce3a527ad0a4f7f644", RevisionSpecType.Range)]
        [InlineData("be3563a^1...be3563a", "9fd738e8f7967c078dceed8190330fc8648ee56a", "be3563ae3f795b2b4353bcce3a527ad0a4f7f644", RevisionSpecType.Range | RevisionSpecType.MergeBase)]
        public void CanParseRevspecRange(string spec, string fromId, string toId, RevisionSpecType type)
        {
            using (var repo = SandboxRepository("testrepo.git"))
            using (var revspec = RevisionSpec.Parse(repo, spec))
            using (var from = revspec.From)
            using (var to = revspec.To)
            {
                if (fromId == null)
                {
                    Assert.Null(from);
                }
                else
                {
                    Assert.Equal(new ObjectId(fromId), from.Id);
                }

                if (toId == null)
                {
                    Assert.Null(to);
                }
                else
                {
                    Assert.Equal(new ObjectId(toId), to.Id);
                }

                Assert.Equal(type, revspec.Type);
            }
        }

        [Theory]
        [InlineData("..")]
        [InlineData("be3563a^1.be3563a")]
        public void ThrowsOnInvalidRevspecRange(string spec)
        {
            using (var repo = SandboxRepository("testrepo.git"))
            {
                Assert.Throws<InvalidSpecificationException>(() => RevisionSpec.Parse(repo, spec));
            }
        }

        [Theory]
        [InlineData("HEAD", "a65fedf39aefe402d3bb6e24df4d4f5fe4547750")]
        [InlineData("be3563a", "be3563ae3f795b2b4353bcce3a527ad0a4f7f644")]
        [InlineData("HEAD~3", "4a202b346bb0fb0db7eff3cffeb3c70babbd2045")]
        [InlineData("be3563a^1", "9fd738e8f7967c078dceed8190330fc8648ee56a")]
        public void CanRevparseSingle(string spec, string expected)
        {
            using (var repo = SandboxRepository("testrepo.git"))
            using (var obj = RevisionSpec.ParseSingle(repo, spec))
            {
                Assert.Equal(new ObjectId(expected), obj.Id);
            }
        }

        [Theory]
        [InlineData("..")]
        [InlineData("be3563a^1.be3563a")]
        public void ThrowsOnInvalidRevspec(string spec)
        {
            using (var repo = SandboxRepository("testrepo.git"))
            {
                Assert.Throws<InvalidSpecificationException>(() => RevisionSpec.ParseSingle(repo, spec));
            }
        }
    }
}
