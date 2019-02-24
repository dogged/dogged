using System;
using System.IO;
using System.Runtime.InteropServices;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// A base class for creating managed object database backends that can
    /// be used to read and/or write objects into the ODB.
    /// </summary>
    public abstract class ManagedObjectDatabaseBackend : ObjectDatabaseBackend
    {
        protected unsafe ManagedObjectDatabaseBackend()
        {
            NativeObject = CreateCallbackStructure();
        }

        private unsafe int ReadProxy(out byte* buffer, out UIntPtr len, out git_object_t type, git_odb_backend* backend, ref git_oid oid)
        {
            buffer = null;
            len = (UIntPtr)0;
            type = 0;

            try
            {
                ObjectType objectType;

                if (Read(ObjectId.FromNative(oid), out ManagedObjectDatabaseBuffer data, out objectType))
                {
                    buffer = data.NativeObject;
                    len = (UIntPtr)data.Data.Length;
                    type = (git_object_t)objectType;

                    return 0;
                }
                else
                {
                    return (int)git_error_code.GIT_ENOTFOUND;
                }
            }
            catch (Exception e)
            {
                libgit2.git_error_set_str(git_error_t.GITERR_ODB, e.Message);
                return (int)git_error_code.GIT_EUSER;
            }
        }

        private unsafe int ReadHeaderProxy(out UIntPtr len, out git_object_t type, git_odb_backend* backend, ref git_oid oid)
        {
            len = (UIntPtr)0;
            type = 0;

            try
            {
                ObjectType objectType;
                long longLength;

                if (ReadHeader(ObjectId.FromNative(oid), out longLength, out objectType))
                {
                    Ensure.CastToUIntPtr(longLength, "length");

                    len = (UIntPtr)longLength;
                    type = (git_object_t)objectType;

                    return 0;
                }
                else
                {
                    return (int)git_error_code.GIT_ENOTFOUND;
                }
            }
            catch (Exception e)
            {
                libgit2.git_error_set_str(git_error_t.GITERR_ODB, e.Message);
                return (int)git_error_code.GIT_EUSER;
            }
        }

        private unsafe int WriteProxy(git_odb_backend* backend, ref git_oid oid, byte* buffer, UIntPtr len, git_object_t type)
        {
            try
            {
                Ensure.CastToInt(len, "length");

                var objectId = ObjectId.FromNative(oid);
                var data = new ReadOnlySpan<byte>(buffer, (int)len);

                Write(objectId, data, (ObjectType)type);

                return 0;
            }
            catch (Exception e)
            {
                libgit2.git_error_set_str(git_error_t.GITERR_ODB, e.Message);
                return (int)git_error_code.GIT_EUSER;
            }
        }

        private unsafe void Free(git_odb_backend* backend)
        {
            Marshal.FreeHGlobal((IntPtr)backend);
        }

        private unsafe git_odb_backend* CreateCallbackStructure()
        {
            git_odb_backend_managed managed_backend = new git_odb_backend_managed();

            managed_backend.version = 1;

            if (CanRead) { managed_backend.read = ReadProxy; } else { managed_backend.read = null; }
            if (CanReadHeader) { managed_backend.read_header = ReadHeaderProxy; } else { managed_backend.read_header = null; }
            if (CanWrite) { managed_backend.write = WriteProxy; } else { managed_backend.write = null; }

            managed_backend.read_prefix = null;
            managed_backend.writestream = null;
            managed_backend.readstream = null;
            managed_backend.exists = null;
            managed_backend.exists_prefix = null;
            managed_backend.refresh = null;
            managed_backend.foreach_object = null;
            managed_backend.writepack = null;
            managed_backend.freshen = null;
            managed_backend.free = Free;

            IntPtr managed_backend_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(managed_backend));
            Marshal.StructureToPtr(managed_backend, managed_backend_ptr, false);
            return (git_odb_backend*)managed_backend_ptr;
        }

        /// <summary>
        /// Allocates space for an object database object.  This must be
        /// returned to libgit2 in the `Read` method; if an error occurs
        /// and you do not wish to return it to libgit2, you must call the
        /// `Free` method on the buffer.
        /// </summary>
        protected unsafe ManagedObjectDatabaseBuffer AllocateBuffer(int length)
        {
            Ensure.ArgumentConformsTo(() => (length >= 0), "length", "length is negative");

            return new ManagedObjectDatabaseBuffer(this, length);
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating
        /// whether the current backend supports reading.
        /// </summary>
        public virtual bool CanRead { get { return false; } }

        /// <summary>
        /// When overridden in a derived class, reads an object from the
        /// object database and places the raw contents into a buffer.  The
        /// buffer can be allocated with `AllocateBuffer`.
        /// </summary>
        /// <param name="id">The object ID to read</param>
        /// <param name="data">The object contents</param>
        /// <param name="type">The type of object that was read</param>
        /// <returns>true if the object exists, false if the object is not in the database</returns>
        public virtual bool Read(ObjectId id, out ManagedObjectDatabaseBuffer data, out ObjectType type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating
        /// whether the current backend supports reading headers.
        /// </summary>
        public virtual bool CanReadHeader { get { return false; } }

        /// <summary>
        /// When overridden in a derived class, reads the properties of an
        /// object from the database.  If this method is not overridden,
        /// the entire object will be read to calculate the length when
        /// required.
        /// </summary>
        /// <param name="id">The object ID to read</param>
        /// <param name="length">The length of the object</param>
        /// <param name="type">The type of object</param>
        /// <returns>true if the object exists, false if the object is not in the database</returns>
        public virtual bool ReadHeader(ObjectId id, out long length, out ObjectType type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating
        /// whether the current backend supports writing.
        /// </summary>
        public virtual bool CanWrite { get { return false; } }

        /// <summary>
        /// When overridden in a derived class, writes the object to the
        /// object database.
        /// </summary>
        /// <param name="id">The object ID to write</param>
        /// <param name="data">The contents of the object to write</param>
        /// <param name="type">The type of object to write</param>
        public virtual void Write(ObjectId id, ReadOnlySpan<byte> data, ObjectType type)
        {
            throw new NotImplementedException();
        }
    }

    public class ManagedObjectDatabaseBuffer
    {
        private ManagedObjectDatabaseBackend backend;
        private int length;

        private unsafe byte* nativeObject;

        internal unsafe ManagedObjectDatabaseBuffer(ManagedObjectDatabaseBackend backend, int length)
        {
            Ensure.ArgumentNotNull(backend, "backend");
            Ensure.ArgumentConformsTo(() => (length >= 0), "length", "length is negative");

            this.backend = backend;
            this.length = length;

            nativeObject = libgit2.git_odb_backend_data_alloc(backend.NativeObject, (UIntPtr)length);

            if (nativeObject == null)
            {
                throw new OutOfMemoryException();
            }
        }

        internal unsafe byte* NativeObject
        {
            get
            {
                return nativeObject;
            }
        }

        public unsafe Span<byte> Data
        {
            get
            {
                return new Span<byte>(nativeObject, length);
            }
        }

        public unsafe void Free()
        {
            libgit2.git_odb_backend_data_free(backend.NativeObject, nativeObject);
        }
    }
}
