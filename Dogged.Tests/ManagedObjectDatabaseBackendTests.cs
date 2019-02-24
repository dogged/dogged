using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

using Dogged;

namespace Dogged.Tests
{
    /// <summary>
    /// Tests for working with managed object database backends.
    /// </summary>
    public class ManagedObjectDatabaseBackendTests : TestBase
    {
        private class InmemoryObjectDatabaseBackend : ManagedObjectDatabaseBackend
        {
            private class ObjectData
            {
                public ObjectData(byte[] buffer, ObjectType type)
                {
                    Buffer = buffer;
                    Type = type;
                }

                public byte[] Buffer
                {
                    get;
                    private set;
                }

                public ObjectType Type
                {
                    get;
                    private set;
                }
            }

            private Dictionary<ObjectId, ObjectData> contents = new Dictionary<ObjectId, ObjectData>();

            public override bool CanRead { get { return true; } }

            public override bool Read(ObjectId id, out ManagedObjectDatabaseBuffer buffer, out ObjectType type)
            {
                ObjectData objectData;

                if (!contents.TryGetValue(id, out objectData))
                {
                    buffer = null;
                    type = 0;
                    return false;
                }

                buffer = AllocateBuffer(objectData.Buffer.Length);

                for (var i = 0; i < objectData.Buffer.Length; i++)
                {
                    buffer.Data[i] = objectData.Buffer[i];
                }

                type = objectData.Type;

                return true;
            }

            public override bool CanReadHeader { get { return true; } }

            public override bool ReadHeader(ObjectId id, out long length, out ObjectType type)
            {
                ObjectData objectData;

                if (!contents.TryGetValue(id, out objectData))
                {
                    length = 0;
                    type = 0;
                    return false;
                }

                length = objectData.Buffer.Length;
                type = objectData.Type;

                return true;
            }

            public override bool CanWrite { get { return true; } }

            public override void Write(ObjectId id, ReadOnlySpan<byte> data, ObjectType type)
            {
                if (contents.ContainsKey(id))
                {
                    return;
                }

                if (data.Length > int.MaxValue)
                {
                    throw new InvalidDataException("file is too large");
                }

                byte[] buf = new byte[data.Length];
                Buffer.BlockCopy(data.ToArray(), 0, buf, 0, data.Length);

                contents[id] = new ObjectData(buf, type);
            }
        }

        [Fact]
        public void CanReadFromCustomObjectDatabase()
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (ObjectDatabase odb = repo.ObjectDatabase)
            {
                var oid = new ObjectId("f75ba05f340c51065cbea2e1fdbfe5fe13144c97");
                var data = Encoding.UTF8.GetBytes("Hello, world.\n");
                var dataSpan = new ReadOnlySpan<byte>(data);

                var backend = new InmemoryObjectDatabaseBackend();
                odb.AddBackend(backend, 10);

                Assert.Throws<NotFoundException>(() => odb.Read(oid));

                backend.Write(oid, dataSpan, ObjectType.Blob);

                var obj = odb.Read(oid);

                Assert.Equal(oid, obj.Id);
                Assert.Equal(ObjectType.Blob, obj.Type);
                Assert.Equal(data.Length, obj.Size);
                Assert.True(dataSpan.SequenceEqual(obj.Data));
            }
        }

        [Fact]
        public void CanReadHeaderFromCustomObjectDatabase()
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (ObjectDatabase odb = repo.ObjectDatabase)
            {
                var oid = new ObjectId("f75ba05f340c51065cbea2e1fdbfe5fe13144c97");
                var data = Encoding.UTF8.GetBytes("Hello, world.\n");

                var backend = new InmemoryObjectDatabaseBackend();
                odb.AddBackend(backend, 10);

                Assert.Throws<NotFoundException>(() => odb.Read(oid));

                backend.Write(oid, new ReadOnlySpan<byte>(data), ObjectType.Blob);
                Assert.Equal((data.Length, ObjectType.Blob), odb.ReadHeader(oid));
            }
        }

        [Fact]
        public void CanWriteToCustomObjectDatabase()
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (ObjectDatabase odb = repo.ObjectDatabase)
            {
                var data = Encoding.UTF8.GetBytes("Hello, world.\n");
                
                ManagedObjectDatabaseBuffer buffer;
                ObjectType objectType;

                var backend = new InmemoryObjectDatabaseBackend();
                odb.AddBackend(backend, 10);

                odb.Write(data, ObjectType.Blob);

                backend.Read(new ObjectId("f75ba05f340c51065cbea2e1fdbfe5fe13144c97"), out buffer, out objectType);

                Assert.Equal(data.Length, buffer.Data.Length);
                Assert.Equal(data, buffer.Data.ToArray());
                Assert.Equal(ObjectType.Blob, objectType);
            }
        }

        [Fact]
        public void CanAddCustomObjectDatabaseToCustomRepository()
        {
            using (Repository repo = SandboxRepository("testrepo"))
            using (ObjectDatabase odb = new ObjectDatabase())
            {
                var backend = new InmemoryObjectDatabaseBackend();
                var oid = new ObjectId("f75ba05f340c51065cbea2e1fdbfe5fe13144c97");

                repo.ObjectDatabase = odb;
                odb.AddBackend(backend, 10);

                var data = Encoding.UTF8.GetBytes("Hello, world.\n");
                odb.Write(data, ObjectType.Blob);

                var obj = odb.Read(oid);

                Assert.Equal(oid, obj.Id);
                Assert.Equal(ObjectType.Blob, obj.Type);
                Assert.Equal(data.Length, obj.Size);
                Assert.True(new ReadOnlySpan<byte>(data).SequenceEqual(obj.Data));
            }
        }

        private class ThrowingObjectDatabaseBackend : ManagedObjectDatabaseBackend
        {
            public override bool CanRead { get { return true; } }

            public override bool Read(ObjectId id, out ManagedObjectDatabaseBuffer buffer, out ObjectType type)
            {
                buffer = AllocateBuffer(42);
                buffer.Free();

                throw new Exception("Read exception");
            }

            public override bool CanReadHeader { get { return true; } }

            public override bool ReadHeader(ObjectId id, out long length, out ObjectType type)
            {
                throw new Exception("Read header exception");
            }

            public override bool CanWrite { get { return true; } }

            public override void Write(ObjectId id, ReadOnlySpan<byte> data, ObjectType type)
            {
                throw new Exception("Write exception");
            }
        }

        [Fact]
        public void CanThrowInObjectDatabaseFunctions()
        {
            using (Repository repo = Repository.CreateCustomRepository())
            using (ObjectDatabase odb = new ObjectDatabase())
            {
                repo.ObjectDatabase = odb;
                odb.AddBackend(new ThrowingObjectDatabaseBackend(), 10);

                Assert.Throws<UserCancelledException>(() => odb.Read(new ObjectId("aaaaaaaaaabbbbbbbbbbccccccccccdddddddddd")));
                Assert.Throws<UserCancelledException>(() => odb.ReadHeader(new ObjectId("aaaaaaaaaabbbbbbbbbbccccccccccdddddddddd")));
                Assert.Throws<UserCancelledException>(() => odb.Write(new byte[] { 0x42 }, ObjectType.Blob));
            }
        }
    }
}
