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
	AllowUnsafe = git_filter_flags_t.GIT_FILTER_ALLOW_UNSAFE,

    /// <summary>
    /// When set, filters will no load configuration from the
    /// system-wide `gitattributes` in `/etc` (or system equivalent).
    /// </summary>
	NoSystemAttributes = git_filter_flags_t.GIT_FILTER_NO_SYSTEM_ATTRIBUTES,

    /// <summary>
    /// When set, filters will be loaded from a `.gitattributes` file
    /// in the HEAD commit.
    /// </summary>
	AttributesFromHead = git_filter_flags_t.GIT_FILTER_ATTRIBUTES_FROM_HEAD,
}

public class FilterList : NativeDisposable
{
    private Repository repository;
    private unsafe git_filter_list* nativeFilterList;

    private unsafe FilterList(Repository repository, git_filter_list* nativeFilterList)
    {
        Ensure.ArgumentNotNull(repository, "repository");
        Ensure.ArgumentNotNull(nativeFilterList, "nativeFilterList");

        this.repository = repository;
        this.nativeFilterList = nativeFilterList;
    }

    internal unsafe static FilterList FromNative(Repository repository, git_filter_list* nativeFilterList)
    {
        return new FilterList(repository, nativeFilterList);
    }

    public unsafe GitBuffer Apply(Blob blob)
    {
        Ensure.ArgumentNotNull(blob, "blob");
        GitBuffer ret = new GitBuffer();

        Ensure.NativeSuccess(() => libgit2.git_filter_list_apply_to_blob(ret.NativeBuffer, nativeFilterList, (git_blob*)blob.NativeObject), this);
        return ret;
    }

    public unsafe GitBuffer Apply(GitBuffer buffer)
    {
        Ensure.ArgumentNotNull(buffer, "buffer");
        GitBuffer ret = new GitBuffer();

        Ensure.NativeSuccess(() => libgit2.git_filter_list_apply_to_data(ret.NativeBuffer, nativeFilterList, buffer.NativeBuffer), this);
        return ret;
    }

    public unsafe GitBuffer Apply(string path)
    {
        Ensure.ArgumentNotNull(path, "path");
        GitBuffer ret = new GitBuffer();

        Ensure.NativeSuccess(() => libgit2.git_filter_list_apply_to_file(ret.NativeBuffer, nativeFilterList, repository.NativeRepository, path), this);
        return ret;
    }

    internal unsafe override bool IsDisposed
    {
        get
        {
            return (nativeFilterList == null);
        }
    }

    internal unsafe override void Dispose(bool disposing)
    {
        if (nativeFilterList != null)
        {
            libgit2.git_filter_list_free(nativeFilterList);
            nativeFilterList = null;
        }
    }
}
