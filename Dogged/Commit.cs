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

        private unsafe Commit(CommitHandle nativeCommit, ObjectId id) :
            base(nativeCommit, id)
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
        }

        internal unsafe static Commit FromNative(CommitHandle nativeCommit, ObjectId id = null)
        {
            return new Commit(nativeCommit, id);
        }

        internal unsafe static new Commit FromNative(ObjectHandle nativeObject, ObjectId id = null)
        {
            CommitHandle nativeCommit = new CommitHandle((git_commit*)nativeObject.DangerousGetHandle());
            nativeObject.SetHandleAsInvalid();

            return FromNative(nativeCommit, id);
        }

        internal unsafe CommitHandle NativeCommit
        {
            get
            {
                return (CommitHandle)NativeObject;
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
                TreeHandle tree = null;

                Ensure.NativeHandleIsValid(NativeCommit, "commit");
                Ensure.NativeSuccess(libgit2.git_commit_tree(out tree, NativeCommit));
                Ensure.NativeHandleIsValid(tree);

                return Tree.FromNative(tree);
            }
        }
    }
}
