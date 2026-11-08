using System;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// An options structure for loose object databases.
    /// </summary>
    public class LooseObjectDatabaseBackendOptions
    {
        private git_odb_backend_loose_options nativeOptions;

        /// <summary>
        /// Create an options structure for loose object databases
        /// </summary>
        public LooseObjectDatabaseBackendOptions()
        {
            libgit2.git_odb_backend_loose_options_init(ref nativeOptions,
                git_odb_backend_loose_options.GIT_ODB_BACKEND_LOOSE_OPTIONS_VERSION);
        }

        internal git_odb_backend_loose_options NativeOptions
        {
            get
            {
                return nativeOptions;
            }
        }
    }

    /// <summary>
    /// An options structure for pack object databases.
    /// </summary>
    public class PackObjectDatabaseBackendOptions
    {
        private git_odb_backend_pack_options nativeOptions;

        /// <summary>
        /// Create an options structure for pack object databases
        /// </summary>
        public PackObjectDatabaseBackendOptions()
        {
            libgit2.git_odb_backend_pack_options_init(ref nativeOptions,
                git_odb_backend_pack_options.GIT_ODB_BACKEND_PACK_OPTIONS_VERSION);
        }

        internal git_odb_backend_pack_options NativeOptions
        {
            get
            {
                return nativeOptions;
            }
        }
    }

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
            LooseObjectDatabaseBackendOptions options)
        {
            Ensure.ArgumentNotNull(path, "path");
            Ensure.ArgumentNotNull(options, "options");

            var nativeOptions = options.NativeOptions;

            git_odb_backend* backend;
            Ensure.NativeSuccess(libgit2.git_odb_backend_loose(out backend, path, ref nativeOptions));

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
        public static unsafe ObjectDatabaseBackend CreatePackBackend(string path, PackObjectDatabaseBackendOptions options)
        {
            Ensure.ArgumentNotNull(path, "path");
            Ensure.ArgumentNotNull(options, "options");

            var nativeOptions = options.NativeOptions;

            git_odb_backend* backend;
            Ensure.NativeSuccess(libgit2.git_odb_backend_pack(out backend, path, ref nativeOptions));

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
        public static unsafe ObjectDatabaseBackend CreatePackfileBackend(string indexFilename, PackObjectDatabaseBackendOptions options)
        {
            Ensure.ArgumentNotNull(indexFilename, "indexFilename");
            Ensure.ArgumentNotNull(options, "options");

            var nativeOptions = options.NativeOptions;

            git_odb_backend* backend;
            Ensure.NativeSuccess(libgit2.git_odb_backend_one_pack(out backend, indexFilename, ref nativeOptions));

            return new ObjectDatabaseBackend(backend);
        }
    }
}
