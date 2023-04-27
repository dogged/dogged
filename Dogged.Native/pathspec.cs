namespace Dogged.Native
{
    /// <summary>
    /// Options controlling how pathspec match should be executed.
    /// </summary>
    public enum git_pathspec_flag_t
    {
        GIT_PATHSPEC_DEFAULT = 0,

        /// <summary>
        /// GIT_PATHSPEC_IGNORE_CASE forces match to ignore case; otherwise
        /// match will use native case sensitivity of platform filesystem
        /// </summary>
        GIT_PATHSPEC_IGNORE_CASE = (1 << 0),

        /// <summary>
        /// GIT_PATHSPEC_USE_CASE forces case sensitive match; otherwise
        /// match will use native case sensitivity of platform filesystem
        /// </summary>
        GIT_PATHSPEC_USE_CASE = (1 << 1),

        /// <summary>
        /// GIT_PATHSPEC_NO_GLOB disables glob patterns and just uses simple
	    /// string comparison for matching
        /// </summary>
        GIT_PATHSPEC_NO_GLOB = (1 << 2),

        /// <summary>
        /// GIT_PATHSPEC_NO_MATCH_ERROR means the match functions return error
        /// code GIT_ENOTFOUND if no matches are found; otherwise no matches is
        /// still success (return 0) but `git_pathspec_match_list_entrycount`
        /// will indicate 0 matches.
        /// </summary>
        GIT_PATHSPEC_NO_MATCH_ERROR = (1 << 3),

        /// <summary>
        /// GIT_PATHSPEC_FIND_FAILURES means that the `git_pathspec_match_list`
        /// should track which patterns matched which files so that at the end of
        /// the match we can identify patterns that did not match any files.
        /// </summary>
        GIT_PATHSPEC_FIND_FAILURES = (1 << 4),

        /// <summary>
        /// GIT_PATHSPEC_FAILURES_ONLY means that the `git_pathspec_match_list`
        /// does not need to keep the actual matching filenames.  Use this to
        /// just test if there were any matches at all or in combination with
        /// GIT_PATHSPEC_FIND_FAILURES to validate a pathspec.
        /// </summary>
        GIT_PATHSPEC_FAILURES_ONLY = (1 << 5),
    }

    /// <summary>
    /// Representation of a pathspec.
    /// </summary>
    public struct git_pathspec { }

    /// <summary>
    /// Representation of a pathspec match list.
    /// </summary>
    public struct git_pathspec_match_list { }
}
