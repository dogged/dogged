using System;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// A commit object.
    /// </summary>
    public class Commit : GitObject
    {
        private unsafe Commit(git_commit* nativeCommit, ObjectId id) :
            base((git_object*)nativeCommit, id)
        {
        }

        internal unsafe static Commit FromNative(git_commit* nativeCommit, ObjectId id)
        {
            return new Commit(nativeCommit, id);
        }

        private unsafe git_commit* NativeCommit
        {
            get
            {
                return (git_commit*)NativeObject;
            }
        }

        public override ObjectType Type
        {
            get
            {
                Ensure.NotDisposed(this);
                return ObjectType.Commit;
            }
        }
    }
}
