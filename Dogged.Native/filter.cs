using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    /// An object (blob, commit, tree, etc) in a Git repository.
    /// </summary>
    public struct git_filter_list { };

    /// <summary>
    /// Flags to control the functionality of `git_blob_filter`.
    /// </summary>
    public enum git_filter_mode_t
    {
    	GIT_FILTER_TO_WORKTREE = 0,
        GIT_FILTER_TO_ODB = 1,

        GIT_FILTER_SMUDGE = GIT_FILTER_TO_WORKTREE,
        GIT_FILTER_CLEAN = GIT_FILTER_TO_ODB,
    }

    /// <summary>
    /// Filtering options.
    /// </summary>
    public enum git_filter_flags_t
    {
        GIT_FILTER_DEFAULT = 0,

        /// <summary>
        /// Don't error for `safecrlf` violations, allow them to continue.
        /// </summary>
        GIT_FILTER_ALLOW_UNSAFE = (1 << 0),

        /// <summary>
        /// When set, filters will no load configuration from the
        /// system-wide `gitattributes` in `/etc` (or system equivalent).
        /// </summary>
        GIT_FILTER_NO_SYSTEM_ATTRIBUTES = (1 << 1),

        /// <summary>
        /// When set, filters will be loaded from a `.gitattributes` file
        /// in the HEAD commit.
        /// </summary>
        GIT_FILTER_ATTRIBUTES_FROM_HEAD = (1 << 2),

        /// <summary>
        /// When set, filters will be loaded from a `.gitattributes` file
        /// in a given commit.  This can only be specified in a
        /// `git_filter_options`.
        /// </summary>
        GIT_FILTER_ATTRIBUTES_FROM_COMMIT = (1 << 3),
    }

    /// <summary>
    /// Filtering options.  Initialize with `GIT_FILTER_OPTIONS_INIT`.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_filter_options
    {
        /// <summary>
        /// Version of the options.
        /// </summary>
        public int version;

        /// <summary>
        /// Flags to control the filtering process, see
        /// `git_blob_filter_flag_t` above.
        /// </summary>
        public git_filter_flags_t flags;

        public IntPtr reserved;

        /// <summary>
        /// The commit to load attributes from, when
        /// `GIT_FILTER_ATTRIBUTES_FROM_COMMIT` is specified.
        /// </summary>
        public git_oid attr_commit_id;

        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static int GIT_FILTER_OPTIONS_VERSION
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// The default values for our options structure.
        /// </summary>
        public static git_filter_options GIT_FILTER_OPTIONS_INIT
        {
            get
            {
                return new git_filter_options() {
                    version = GIT_FILTER_OPTIONS_VERSION,
                    flags = git_filter_flags_t.GIT_FILTER_DEFAULT
                };
            }
        }
    }

    public static partial class libgit2
    {
        /// <summary>
        /// Apply a filter list to the contents of a blob.
        /// </summary>
        /// <param name="outputBuffer">Buffer to store the result of the filtering</param>
        /// <param name="filters">A loaded git_filter_list (or null)</param>
        /// <param name="blob">The blob to filter</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_filter_list_apply_to_blob(git_buf outputBuffer, git_filter_list* filters, git_blob* blob);

        /// <summary>
        /// Apply filter list to a data buffer.
        ///
        /// <para>
        /// If the `inputBuffer` buffer holds data allocated by libgit2 (i.e.
        /// `inputBuffer->asize` is not zero), then it will be overwritten when
        /// applying the filters.  If not, then it will be left untouched.
        /// </para>
        ///
        /// <para>
        /// If there are no filters to apply (or `filters` is NULL), then the
        /// `outputBuffer` buffer will reference the `inputBuffer` buffer data
        /// (with `asize` set to zero) instead of allocating data.  This keeps
        /// allocations to a minimum, but it means you have to be careful about
        /// freeing the `inputBuffer` data since `outputBuffer` may be pointing
        /// to it!
        /// </para>
        /// </summary>
        /// <param name="outputBuffer">Buffer to store the result of the filtering</param>
        /// <param name="filters">A loaded git_filter_list (or null)</param>
        /// <param name="inputBuffer">Buffer containing the data to filter</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_filter_list_apply_to_data(git_buf outputBuffer, git_filter_list* filters, git_buf inputBuffer);

        /// <summary>
        /// Apply a filter list to the contents of a file on disk.
        /// </summary>
        /// <param name="outputBuffer">Buffer to store the result of the filtering</param>
        /// <param name="filters">A loaded git_filter_list (or null)</param>
        /// <param name="repo">The repository to perform the filtering in</param>
        /// <param name="path">The path of the file to filter; a relative path will be taken as relative to the workdir</param>
        /// <param name="blob">The blob to filter</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_filter_list_apply_to_file(git_buf outputBuffer, git_filter_list* filters, git_repository* repo, string path);

        /// <summary>
        /// Free a git filter list.
        /// </summary>
        ///
        /// <param name="filters">The filter list to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_filter_list_free(git_filter_list* filters);

        /// <summary>
        /// Load the filter list for a given path.
        ///
        /// <para>
        /// This will return 0 (success) but set the output git_filter_list
        /// to NULL if no filters are requested for the given file.
        /// </para>
        /// </summary>
        /// <param name="filters">Output newly created git_filter_list (or NULL)</param>
        /// <param name="repo">Repository object that contains path</param>
        /// <param name="blob">The blob to which the filter will be applied (if known)</param>
        /// <param name="path">Relative path of the file to be filtered</param>
        /// <param name="mode">Filtering direction (WT->ODB or ODB->WT)</param>
        /// <param name="flags">Combination of `git_filter_flag_t` flags</param>
        /// <returns>0 on success (which could still return NULL if no filters are needed for the requested file) or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_filter_list_load(
            out git_filter_list* filters,
            git_repository* repo,
            git_blob* blob,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            git_filter_mode_t mode,
            uint flags);

        /// <summary>
        /// Load the filter list for a given path.
        ///
        /// <para>
        /// This will return 0 (success) but set the output git_filter_list
        /// to NULL if no filters are requested for the given file.
        /// </para>
        /// </summary>
        /// <param name="filters">Output newly created git_filter_list (or NULL)</param>
        /// <param name="repo">Repository object that contains path</param>
        /// <param name="blob">The blob to which the filter will be applied (if known)</param>
        /// <param name="path">Relative path of the file to be filtered</param>
        /// <param name="mode">Filtering direction (WT->ODB or ODB->WT)</param>
        /// <param name="flags">Combination of `git_filter_flag_t` flags</param>
        /// <returns>0 on success (which could still return NULL if no filters are needed for the requested file) or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_filter_list_load_ext(
            out git_filter_list* filters,
            git_repository* repo,
            git_blob* blob,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            git_filter_mode_t mode,
            ref git_filter_options opts);
    }
}
