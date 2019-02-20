using System;
using System.Collections;
using System.Collections.Generic;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// An object database stores the objects (commit, trees, blobs, tags,
    /// etc) for a repository.
    /// </summary>
    public class ObjectDatabase : NativeDisposable
    {
        private unsafe git_odb* nativeOdb;

        private unsafe ObjectDatabase(git_odb* nativeOdb)
        {
            Ensure.ArgumentNotNull(nativeOdb, "odb");
            this.nativeOdb = nativeOdb;
        }

        /// <summary>
        /// Add a custom backend to an existing object database.
        ///
        /// <para>
        /// The backends are checked in relative ordering, based on the
        /// value of the `priority` parameter.
        /// </para>
        /// </summary>
        /// <param name="backend">The object database backend to add</param>
        /// <param name="priority">Value for ordering the backends queue</param>
        public unsafe void AddBackend(ObjectDatabaseBackend backend, int priority)
        {
            Ensure.ArgumentNotNull(backend, "backend");
            Ensure.NativeSuccess(() => libgit2.git_odb_add_backend(nativeOdb, backend.NativeObject, priority), this);
        }

        internal unsafe static ObjectDatabase FromNative(git_odb* nativeOdb)
        {
            return new ObjectDatabase(nativeOdb);
        }

        /// <summary>
        /// Read the metadata describing a git object.  If the actual object
        /// contents are not needed this may be more performant than reading
        /// the entire object, depending on the object database backend.
        /// </summary>
        /// <param name="id">The object ID to lookup</param>
        /// <exception cref="Dogged.NotFoundException">Thrown when the object is not found in the database</exception>
        /// <returns>An OdbObjectHeader on success</returns>
        public unsafe (long size, ObjectType type) ReadHeader(ObjectId id)
        {
            UIntPtr size = UIntPtr.Zero;
            git_object_t type = git_object_t.GIT_OBJECT_INVALID;

            Ensure.ArgumentNotNull(id, "id");

            git_oid oid = id.ToNative();

            Ensure.NativeSuccess(() => libgit2.git_odb_read_header(out size, out type, nativeOdb, ref oid), this);
            Ensure.EnumDefined(typeof(ObjectType), (int)type, "object type");

            return (Ensure.CastToLong(size, "size"), (ObjectType)type);
        }

        internal unsafe override bool IsDisposed
        {
            get
            {
                return (nativeOdb == null);
            }
        }

        internal unsafe override void Dispose(bool disposing)
        {
            if (nativeOdb != null)
            {
                libgit2.git_odb_free(nativeOdb);
                nativeOdb = null;
            }
        }
    }
}
