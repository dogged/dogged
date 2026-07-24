using System;
using System.Collections;
using System.Collections.Generic;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// A tree (directory listing) object.
    /// </summary>
    public class Tree : GitObject, IEnumerable<TreeEntry>
    {
        private readonly LazyNative<int> count;

        private unsafe Tree(TreeHandle nativeTree) :
            this(nativeTree, null)
        {
        }

        private unsafe Tree(TreeHandle nativeTree, ObjectId id) :
            base(nativeTree, id)
        {
            count = new LazyNative<int>(() => {
                UIntPtr count = libgit2.git_tree_entrycount(NativeTree);
                return Ensure.CastToInt(count, "count");
            }, this);
        }

        internal unsafe static Tree FromNative(TreeHandle nativeTree, ObjectId id = null)
        {
            return new Tree(nativeTree, id);
        }

        internal unsafe static new Tree FromNative(ObjectHandle nativeObject, ObjectId id = null)
        {
            TreeHandle nativeTree = new TreeHandle((git_tree*)nativeObject.DangerousGetHandle());
            nativeObject.SetHandleAsInvalid();

            return FromNative(nativeTree, id);
        }

        internal unsafe TreeHandle NativeTree
        {
            get
            {
                return (TreeHandle)NativeObject;
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

        /// <summary>
        /// Gets the number of entries in the tree.
        /// </summary>
        public unsafe int Count
        {
            get
            {
                return count.Value;
            }
        }

        /// <summary>
        /// Gets the index entry at the specified index.
        /// </summary>
        public unsafe TreeEntry this[int position]
        {
            get
            {
                Ensure.NativeHandleIsValid(NativeTree, "tree");
                Ensure.ArgumentConformsTo(() => position >= 0, "position", "position must not be negative");

                git_tree_entry* entry = libgit2.git_tree_entry_byindex(NativeTree, (UIntPtr)position);
                GC.KeepAlive(this);

                if (entry == null)
                {
                    throw new IndexOutOfRangeException(string.Format("there is no tree entry at position {0}", position));
                }

                return TreeEntry.FromNative(this, entry);
            }
        }

        /// <summary>
        /// Gets the index entry at the specified path.
        /// </summary>
        public unsafe TreeEntry this[string name]
        {
            get
            {
                Ensure.NativeHandleIsValid(NativeTree, "tree");
                Ensure.ArgumentNotNull(name, "name");

                git_tree_entry* entry = libgit2.git_tree_entry_byname(NativeTree, name);
                GC.KeepAlive(this);

                if (entry == null)
                {
                    throw new KeyNotFoundException(string.Format("there is no tree entry for path '{0}'", name));
                }

                return TreeEntry.FromNative(this, entry);
            }
        }

        /// <summary>
        /// Returns an enumerate that iterates through tree entries.
        /// </summary>
        /// <returns>An enumerator of <see cref="TreeEntry"/> objects</returns>
        public IEnumerator<TreeEntry> GetEnumerator()
        {
            int count = Count;

            for (int i = 0; i < count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
