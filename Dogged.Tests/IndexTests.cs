using System;
using System.IO;
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
    }
}
