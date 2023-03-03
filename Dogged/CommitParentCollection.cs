using System;
using System.Collections;
using System.Collections.Generic;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// Accessors for the Git parents (commits) of a commit.
    /// </summary>
    public class CommitParentCollection
    {
        private Commit commit;
        private readonly LazyNative<int> count;

        internal unsafe CommitParentCollection(Commit commit)
        {
            this.commit = commit;
            count = new LazyNative<int>(() => {
                UIntPtr count = Ensure.NativeCall<UIntPtr>(() => libgit2.git_commit_parentcount(commit.NativeCommit), this.commit);
                return Ensure.CastToInt(count, "count");
            }, this.commit);
        }

        /// <summary>
        /// Gets the number of parent commits.
        /// </summary>
        public int Count
        {
            get
            {
                return count.Value;
            }
        }

        /// <summary>
        /// Gets the parent commit at the specified index.
        /// </summary>
        public unsafe Commit this[int position]
        {
            get
            {
                Ensure.NotDisposed(commit.NativeObject, "commit");
                Ensure.ArgumentConformsTo(() => position >= 0, "position", "position must not be negative");

                git_commit* parent = null;
                Ensure.NativeSuccess(() => libgit2.git_commit_parent(out parent, commit.NativeCommit, (UIntPtr)position), this.commit);
                Ensure.NativePointerNotNull(parent);

                return Commit.FromNative(parent);
            }
        }
    }
}
