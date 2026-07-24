using System;

using Dogged;
using Dogged.Native;

/// <summary>
/// Filters are applied in one of two directions: smudging - which is
/// exporting a file from the Git object database to the working directory,
/// and cleaning - which is importing a file from the working directory to
/// the Git object database.  These values control which direction of
/// change is being applied.
/// </summary>
public enum FilterMode
{
    ToWorktree = git_filter_mode_t.GIT_FILTER_TO_WORKTREE,
    ToObjectDatabase = git_filter_mode_t.GIT_FILTER_TO_ODB,
}

/// <summary>
/// Filter option flags.
/// </summary>
public enum FilterFlags
{
    /// <summary>
    /// Don't error for `safecrlf` violations, allow them to continue.
    /// </summary>
	AllowUnsafe = git_filter_flag_t.GIT_FILTER_ALLOW_UNSAFE,

    /// <summary>
    /// When set, filters will no load configuration from the
    /// system-wide `gitattributes` in `/etc` (or system equivalent).
    /// </summary>
	NoSystemAttributes = git_filter_flag_t.GIT_FILTER_NO_SYSTEM_ATTRIBUTES,

    /// <summary>
    /// When set, filters will be loaded from a `.gitattributes` file
    /// in the HEAD commit.
    /// </summary>
	AttributesFromHead = git_filter_flag_t.GIT_FILTER_ATTRIBUTES_FROM_HEAD,

    /// <summary>
    /// When set, filters will be loaded from a `.gitattributes` file
    /// in a given commit.
    /// </summary>
	AttributesFromCommit = git_filter_flag_t.GIT_FILTER_ATTRIBUTES_FROM_COMMIT,
}

/// <summary>
/// The options used when applying filter options to a file.
/// </summary>
public class FilterOptions
{
    private git_filter_options nativeOptions;

    /// <summary>
    /// Create an options structure for blob content filtering.
    /// </summary>
    public FilterOptions()
    {
        libgit2.git_filter_options_init(ref nativeOptions,
            git_filter_options.GIT_FILTER_OPTIONS_VERSION);
    }

    /// <summary>
    /// Flags to control the functionality of content filtering.
    /// </summary>
    public FilterFlags Flags
    {
        get
        {
            return (FilterFlags)nativeOptions.flags;
        }
        set
        {
            nativeOptions.flags = (git_filter_flag_t)((int)value);
        }
    }

    public unsafe ObjectId AttributeCommitId
    {
        get
        {
            return ObjectId.FromNative(nativeOptions.attr_commit_id);
        }
        set
        {
            git_oid src = value.ToNative();

            fixed (git_oid* dest = &nativeOptions.attr_commit_id)
            {
                ObjectId.NativeCopy(&src, dest);
            }
        }
    }

    internal git_filter_options NativeOptions
    {
        get
        {
            return nativeOptions;
        }
    }
}

public class FilterList : NativeDisposable
{
    private Repository repository;
    private unsafe FilterListHandle filterListHandle;

    public unsafe static FilterList Load(Repository repository, string path, Blob blob, FilterMode mode, FilterOptions options)
    {
        Ensure.ArgumentNotNull(repository, "repository");
        Ensure.ArgumentNotNull(path, "path");

        FilterListHandle filterListHandle = null;

        var nativeOptions = options.NativeOptions;

        Ensure.NativeSuccess(libgit2.git_filter_list_load_ext(
            out filterListHandle,
            repository.NativeRepository,
            (blob != null) ? blob.NativeBlob : null,
            path,
            (git_filter_mode_t)mode,
            ref nativeOptions));

        Ensure.ArgumentNotNull(filterListHandle, "filterListHandle");
        return (!filterListHandle.IsInvalid) ? FilterList.FromNative(repository, filterListHandle) : null;
    }

    public unsafe static FilterList Load(Repository repository, string path, FilterMode mode, FilterOptions options)
    {
        return Load(repository, path, null, mode, options);
    }

    private unsafe FilterList(Repository repository, FilterListHandle filterListHandle)
    {
        Ensure.ArgumentNotNull(repository, "repository");
        Ensure.ArgumentNotNullOrInvalid(filterListHandle, "filterListHandle");

        this.repository = repository;
        this.filterListHandle = filterListHandle;
    }

    internal unsafe static FilterList FromNative(Repository repository, FilterListHandle filterListHandle)
    {
        Ensure.ArgumentNotNull(repository, "repository");
        Ensure.ArgumentNotNullOrInvalid(filterListHandle, "filterListHandle");

        return new FilterList(repository, filterListHandle);
    }

    public unsafe GitBuffer Apply(Blob blob)
    {
        Ensure.ArgumentNotNull(blob, "blob");
        GitBuffer ret = new GitBuffer();

        Ensure.NativeSuccess(libgit2.git_filter_list_apply_to_blob(ref ret.NativeBuffer, filterListHandle, blob.NativeBlob));
        return ret;
    }

    public unsafe GitBuffer Apply(GitBuffer buffer)
    {
        Ensure.ArgumentNotNull(buffer, "buffer");
        GitBuffer ret = new GitBuffer();

        fixed (byte* ptr = buffer.Content) {
            Ensure.NativeSuccess(libgit2.git_filter_list_apply_to_buffer(ref ret.NativeBuffer, filterListHandle, ptr, (UIntPtr)buffer.Length));
        }

        return ret;
    }

    public unsafe GitBuffer Apply(byte[] buffer)
    {
        Ensure.ArgumentNotNull(buffer, "buffer");
        GitBuffer ret = new GitBuffer();

        fixed (byte* ptr = buffer) {
                Ensure.NativeSuccess(libgit2.git_filter_list_apply_to_buffer(ref ret.NativeBuffer, filterListHandle, ptr, (UIntPtr)buffer.Length));
        }

        return ret;
    }

    public unsafe GitBuffer Apply(string path)
    {
        Ensure.ArgumentNotNull(path, "path");
        GitBuffer ret = new GitBuffer();

        Ensure.NativeSuccess(libgit2.git_filter_list_apply_to_file(ref ret.NativeBuffer, filterListHandle, repository.NativeRepository, path));

        return ret;
    }

    internal unsafe override bool IsDisposed
    {
        get
        {
            return (filterListHandle == null);
        }
    }

    internal unsafe override void Dispose(bool disposing)
    {
        if (filterListHandle != null)
        {
            filterListHandle.Dispose();
            filterListHandle = null;
        }
    }
}
