using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    /// Flags to control the functionality of `git_blob_filter`.
    /// </summary>
    public enum git_blob_filter_flag_t
    {
        /// <summary>
        /// When set, filters will not be applied to binary files.
        /// </summary>
        GIT_BLOB_FILTER_CHECK_FOR_BINARY = (1 << 0),

        /// <summary>
        /// When set, filters will no load configuration from the
        /// system-wide `gitattributes` in `/etc` (or system equivalent).
        /// </summary>
        GIT_BLOB_FILTER_NO_SYSTEM_ATTRIBUTES = (1 << 1),

        /// <summary>
        /// When set, filters will be loaded from a `.gitattributes` file
        /// in the HEAD commit.
        /// </summary>
        GIT_BLOB_FILTER_ATTTRIBUTES_FROM_HEAD = (1 << 2),

        /// <summary>
        /// When set, filters will be loaded from a `.gitattributes` file
        /// in the specified commit.
        /// </summary>
        GIT_BLOB_FILTER_ATTTRIBUTES_FROM_COMMIT = (1 << 3),
    }

    /// <summary>
    /// The options used when applying filter options to a file.
    /// Initialize with `GIT_BLOB_FILTER_OPTIONS_INIT`.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_blob_filter_options
    {
        /// <summary>
        /// Version of the options.
        /// </summary>
        public int version;

        /// <summary>
        /// Flags to control the filtering process, see
        /// `git_blob_filter_flag_t` above.
        /// </summary>
        public git_blob_filter_flag_t flags;

        public IntPtr reserved;

        /// <summary>
        /// The commit to load attributes from, when
        /// `GIT_BLOB_FILTER_ATTRIBUTES_FROM_COMMIT` is specified.
        /// </summary>
        public git_oid attr_commit_id;

        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static int GIT_BLOB_FILTER_OPTIONS_VERSION
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// The default values for our options structure.
        /// </summary>
        public static git_blob_filter_options GIT_BLOB_FILTER_OPTIONS_INIT
        {
            get
            {
                return new git_blob_filter_options() {
                    version = GIT_BLOB_FILTER_OPTIONS_VERSION,
                    flags = git_blob_filter_flag_t.GIT_BLOB_FILTER_CHECK_FOR_BINARY
                };
            }
        }
    }

    public static partial class libgit2
    {
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_blob_filter(
            git_buf content,
            git_blob* blob,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            git_blob_filter_options* options);

        /// <summary>
        /// Determine if the blob content is binary or not.

        /// <para>
        /// The heuristic used to guess if a file is binary is taken from core git:
        /// Searching for NUL bytes and looking for a reasonable ratio of printable
        /// to non-printable characters among the first 8000 bytes.
        /// </para>
        /// </summary>
        /// <param name="blob">The blob to examine</param>
        /// <returns>1 if the content of the blob is detected as binary; 0 otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_blob_is_binary(git_blob* blob);

        /// <summary>
        /// Look up a blob from the repository.
        /// </summary>
        /// <param name="blob">Pointer to the blob that was loaded from the repository</param>
        /// <param name="repo">The repository that contains the blob</param>
        /// <param name="id">The id of the blob to lookup</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_blob_lookup(out git_blob* blob, git_repository* repo, ref git_oid id);

        /// <summary>
        /// Get a read-only buffer with the raw content of a blob.
        ///
        /// <para>
        /// A pointer to the raw content of a blob is returned;
        /// this pointer is owned internally by the object and shall
        /// not be free'd. The pointer may be invalidated at a later
        /// time.
        /// <para>
        /// </summary>
        /// <param name="blob">The blob to examine</param>
        /// <returns>The buffer.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe byte* git_blob_rawcontent(git_blob* blob);

        /// <summary>
        /// Get the size in bytes of the contents of a blob.
        /// </summary>
        /// <param name="blob">The blob to examine</param>
        /// <returns>Raw size of the blob in bytes</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe long git_blob_rawsize(git_blob* blob);
    }
}
