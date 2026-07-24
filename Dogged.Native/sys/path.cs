using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  The kinds of git-specific files we know about.
    ///
    ///  <para>
    ///   The order needs to stay the same to not break the `gitfiles`
    ///   array in path.c
    ///  </para>
    /// </summary>
    public enum git_path_gitfile
    {
        /// <summary>
        ///  Check for the .gitignore file
        /// </summary>
        GIT_PATH_GITFILE_GITIGNORE,

        /// <summary>
        ///  Check for the .gitmodules file
        /// </summary>
        GIT_PATH_GITFILE_GITMODULES,

        /// <summary>
        ///  Check for the .gitattributes file
        /// </summary>
        GIT_PATH_GITFILE_GITATTRIBUTES,
    }

    /// <summary>
    ///  The kinds of checks to perform according to which filesystem we
    ///  are trying to protect.
    /// </summary>
    public enum git_path_fs
    {
        /// <summary>
        ///  Do both NTFS- and HFS-specific checks
        /// </summary>
        GIT_PATH_FS_GENERIC,

        /// <summary>
        ///  Do NTFS-specific checks only
        /// </summary>
        GIT_PATH_FS_NTFS,

        /// <summary>
        ///  Do HFS-specific checks only
        /// </summary>
        GIT_PATH_FS_HFS,
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Check whether a path component corresponds to a .git$SUFFIX
        ///  file.
        ///
        ///  <para>
        ///   As some filesystems do special things to filenames when
        ///   writing files to disk, you cannot always do a plain
        ///   string comparison to verify whether a file name matches
        ///   an expected path or not. This function can do the
        ///   comparison for you, depending on the filesystem you're
        ///   on.
        ///  </para>
        /// </summary>
        /// <param name="path">the path component to check</param>
        /// <param name="pathlen">
        ///  the length of `path` that is to be checked
        /// </param>
        /// <param name="gitfile">which file to check against</param>
        /// <param name="fs">which filesystem-specific checks to use</param>
        /// <returns>
        ///  0 in case the file does not match, a positive value if it
        ///  does; -1 in case of an error
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_path_is_gitfile(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            UIntPtr pathlen,
            git_path_gitfile gitfile,
            git_path_fs fs);
    }
}
