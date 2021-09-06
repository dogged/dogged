using Dogged;
using Dogged.Native;

/// <summary>
/// Flags to control the functionality of blob content filtering.
/// </summary>
public enum BlobFilterFlags
{
    /// <summary>
    /// When set, filters will not be applied to binary files.
    /// </summary>
    CheckForBinary = git_blob_filter_flag_t.GIT_BLOB_FILTER_CHECK_FOR_BINARY,

    /// <summary>
    /// When set, filters will no load configuration from the
    /// system-wide `gitattributes` in `/etc` (or system equivalent).
    /// </summary>
    NoSystemAttributes = git_blob_filter_flag_t.GIT_BLOB_FILTER_NO_SYSTEM_ATTRIBUTES,

    /// <summary>
    /// When set, filters will be loaded from a `.gitattributes` file
    /// in the HEAD commit.
    /// </summary>
    AttributesFromHead = git_blob_filter_flag_t.GIT_BLOB_FILTER_ATTTRIBUTES_FROM_HEAD,

    /// <summary>
    /// When set, filters will be loaded from a `.gitattributes` file
    /// in the specified commit.
    /// </summary>
    AttributesFromCommit = git_blob_filter_flag_t.GIT_BLOB_FILTER_ATTTRIBUTES_FROM_COMMIT,
}

/// <summary>
/// The options used when applying filter options to a file.
/// </summary>
public class BlobFilterOptions
{
    private git_blob_filter_options nativeOptions;

    /// <summary>
    /// Create an options structure for blob content filtering.
    /// </summary>
    public BlobFilterOptions()
    {
        nativeOptions = git_blob_filter_options.GIT_BLOB_FILTER_OPTIONS_INIT;
    }

    /// <summary>
    /// Flags to control the functionality of blob content filtering.
    /// </summary>
    public BlobFilterFlags Flags
    {
        get
        {
            return (BlobFilterFlags)nativeOptions.flags;
        }
        set
        {
            nativeOptions.flags = (git_blob_filter_flag_t)((int)value);
        }
    }

    public unsafe ObjectId CommitId
    {
        get
        {
            return ObjectId.FromNative(nativeOptions.commit_id);
        }
        set
        {
            git_oid src = value.ToNative();

            fixed (git_oid* dest = &nativeOptions.commit_id)
            {
                ObjectId.NativeCopy(&src, dest);
            }
        }
    }

    internal git_blob_filter_options NativeOptions
    {
        get
        {
            return nativeOptions;
        }
    }
}
