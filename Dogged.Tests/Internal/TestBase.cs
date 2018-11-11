using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace Dogged.Tests
{
    [Collection("Temporary Directory")]
    public abstract class TestBase
    {
        private const string resourceDirectory = "Resources";
        protected string TemporaryDirectory
        {
            get
            {
                return TemporaryDirectoryManager.Instance.TemporaryDirectory;
            }
        }

        private static string GetExecutingAssemblyDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        protected string SandboxResource(string resource)
        {
            var source = Path.Combine(Path.Combine(GetExecutingAssemblyDirectory(), "Resources"), resource);
            var target = Path.Combine(TemporaryDirectory, string.Format("{0}.{1}", resource, Guid.NewGuid()));

            DirectoryHelper.CopyFilesRecursively(source, target);

            return target;
        }

        protected Repository SandboxRepository(string resource)
        {
            return new Repository(SandboxResource(resource));
        }
    }

    public class TemporaryDirectoryManager : IDisposable
    {
        public static TemporaryDirectoryManager Instance { get; private set; }

        public TemporaryDirectoryManager()
        {
            var path = Path.Combine(Path.GetTempPath(), String.Format("Dogged.Tests.{0}", Guid.NewGuid()));
            Directory.CreateDirectory(path);

            TemporaryDirectory = path;

            Instance = this;
        }

        public string TemporaryDirectory { get; private set; }

        public void Dispose()
        {
            DirectoryHelper.DeleteDirectory(TemporaryDirectory);
        }
    }

    [CollectionDefinition("Temporary Directory")]
    public class TemporaryDirectoryCollection : ICollectionFixture<TemporaryDirectoryManager>
    {
    }
}
