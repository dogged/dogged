using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Reference lookup strategy
    ///
    ///  <para>
    ///   These behave like the --tags and --all options to
    ///   git-describe, namely they say to look for any reference in
    ///   either refs/tags/ or refs/ respectively.
    ///  </para>
    /// </summary>
    public enum git_describe_strategy_t
    {
        GIT_DESCRIBE_DEFAULT,
        GIT_DESCRIBE_TAGS,
        GIT_DESCRIBE_ALL,
    }

    /// <summary>
    ///  Describe options structure
    ///
    ///  <para>
    ///   Initialize with `GIT_DESCRIBE_OPTIONS_INIT`. Alternatively,
    ///   you can use `git_describe_options_init`.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_describe_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_DESCRIBE_OPTIONS_VERSION = 1;


        public uint version;

        /// <summary>
        ///  default: 10
        /// </summary>
        public uint max_candidates_tags;

        /// <summary>
        ///  default: GIT_DESCRIBE_DEFAULT
        /// </summary>
        public uint describe_strategy;

        public IntPtr pattern;

        /// <summary>
        ///  When calculating the distance from the matching tag or
        ///  reference, only walk down the first-parent ancestry.
        /// </summary>
        public int only_follow_first_parent;

        /// <summary>
        ///  If no matching tag or reference is found, the describe
        ///  operation would normally fail. If this option is set, it
        ///  will instead fall back to showing the full id of the commit.
        /// </summary>
        public int show_commit_oid_as_fallback;
    }

    /// <summary>
    ///  Describe format options structure
    ///
    ///  <para>
    ///   Initialize with `GIT_DESCRIBE_FORMAT_OPTIONS_INIT`.
    ///   Alternatively, you can use
    ///   `git_describe_format_options_init`.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_describe_format_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_DESCRIBE_FORMAT_OPTIONS_VERSION = 1;


        public uint version;

        /// <summary>
        ///  Size of the abbreviated commit id to use. This value is the
        ///  lower bound for the length of the abbreviated string. The
        ///  default is 7.
        /// </summary>
        public uint abbreviated_size;

        /// <summary>
        ///  Set to use the long format even when a shorter name could be
        ///  used.
        /// </summary>
        public int always_use_long_format;

        /// <summary>
        ///  If the workdir is dirty and this is set, this string will be
        ///  appended to the description string.
        /// </summary>
        public IntPtr dirty_suffix;
    }

    /// <summary>
    ///  A struct that stores the result of a describe operation.
    /// </summary>
    public struct git_describe_result
    {
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Initialize git_describe_options structure
        ///
        ///  <para>
        ///   Initializes a `git_describe_options` with default
        ///   values. Equivalent to creating an instance with
        ///   GIT_DESCRIBE_OPTIONS_INIT.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_describe_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass `GIT_DESCRIBE_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_describe_options_init(
            ref git_describe_options opts,
            uint version);

        /// <summary>
        ///  Initialize git_describe_format_options structure
        ///
        ///  <para>
        ///   Initializes a `git_describe_format_options` with default
        ///   values. Equivalent to creating an instance with
        ///   GIT_DESCRIBE_FORMAT_OPTIONS_INIT.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_describe_format_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass
        ///  `GIT_DESCRIBE_FORMAT_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_describe_format_options_init(
            ref git_describe_format_options opts,
            uint version);

        /// <summary>
        ///  Describe a commit
        ///
        ///  <para>
        ///   Perform the describe operation on the given committish
        ///   object.
        ///  </para>
        /// </summary>
        /// <param name="result">
        ///  pointer to store the result. You must free this once you're
        ///  done with it.
        /// </param>
        /// <param name="committish">a committish to describe</param>
        /// <param name="opts">
        ///  the lookup options (or NULL for defaults)
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_describe_commit(
            out git_describe_result* result,
            git_object* committish,
            ref git_describe_options opts);

        /// <summary>
        ///  Describe a commit
        ///
        ///  <para>
        ///   Perform the describe operation on the current commit and
        ///   the worktree. After performing describe on HEAD, a
        ///   status is run and the description is considered to be
        ///   dirty if there are.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  pointer to store the result. You must free this once you're
        ///  done with it.
        /// </param>
        /// <param name="repo">
        ///  the repository in which to perform the describe
        /// </param>
        /// <param name="opts">
        ///  the lookup options (or NULL for defaults)
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_describe_workdir(
            out git_describe_result* _out,
            git_repository* repo,
            ref git_describe_options opts);

        /// <summary>
        ///  Print the describe result to a buffer
        /// </summary>
        /// <param name="_out">The buffer to store the result</param>
        /// <param name="result">
        ///  the result from `git_describe_commit()` or
        ///  `git_describe_workdir()`.
        /// </param>
        /// <param name="opts">
        ///  the formatting options (or NULL for defaults)
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_describe_format(
            ref git_buf _out,
            git_describe_result* result,
            ref git_describe_format_options opts);

        /// <summary>
        ///  Free the describe result.
        /// </summary>
        /// <param name="result">The result to free.</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_describe_result_free(
            git_describe_result* result);
    }
}
