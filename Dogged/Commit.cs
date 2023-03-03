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
        private readonly Lazy<CommitParentCollection> parents;

        private unsafe Commit(git_commit* nativeCommit, ObjectId id) :
            base((git_object*)nativeCommit, id)
        {
            author = new LazyNative<Signature>(() => {
                git_signature* author = libgit2.git_commit_author(NativeCommit);
                return Signature.FromNative(author);
            }, this);
            committer = new LazyNative<Signature>(() => {
                git_signature* signature = libgit2.git_commit_committer(NativeCommit);
                return Signature.FromNative(signature);
            }, this);
            treeId = new LazyNative<ObjectId>(() => {
                git_oid* oid = libgit2.git_commit_tree_id(NativeCommit);
                Ensure.NativePointerNotNull(oid);
                return ObjectId.FromNative(*oid);
            }, this);
            parents = new Lazy<CommitParentCollection>(() => new CommitParentCollection(this));
        }

        internal unsafe static Commit FromNative(git_commit* nativeCommit, ObjectId id = null)
        {
            return new Commit(nativeCommit, id);
        }

        internal unsafe git_commit* NativeCommit
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
        public Signature Committer
        {
            get
            {
                return committer.Value;
            }
        }

        /// <summary>
        /// Gets the signature of the author of this commit.
        /// </summary>
        public Signature Author
        {
            get
            {
                return author.Value;
            }
        }

        /// <summary>
        /// Gets the object ID of the tree that this commit points to.
        /// </summary>
        public ObjectId TreeId
        {
            get
            {
                return treeId.Value;
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

        /// <summary>
        /// Gets the message for this commit.
        /// </summary>
        public unsafe string Message
        {
            get
            {
                return libgit2.git_commit_message(NativeCommit);
            }
        }

        /// <summary>
        /// An accessor for parent commits.
        /// </summary>
        public CommitParentCollection Parents
        {
            get
            {
                return parents.Value;
            }
        }
    }
}
