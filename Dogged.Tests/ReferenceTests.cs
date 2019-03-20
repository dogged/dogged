using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

using Dogged;

namespace Dogged.Tests
{
    /// <summary>
    /// Tests for references.
    /// </summary>
    public class ReferenceTests : TestBase
    {
        [Fact]
        public void AttemptsToAccessDisposedReferenceThrow()
        {
            Reference reference;

            using (Repository repo = SandboxRepository("testrepo.git"))
            using (reference = repo.Head)
            {
            }

            Assert.Throws<ObjectDisposedException>(() => reference.Name);
            Assert.Throws<ObjectDisposedException>(() => ((DirectReference)reference).Target);
        }

        [Fact]
        public void CanLookupHEAD()
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (Reference reference = repo.References.Lookup("HEAD"))
            {
                Assert.NotNull(reference);
                Assert.IsType<SymbolicReference>(reference);
                Assert.Equal("HEAD", reference.Name);
                Assert.Equal("refs/heads/master", ((SymbolicReference)reference).Target);
            }
        }

        [Fact]
        public void CanLookupReference()
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (Reference reference = repo.References.Lookup("refs/heads/master"))
            {
                Assert.NotNull(reference);
                Assert.IsType<DirectReference>(reference);
                Assert.Equal("refs/heads/master", reference.Name);
                Assert.Equal(new ObjectId("099fabac3a9ea935598528c27f866e34089c2eff"), ((DirectReference)reference).Target);
            }
        }

        [Fact]
        public void CanIteratorReferences()
        {
            string[] expected = new string[] {
                "refs/heads/br2",
                "refs/heads/dir",
                "refs/heads/executable",
                "refs/heads/ident",
                "refs/heads/long-file-name",
                "refs/heads/master",
                "refs/heads/merge-conflict",
                "refs/heads/packed-test",
                "refs/heads/subtrees",
                "refs/heads/test",
                "refs/heads/testrepo-worktree",
                "refs/tags/e90810b",
                "refs/tags/foo/bar",
                "refs/tags/foo/foo/bar",
                "refs/tags/point_to_blob",
                "refs/tags/test",
                "refs/heads/packed",
                "refs/tags/packed-tag"
            };
            List<string> actual = new List<string>();

            using (Repository repo = SandboxRepository("testrepo"))
            {
                foreach (Reference reference in repo.References)
                using (reference)
                {
                    actual.Add(reference.Name);
                }
            }
        }
    }
}
