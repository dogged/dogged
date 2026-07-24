using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Formatting options for diff e-mail generation
    /// </summary>
    public enum git_email_create_flags_t
    {
        /// <summary>
        ///  Normal patch, the default
        /// </summary>
        GIT_EMAIL_CREATE_DEFAULT = 0,

        /// <summary>
        ///  Do not include patch numbers in the subject prefix.
        /// </summary>
        GIT_EMAIL_CREATE_OMIT_NUMBERS = 1,

        /// <summary>
        ///  Include numbers in the subject prefix even when the patch is
        ///  for a single commit (1/1).
        /// </summary>
        GIT_EMAIL_CREATE_ALWAYS_NUMBER = 2,

        /// <summary>
        ///  Do not perform rename or similarity detection.
        /// </summary>
        GIT_EMAIL_CREATE_NO_RENAMES = 4,
    }

    /// <summary>
    ///  Options for controlling the formatting of the generated e-mail.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_email_create_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_EMAIL_CREATE_OPTIONS_VERSION = 1;


        public uint version;

        /// <summary>
        ///  see `git_email_create_flags_t` above
        /// </summary>
        public uint flags;

        /// <summary>
        ///  Options to use when creating diffs
        /// </summary>
        public git_diff_options diff_opts;

        /// <summary>
        ///  Options for finding similarities within diffs
        /// </summary>
        public git_diff_find_options diff_find_opts;

        /// <summary>
        ///  The subject prefix, by default "PATCH". If set to an empty
        ///  string ("") then only the patch numbers will be shown in the
        ///  prefix. If the subject_prefix is empty and patch numbers are
        ///  not being shown, the prefix will be omitted entirely.
        /// </summary>
        public IntPtr subject_prefix;

        /// <summary>
        ///  The starting patch number; this cannot be 0. By default,
        ///  this is 1.
        /// </summary>
        public UIntPtr start_number;

        /// <summary>
        ///  The "re-roll" number. By default, there is no re-roll.
        /// </summary>
        public UIntPtr reroll_number;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Initialize a `git_email_create_options` with default values.
        ///  Equivalent to creating an instance with
        ///  GIT_EMAIL_CREATE_OPTIONS_INIT.
        /// </summary>
        /// <param name="opts">
        ///  The `git_email_create_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass `GIT_EMAIL_CREATE_OPTIONS_VERSION`
        /// </param>
        /// <returns>0 on success or -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_email_create_options_init(
            ref git_email_create_options opts,
            uint version);

        /// <summary>
        ///  Create a diff for a commit in mbox format for sending via
        ///  email. The commit must not be a merge commit.
        /// </summary>
        /// <param name="_out">buffer to store the e-mail patch in</param>
        /// <param name="commit">commit to create a patch for</param>
        /// <param name="opts">email creation options</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_email_create_from_commit(
            ref git_buf _out,
            git_commit* commit,
            ref git_email_create_options opts);
    }
}
