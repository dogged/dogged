using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
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
            RepositoryHandle repo,
            ref git_signature stasher,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string message,
            uint flags);

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
            RepositoryHandle repo,
            ref git_stash_save_options opts);

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
            RepositoryHandle repo,
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
            RepositoryHandle repo,
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
            RepositoryHandle repo,
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
            RepositoryHandle repo,
            UIntPtr index,
            ref git_stash_apply_options options);
    }
}
