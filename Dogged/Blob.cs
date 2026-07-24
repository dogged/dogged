using System;
using System.Runtime.InteropServices;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// A blob (file) object.
    /// </summary>
    public class Blob : GitObject
    {
        private readonly LazyNative<long> rawSize;
        private readonly LazyNative<bool> isBinary;

        private unsafe Blob(BlobHandle nativeBlob, ObjectId id) :
            base(nativeBlob, id)
        {
            rawSize = new LazyNative<long>(() => libgit2.git_blob_rawsize(nativeBlob), this);
            isBinary = new LazyNative<bool>(() => libgit2.git_blob_is_binary(nativeBlob) == 0 ? false : true, this);
        }

        internal unsafe static Blob FromNative(BlobHandle nativeBlob, ObjectId id = null)
        {
            return new Blob(nativeBlob, id);
        }

        internal unsafe static new Blob FromNative(ObjectHandle nativeObject, ObjectId id = null)
        {
            BlobHandle nativeBlob = new BlobHandle((git_blob*)nativeObject.DangerousGetHandle());
            nativeObject.SetHandleAsInvalid();

            return FromNative(nativeBlob, id);
        }

        internal unsafe BlobHandle NativeBlob
        {
            get
            {
                return (BlobHandle)NativeObject;
            }
        }

        public override ObjectType Type
        {
            get
            {
                Ensure.NotDisposed(this);
                return ObjectType.Blob;
            }
        }

        public long RawSize
        {
            get
            {
                return rawSize.Value;
            }
        }

        public unsafe ReadOnlySpan<byte> RawContent
        {
            get
            {
                Ensure.NotDisposed(this);

                var size = Ensure.CastToInt(RawSize, "rawsize");
                return new ReadOnlySpan<byte>(libgit2.git_blob_rawcontent(NativeBlob), size);
            }
        }

        public unsafe GitBuffer GetFilteredContent(string path)
        {
            return GetFilteredContent(path, new BlobFilterOptions());
        }

        public unsafe GitBuffer GetFilteredContent(string path, BlobFilterOptions options)
        {
            Ensure.ArgumentNotNull(path, "path");
            Ensure.ArgumentNotNull(options, "options");

            GitBuffer buf = new GitBuffer();
            git_blob_filter_options nativeOptions = options.NativeOptions;
            Ensure.NativeSuccess(libgit2.git_blob_filter(ref buf.NativeBuffer, NativeBlob, path, ref nativeOptions));

            return buf;
        }

        public bool IsBinary
        {
            get
            {
                return isBinary.Value;
            }
        }
    }
}
