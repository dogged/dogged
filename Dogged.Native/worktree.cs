using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Representation of a working tree
    /// </summary>
    public struct git_worktree
    {
    }

    /// <summary>
    ///  Worktree add options structure
    ///
    ///  <para>
    ///   Initialize with `GIT_WORKTREE_ADD_OPTIONS_INIT`.
    ///   Alternatively, you can use `git_worktree_add_options_init`.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_worktree_add_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_WORKTREE_ADD_OPTIONS_VERSION = 1;


        public uint version;

        /// <summary>
        ///  lock newly created worktree
        /// </summary>
        public int should_lock;

        /// <summary>
        ///  allow checkout of existing branch matching worktree name
        /// </summary>
        public int checkout_existing;

        /// <summary>
        ///  reference to use for the new worktree HEAD
        /// </summary>
        public IntPtr ref_name;

        /// <summary>
        ///  Options for the checkout.
        /// </summary>
        public git_checkout_options checkout_options;
    }

    /// <summary>
    ///  Flags which can be passed to git_worktree_prune to alter its
    ///  behavior.
    /// </summary>
    public enum git_worktree_prune_t
    {
        GIT_WORKTREE_PRUNE_VALID = 1,
        GIT_WORKTREE_PRUNE_LOCKED = 2,
        GIT_WORKTREE_PRUNE_WORKING_TREE = 4,
    }

    /// <summary>
    ///  Worktree prune options structure
    ///
    ///  <para>
    ///   Initialize with `GIT_WORKTREE_PRUNE_OPTIONS_INIT`.
    ///   Alternatively, you can use
    ///   `git_worktree_prune_options_init`.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_worktree_prune_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_WORKTREE_PRUNE_OPTIONS_VERSION = 1;


        public uint version;

        /// <summary>
        ///  A combination of `git_worktree_prune_t`
        /// </summary>
        public uint flags;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  List names of linked working trees
        ///
        ///  <para>
        ///   The returned list should be released with
        ///   `git_strarray_free` when no longer needed.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  pointer to the array of working tree names
        /// </param>
        /// <param name="repo">
        ///  the repo to use when listing working trees
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_worktree_list(
            ref git_strarray _out,
            git_repository* repo);

        /// <summary>
        ///  Lookup a working tree by its name for a given repository
        /// </summary>
        /// <param name="_out">
        ///  Output pointer to looked up worktree or `NULL`
        /// </param>
        /// <param name="repo">The repository containing worktrees</param>
        /// <param name="name">Name of the working tree to look up</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_worktree_lookup(
            out git_worktree* _out,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Open a worktree of a given repository
        ///
        ///  <para>
        ///   If a repository is not the main tree but a worktree,
        ///   this function will look up the worktree inside the
        ///   parent repository and create a new `git_worktree`
        ///   structure.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Out-pointer for the newly allocated worktree
        /// </param>
        /// <param name="repo">Repository to look up worktree for</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_worktree_open_from_repository(
            out git_worktree* _out,
            git_repository* repo);

        /// <summary>
        ///  Free a previously allocated worktree
        /// </summary>
        /// <param name="wt">
        ///  worktree handle to close. If NULL nothing occurs.
        /// </param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_worktree_free(
            git_worktree* wt);

        /// <summary>
        ///  Check if worktree is valid
        ///
        ///  <para>
        ///   A valid worktree requires both the git data structures
        ///   inside the linked parent repository and the linked
        ///   working copy to be present.
        ///  </para>
        /// </summary>
        /// <param name="wt">Worktree to check</param>
        /// <returns>
        ///  0 when worktree is valid, error-code otherwise
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_worktree_validate(
            git_worktree* wt);

        /// <summary>
        ///  Initialize git_worktree_add_options structure
        ///
        ///  <para>
        ///   Initializes a `git_worktree_add_options` with default
        ///   values. Equivalent to creating an instance with
        ///   `GIT_WORKTREE_ADD_OPTIONS_INIT`.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_worktree_add_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass `GIT_WORKTREE_ADD_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_worktree_add_options_init(
            ref git_worktree_add_options opts,
            uint version);

        /// <summary>
        ///  Add a new working tree
        ///
        ///  <para>
        ///   Add a new working tree for the repository, that is
        ///   create the required data structures inside the
        ///   repository and check out the current HEAD at `path`
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Output pointer containing new working tree
        /// </param>
        /// <param name="repo">Repository to create working tree for</param>
        /// <param name="name">Name of the working tree</param>
        /// <param name="path">Path to create working tree at</param>
        /// <param name="opts">
        ///  Options to modify default behavior. May be NULL
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_worktree_add(
            out git_worktree* _out,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            ref git_worktree_add_options opts);

        /// <summary>
        ///  Lock worktree if not already locked
        ///
        ///  <para>
        ///   Lock a worktree, optionally specifying a reason why the
        ///   linked working tree is being locked.
        ///  </para>
        /// </summary>
        /// <param name="wt">Worktree to lock</param>
        /// <param name="reason">
        ///  Reason why the working tree is being locked
        /// </param>
        /// <returns>0 on success, non-zero otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_worktree_lock(
            git_worktree* wt,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string reason);

        /// <summary>
        ///  Unlock a locked worktree
        /// </summary>
        /// <param name="wt">Worktree to unlock</param>
        /// <returns>
        ///  0 on success, 1 if worktree was not locked, error-code
        ///  otherwise
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_worktree_unlock(
            git_worktree* wt);

        /// <summary>
        ///  Check if worktree is locked
        ///
        ///  <para>
        ///   A worktree may be locked if the linked working tree is
        ///   stored on a portable device which is not available.
        ///  </para>
        /// </summary>
        /// <param name="reason">
        ///  Buffer to store reason in. If NULL no reason is stored.
        /// </param>
        /// <param name="wt">Worktree to check</param>
        /// <returns>
        ///  0 when the working tree not locked, a value greater than
        ///  zero if it is locked, less than zero if there was an error
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_worktree_is_locked(
            ref git_buf reason,
            git_worktree* wt);

        /// <summary>
        ///  Retrieve the name of the worktree
        /// </summary>
        /// <param name="wt">Worktree to get the name for</param>
        /// <returns>
        ///  The worktree's name. The pointer returned is valid for the
        ///  lifetime of the git_worktree
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_worktree_name(
            git_worktree* wt);

        /// <summary>
        ///  Retrieve the filesystem path for the worktree
        /// </summary>
        /// <param name="wt">Worktree to get the path for</param>
        /// <returns>
        ///  The worktree's filesystem path. The pointer returned is
        ///  valid for the lifetime of the git_worktree.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_worktree_path(
            git_worktree* wt);

        /// <summary>
        ///  Initialize git_worktree_prune_options structure
        ///
        ///  <para>
        ///   Initializes a `git_worktree_prune_options` with default
        ///   values. Equivalent to creating an instance with
        ///   `GIT_WORKTREE_PRUNE_OPTIONS_INIT`.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_worktree_prune_options` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass
        ///  `GIT_WORKTREE_PRUNE_OPTIONS_VERSION`.
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_worktree_prune_options_init(
            ref git_worktree_prune_options opts,
            uint version);

        /// <summary>
        ///  Is the worktree prunable with the given options?
        ///
        ///  <para>
        ///   A worktree is not prunable in the following scenarios:
        ///  </para>
        ///
        ///  <para>
        ///   - the worktree is linking to a valid on-disk worktree.
        ///   The `valid` member will cause this check to be ignored.
        ///   - the worktree is locked. The `locked` flag will cause
        ///   this check to be ignored.
        ///  </para>
        ///
        ///  <para>
        ///   If the worktree is not valid and not locked or if the
        ///   above flags have been passed in, this function will
        ///   return a positive value. If the worktree is not
        ///   prunable, an error message will be set (visible in
        ///   `giterr_last`) with details about why.
        ///  </para>
        /// </summary>
        /// <param name="wt">Worktree to check.</param>
        /// <param name="opts">The prunable options.</param>
        /// <returns>
        ///  1 if the worktree is prunable, 0 otherwise, or an error
        ///  code.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_worktree_is_prunable(
            git_worktree* wt,
            ref git_worktree_prune_options opts);

        /// <summary>
        ///  Prune working tree
        ///
        ///  <para>
        ///   Prune the working tree, that is remove the git data
        ///   structures on disk. The repository will only be pruned
        ///   of `git_worktree_is_prunable` succeeds.
        ///  </para>
        /// </summary>
        /// <param name="wt">Worktree to prune</param>
        /// <param name="opts">
        ///  Specifies which checks to override. See
        ///  `git_worktree_is_prunable`. May be NULL
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_worktree_prune(
            git_worktree* wt,
            ref git_worktree_prune_options opts);
    }
}
