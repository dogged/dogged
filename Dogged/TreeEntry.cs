using System;
using System.Collections.Generic;
using Dogged.Native;
using Dogged.Native.Services;

namespace Dogged
{
    /// <summary>
    /// Representation of an entry in a tree.
    /// </summary>
    public class TreeEntry
    {
        private readonly Tree parent;
        private unsafe readonly git_tree_entry* nativeEntry;

        private readonly LazyNative<FileMode> mode;
        private readonly LazyNative<ObjectId> id;
        private readonly LazyNative<string> name;

        private unsafe TreeEntry(Tree parent, git_tree_entry* nativeEntry)
        {
            Ensure.ArgumentNotNull(parent, "parent");
            Ensure.ArgumentNotNull(nativeEntry, "nativeEntry");

            this.parent = parent;
            this.nativeEntry = nativeEntry;

            mode = new LazyNative<FileMode>(() => (FileMode)libgit2.git_tree_entry_filemode(nativeEntry), parent);

            id = new LazyNative<ObjectId>(() => {
                git_oid* oid = libgit2.git_tree_entry_id(nativeEntry);
                Ensure.NativePointerNotNull(oid);
                return ObjectId.FromNative(*oid);
            }, parent);
            name = new LazyNative<string>(() => libgit2.git_tree_entry_name(nativeEntry), parent);
        }

        internal unsafe static TreeEntry FromNative(Tree parent, git_tree_entry* nativeEntry)
        {
            return new TreeEntry(parent, nativeEntry);
        }

        /// <summary>
        /// Gets the file mode for the tree entry.
        /// </summary>
        public FileMode Mode
        {
            get
            {
                return mode.Value;
            }
        }

        /// <summary>
        /// Gets the type of the tree entry.
        /// </summary>
        public ObjectType Type
        {
            get
            {
                if (mode.Value == FileMode.Commit)
                {
                    return ObjectType.Commit;
                }
                else if (mode.Value == FileMode.Tree)
                {
                    return ObjectType.Tree;
                }
                else
                {
                    return ObjectType.Blob;
                }
            }
        }

        /// <summary>
        /// Gets the object id of the tree entry.
        /// </summary>
        public ObjectId Id
        {
            get
            {
                return id.Value;
            }
        }

        /// <summary>
        /// Gets the filename of the tree entry.
        /// </summary>
        public string Name
        {
            get
            {
                return name.Value;
            }
        }
    }
}
