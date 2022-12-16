using Dogged.Native;

namespace Dogged
{
    public enum PathSpecFlags
    {
        Default = git_pathspec_flag_t.GIT_PATHSPEC_DEFAULT,

        /// <summary>
        /// Forces match to ignore case; otherwise match will use native case sensitivity of platform filesystem.
        /// </summary>
        IgnoreCase = git_pathspec_flag_t.GIT_PATHSPEC_IGNORE_CASE,

        /// <summary>
        /// Forces case sensitive match; otherwise match will use native case sensitivity of platform filesystem.
        /// </summary>
        UseCase = git_pathspec_flag_t.GIT_PATHSPEC_USE_CASE,

        /// <summary>
        /// Disables glob patterns and just uses simple string comparison for matching.
        /// </summary>
        NoGlob = git_pathspec_flag_t.GIT_PATHSPEC_NO_GLOB,

        /// <summary>
        /// Means the match functions return error code GIT_ENOTFOUND if no matches are found;
        /// otherwise no matches is still success (return 0) but `git_pathspec_match_list_entrycount`
        /// will indicate 0 matches.
        /// </summary>
        NoMatchError = git_pathspec_flag_t.GIT_PATHSPEC_NO_MATCH_ERROR,

        /// <summary>
        /// Indicates that the `git_pathspec_match_list` should track which patterns matched
        /// which files so that at the end of the match we can identify patterns that did not
        /// match any files.
        /// </summary>
        FindFailures = git_pathspec_flag_t.GIT_PATHSPEC_FIND_FAILURES,

        /// <summary>
        /// Indicates that the `git_pathspec_match_list` does not need to keep the actual
        /// matching filenames. Use this to just test if there were any matches at all or
        /// in combination with GIT_PATHSPEC_FIND_FAILURES to validate a pathspec.
        /// </summary>
        FailuresOnly = git_pathspec_flag_t.GIT_PATHSPEC_FAILURES_ONLY,
    }

    /// <summary>
    /// Representation of a pathspec.
    /// </summary>
    public unsafe class PathSpec : NativeDisposable
    {
        private git_pathspec* nativePathspec;

        private PathSpec(git_pathspec* nativePathspec)
        {
            Ensure.NativePointerNotNull(nativePathspec);
            this.nativePathspec = nativePathspec;
        }

        public unsafe static PathSpec Create(params string[] paths)
        {
            Ensure.ArgumentNotNull(paths, "paths");

            git_pathspec* nativePathspec;
            Ensure.NativeSuccess(libgit2.git_pathspec_new(out nativePathspec, paths));
            return new PathSpec(nativePathspec);
        }

        internal git_pathspec* NativePathspec
        {
            get
            {
                return nativePathspec;
            }
        }

        internal unsafe override bool IsDisposed
        {
            get
            {
                return (nativePathspec == null);
            }
        }

        internal unsafe override void Dispose(bool disposing)
        {
            if (nativePathspec != null)
            {
                libgit2.git_pathspec_free(nativePathspec);
                nativePathspec = null;
            }
        }
    }
}
