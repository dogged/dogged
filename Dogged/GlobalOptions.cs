using System;
using Dogged.Native;
using Dogged.Native.Services;

namespace Dogged
{
    /// <summary>
    /// Global options that control all libgit2 operations in all
    /// repositories.
    /// </summary>
    public static class GlobalOptions
    {
        public unsafe static string GetSearchPath(ConfigurationLevel level)
        {
            GitBuffer buf = new GitBuffer();
            ref git_buf nativeBuffer = ref buf.NativeBuffer;

            Ensure.NativeSuccess(libgit2.git_libgit2_opts(git_libgit2_opt_t.GIT_OPT_GET_SEARCH_PATH, (git_config_level_t)level, ref nativeBuffer));

            fixed (byte* bp = buf.Content)
            {
                return Utf8Converter.FromNative(bp);
            }
        }

        public unsafe static void SetSearchPath(ConfigurationLevel level, string path)
        {
            Ensure.NativeSuccess(libgit2.git_libgit2_opts(git_libgit2_opt_t.GIT_OPT_SET_SEARCH_PATH, (git_config_level_t)level, path));
        }
    }
}
