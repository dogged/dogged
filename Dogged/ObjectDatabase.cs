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
        private unsafe OdbHandle nativeOdb;

        public unsafe ObjectDatabase()
        {
            OdbHandle nativeOdb;

            Ensure.NativeSuccess(libgit2.git_odb_new(out nativeOdb));
            this.nativeOdb = nativeOdb;
        }

        private unsafe ObjectDatabase(OdbHandle nativeOdb)
        {
            Ensure.ArgumentNotNullOrInvalid(nativeOdb, "odb");
            this.nativeOdb = nativeOdb;
        }

        internal unsafe OdbHandle NativeOdb
        {
            get
            {
                return nativeOdb;
            }
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

            git_odb_backend* nativeBackend = backend.NativeObject;
            Ensure.NativeSuccess(libgit2.git_odb_add_backend(nativeOdb, ref *nativeBackend, priority));
            GC.KeepAlive(backend);
        }

        internal unsafe static ObjectDatabase FromNative(OdbHandle nativeOdb)
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

            Ensure.NativeSuccess(libgit2.git_odb_read_header(out size, out type, nativeOdb, ref oid));
            Ensure.EnumDefined(typeof(ObjectType), (int)type, "object type");

            return (Ensure.CastToLong(size, "size"), (ObjectType)type);
        }

        /// <summary>
        /// Read a git object directly from the database.
        /// </summary>
        /// <param name="id">The object ID to read</param>
        public unsafe ObjectDatabaseObject Read(ObjectId id)
        {
            Ensure.ArgumentNotNull(id, "id");

            OdbObjectHandle obj = null;
            git_oid oid = id.ToNative();

            Ensure.NativeSuccess(libgit2.git_odb_read(out obj, nativeOdb, ref oid));
            return ObjectDatabaseObject.FromNative(obj);
        }

        /// <summary>
        /// Write an object directly into the object database.
        /// </summary>
        /// <param nam="data">The raw bytes to write</param>
        /// <param name="type">The type of object to write</param>
        /// <returns>The id of the object written to the database</returns>
        public unsafe ObjectId Write(byte[] data, ObjectType type)
        {
            git_oid id = new git_oid();
            UIntPtr length = Ensure.CastToUIntPtr(data.LongLength, "data.Length");

            fixed (byte* ptr = data)
            {
                Ensure.NativeSuccess(libgit2.git_odb_write(ref id, nativeOdb, ptr, length, (git_object_t)type));
                GC.KeepAlive(this);
            }

            return ObjectId.FromNative(id);
        }

        /// <summary>
        /// Iterate over each object in the database, calling the given
        /// action for each.  Return true from the action to continue
        /// iterating; return false to stop.
        /// </summary>
        /// <param name="action">The action to invoke</param>
        public unsafe void ForEachObject(Func<ObjectId, bool> action)
        {
            Exception caught = null;

            git_odb_foreach_cb cb = (ref git_oid id, IntPtr payload) => {
                try
                {
                    if (action.Invoke(ObjectId.FromNative(id)))
                    {
                        return 0;
                    }
                }
                catch (Exception e)
                {
                    caught = e;
                }

                return (int)git_error_code.GIT_EUSER;
            };

            int ret = libgit2.git_odb_foreach(nativeOdb, cb, IntPtr.Zero);

            if (ret == (int)git_error_code.GIT_EUSER)
            {
                if (caught != null)
                {
                    throw caught;
                }

                return;
            }

            Ensure.NativeSuccess(ret);
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
                nativeOdb.Dispose();
                nativeOdb = null;
            }
        }
    }
}
