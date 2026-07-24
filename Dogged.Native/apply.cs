using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  When applying a patch, callback that will be made per delta
    ///  (file).
    ///
    ///  <para>
    ///   When the callback: - returns < 0, the apply process will be
    ///   aborted. - returns > 0, the delta will not be applied, but
    ///   the apply process continues - returns 0, the delta is
    ///   applied, and the apply process continues.
    ///  </para>
    /// </summary>
    /// <param name="delta">The delta to be applied</param>
    /// <param name="payload">User-specified payload</param>
    /// <returns>
    ///  0 if the delta is applied, < 0 if the apply process will be
    ///  aborted or > 0 if the delta will not be applied.
    /// </returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_apply_delta_cb(
        ref git_diff_delta delta,
        IntPtr payload);

    /// <summary>
    ///  When applying a patch, callback that will be made per hunk.
    ///
    ///  <para>
    ///   When the callback: - returns < 0, the apply process will be
    ///   aborted. - returns > 0, the hunk will not be applied, but
    ///   the apply process continues - returns 0, the hunk is
    ///   applied, and the apply process continues.
    ///  </para>
    /// </summary>
    /// <param name="hunk">The hunk to be applied</param>
    /// <param name="payload">User-specified payload</param>
    /// <returns>
    ///  0 if the hunk is applied, < 0 if the apply process will be
    ///  aborted or > 0 if the hunk will not be applied.
    /// </returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_apply_hunk_cb(
        ref git_diff_hunk hunk,
        IntPtr payload);

    /// <summary>
    ///  Flags controlling the behavior of `git_apply`.
    ///
    ///  <para>
    ///   When the callback: - returns < 0, the apply process will be
    ///   aborted. - returns > 0, the hunk will not be applied, but
    ///   the apply process continues - returns 0, the hunk is
    ///   applied, and the apply process continues.
    ///  </para>
    /// </summary>
    public enum git_apply_flags_t
    {
        /// <summary>
        ///  Don't actually make changes, just test that the patch
        ///  applies. This is the equivalent of `git apply --check`.
        /// </summary>
        GIT_APPLY_CHECK = 1,
    }

    /// <summary>
    ///  Apply options structure.
    ///
    ///  <para>
    ///   When the callback: - returns < 0, the apply process will be
    ///   aborted. - returns > 0, the hunk will not be applied, but
    ///   the apply process continues - returns 0, the hunk is
    ///   applied, and the apply process continues.
    ///  </para>
    ///
    ///  <para>
    ///   Initialize with `GIT_APPLY_OPTIONS_INIT`. Alternatively, you
    ///   can use `git_apply_options_init`.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_apply_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_APPLY_OPTIONS_VERSION = 1;


        /// <summary>
        ///  The version
        /// </summary>
        public uint version;

        /// <summary>
        ///  When applying a patch, callback that will be made per delta
        ///  (file).
        /// </summary>
        public git_apply_delta_cb delta_cb;

        /// <summary>
        ///  When applying a patch, callback that will be made per hunk.
        /// </summary>
        public git_apply_hunk_cb hunk_cb;

        /// <summary>
        ///  Payload passed to both `delta_cb` & `hunk_cb`.
        /// </summary>
        public IntPtr payload;

        /// <summary>
        ///  Bitmask of `git_apply_flags_t`
        /// </summary>
        public uint flags;
    }

    /// <summary>
    ///  Possible application locations for git_apply
    /// </summary>
    public enum git_apply_location_t
    {
        /// <summary>
        ///  Apply the patch to the workdir, leaving the index untouched.
        ///  This is the equivalent of `git apply` with no location
        ///  argument.
        /// </summary>
        GIT_APPLY_LOCATION_WORKDIR = 0,

        /// <summary>
        ///  Apply the patch to the index, leaving the working directory
        ///  untouched. This is the equivalent of `git apply --cached`.
        /// </summary>
        GIT_APPLY_LOCATION_INDEX = 1,

        /// <summary>
        ///  Apply the patch to both the working directory and the index.
        ///  This is the equivalent of `git apply --index`.
        /// </summary>
        GIT_APPLY_LOCATION_BOTH = 2,
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Initialize git_apply_options structure
        ///
        ///  <para>
        ///   Initialize a `git_apply_options` with default values.
        ///   Equivalent to creating an instance with
        ///   GIT_APPLY_OPTIONS_INIT.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_apply_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass `GIT_APPLY_OPTIONS_VERSION`
        /// </param>
        /// <returns>0 on success or -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_apply_options_init(
            ref git_apply_options opts,
            uint version);

        /// <summary>
        ///  Apply a `git_diff` to a `git_tree`, and return the resulting
        ///  image as an index.
        /// </summary>
        /// <param name="_out">the postimage of the application</param>
        /// <param name="repo">the repository to apply</param>
        /// <param name="preimage">the tree to apply the diff to</param>
        /// <param name="diff">the diff to apply</param>
        /// <param name="options">
        ///  the options for the apply (or null for defaults)
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_apply_to_tree(
            out git_index* _out,
            git_repository* repo,
            git_tree* preimage,
            git_diff* diff,
            ref git_apply_options options);

        /// <summary>
        ///  Apply a `git_diff` to the given repository, making changes
        ///  directly in the working directory, the index, or both.
        /// </summary>
        /// <param name="repo">the repository to apply to</param>
        /// <param name="diff">the diff to apply</param>
        /// <param name="location">
        ///  the location to apply (workdir, index or both)
        /// </param>
        /// <param name="options">
        ///  the options for the apply (or null for defaults)
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_apply(
            git_repository* repo,
            git_diff* diff,
            git_apply_location_t location,
            ref git_apply_options options);
    }
}
