using System;
using System.Runtime.InteropServices;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// Representation of an object as read from the database.
    /// </summary>
    public class ObjectDatabaseObject : NativeDisposable
    {
        private unsafe git_odb_object* nativeObject;

        private readonly LazyNative<ObjectId> id;
        private readonly LazyNative<ObjectType> type;
        private readonly LazyNative<long> size;

        private unsafe ObjectDatabaseObject(git_odb_object* nativeObject)
        {
            Ensure.NativePointerNotNull(nativeObject);

            this.nativeObject = nativeObject;

            id = new LazyNative<ObjectId>(() => {
                git_oid* oid = libgit2.git_odb_object_id(nativeObject);
                Ensure.NativePointerNotNull(oid);
                return ObjectId.FromNative(*oid);
            }, this);
            type = new LazyNative<ObjectType>(() => {
                git_object_t type = libgit2.git_odb_object_type(nativeObject);
                Ensure.EnumDefined(typeof(ObjectType), (int)type, "object type");
                return (ObjectType)type;
            }, this);
            size = new LazyNative<long>(() => {
                UIntPtr size = libgit2.git_odb_object_size(nativeObject);
                return Ensure.CastToLong(size, "size");
            }, this);
        }

        internal unsafe static ObjectDatabaseObject FromNative(git_odb_object* nativeObject)
        {
            return new ObjectDatabaseObject(nativeObject);
        }

        public ObjectId Id
        {
            get
            {
                return id.Value;
            }
        }

        public ObjectType Type
        {
            get
            {
                return type.Value;
            }
        }

        public long Size
        {
            get
            {
                return size.Value;
            }
        }

        public unsafe ReadOnlySpan<byte> Data
        {
            get
            {
                int size = Ensure.CastToInt(Size, "size");

                return new ReadOnlySpan<byte>(libgit2.git_odb_object_data(nativeObject), size);
            }
        }

        internal unsafe override bool IsDisposed
        {
            get
            {
                return (nativeObject == null);
            }
        }

        internal unsafe override void Dispose(bool disposing)
        {
            if (nativeObject != null)
            {
                libgit2.git_odb_object_free(nativeObject);
                nativeObject = null;
            }
        }
    }
}
