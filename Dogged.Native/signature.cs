using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  An action signature (e.g. for committers, taggers, etc)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_signature
    {
        /// <summary>
        ///  full name of the author
        /// </summary>
        public byte* name;

        /// <summary>
        ///  email of the author
        /// </summary>
        public byte* email;

        /// <summary>
        ///  time when the action happened
        /// </summary>
        public git_time when;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Create a new action signature.
        ///
        ///  <para>
        ///   Call `git_signature_free()` to free the data.
        ///  </para>
        ///
        ///  <para>
        ///   Note: angle brackets (' < ' and '>') characters are not
        ///   allowed to be used in either the `name` or the `email`
        ///   parameter.
        ///  </para>
        /// </summary>
        /// <param name="_out">new signature, in case of error NULL</param>
        /// <param name="name">name of the person</param>
        /// <param name="email">email of the person</param>
        /// <param name="time">
        ///  time (in seconds from epoch) when the action happened
        /// </param>
        /// <param name="offset">
        ///  timezone offset (in minutes) for the time
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_signature_new(
            out git_signature* _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string email,
            long time,
            int offset);

        /// <summary>
        ///  Create a new action signature with a timestamp of 'now'.
        ///
        ///  <para>
        ///   Call `git_signature_free()` to free the data.
        ///  </para>
        /// </summary>
        /// <param name="_out">new signature, in case of error NULL</param>
        /// <param name="name">name of the person</param>
        /// <param name="email">email of the person</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_signature_now(
            out git_signature* _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string email);

        /// <summary>
        ///  Create a new author and/or committer signatures with default
        ///  information based on the configuration and environment
        ///  variables.
        ///
        ///  <para>
        ///   If `author_out` is set, it will be populated with the
        ///   author information. The `GIT_AUTHOR_NAME` and
        ///   `GIT_AUTHOR_EMAIL` environment variables will be
        ///   honored, and `user.name` and `user.email` configuration
        ///   options will be honored if the environment variables are
        ///   unset. For timestamps, `GIT_AUTHOR_DATE` will be used,
        ///   otherwise the current time will be used.
        ///  </para>
        ///
        ///  <para>
        ///   If `committer_out` is set, it will be populated with the
        ///   committer information. The `GIT_COMMITTER_NAME` and
        ///   `GIT_COMMITTER_EMAIL` environment variables will be
        ///   honored, and `user.name` and `user.email` configuration
        ///   options will be honored if the environment variables are
        ///   unset. For timestamps, `GIT_COMMITTER_DATE` will be
        ///   used, otherwise the current time will be used.
        ///  </para>
        ///
        ///  <para>
        ///   If neither `GIT_AUTHOR_DATE` nor `GIT_COMMITTER_DATE`
        ///   are set, both timestamps will be set to the same time.
        ///  </para>
        ///
        ///  <para>
        ///   It will return `GIT_ENOTFOUND` if either the `user.name`
        ///   or `user.email` are not set and there is no fallback
        ///   from an environment variable. One of `author_out` or
        ///   `committer_out` must be set.
        ///  </para>
        /// </summary>
        /// <param name="author_out">
        ///  pointer to set the author signature, or NULL
        /// </param>
        /// <param name="committer_out">
        ///  pointer to set the committer signature, or NULL
        /// </param>
        /// <param name="repo">repository pointer</param>
        /// <returns>
        ///  0 on success, GIT_ENOTFOUND if config is missing, or error
        ///  code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_signature_default_from_env(
            out git_signature* author_out,
            out git_signature* committer_out,
            git_repository* repo);

        /// <summary>
        ///  Create a new action signature with default user and now
        ///  timestamp.
        ///
        ///  <para>
        ///   This looks up the user.name and user.email from the
        ///   configuration and uses the current time as the
        ///   timestamp, and creates a new signature based on that
        ///   information. It will return GIT_ENOTFOUND if either the
        ///   user.name or user.email are not set.
        ///  </para>
        ///
        ///  <para>
        ///   Note that these do not examine environment variables,
        ///   only the configuration files. Use
        ///   `git_signature_default_from_env` to consider the
        ///   environment variables.
        ///  </para>
        /// </summary>
        /// <param name="_out">new signature</param>
        /// <param name="repo">repository pointer</param>
        /// <returns>
        ///  0 on success, GIT_ENOTFOUND if config is missing, or error
        ///  code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_signature_default(
            out git_signature* _out,
            git_repository* repo);

        /// <summary>
        ///  Create a new signature by parsing the given buffer, which is
        ///  expected to be in the format "Real Name <email> timestamp
        ///  tzoffset", where `timestamp` is the number of seconds since
        ///  the Unix epoch and `tzoffset` is the timezone offset in
        ///  `hhmm` format (note the lack of a colon separator).
        /// </summary>
        /// <param name="_out">new signature</param>
        /// <param name="buf">signature string</param>
        /// <returns>
        ///  0 on success, GIT_EINVALID if the signature is not
        ///  parseable, or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_signature_from_buffer(
            out git_signature* _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string buf);

        /// <summary>
        ///  Create a copy of an existing signature. All internal strings
        ///  are also duplicated.
        ///
        ///  <para>
        ///   Call `git_signature_free()` to free the data.
        ///  </para>
        /// </summary>
        /// <param name="dest">pointer where to store the copy</param>
        /// <param name="sig">signature to duplicate</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_signature_dup(
            IntPtr dest,
            ref git_signature sig);

        /// <summary>
        ///  Free an existing signature.
        ///
        ///  <para>
        ///   Because the signature is not an opaque structure, it is
        ///   legal to free it manually, but be sure to free the
        ///   "name" and "email" strings in addition to the structure
        ///   itself.
        ///  </para>
        /// </summary>
        /// <param name="sig">signature to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_signature_free(
            ref git_signature sig);
    }
}
