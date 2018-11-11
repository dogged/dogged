using System;
using System.IO;
using Xunit;

using Dogged.Native;

namespace Dogged.Native.Tests
{
    /// <summary>
    /// Tests for the native methods.  These tests are primarily to ensure
    /// that the library can be loaded and basic functionality executed.
    /// Most of the actual functional testing is performed transitively,
    /// by invoking the Dogged library itself.
    /// </summary>
    public class NativeTests
    {
        /// <summary>
        /// Ensure that we can load the native library and successfully
        /// invoke a trivial function.
        /// </summary>
        [Fact]
        public void CanLoadNativeLibrary()
        {
            git_feature_t features = libgit2.git_libgit2_features();
            Assert.Equal(git_feature_t.GIT_FEATURE_THREADS, (features & git_feature_t.GIT_FEATURE_THREADS));
        }
    }
}
