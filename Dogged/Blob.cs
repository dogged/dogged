using System;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// A blob (file) object.
    /// </summary>
    public class Blob : GitObject
    {
        private unsafe Blob(git_blob* nativeBlob, ObjectId id) :
            base((git_object*)nativeBlob, id)
        {
        }

        internal unsafe static Blob FromNative(git_blob* nativeBlob, ObjectId id)
        {
            return new Blob(nativeBlob, id);
        }

        private unsafe git_blob* NativeBlob
        {
            get
            {
                return (git_blob*)NativeObject;
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
    }
}
