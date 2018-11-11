using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Dogged.Tests
{
    internal static class DirectoryHelper
    {
        private static readonly Dictionary<string, string> toRename = new Dictionary<string, string>
        {
            { ".gitted", ".git" },
        };

        private static readonly Type[] whitelist = { typeof(IOException), typeof(UnauthorizedAccessException) };

        public static void CopyFilesRecursively(string source, string target)
        {
            CopyFilesRecursively(new DirectoryInfo(source), new DirectoryInfo(target));
        }

        public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
            {
                CopyFilesRecursively(dir, target.CreateSubdirectory(Rename(dir.Name)));
            }
            foreach (FileInfo file in source.GetFiles())
            {
                file.CopyTo(Path.Combine(target.FullName, Rename(file.Name)));
            }
        }

        private static string Rename(string name)
        {
            return toRename.ContainsKey(name) ? toRename[name] : name;
        }

        public static void DeleteDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                return;
            }
            NormalizeAttributes(directoryPath);
            DeleteDirectory(directoryPath, maxAttempts: 5, initialTimeout: 16, timeoutFactor: 2);
        }

        private static void NormalizeAttributes(string directoryPath)
        {
            string[] filePaths = Directory.GetFiles(directoryPath);
            string[] subdirectoryPaths = Directory.GetDirectories(directoryPath);

            foreach (string filePath in filePaths)
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }
            foreach (string subdirectoryPath in subdirectoryPaths)
            {
                NormalizeAttributes(subdirectoryPath);
            }
            File.SetAttributes(directoryPath, FileAttributes.Normal);
        }

        private static void DeleteDirectory(string directoryPath, int maxAttempts, int initialTimeout, int timeoutFactor)
        {
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    Directory.Delete(directoryPath, true);
                    return;
                }
                catch (Exception ex)
                {
                    var caughtExceptionType = ex.GetType();

                    if (!whitelist.Any(knownExceptionType => knownExceptionType.GetTypeInfo().IsAssignableFrom(caughtExceptionType)))
                    {
                        throw;
                    }

                    if (attempt < maxAttempts)
                    {
                        Thread.Sleep(initialTimeout * (int)Math.Pow(timeoutFactor, attempt - 1));
                        continue;
                    }
                }
            }
        }
    }
}
