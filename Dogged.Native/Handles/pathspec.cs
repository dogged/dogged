using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public sealed class PathspecHandle : SafeHandle
    {
        public PathspecHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe PathspecHandle(git_pathspec* nativePathspec) :
            base((IntPtr)nativePathspec, true)
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
            libgit2.git_pathspec_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public sealed class PathspecMatchListHandle : SafeHandle
    {
        public PathspecMatchListHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe PathspecMatchListHandle(git_pathspec_match_list* nativePathspecMatchList) :
            base((IntPtr)nativePathspecMatchList, true)
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
            libgit2.git_pathspec_match_list_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Compile a pathspec
        /// </summary>
        /// <param name="_out">Output of the compiled pathspec</param>
        /// <param name="pathspec">
        ///  A git_strarray of the paths to match
        /// </param>
        /// <returns>0 on success, < 0 on failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_pathspec_new(
            out PathspecHandle _out,
            ref git_strarray pathspec);

        /// <summary>
        ///  Free a pathspec
        /// </summary>
        /// <param name="ps">The compiled pathspec</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_pathspec_free(
            IntPtr ps);

        /// <summary>
        ///  Try to match a path against a pathspec
        ///
        ///  <para>
        ///   Unlike most of the other pathspec matching functions,
        ///   this will not fall back on the native case-sensitivity
        ///   for your platform. You must explicitly pass flags to
        ///   control case sensitivity or else this will fall back on
        ///   being case sensitive.
        ///  </para>
        /// </summary>
        /// <param name="ps">The compiled pathspec</param>
        /// <param name="flags">
        ///  Combination of git_pathspec_flag_t options to control match
        /// </param>
        /// <param name="path">The pathname to attempt to match</param>
        /// <returns>1 is path matches spec, 0 if it does not</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_pathspec_matches_path(
            PathspecHandle ps,
            uint flags,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path);

        /// <summary>
        ///  Match a pathspec against the working directory of a
        ///  repository.
        ///
        ///  <para>
        ///   This matches the pathspec against the current files in
        ///   the working directory of the repository. It is an error
        ///   to invoke this on a bare repo. This handles git ignores
        ///   (i.e. ignored files will not be considered to match the
        ///   `pathspec` unless the file is tracked in the index).
        ///  </para>
        ///
        ///  <para>
        ///   If `out` is not NULL, this returns a
        ///   `git_patchspec_match_list`. That contains the list of
        ///   all matched filenames (unless you pass the
        ///   `GIT_PATHSPEC_FAILURES_ONLY` flag) and may also contain
        ///   the list of pathspecs with no match (if you used the
        ///   `GIT_PATHSPEC_FIND_FAILURES` flag). You must call
        ///   `git_pathspec_match_list_free()` on this object.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Output list of matches; pass NULL to just get return value
        /// </param>
        /// <param name="repo">
        ///  The repository in which to match; bare repo is an error
        /// </param>
        /// <param name="flags">
        ///  Combination of git_pathspec_flag_t options to control match
        /// </param>
        /// <param name="ps">Pathspec to be matched</param>
        /// <returns>
        ///  0 on success, -1 on error, GIT_ENOTFOUND if no matches and
        ///  the GIT_PATHSPEC_NO_MATCH_ERROR flag was given
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_pathspec_match_workdir(
            out PathspecMatchListHandle _out,
            RepositoryHandle repo,
            uint flags,
            PathspecHandle ps);

        /// <summary>
        ///  Match a pathspec against entries in an index.
        ///
        ///  <para>
        ///   This matches the pathspec against the files in the
        ///   repository index.
        ///  </para>
        ///
        ///  <para>
        ///   NOTE: At the moment, the case sensitivity of this match
        ///   is controlled by the current case-sensitivity of the
        ///   index object itself and the USE_CASE and IGNORE_CASE
        ///   flags will have no effect. This behavior will be
        ///   corrected in a future release.
        ///  </para>
        ///
        ///  <para>
        ///   If `out` is not NULL, this returns a
        ///   `git_patchspec_match_list`. That contains the list of
        ///   all matched filenames (unless you pass the
        ///   `GIT_PATHSPEC_FAILURES_ONLY` flag) and may also contain
        ///   the list of pathspecs with no match (if you used the
        ///   `GIT_PATHSPEC_FIND_FAILURES` flag). You must call
        ///   `git_pathspec_match_list_free()` on this object.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Output list of matches; pass NULL to just get return value
        /// </param>
        /// <param name="index">The index to match against</param>
        /// <param name="flags">
        ///  Combination of git_pathspec_flag_t options to control match
        /// </param>
        /// <param name="ps">Pathspec to be matched</param>
        /// <returns>
        ///  0 on success, -1 on error, GIT_ENOTFOUND if no matches and
        ///  the GIT_PATHSPEC_NO_MATCH_ERROR flag is used
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_pathspec_match_index(
            out PathspecMatchListHandle _out,
            IndexHandle index,
            uint flags,
            PathspecHandle ps);

        /// <summary>
        ///  Match a pathspec against files in a tree.
        ///
        ///  <para>
        ///   This matches the pathspec against the files in the given
        ///   tree.
        ///  </para>
        ///
        ///  <para>
        ///   If `out` is not NULL, this returns a
        ///   `git_patchspec_match_list`. That contains the list of
        ///   all matched filenames (unless you pass the
        ///   `GIT_PATHSPEC_FAILURES_ONLY` flag) and may also contain
        ///   the list of pathspecs with no match (if you used the
        ///   `GIT_PATHSPEC_FIND_FAILURES` flag). You must call
        ///   `git_pathspec_match_list_free()` on this object.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Output list of matches; pass NULL to just get return value
        /// </param>
        /// <param name="tree">The root-level tree to match against</param>
        /// <param name="flags">
        ///  Combination of git_pathspec_flag_t options to control match
        /// </param>
        /// <param name="ps">Pathspec to be matched</param>
        /// <returns>
        ///  0 on success, -1 on error, GIT_ENOTFOUND if no matches and
        ///  the GIT_PATHSPEC_NO_MATCH_ERROR flag is used
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_pathspec_match_tree(
            out PathspecMatchListHandle _out,
            TreeHandle tree,
            uint flags,
            PathspecHandle ps);

        /// <summary>
        ///  Match a pathspec against files in a diff list.
        ///
        ///  <para>
        ///   This matches the pathspec against the files in the given
        ///   diff list.
        ///  </para>
        ///
        ///  <para>
        ///   If `out` is not NULL, this returns a
        ///   `git_patchspec_match_list`. That contains the list of
        ///   all matched filenames (unless you pass the
        ///   `GIT_PATHSPEC_FAILURES_ONLY` flag) and may also contain
        ///   the list of pathspecs with no match (if you used the
        ///   `GIT_PATHSPEC_FIND_FAILURES` flag). You must call
        ///   `git_pathspec_match_list_free()` on this object.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Output list of matches; pass NULL to just get return value
        /// </param>
        /// <param name="diff">A generated diff list</param>
        /// <param name="flags">
        ///  Combination of git_pathspec_flag_t options to control match
        /// </param>
        /// <param name="ps">Pathspec to be matched</param>
        /// <returns>
        ///  0 on success, -1 on error, GIT_ENOTFOUND if no matches and
        ///  the GIT_PATHSPEC_NO_MATCH_ERROR flag is used
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_pathspec_match_diff(
            out PathspecMatchListHandle _out,
            DiffHandle diff,
            uint flags,
            PathspecHandle ps);

        /// <summary>
        ///  Free memory associates with a git_pathspec_match_list
        /// </summary>
        /// <param name="m">The git_pathspec_match_list to be freed</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_pathspec_match_list_free(
            IntPtr m);

        /// <summary>
        ///  Get the number of items in a match list.
        /// </summary>
        /// <param name="m">The git_pathspec_match_list object</param>
        /// <returns>Number of items in match list</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_pathspec_match_list_entrycount(
            PathspecMatchListHandle m);

        /// <summary>
        ///  Get a matching filename by position.
        ///
        ///  <para>
        ///   This routine cannot be used if the match list was
        ///   generated by `git_pathspec_match_diff`. If so, it will
        ///   always return NULL.
        ///  </para>
        /// </summary>
        /// <param name="m">The git_pathspec_match_list object</param>
        /// <param name="pos">The index into the list</param>
        /// <returns>The filename of the match</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_pathspec_match_list_entry(
            PathspecMatchListHandle m,
            UIntPtr pos);

        /// <summary>
        ///  Get a matching diff delta by position.
        ///
        ///  <para>
        ///   This routine can only be used if the match list was
        ///   generated by `git_pathspec_match_diff`. Otherwise it
        ///   will always return NULL.
        ///  </para>
        /// </summary>
        /// <param name="m">The git_pathspec_match_list object</param>
        /// <param name="pos">The index into the list</param>
        /// <returns>The filename of the match</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_diff_delta* git_pathspec_match_list_diff_entry(
            PathspecMatchListHandle m,
            UIntPtr pos);

        /// <summary>
        ///  Get the number of pathspec items that did not match.
        ///
        ///  <para>
        ///   This will be zero unless you passed
        ///   GIT_PATHSPEC_FIND_FAILURES when generating the
        ///   git_pathspec_match_list.
        ///  </para>
        /// </summary>
        /// <param name="m">The git_pathspec_match_list object</param>
        /// <returns>
        ///  Number of items in original pathspec that had no matches
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_pathspec_match_list_failed_entrycount(
            PathspecMatchListHandle m);

        /// <summary>
        ///  Get an original pathspec string that had no matches.
        ///
        ///  <para>
        ///   This will be return NULL for positions out of range.
        ///  </para>
        /// </summary>
        /// <param name="m">The git_pathspec_match_list object</param>
        /// <param name="pos">The index into the failed items</param>
        /// <returns>
        ///  The pathspec pattern that didn't match anything
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_pathspec_match_list_failed_entry(
            PathspecMatchListHandle m,
            UIntPtr pos);
    }
}
