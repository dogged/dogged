using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
        /// <summary>
        ///  Add ignore rules for a repository.
        ///
        ///  <para>
        ///   Excludesfile rules (i.e. .gitignore rules) are generally
        ///   read from .gitignore files in the repository tree or
        ///   from a shared system file only if a "core.excludesfile"
        ///   config value is set. The library also keeps a set of
        ///   per-repository internal ignores that can be configured
        ///   in-memory and will not persist. This function allows you
        ///   to add to that internal rules list.
        ///  </para>
        ///
        ///  <para>
        ///   Example usage:
        ///  </para>
        ///
        ///  <para>
        ///   error = git_ignore_add_rule(myrepo, "*.c\ndir/\nFile
        ///   with space\n");
        ///  </para>
        ///
        ///  <para>
        ///   This would add three rules to the ignores.
        ///  </para>
        /// </summary>
        /// <param name="repo">
        ///  The repository to add ignore rules to.
        /// </param>
        /// <param name="rules">
        ///  Text of rules, the contents to add on a .gitignore file. It
        ///  is okay to have multiple rules in the text; if so, each rule
        ///  should be terminated with a newline.
        /// </param>
        /// <returns>0 on success</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_ignore_add_rule(
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string rules);

        /// <summary>
        ///  Clear ignore rules that were explicitly added.
        ///
        ///  <para>
        ///   Resets to the default internal ignore rules. This will
        ///   not turn off rules in .gitignore files that actually
        ///   exist in the filesystem.
        ///  </para>
        ///
        ///  <para>
        ///   The default internal ignores ignore ".", ".." and ".git"
        ///   entries.
        ///  </para>
        /// </summary>
        /// <param name="repo">
        ///  The repository to remove ignore rules from.
        /// </param>
        /// <returns>0 on success</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_ignore_clear_internal_rules(
            RepositoryHandle repo);

        /// <summary>
        ///  Test if the ignore rules apply to a given path.
        ///
        ///  <para>
        ///   This function checks the ignore rules to see if they
        ///   would apply to the given file. This indicates if the
        ///   file would be ignored regardless of whether the file is
        ///   already in the index or committed to the repository.
        ///  </para>
        ///
        ///  <para>
        ///   One way to think of this is if you were to do "git
        ///   check-ignore --no-index" on the given file, would it be
        ///   shown or not?
        ///  </para>
        /// </summary>
        /// <param name="ignored">
        ///  boolean returning 0 if the file is not ignored, 1 if it is
        /// </param>
        /// <param name="repo">a repository object</param>
        /// <param name="path">
        ///  the file to check ignores for, relative to the repo's
        ///  workdir.
        /// </param>
        /// <returns>
        ///  0 if ignore rules could be processed for the file
        ///  (regardless of whether it exists or not), or an error < 0 if
        ///  they could not.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_ignore_path_is_ignored(
            int* ignored,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path);
    }
}
