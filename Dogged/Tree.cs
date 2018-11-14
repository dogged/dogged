using System;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// A tree (directory listing) object.
    /// </summary>
    public class Tree : GitObject
    {
        private unsafe Tree(git_tree* nativeTree, ObjectId id) :
            base((git_object*)nativeTree, id)
        {
        }

        internal unsafe static Tree FromNative(git_tree* nativeTree, ObjectId id)
        {
            return new Tree(nativeTree, id);
        }

        private unsafe git_tree* NativeTree
        {
            get
            {
                return (git_tree*)NativeObject;
            }
        }

        public override ObjectType Type
        {
            get
            {
                Ensure.NotDisposed(this);
                return ObjectType.Tree;
            }
        }
    }
}
