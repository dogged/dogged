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
    }
}
