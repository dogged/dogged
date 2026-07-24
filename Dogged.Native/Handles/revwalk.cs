using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public sealed class RevwalkHandle : SafeHandle
    {
        public RevwalkHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe RevwalkHandle(git_revwalk* nativeRevwalk) :
            base((IntPtr)nativeRevwalk, true)
        {
        }

        public override bool IsInvalid
        {
            get
            {
                return handle == IntPtr.Zero;
            }
        }

        protected override unsafe bool ReleaseHandle()
        {
            libgit2.git_revwalk_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Allocate a new revision walker to iterate through a repo.
        ///
        ///  <para>
        ///   This revision walker uses a custom memory pool and an
        ///   internal commit cache, so it is relatively expensive to
        ///   allocate.
        ///  </para>
        ///
        ///  <para>
        ///   For maximum performance, this revision walker should be
        ///   reused for different walks.
        ///  </para>
        ///
        ///  <para>
        ///   This revision walker is *not* thread safe: it may only
        ///   be used to walk a repository on a single thread;
        ///   however, it is possible to have several revision walkers
        ///   in several different threads walking the same
        ///   repository.
        ///  </para>
        /// </summary>
        /// <param name="_out">pointer to the new revision walker</param>
        /// <param name="repo">the repo to walk through</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_revwalk_new(
            out RevwalkHandle _out,
            RepositoryHandle repo);

        /// <summary>
        ///  Reset the revision walker for reuse.
        ///
        ///  <para>
        ///   This will clear all the pushed and hidden commits, and
        ///   leave the walker in a blank state (just like at
        ///   creation) ready to receive new commit pushes and start a
        ///   new walk.
        ///  </para>
        ///
        ///  <para>
        ///   The revision walk is automatically reset when a walk is
        ///   over.
        ///  </para>
        /// </summary>
        /// <param name="walker">handle to reset.</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_revwalk_reset(
            RevwalkHandle walker);

        /// <summary>
        ///  Add a new root for the traversal
        ///
        ///  <para>
        ///   The pushed commit will be marked as one of the roots
        ///   from which to start the walk. This commit may not be
        ///   walked if it or a child is hidden.
        ///  </para>
        ///
        ///  <para>
        ///   At least one commit must be pushed onto the walker
        ///   before a walk can be started.
        ///  </para>
        ///
        ///  <para>
        ///   The given id must belong to a committish on the walked
        ///   repository.
        ///  </para>
        /// </summary>
        /// <param name="walk">
        ///  the walker being used for the traversal.
        /// </param>
        /// <param name="id">the oid of the commit to start from.</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_revwalk_push(
            RevwalkHandle walk,
            ref git_oid id);

        /// <summary>
        ///  Push matching references
        ///
        ///  <para>
        ///   The OIDs pointed to by the references that match the
        ///   given glob pattern will be pushed to the revision
        ///   walker.
        ///  </para>
        ///
        ///  <para>
        ///   A leading 'refs/' is implied if not present as well as a
        ///   trailing '/ \ *' if the glob lacks '?', ' \ *' or '['.
        ///  </para>
        ///
        ///  <para>
        ///   Any references matching this glob which do not point to
        ///   a committish will be ignored.
        ///  </para>
        /// </summary>
        /// <param name="walk">
        ///  the walker being used for the traversal
        /// </param>
        /// <param name="glob">
        ///  the glob pattern references should match
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_revwalk_push_glob(
            RevwalkHandle walk,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string glob);

        /// <summary>
        ///  Push the repository's HEAD
        /// </summary>
        /// <param name="walk">
        ///  the walker being used for the traversal
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_revwalk_push_head(
            RevwalkHandle walk);

        /// <summary>
        ///  Mark a commit (and its ancestors) uninteresting for the
        ///  output.
        ///
        ///  <para>
        ///   The given id must belong to a committish on the walked
        ///   repository.
        ///  </para>
        ///
        ///  <para>
        ///   The resolved commit and all its parents will be hidden
        ///   from the output on the revision walk.
        ///  </para>
        /// </summary>
        /// <param name="walk">
        ///  the walker being used for the traversal.
        /// </param>
        /// <param name="commit_id">
        ///  the oid of commit that will be ignored during the traversal
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_revwalk_hide(
            RevwalkHandle walk,
            ref git_oid commit_id);

        /// <summary>
        ///  Hide matching references.
        ///
        ///  <para>
        ///   The OIDs pointed to by the references that match the
        ///   given glob pattern and their ancestors will be hidden
        ///   from the output on the revision walk.
        ///  </para>
        ///
        ///  <para>
        ///   A leading 'refs/' is implied if not present as well as a
        ///   trailing '/ \ *' if the glob lacks '?', ' \ *' or '['.
        ///  </para>
        ///
        ///  <para>
        ///   Any references matching this glob which do not point to
        ///   a committish will be ignored.
        ///  </para>
        /// </summary>
        /// <param name="walk">
        ///  the walker being used for the traversal
        /// </param>
        /// <param name="glob">
        ///  the glob pattern references should match
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_revwalk_hide_glob(
            RevwalkHandle walk,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string glob);

        /// <summary>
        ///  Hide the repository's HEAD
        /// </summary>
        /// <param name="walk">
        ///  the walker being used for the traversal
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_revwalk_hide_head(
            RevwalkHandle walk);

        /// <summary>
        ///  Push the OID pointed to by a reference
        ///
        ///  <para>
        ///   The reference must point to a committish.
        ///  </para>
        /// </summary>
        /// <param name="walk">
        ///  the walker being used for the traversal
        /// </param>
        /// <param name="refname">the reference to push</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_revwalk_push_ref(
            RevwalkHandle walk,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string refname);

        /// <summary>
        ///  Hide the OID pointed to by a reference
        ///
        ///  <para>
        ///   The reference must point to a committish.
        ///  </para>
        /// </summary>
        /// <param name="walk">
        ///  the walker being used for the traversal
        /// </param>
        /// <param name="refname">the reference to hide</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_revwalk_hide_ref(
            RevwalkHandle walk,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string refname);

        /// <summary>
        ///  Get the next commit from the revision walk.
        ///
        ///  <para>
        ///   The initial call to this method is *not* blocking when
        ///   iterating through a repo with a time-sorting mode.
        ///  </para>
        ///
        ///  <para>
        ///   Iterating with Topological or inverted modes makes the
        ///   initial call blocking to preprocess the commit list, but
        ///   this block should be mostly unnoticeable on most
        ///   repositories (topological preprocessing times at 0.3s on
        ///   the git.git repo).
        ///  </para>
        ///
        ///  <para>
        ///   The revision walker is reset when the walk is over.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Pointer where to store the oid of the next commit
        /// </param>
        /// <param name="walk">the walker to pop the commit from.</param>
        /// <returns>
        ///  0 if the next commit was found; GIT_ITEROVER if there are no
        ///  commits left to iterate
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_revwalk_next(
            ref git_oid _out,
            RevwalkHandle walk);

        /// <summary>
        ///  Change the sorting mode when iterating through the
        ///  repository's contents.
        ///
        ///  <para>
        ///   Changing the sorting mode resets the walker.
        ///  </para>
        /// </summary>
        /// <param name="walk">
        ///  the walker being used for the traversal.
        /// </param>
        /// <param name="sort_mode">
        ///  combination of GIT_SORT_XXX flags
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_revwalk_sorting(
            RevwalkHandle walk,
            uint sort_mode);

        /// <summary>
        ///  Set a git_pathspec object to filter commits on
        ///
        ///  <para>
        ///   Changing the pathspec rests the walker
        ///  </para>
        /// </summary>
        /// <param name="walk">
        ///  the walker being used for the traversal.
        /// </param>
        /// <param name="pathspec">Paths to filter commits on</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_revwalk_pathspec(
            RevwalkHandle walk,
            PathspecHandle pathspec);

        /// <summary>
        ///  Push and hide the respective endpoints of the given range.
        ///
        ///  <para>
        ///   The range should be of the form <commit>.. <commit>
        ///   where each <commit> is in the form accepted by
        ///   'git_revparse_single'. The left-hand commit will be
        ///   hidden and the right-hand commit pushed.
        ///  </para>
        /// </summary>
        /// <param name="walk">
        ///  the walker being used for the traversal
        /// </param>
        /// <param name="range">the range</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_revwalk_push_range(
            RevwalkHandle walk,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string range);

        /// <summary>
        ///  Simplify the history by first-parent
        ///
        ///  <para>
        ///   No parents other than the first for each commit will be
        ///   enqueued.
        ///  </para>
        /// </summary>
        /// <param name="walk">The revision walker.</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_revwalk_simplify_first_parent(
            RevwalkHandle walk);

        /// <summary>
        ///  Free a revision walker previously allocated.
        /// </summary>
        /// <param name="walk">
        ///  traversal handle to close. If NULL nothing occurs.
        /// </param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_revwalk_free(
            IntPtr walk);

        /// <summary>
        ///  Return the repository on which this walker is operating.
        /// </summary>
        /// <param name="walk">the revision walker</param>
        /// <returns>the repository being walked</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_repository* git_revwalk_repository(
            RevwalkHandle walk);

        /// <summary>
        ///  Adds, changes or removes a callback function to hide a
        ///  commit and its parents
        /// </summary>
        /// <param name="walk">the revision walker</param>
        /// <param name="hide_cb">
        ///  callback function to hide a commit and its parents
        /// </param>
        /// <param name="payload">
        ///  data payload to be passed to callback function
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_revwalk_add_hide_cb(
            RevwalkHandle walk,
            git_revwalk_hide_cb hide_cb,
            IntPtr payload);
    }
}
