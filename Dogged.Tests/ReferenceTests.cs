using System;
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
    }
}
