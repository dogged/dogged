using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Stash flags
    /// </summary>
    public enum git_stash_flags
    {
        /// <summary>
        ///  No option, default
        /// </summary>
        GIT_STASH_DEFAULT = 0,

        /// <summary>
        ///  All changes already added to the index are left intact in
        ///  the working directory
        /// </summary>
        GIT_STASH_KEEP_INDEX = 1,

        /// <summary>
        ///  All untracked files are also stashed and then cleaned up
        ///  from the working directory
        /// </summary>
        GIT_STASH_INCLUDE_UNTRACKED = 2,

        /// <summary>
        ///  All ignored files are also stashed and then cleaned up from
        ///  the working directory
        /// </summary>
        GIT_STASH_INCLUDE_IGNORED = 4,

        /// <summary>
        ///  All changes in the index and working directory are left
        ///  intact
        /// </summary>
        GIT_STASH_KEEP_ALL = 8,
    }

    /// <summary>
    ///  Stash save options structure
    ///
    ///  <para>
    ///   Initialize with `GIT_STASH_SAVE_OPTIONS_INIT`.
    ///   Alternatively, you can use `git_stash_save_options_init`.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_stash_save_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_STASH_SAVE_OPTIONS_VERSION = 1;


        public uint version;

        /// <summary>
        ///  Flags to control the stashing process. (see GIT_STASH_*
        ///  above)
        /// </summary>
        public uint flags;

        /// <summary>
        ///  The identity of the person performing the stashing.
        /// </summary>
        public IntPtr stasher;

        /// <summary>
        ///  Optional description along with the stashed state.
        /// </summary>
        public IntPtr message;

        /// <summary>
        ///  Optional paths that control which files are stashed.
        /// </summary>
        public git_strarray paths;
    }

    /// <summary>
    ///  Stash application flags.
    /// </summary>
    public enum git_stash_apply_flags
    {
        GIT_STASH_APPLY_DEFAULT = 0,
        GIT_STASH_APPLY_REINSTATE_INDEX = 1,
    }

    /// <summary>
    ///  Stash apply progression states
    /// </summary>
    public enum git_stash_apply_progress_t
    {
        GIT_STASH_APPLY_PROGRESS_NONE = 0,

        /// <summary>
        ///  Loading the stashed data from the object database.
        /// </summary>
        GIT_STASH_APPLY_PROGRESS_LOADING_STASH,

        /// <summary>
        ///  The stored index is being analyzed.
        /// </summary>
        GIT_STASH_APPLY_PROGRESS_ANALYZE_INDEX,

        /// <summary>
        ///  The modified files are being analyzed.
        /// </summary>
        GIT_STASH_APPLY_PROGRESS_ANALYZE_MODIFIED,

        /// <summary>
        ///  The untracked and ignored files are being analyzed.
        /// </summary>
        GIT_STASH_APPLY_PROGRESS_ANALYZE_UNTRACKED,

        /// <summary>
        ///  The untracked files are being written to disk.
        /// </summary>
        GIT_STASH_APPLY_PROGRESS_CHECKOUT_UNTRACKED,

        /// <summary>
        ///  The modified files are being written to disk.
        /// </summary>
        GIT_STASH_APPLY_PROGRESS_CHECKOUT_MODIFIED,

        /// <summary>
        ///  The stash was applied successfully.
        /// </summary>
        GIT_STASH_APPLY_PROGRESS_DONE,
    }

    /// <summary>
    ///  Stash application progress notification function. Return 0 to
    ///  continue processing, or a negative value to abort the stash
    ///  application.
    /// </summary>
    /// <param name="progress">the progress information</param>
    /// <param name="payload">
    ///  the user-specified payload to the apply function
    /// </param>
    /// <returns>0 on success, -1 on error</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_stash_apply_progress_cb(
        git_stash_apply_progress_t progress,
        IntPtr payload);

    /// <summary>
    ///  Stash application options structure
    ///
    ///  <para>
    ///   Initialize with `GIT_STASH_APPLY_OPTIONS_INIT`.
    ///   Alternatively, you can use `git_stash_apply_options_init`.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_stash_apply_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_STASH_APPLY_OPTIONS_VERSION = 1;


        public uint version;

        /// <summary>
        ///  See `git_stash_apply_flags`, above.
        /// </summary>
        public uint flags;

        /// <summary>
        ///  Options to use when writing files to the working directory.
        /// </summary>
        public git_checkout_options checkout_options;

        /// <summary>
        ///  Optional callback to notify the consumer of application
        ///  progress.
        /// </summary>
        public git_stash_apply_progress_cb progress_cb;

        public IntPtr progress_payload;
    }

    /// <summary>
    ///  This is a callback function you can provide to iterate over all
    ///  the stashed states that will be invoked per entry.
    /// </summary>
    /// <param name="index">
    ///  The position within the stash list. 0 points to the most recent
    ///  stashed state.
    /// </param>
    /// <param name="message">The stash message.</param>
    /// <param name="stash_id">The commit oid of the stashed state.</param>
    /// <param name="payload">Extra parameter to callback function.</param>
    /// <returns>0 to continue iterating or non-zero to stop.</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_stash_cb(
        UIntPtr index,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string message,
        ref git_oid stash_id,
        IntPtr payload);

    public static partial class libgit2
    {
        /// <summary>
        ///  Save the local modifications to a new stash.
        /// </summary>
        /// <param name="_out">
        ///  Object id of the commit containing the stashed state. This
        ///  commit is also the target of the direct reference
        ///  refs/stash.
        /// </param>
        /// <param name="repo">The owning repository.</param>
        /// <param name="stasher">
        ///  The identity of the person performing the stashing.
        /// </param>
        /// <param name="message">
        ///  Optional description along with the stashed state.
        /// </param>
        /// <param name="flags">
        ///  Flags to control the stashing process. (see GIT_STASH_*
        ///  above)
        /// </param>
        /// <returns>
        ///  0 on success, GIT_ENOTFOUND where there's nothing to stash,
        ///  or error code.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_stash_save(
            ref git_oid _out,
            git_repository* repo,
            ref git_signature stasher,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string message,
            uint flags);

        /// <summary>
        ///  Initialize git_stash_save_options structure
        ///
        ///  <para>
        ///   Initializes a `git_stash_save_options` with default
        ///   values. Equivalent to creating an instance with
        ///   `GIT_STASH_SAVE_OPTIONS_INIT`.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_stash_save_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass `GIT_STASH_SAVE_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_stash_save_options_init(
            ref git_stash_save_options opts,
            uint version);

        /// <summary>
        ///  Save the local modifications to a new stash, with options.
        /// </summary>
        /// <param name="_out">
        ///  Object id of the commit containing the stashed state. This
        ///  commit is also the target of the direct reference
        ///  refs/stash.
        /// </param>
        /// <param name="repo">The owning repository.</param>
        /// <param name="opts">The stash options.</param>
        /// <returns>
        ///  0 on success, GIT_ENOTFOUND where there's nothing to stash,
        ///  or error code.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_stash_save_with_opts(
            ref git_oid _out,
            git_repository* repo,
            ref git_stash_save_options opts);

        /// <summary>
        ///  Initialize git_stash_apply_options structure
        ///
        ///  <para>
        ///   Initializes a `git_stash_apply_options` with default
        ///   values. Equivalent to creating an instance with
        ///   `GIT_STASH_APPLY_OPTIONS_INIT`.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_stash_apply_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass `GIT_STASH_APPLY_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_stash_apply_options_init(
            ref git_stash_apply_options opts,
            uint version);

        /// <summary>
        ///  Apply a single stashed state from the stash list.
        ///
        ///  <para>
        ///   If local changes in the working directory conflict with
        ///   changes in the stash then GIT_EMERGECONFLICT will be
        ///   returned. In this case, the index will always remain
        ///   unmodified and all files in the working directory will
        ///   remain unmodified. However, if you are restoring
        ///   untracked files or ignored files and there is a conflict
        ///   when applying the modified files, then those files will
        ///   remain in the working directory.
        ///  </para>
        ///
        ///  <para>
        ///   If passing the GIT_STASH_APPLY_REINSTATE_INDEX flag and
        ///   there would be conflicts when reinstating the index, the
        ///   function will return GIT_EMERGECONFLICT and both the
        ///   working directory and index will be left unmodified.
        ///  </para>
        /// </summary>
        /// <param name="repo">The owning repository.</param>
        /// <param name="index">
        ///  The position within the stash list. 0 points to the most
        ///  recent stashed state.
        /// </param>
        /// <param name="options">
        ///  Optional options to control how stashes are applied.
        /// </param>
        /// <returns>
        ///  0 on success, GIT_ENOTFOUND if there's no stashed state for
        ///  the given index, GIT_EMERGECONFLICT if changes exist in the
        ///  working directory, or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_stash_apply(
            git_repository* repo,
            UIntPtr index,
            ref git_stash_apply_options options);

        /// <summary>
        ///  Loop over all the stashed states and issue a callback for
        ///  each one.
        ///
        ///  <para>
        ///   If the callback returns a non-zero value, this will stop
        ///   looping.
        ///  </para>
        /// </summary>
        /// <param name="repo">Repository where to find the stash.</param>
        /// <param name="callback">
        ///  Callback to invoke per found stashed state. The most recent
        ///  stash state will be enumerated first.
        /// </param>
        /// <param name="payload">
        ///  Extra parameter to callback function.
        /// </param>
        /// <returns>
        ///  0 on success, non-zero callback return value, or error code.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_stash_foreach(
            git_repository* repo,
            git_stash_cb callback,
            IntPtr payload);

        /// <summary>
        ///  Remove a single stashed state from the stash list.
        /// </summary>
        /// <param name="repo">The owning repository.</param>
        /// <param name="index">
        ///  The position within the stash list. 0 points to the most
        ///  recent stashed state.
        /// </param>
        /// <returns>
        ///  0 on success, GIT_ENOTFOUND if there's no stashed state for
        ///  the given index, or error code.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_stash_drop(
            git_repository* repo,
            UIntPtr index);

        /// <summary>
        ///  Apply a single stashed state from the stash list and remove
        ///  it from the list if successful.
        /// </summary>
        /// <param name="repo">The owning repository.</param>
        /// <param name="index">
        ///  The position within the stash list. 0 points to the most
        ///  recent stashed state.
        /// </param>
        /// <param name="options">
        ///  Optional options to control how stashes are applied.
        /// </param>
        /// <returns>
        ///  0 on success, GIT_ENOTFOUND if there's no stashed state for
        ///  the given index, or error code. (see git_stash_apply() above
        ///  for details)
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_stash_pop(
            git_repository* repo,
            UIntPtr index,
            ref git_stash_apply_options options);
    }
}
