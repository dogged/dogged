using System;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// An object database backend, responsible for actually reading and
    /// writing objects into a storage location.
    /// </summary>
    public class ObjectDatabaseBackend
    {
        private unsafe ObjectDatabaseBackend(git_odb_backend* nativeObjectDatabaseBackend)
        {
            Ensure.ArgumentNotNull(nativeObjectDatabaseBackend, "nativeObjectDatabaseBackend");
            NativeObject = nativeObjectDatabaseBackend;
        }

        internal unsafe git_odb_backend* NativeObject
        {
            get;
            private set;
        }

        /// <summary>
        /// An object database backend for "loose" objects, stored on disk
        /// compressed into a single file per object.  The directory is stored
        /// in standard git loose object format, with a "fanout" of
        /// subdirectories named with the first two characters of the object's
        /// ID and the object stored in a file beneath that, named with the
        /// remaining 38 characters of the ID.
        ///
        /// <para>
        /// This object database backend is read/write.
        /// </para>
        /// </summary>
        /// <param name="path">The path to the object directory</param>
        /// <param name="compressionLevel">The zlib compression level.  0-9 with 0 being no compression, and 9 being maximum compression.</param>
        /// <param name="fsync">If true, fsync will be called after each write</param>
        /// <param name="directoryMode">The POSIX mode to create directories with, or 0 for defaults.</param>
        /// <param name="fileMode">The POSIX mode to create files with, or 0 for defaults.</param>
        public static unsafe ObjectDatabaseBackend CreateLooseBackend(
            string path,
            CompressionLevel compressionLevel = CompressionLevel.Default,
            bool fsync = false,
            FileMode directoryMode = 0,
            FileMode fileMode = 0)
        {
            Ensure.ArgumentNotNull(path, "path");

            git_odb_backend* backend;
            Ensure.NativeSuccess(libgit2.git_odb_backend_loose(out backend, path, (int)compressionLevel, fsync ? 1 : 0, (uint)directoryMode, (uint)fileMode));

            return new ObjectDatabaseBackend(backend);
        }

        /// <summary>
        /// An object database backend for a directory of packfiles stored on
        /// disk.  The directory is stored in typical git object format,
        /// containing a "pack" directory that will be consulted to find the
        /// packfiles to load.  Each packfile within that "pack" directory
        /// will be consulted as storage for the object database.
        ///
        /// <para>
        /// This object database backend is read-only.
        /// </para>
        /// </summary>
        /// <param name="path">The path to the directory containing packfiles</param>
        public static unsafe ObjectDatabaseBackend CreatePackBackend(string path)
        {
            Ensure.ArgumentNotNull(path, "path");

            git_odb_backend* backend;
            Ensure.NativeSuccess(libgit2.git_odb_backend_pack(out backend, path));

            return new ObjectDatabaseBackend(backend);
        }

        /// <summary>
        /// An object database backend for a single packfile stored on disk.
        /// Only the given packfile will be consulted as storage for the object
        /// database.
        ///
        /// <para>
        /// This object database backend is read-only.
        /// </para>
        /// </summary>
        /// <param name="path">The path to the packfile's index (.idx) file</param>
        public static unsafe ObjectDatabaseBackend CreatePackfileBackend(string indexFilename)
        {
            Ensure.ArgumentNotNull(indexFilename, "indexFilename");

            git_odb_backend* backend;
            Ensure.NativeSuccess(libgit2.git_odb_backend_one_pack(out backend, indexFilename));

            return new ObjectDatabaseBackend(backend);
        }
    }
}
