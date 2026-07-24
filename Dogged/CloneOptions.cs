using Dogged;
using Dogged.Native;

/// <summary>
/// The options used when cloning a repository.
/// </summary>
public class CloneOptions
{
    private git_clone_options nativeOptions;

    /// <summary>
    /// Create an options structure for cloning a repository.
    /// </summary>
    public CloneOptions()
    {
        libgit2.git_clone_options_init(ref nativeOptions,
            git_clone_options.GIT_CLONE_OPTIONS_VERSION);
    }

    internal git_clone_options NativeOptions
    {
        get
        {
            return nativeOptions;
        }
    }
}
