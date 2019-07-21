using System;
using Dogged.Native;

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
}

public class BlobFilterOptions
{
    public BlobFilterOptions()
    {
        Flags = BlobFilterFlags.CheckForBinary;
    }

    public BlobFilterFlags Flags
    {
        get;
        set;
    }

    internal git_blob_filter_options ToNative()
    {
        return new git_blob_filter_options {
            version = 1,
            flags = (git_blob_filter_flag_t)((int)Flags)
        };
    }
}
