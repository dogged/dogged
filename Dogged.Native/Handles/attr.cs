using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
        /// <summary>
        ///  Look up the value of one git attribute for path.
        /// </summary>
        /// <param name="value_out">
        ///  Output of the value of the attribute. Use the GIT_ATTR_...
        ///  macros to test for TRUE, FALSE, UNSPECIFIED, etc. or just
        ///  use the string value for attributes set to a value. You
        ///  should NOT modify or free this value.
        /// </param>
        /// <param name="repo">The repository containing the path.</param>
        /// <param name="flags">
        ///  A combination of GIT_ATTR_CHECK... flags.
        /// </param>
        /// <param name="path">
        ///  The path to check for attributes. Relative paths are
        ///  interpreted relative to the repo root. The file does not
        ///  have to exist, but if it does not, then it will be treated
        ///  as a plain file (not a directory).
        /// </param>
        /// <param name="name">The name of the attribute to look up.</param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_attr_get(
            char** value_out,
            RepositoryHandle repo,
            uint flags,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Look up the value of one git attribute for path with
        ///  extended options.
        /// </summary>
        /// <param name="value_out">
        ///  Output of the value of the attribute. Use the GIT_ATTR_...
        ///  macros to test for TRUE, FALSE, UNSPECIFIED, etc. or just
        ///  use the string value for attributes set to a value. You
        ///  should NOT modify or free this value.
        /// </param>
        /// <param name="repo">The repository containing the path.</param>
        /// <param name="opts">
        ///  The `git_attr_options` to use when querying these
        ///  attributes.
        /// </param>
        /// <param name="path">
        ///  The path to check for attributes. Relative paths are
        ///  interpreted relative to the repo root. The file does not
        ///  have to exist, but if it does not, then it will be treated
        ///  as a plain file (not a directory).
        /// </param>
        /// <param name="name">The name of the attribute to look up.</param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_attr_get_ext(
            char** value_out,
            RepositoryHandle repo,
            ref git_attr_options opts,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Look up a list of git attributes for path.
        ///
        ///  <para>
        ///   Use this if you have a known list of attributes that you
        ///   want to look up in a single call. This is somewhat more
        ///   efficient than calling `git_attr_get()` multiple times.
        ///  </para>
        ///
        ///  <para>
        ///   For example, you might write:
        ///  </para>
        ///
        ///  <para>
        ///   const char *attrs[] = { "crlf", "diff", "foo" }; const
        ///   char **values[3]; git_attr_get_many(values, repo, 0,
        ///   "my/fun/file.c", 3, attrs);
        ///  </para>
        ///
        ///  <para>
        ///   Then you could loop through the 3 values to get the
        ///   settings for the three attributes you asked about.
        ///  </para>
        /// </summary>
        /// <param name="values_out">
        ///  An array of num_attr entries that will have string pointers
        ///  written into it for the values of the attributes. You should
        ///  not modify or free the values that are written into this
        ///  array (although of course, you should free the array itself
        ///  if you allocated it).
        /// </param>
        /// <param name="repo">The repository containing the path.</param>
        /// <param name="flags">
        ///  A combination of GIT_ATTR_CHECK... flags.
        /// </param>
        /// <param name="path">
        ///  The path inside the repo to check attributes. This does not
        ///  have to exist, but if it does not, then it will be treated
        ///  as a plain file (i.e. not a directory).
        /// </param>
        /// <param name="num_attr">
        ///  The number of attributes being looked up
        /// </param>
        /// <param name="names">
        ///  An array of num_attr strings containing attribute names.
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_attr_get_many(
            char** values_out,
            RepositoryHandle repo,
            uint flags,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            UIntPtr num_attr,
            char** names);

        /// <summary>
        ///  Look up a list of git attributes for path with extended
        ///  options.
        /// </summary>
        /// <param name="values_out">
        ///  An array of num_attr entries that will have string pointers
        ///  written into it for the values of the attributes. You should
        ///  not modify or free the values that are written into this
        ///  array (although of course, you should free the array itself
        ///  if you allocated it).
        /// </param>
        /// <param name="repo">The repository containing the path.</param>
        /// <param name="opts">
        ///  The `git_attr_options` to use when querying these
        ///  attributes.
        /// </param>
        /// <param name="path">
        ///  The path inside the repo to check attributes. This does not
        ///  have to exist, but if it does not, then it will be treated
        ///  as a plain file (i.e. not a directory).
        /// </param>
        /// <param name="num_attr">
        ///  The number of attributes being looked up
        /// </param>
        /// <param name="names">
        ///  An array of num_attr strings containing attribute names.
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_attr_get_many_ext(
            char** values_out,
            RepositoryHandle repo,
            ref git_attr_options opts,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            UIntPtr num_attr,
            char** names);

        /// <summary>
        ///  Loop over all the git attributes for a path.
        /// </summary>
        /// <param name="repo">The repository containing the path.</param>
        /// <param name="flags">
        ///  A combination of GIT_ATTR_CHECK... flags.
        /// </param>
        /// <param name="path">
        ///  Path inside the repo to check attributes. This does not have
        ///  to exist, but if it does not, then it will be treated as a
        ///  plain file (i.e. not a directory).
        /// </param>
        /// <param name="callback">
        ///  Function to invoke on each attribute name and value. See
        ///  git_attr_foreach_cb.
        /// </param>
        /// <param name="payload">
        ///  Passed on as extra parameter to callback function.
        /// </param>
        /// <returns>
        ///  0 on success, non-zero callback return value, or error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_attr_foreach(
            RepositoryHandle repo,
            uint flags,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            git_attr_foreach_cb callback,
            IntPtr payload);

        /// <summary>
        ///  Loop over all the git attributes for a path with extended
        ///  options.
        /// </summary>
        /// <param name="repo">The repository containing the path.</param>
        /// <param name="opts">
        ///  The `git_attr_options` to use when querying these
        ///  attributes.
        /// </param>
        /// <param name="path">
        ///  Path inside the repo to check attributes. This does not have
        ///  to exist, but if it does not, then it will be treated as a
        ///  plain file (i.e. not a directory).
        /// </param>
        /// <param name="callback">
        ///  Function to invoke on each attribute name and value. See
        ///  git_attr_foreach_cb.
        /// </param>
        /// <param name="payload">
        ///  Passed on as extra parameter to callback function.
        /// </param>
        /// <returns>
        ///  0 on success, non-zero callback return value, or error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_attr_foreach_ext(
            RepositoryHandle repo,
            ref git_attr_options opts,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            git_attr_foreach_cb callback,
            IntPtr payload);

        /// <summary>
        ///  Flush the gitattributes cache.
        ///
        ///  <para>
        ///   Call this if you have reason to believe that the
        ///   attributes files on disk no longer match the cached
        ///   contents of memory. This will cause the attributes files
        ///   to be reloaded the next time that an attribute access
        ///   function is called.
        ///  </para>
        /// </summary>
        /// <param name="repo">
        ///  The repository containing the gitattributes cache
        /// </param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_attr_cache_flush(
            RepositoryHandle repo);

        /// <summary>
        ///  Add a macro definition.
        ///
        ///  <para>
        ///   Macros will automatically be loaded from the top level
        ///   `.gitattributes` file of the repository (plus the
        ///   built-in "binary" macro). This function allows you to
        ///   add others. For example, to add the default macro, you
        ///   would call:
        ///  </para>
        ///
        ///  <para>
        ///   git_attr_add_macro(repo, "binary", "-diff -crlf");
        ///  </para>
        /// </summary>
        /// <param name="repo">The repository to add the macro in.</param>
        /// <param name="name">The name of the macro.</param>
        /// <param name="values">The value for the macro.</param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_attr_add_macro(
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string values);
    }
}
