using System;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// A commit object.
    /// </summary>
    public class Commit : GitObject
    {
        private LazyNative<Signature> author;
        private LazyNative<Signature> committer;
        private readonly LazyNative<ObjectId> treeId;

        private unsafe Commit(git_commit* nativeCommit, ObjectId id) :
            base((git_object*)nativeCommit, id)
        {
            author = new LazyNative<Signature>(this);
            committer = new LazyNative<Signature>(this);
            treeId = new LazyNative<ObjectId>(this);
        }

        internal unsafe static Commit FromNative(git_commit* nativeCommit, ObjectId id = null)
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

        /// <summary>
        /// Gets the signature of the committer of this commit.
        /// </summary>
        public unsafe Signature Committer
        {
            get
            {
                return committer.Get(() => {
                    git_signature* signature = libgit2.git_commit_committer(NativeCommit);
                    return Signature.FromNative(signature);
                });
            }
        }

        /// <summary>
        /// Gets the signature of the author of this commit.
        /// </summary>
        public unsafe Signature Author
        {
            get
            {
                return author.Get(() => {
                    git_signature* author = libgit2.git_commit_author(NativeCommit);
                    return Signature.FromNative(author);
                });
            }
        }

        /// <summary>
        /// Gets the object ID of the tree that this commit points to.
        /// </summary>
        public unsafe ObjectId TreeId
        {
            get
            {
                return treeId.Get(() => {
                    git_oid* oid = libgit2.git_commit_tree_id(NativeCommit);
                    Ensure.NativePointerNotNull(oid);
                    return ObjectId.FromNative(*oid);
                });
            }
        }

        /// <summary>
        /// Gets the tree that this commit points to.
        /// </summary>
        public unsafe Tree Tree
        {
            get
            {
                git_tree* tree = null;

                Ensure.NativeSuccess(() => libgit2.git_commit_tree(out tree, NativeCommit), this);
                Ensure.NativePointerNotNull(tree);

                return Tree.FromNative(tree);
            }
        }
    }
}
