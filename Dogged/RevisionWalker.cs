using System;
using System.Collections;
using System.Collections.Generic;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// The sorting to use with <see cref="RevisionWalker"/>.
    /// </summary>
    public enum SortFlags
    {
        /// <inheritdoc cref="git_sort_t.GIT_SORT_NONE"/>
        None = git_sort_t.GIT_SORT_NONE,

        /// <inheritdoc cref="git_sort_t.GIT_SORT_TOPOLOGICAL"/>
        Topological = git_sort_t.GIT_SORT_TOPOLOGICAL,

        /// <inheritdoc cref="git_sort_t.GIT_SORT_TIME"/>
        Time = git_sort_t.GIT_SORT_TIME,

        /// <inheritdoc cref="git_sort_t.GIT_SORT_REVERSE"/>
        Reverse = git_sort_t.GIT_SORT_REVERSE,
    }

    /// <summary>
    /// A revision walker to iterate over the commits of a repository.
    /// </summary>
    public class RevisionWalker : NativeDisposable, IEnumerable<Commit>
    {
        private unsafe git_revwalk* nativeRevwalk;
        private readonly Repository repository;
        private SortFlags sortFlags;
        private ObjectId currentObjectId;

        private unsafe RevisionWalker(git_revwalk* nativeRevwalk, Repository repository)
        {
            Ensure.NativePointerNotNull(nativeRevwalk);
            this.nativeRevwalk = nativeRevwalk;
            this.repository = repository;
        }

        /// <summary>
        /// Creates a new revision walker for the Git repository.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <returns>The revision walker.</returns>
        public unsafe static RevisionWalker Create(Repository repository)
        {
            Ensure.ArgumentNotNull(repository, "repository");

            git_revwalk* nativeRevwalk;
            Ensure.NativeSuccess(libgit2.git_revwalk_new(out nativeRevwalk, repository.NativeRepository));
            return new RevisionWalker(nativeRevwalk, repository);
        }

        internal unsafe git_revwalk* NativeRevwalk
        {
            get
            {
                return nativeRevwalk;
            }
        }

        internal unsafe override bool IsDisposed
        {
            get
            {
                return (nativeRevwalk == null);
            }
        }

        internal unsafe override void Dispose(bool disposing)
        {
            if (nativeRevwalk != null)
            {
                libgit2.git_revwalk_free(nativeRevwalk);
                nativeRevwalk = null;
            }
        }

        public Repository Repository
        {
            get
            {
                return repository;
            }
        }

        /// <summary>
        /// The sorting mode when iterating through the repository's contents.
        /// </summary>
        public unsafe SortFlags SortFlags
        {
            get
            {
                return sortFlags;
            }
            set
            {
                Ensure.NativeSuccess(() => libgit2.git_revwalk_sorting(NativeRevwalk, (git_sort_t)value), this);
                sortFlags = value;
            }
        }

        public IEnumerator<Commit> GetEnumerator()
        {
            while (MoveNext())
            {
                yield return CurrentCommit;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Reset the revision walker for reuse.
        /// </summary>
        /// <remarks>
        /// This will clear all the pushed and hidden commits, and leave the walker in a blank state (just like at creation)
        /// ready to receive new commit pushes and start a new walk. The revision walk is automatically reset when a walk is over.
        /// </remarks>
        public unsafe void Reset()
        {
            Ensure.NativeSuccess(() => libgit2.git_revwalk_reset(NativeRevwalk), this);
        }

        public unsafe bool MoveNext()
        {
            git_oid nativeId;
            int ret = Ensure.NativeCall(() => libgit2.git_revwalk_next(ref nativeId, NativeRevwalk), this);

            if (ret == (int)git_error_code.GIT_ITEROVER)
            {
                return false;
            }
            Ensure.NativeSuccess(ret);

            currentObjectId = ObjectId.FromNative(nativeId);
            return true;
        }

        public ObjectId CurrentObjectId
        {
            get
            {
                return currentObjectId;
            }
        }

        public Commit CurrentCommit
        {
            get
            {
                return Repository.Objects.Lookup<Commit>(CurrentObjectId);
            }
        }

        /// <summary>
        /// Mark a commit (and its ancestors) uninteresting for the output.
        /// </summary>
        /// <param name="commit">The commit that will be ignored during the traversal.</param>
        public unsafe void HideCommit(Commit commit)
        {
            HideCommit(commit.Id);
        }

        /// <summary>
        /// Mark a commit (and its ancestors) uninteresting for the output.
        /// </summary>
        /// <param name="id">The commit that will be ignored during the traversal.</param>
        public unsafe void HideCommit(ObjectId id)
        {
            git_oid oid = id.ToNative();
            Ensure.NativeSuccess(() => libgit2.git_revwalk_hide(NativeRevwalk, ref oid), this);
        }

        /// <summary>
        /// Hides the repository's HEAD.
        /// </summary>
        public unsafe void HideHead()
        {
            Ensure.NativeSuccess(() => libgit2.git_revwalk_hide_head(NativeRevwalk), this);
        }

        /// <summary>
        /// Hides matching references.
        /// </summary>
        /// <param name="glob">The glob pattern references should match.</param>
        public unsafe void HideGlob(string glob)
        {
            Ensure.NativeSuccess(() => libgit2.git_revwalk_hide_glob(NativeRevwalk, glob), this);
        }

        /// <summary>
        /// Hide the OID pointed to by a reference.
        /// </summary>
        /// <param name="reference">The reference to hide.</param>
        public unsafe void HideReference(Reference reference)
        {
            HideReference(reference.Name);
        }

        /// <summary>
        /// Hide the OID pointed to by a reference.
        /// </summary>
        /// <param name="refname">The reference to hide.</param>
        public unsafe void HideReference(string refname)
        {
            Ensure.NativeSuccess(() => libgit2.git_revwalk_hide_ref(NativeRevwalk, refname), this);
        }

        /// <summary>
        /// Add a new root for the traversal.
        /// </summary>
        /// <param name="commit">The commit to start from.</param>
        public unsafe void PushCommit(Commit commit)
        {
            PushCommit(commit.Id);
        }

        /// <summary>
        /// Add a new root for the traversal.
        /// </summary>
        /// <param name="id">The commit to start from.</param>
        public unsafe void PushCommit(ObjectId id)
        {
            git_oid oid = id.ToNative();
            Ensure.NativeSuccess(() => libgit2.git_revwalk_push(NativeRevwalk, ref oid), this);
        }

        /// <summary>
        /// Push the repository's HEAD.
        /// </summary>
        public unsafe void PushHead()
        {
            Ensure.NativeSuccess(() => libgit2.git_revwalk_push_head(NativeRevwalk), this);
        }

        /// <summary>
        /// Push matching references.
        /// </summary>
        /// <param name="glob">The glob pattern references should match.</param>
        public unsafe void PushGlob(string glob)
        {
            Ensure.NativeSuccess(() => libgit2.git_revwalk_push_glob(NativeRevwalk, glob), this);
        }

        /// <summary>
        /// Push and hide the respective endpoints of the given range.
        /// </summary>
        /// <param name="range">The range.</param>
        public unsafe void PushRange(string range)
        {
            Ensure.NativeSuccess(() => libgit2.git_revwalk_push_range(NativeRevwalk, range), this);
        }

        /// <summary>
        /// Push the OID pointed to by a reference.
        /// </summary>
        /// <param name="reference">The reference to push.</param>
        public unsafe void PushReference(Reference reference)
        {
            PushReference(reference.Name);
        }

        /// <summary>
        /// Push the OID pointed to by a reference.
        /// </summary>
        /// <param name="refname">The reference to push.</param>
        public unsafe void PushReference(string refname)
        {
            Ensure.NativeSuccess(() => libgit2.git_revwalk_push_ref(NativeRevwalk, refname), this);
        }

        /// <summary>
        /// Simplify the history by first-parent.
        /// </summary>
        public unsafe void SimplifyFirstParent()
        {
            Ensure.NativeSuccess(() => libgit2.git_revwalk_simplify_first_parent(NativeRevwalk), this);
        }
    }
}
