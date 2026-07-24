using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public sealed class StatusListHandle : SafeHandle
    {
        public StatusListHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe StatusListHandle(git_status_list* nativeStatusList) :
            base((IntPtr)nativeStatusList, true)
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
            libgit2.git_status_list_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Gather file statuses and run a callback for each one.
        ///
        ///  <para>
        ///   The callback is passed the path of the file, the status
        ///   (a combination of the `git_status_t` values above) and
        ///   the `payload` data pointer passed into this function.
        ///  </para>
        ///
        ///  <para>
        ///   If the callback returns a non-zero value, this function
        ///   will stop looping and return that value to caller.
        ///  </para>
        /// </summary>
        /// <param name="repo">A repository object</param>
        /// <param name="callback">The function to call on each file</param>
        /// <param name="payload">
        ///  Pointer to pass through to callback function
        /// </param>
        /// <returns>
        ///  0 on success, non-zero callback return value, or error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_status_foreach(
            RepositoryHandle repo,
            git_status_cb callback,
            IntPtr payload);

        /// <summary>
        ///  Gather file status information and run callbacks as
        ///  requested.
        ///
        ///  <para>
        ///   This is an extended version of the
        ///   `git_status_foreach()` API that allows for more granular
        ///   control over which paths will be processed and in what
        ///   order. See the `git_status_options` structure for
        ///   details about the additional controls that this makes
        ///   available.
        ///  </para>
        ///
        ///  <para>
        ///   Note that if a `pathspec` is given in the
        ///   `git_status_options` to filter the status, then the
        ///   results from rename detection (if you enable it) may not
        ///   be accurate. To do rename detection properly, this must
        ///   be called with no `pathspec` so that all files can be
        ///   considered.
        ///  </para>
        /// </summary>
        /// <param name="repo">Repository object</param>
        /// <param name="opts">Status options structure</param>
        /// <param name="callback">The function to call on each file</param>
        /// <param name="payload">
        ///  Pointer to pass through to callback function
        /// </param>
        /// <returns>
        ///  0 on success, non-zero callback return value, or error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_status_foreach_ext(
            RepositoryHandle repo,
            ref git_status_options opts,
            git_status_cb callback,
            IntPtr payload);

        /// <summary>
        ///  Get file status for a single file.
        ///
        ///  <para>
        ///   This tries to get status for the filename that you give.
        ///   If no files match that name (in either the HEAD, index,
        ///   or working directory), this returns GIT_ENOTFOUND.
        ///  </para>
        ///
        ///  <para>
        ///   If the name matches multiple files (for example, if the
        ///   `path` names a directory or if running on a case-
        ///   insensitive filesystem and yet the HEAD has two entries
        ///   that both match the path), then this returns
        ///   GIT_EAMBIGUOUS because it cannot give correct results.
        ///  </para>
        ///
        ///  <para>
        ///   This does not do any sort of rename detection. Renames
        ///   require a set of targets and because of the path
        ///   filtering, there is not enough information to check
        ///   renames correctly. To check file status with rename
        ///   detection, there is no choice but to do a full
        ///   `git_status_list_new` and scan through looking for the
        ///   path that you are interested in.
        ///  </para>
        /// </summary>
        /// <param name="status_flags">
        ///  Output combination of git_status_t values for file
        /// </param>
        /// <param name="repo">A repository object</param>
        /// <param name="path">
        ///  The exact path to retrieve status for relative to the
        ///  repository working directory
        /// </param>
        /// <returns>
        ///  0 on success, GIT_ENOTFOUND if the file is not found in the
        ///  HEAD, index, and work tree, GIT_EAMBIGUOUS if `path` matches
        ///  multiple files or if it refers to a folder, and -1 on other
        ///  errors.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_status_file(
            uint* status_flags,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path);

        /// <summary>
        ///  Gather file status information and populate the
        ///  `git_status_list`.
        ///
        ///  <para>
        ///   Note that if a `pathspec` is given in the
        ///   `git_status_options` to filter the status, then the
        ///   results from rename detection (if you enable it) may not
        ///   be accurate. To do rename detection properly, this must
        ///   be called with no `pathspec` so that all files can be
        ///   considered.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Pointer to store the status results in
        /// </param>
        /// <param name="repo">Repository object</param>
        /// <param name="opts">Status options structure</param>
        /// <returns>0 on success or error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_status_list_new(
            out StatusListHandle _out,
            RepositoryHandle repo,
            ref git_status_options opts);

        /// <summary>
        ///  Gets the count of status entries in this list.
        ///
        ///  <para>
        ///   If there are no changes in status (at least according
        ///   the options given when the status list was created),
        ///   this can return 0.
        ///  </para>
        /// </summary>
        /// <param name="statuslist">Existing status list object</param>
        /// <returns>the number of status entries</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_status_list_entrycount(
            StatusListHandle statuslist);

        /// <summary>
        ///  Get a pointer to one of the entries in the status list.
        ///
        ///  <para>
        ///   The entry is not modifiable and should not be freed.
        ///  </para>
        /// </summary>
        /// <param name="statuslist">Existing status list object</param>
        /// <param name="idx">Position of the entry</param>
        /// <returns>Pointer to the entry; NULL if out of bounds</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_status_entry* git_status_byindex(
            StatusListHandle statuslist,
            UIntPtr idx);

        /// <summary>
        ///  Free an existing status list
        /// </summary>
        /// <param name="statuslist">Existing status list object</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_status_list_free(
            IntPtr statuslist);

        /// <summary>
        ///  Test if the ignore rules apply to a given file.
        ///
        ///  <para>
        ///   This function checks the ignore rules to see if they
        ///   would apply to the given file. This indicates if the
        ///   file would be ignored regardless of whether the file is
        ///   already in the index or committed to the repository.
        ///  </para>
        ///
        ///  <para>
        ///   One way to think of this is if you were to do "git add
        ///   ." on the directory containing the file, would it be
        ///   added or not?
        ///  </para>
        /// </summary>
        /// <param name="ignored">
        ///  Boolean returning 0 if the file is not ignored, 1 if it is
        /// </param>
        /// <param name="repo">A repository object</param>
        /// <param name="path">
        ///  The file to check ignores for, rooted at the repo's workdir.
        /// </param>
        /// <returns>
        ///  0 if ignore rules could be processed for the file
        ///  (regardless of whether it exists or not), or an error < 0 if
        ///  they could not.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_status_should_ignore(
            int* ignored,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path);
    }
}
