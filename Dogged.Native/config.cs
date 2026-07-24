using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Memory representation of a set of config files
    /// </summary>
    public struct git_config
    {
    }

    /// <summary>
    ///  Priority level of a config file.
    ///
    ///  <para>
    ///   These priority levels correspond to the natural escalation
    ///   logic (from higher to lower) when reading or searching for
    ///   config entries in git.git. Meaning that for the same key,
    ///   the configuration in the local configuration is preferred
    ///   over the configuration in the system configuration file.
    ///  </para>
    ///
    ///  <para>
    ///   Callers can add their own custom configuration, beginning at
    ///   the `GIT_CONFIG_LEVEL_APP` level.
    ///  </para>
    ///
    ///  <para>
    ///   Writes, by default, occur in the highest priority level
    ///   backend that is writable. This ordering can be overridden
    ///   with `git_config_set_writeorder`.
    ///  </para>
    ///
    ///  <para>
    ///   git_config_open_default() and git_repository_config() honor
    ///   those priority levels as well.
    ///  </para>
    /// </summary>
    public enum git_config_level_t
    {
        /// <summary>
        ///  System-wide on Windows, for compatibility with "Portable
        ///  Git".
        /// </summary>
        GIT_CONFIG_LEVEL_PROGRAMDATA = 1,

        /// <summary>
        ///  System-wide configuration file; `/etc/gitconfig` on Linux.
        /// </summary>
        GIT_CONFIG_LEVEL_SYSTEM = 2,

        /// <summary>
        ///  XDG compatible configuration file; typically
        ///  `~/.config/git/config`.
        /// </summary>
        GIT_CONFIG_LEVEL_XDG = 3,

        /// <summary>
        ///  Global configuration file is the user-specific
        ///  configuration; typically `~/.gitconfig`.
        /// </summary>
        GIT_CONFIG_LEVEL_GLOBAL = 4,

        /// <summary>
        ///  Local configuration, the repository-specific configuration
        ///  file; typically `$GIT_DIR/config`.
        /// </summary>
        GIT_CONFIG_LEVEL_LOCAL = 5,

        /// <summary>
        ///  Worktree-specific configuration; typically
        ///  `$GIT_DIR/config.worktree`.
        /// </summary>
        GIT_CONFIG_LEVEL_WORKTREE = 6,

        /// <summary>
        ///  Application-specific configuration file. Callers into
        ///  libgit2 can add their own configuration beginning at this
        ///  level.
        /// </summary>
        GIT_CONFIG_LEVEL_APP = 7,

        /// <summary>
        ///  Not a configuration level; callers can use this value when
        ///  querying configuration levels to specify that they want to
        ///  have data from the highest-level currently configuration.
        ///  This can be used to indicate that callers want the most
        ///  specific config file available that actually is loaded.
        /// </summary>
        GIT_CONFIG_HIGHEST_LEVEL = -1,
    }

    /// <summary>
    ///  An entry in a configuration file
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_config_entry
    {
        /// <summary>
        ///  Name of the configuration entry (normalized).
        /// </summary>
        public IntPtr name;

        /// <summary>
        ///  Literal (string) value of the entry.
        /// </summary>
        public IntPtr value;

        /// <summary>
        ///  The type of backend that this entry exists in (eg, "file").
        /// </summary>
        public IntPtr backend_type;

        /// <summary>
        ///  The path to the origin of this entry. For config files, this
        ///  is the path to the file.
        /// </summary>
        public IntPtr origin_path;

        /// <summary>
        ///  Depth of includes where this variable was found.
        /// </summary>
        public uint include_depth;

        /// <summary>
        ///  Configuration level for the file this was found in.
        /// </summary>
        public git_config_level_t level;
    }

    /// <summary>
    ///  A config enumeration callback.
    /// </summary>
    /// <param name="entry">the entry currently being enumerated</param>
    /// <param name="payload">a user-specified pointer</param>
    /// <returns>non-zero to terminate the iteration.</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_config_foreach_cb(
        ref git_config_entry entry,
        IntPtr payload);

    /// <summary>
    ///  Config var type
    /// </summary>
    public enum git_configmap_t
    {
        GIT_CONFIGMAP_FALSE = 0,
        GIT_CONFIGMAP_TRUE = 1,
        GIT_CONFIGMAP_INT32,
        GIT_CONFIGMAP_STRING,
    }

    /// <summary>
    ///  Mapping from config variables to values.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_configmap
    {
        public git_configmap_t type;

        public IntPtr str_match;

        public int map_value;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Free a config entry.
        /// </summary>
        /// <param name="entry">The entry to free.</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_config_entry_free(
            ref git_config_entry entry);

        /// <summary>
        ///  Locate the path to the global configuration file
        ///
        ///  <para>
        ///   The user or global configuration file is usually located
        ///   in `$HOME/.gitconfig`.
        ///  </para>
        ///
        ///  <para>
        ///   This method will try to guess the full path to that
        ///   file, if the file exists. The returned path may be used
        ///   on any `git_config` call to load the global
        ///   configuration file.
        ///  </para>
        ///
        ///  <para>
        ///   This method will not guess the path to the xdg
        ///   compatible config file (`.config/git/config`).
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Pointer to a user-allocated git_buf in which to store the
        ///  path
        /// </param>
        /// <returns>
        ///  0 if a global configuration file has been found. Its path
        ///  will be stored in `out`.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_find_global(
            ref git_buf _out);

        /// <summary>
        ///  Locate the path to the global xdg compatible configuration
        ///  file
        ///
        ///  <para>
        ///   The xdg compatible configuration file is usually located
        ///   in `$HOME/.config/git/config`.
        ///  </para>
        ///
        ///  <para>
        ///   This method will try to guess the full path to that
        ///   file, if the file exists. The returned path may be used
        ///   on any `git_config` call to load the xdg compatible
        ///   configuration file.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Pointer to a user-allocated git_buf in which to store the
        ///  path
        /// </param>
        /// <returns>
        ///  0 if a xdg compatible configuration file has been found. Its
        ///  path will be stored in `out`.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_find_xdg(
            ref git_buf _out);

        /// <summary>
        ///  Locate the path to the system configuration file
        ///
        ///  <para>
        ///   If `/etc/gitconfig` doesn't exist, it will look for
        ///   `%PROGRAMFILES%\Git\etc\gitconfig `.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Pointer to a user-allocated git_buf in which to store the
        ///  path
        /// </param>
        /// <returns>
        ///  0 if a system configuration file has been found. Its path
        ///  will be stored in `out`.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_find_system(
            ref git_buf _out);

        /// <summary>
        ///  Locate the path to the configuration file in ProgramData
        ///
        ///  <para>
        ///   Look for the file in `%PROGRAMDATA%\Git\config ` used by
        ///   portable git.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Pointer to a user-allocated git_buf in which to store the
        ///  path
        /// </param>
        /// <returns>
        ///  0 if a ProgramData configuration file has been found. Its
        ///  path will be stored in `out`.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_find_programdata(
            ref git_buf _out);

        /// <summary>
        ///  Open the global, XDG and system configuration files
        ///
        ///  <para>
        ///   Utility wrapper that finds the global, XDG and system
        ///   configuration files and opens them into a single
        ///   prioritized config object that can be used when
        ///   accessing default config data outside a repository.
        ///  </para>
        /// </summary>
        /// <param name="_out">Pointer to store the config instance</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_open_default(
            out git_config* _out);

        /// <summary>
        ///  Allocate a new configuration object
        ///
        ///  <para>
        ///   This object is empty, so you have to add a file to it
        ///   before you can do anything with it.
        ///  </para>
        /// </summary>
        /// <param name="_out">pointer to the new configuration</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_new(
            out git_config* _out);

        /// <summary>
        ///  Add an on-disk config file instance to an existing config
        ///
        ///  <para>
        ///   The on-disk file pointed at by `path` will be opened and
        ///   parsed; it's expected to be a native Git config file
        ///   following the default Git config syntax (see man
        ///   git-config).
        ///  </para>
        ///
        ///  <para>
        ///   If the file does not exist, the file will still be added
        ///   and it will be created the first time we write to it.
        ///  </para>
        ///
        ///  <para>
        ///   Note that the configuration object will free the file
        ///   automatically.
        ///  </para>
        ///
        ///  <para>
        ///   Further queries on this config object will access each
        ///   of the config file instances in order (instances with a
        ///   higher priority level will be accessed first).
        ///  </para>
        /// </summary>
        /// <param name="cfg">the configuration to add the file to</param>
        /// <param name="path">path to the configuration file to add</param>
        /// <param name="level">the priority level of the backend</param>
        /// <param name="repo">
        ///  optional repository to allow parsing of conditional includes
        /// </param>
        /// <param name="force">
        ///  replace config file at the given priority level
        /// </param>
        /// <returns>
        ///  0 on success, GIT_EEXISTS when adding more than one file for
        ///  a given priority level (and force_replace set to 0),
        ///  GIT_ENOTFOUND when the file doesn't exist or error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_add_file_ondisk(
            git_config* cfg,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path,
            git_config_level_t level,
            git_repository* repo,
            int force);

        /// <summary>
        ///  Create a new config instance containing a single on-disk
        ///  file
        ///
        ///  <para>
        ///   This method is a simple utility wrapper for the
        ///   following sequence of calls: - git_config_new -
        ///   git_config_add_file_ondisk
        ///  </para>
        /// </summary>
        /// <param name="_out">The configuration instance to create</param>
        /// <param name="path">Path to the on-disk file to open</param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_open_ondisk(
            out git_config* _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path);

        /// <summary>
        ///  Build a single-level focused config object from a
        ///  multi-level one.
        ///
        ///  <para>
        ///   The returned config object can be used to perform
        ///   get/set/delete operations on a single specific level.
        ///  </para>
        ///
        ///  <para>
        ///   Getting several times the same level from the same
        ///   parent multi-level config will return different config
        ///   instances, but containing the same config_file instance.
        ///  </para>
        /// </summary>
        /// <param name="_out">The configuration instance to create</param>
        /// <param name="parent">
        ///  Multi-level config to search for the given level
        /// </param>
        /// <param name="level">Configuration level to search for</param>
        /// <returns>
        ///  0, GIT_ENOTFOUND if the passed level cannot be found in the
        ///  multi-level parent config, or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_open_level(
            out git_config* _out,
            git_config* parent,
            git_config_level_t level);

        /// <summary>
        ///  Open the global/XDG configuration file according to git's
        ///  rules
        ///
        ///  <para>
        ///   Git allows you to store your global configuration at
        ///   `$HOME/.gitconfig` or `$XDG_CONFIG_HOME/git/config`. For
        ///   backwards compatibility, the XDG file shouldn't be used
        ///   unless the use has created it explicitly. With this
        ///   function you'll open the correct one to write to.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  pointer in which to store the config object
        /// </param>
        /// <param name="config">the config object in which to look</param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_open_global(
            out git_config* _out,
            git_config* config);

        /// <summary>
        ///  Set the write order for configuration backends. By default,
        ///  the write ordering does not match the read ordering; for
        ///  example, the worktree configuration is a high-priority for
        ///  reading, but is not written to unless explicitly chosen.
        /// </summary>
        /// <param name="cfg">
        ///  the configuration to change write order of
        /// </param>
        /// <param name="levels">the ordering of levels for writing</param>
        /// <param name="len">the length of the levels array</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_set_writeorder(
            git_config* cfg,
            git_config_level_t* levels,
            UIntPtr len);

        /// <summary>
        ///  Create a snapshot of the configuration
        ///
        ///  <para>
        ///   Create a snapshot of the current state of a
        ///   configuration, which allows you to look into a
        ///   consistent view of the configuration for looking up
        ///   complex values (e.g. a remote, submodule).
        ///  </para>
        ///
        ///  <para>
        ///   The string returned when querying such a config object
        ///   is valid until it is freed.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  pointer in which to store the snapshot config object
        /// </param>
        /// <param name="config">configuration to snapshot</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_snapshot(
            out git_config* _out,
            git_config* config);

        /// <summary>
        ///  Free the configuration and its associated memory and files
        /// </summary>
        /// <param name="cfg">the configuration to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_config_free(
            git_config* cfg);

        /// <summary>
        ///  Get the git_config_entry of a config variable.
        ///
        ///  <para>
        ///   Free the git_config_entry after use with
        ///   `git_config_entry_free()`.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  pointer to the variable git_config_entry
        /// </param>
        /// <param name="cfg">where to look for the variable</param>
        /// <param name="name">the variable's name</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_get_entry(
            out git_config_entry* _out,
            git_config* cfg,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Get the value of an integer config variable.
        ///
        ///  <para>
        ///   All config files will be looked into, in the order of
        ///   their defined level. A higher level means a higher
        ///   priority. The first occurrence of the variable will be
        ///   returned here.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  pointer to the variable where the value should be stored
        /// </param>
        /// <param name="cfg">where to look for the variable</param>
        /// <param name="name">the variable's name</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_get_int32(
            out int _out,
            git_config* cfg,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Get the value of a long integer config variable.
        ///
        ///  <para>
        ///   All config files will be looked into, in the order of
        ///   their defined level. A higher level means a higher
        ///   priority. The first occurrence of the variable will be
        ///   returned here.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  pointer to the variable where the value should be stored
        /// </param>
        /// <param name="cfg">where to look for the variable</param>
        /// <param name="name">the variable's name</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_get_int64(
            out long _out,
            git_config* cfg,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Get the value of a boolean config variable.
        ///
        ///  <para>
        ///   This function uses the usual C convention of 0 being
        ///   false and anything else true.
        ///  </para>
        ///
        ///  <para>
        ///   All config files will be looked into, in the order of
        ///   their defined level. A higher level means a higher
        ///   priority. The first occurrence of the variable will be
        ///   returned here.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  pointer to the variable where the value should be stored
        /// </param>
        /// <param name="cfg">where to look for the variable</param>
        /// <param name="name">the variable's name</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_get_bool(
            out int _out,
            git_config* cfg,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Get the value of a path config variable.
        ///
        ///  <para>
        ///   A leading '~' will be expanded to the global search path
        ///   (which defaults to the user's home directory but can be
        ///   overridden via `git_libgit2_opts()`.
        ///  </para>
        ///
        ///  <para>
        ///   All config files will be looked into, in the order of
        ///   their defined level. A higher level means a higher
        ///   priority. The first occurrence of the variable will be
        ///   returned here.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  the buffer in which to store the result
        /// </param>
        /// <param name="cfg">where to look for the variable</param>
        /// <param name="name">the variable's name</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_get_path(
            ref git_buf _out,
            git_config* cfg,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Get the value of a string config variable.
        ///
        ///  <para>
        ///   This function can only be used on snapshot config
        ///   objects. The string is owned by the config and should
        ///   not be freed by the user. The pointer will be valid
        ///   until the config is freed.
        ///  </para>
        ///
        ///  <para>
        ///   All config files will be looked into, in the order of
        ///   their defined level. A higher level means a higher
        ///   priority. The first occurrence of the variable will be
        ///   returned here.
        ///  </para>
        /// </summary>
        /// <param name="_out">pointer to the string</param>
        /// <param name="cfg">where to look for the variable</param>
        /// <param name="name">the variable's name</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_get_string(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))] out string _out,
            git_config* cfg,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Get the value of a string config variable.
        ///
        ///  <para>
        ///   The value of the config will be copied into the buffer.
        ///  </para>
        ///
        ///  <para>
        ///   All config files will be looked into, in the order of
        ///   their defined level. A higher level means a higher
        ///   priority. The first occurrence of the variable will be
        ///   returned here.
        ///  </para>
        /// </summary>
        /// <param name="_out">buffer in which to store the string</param>
        /// <param name="cfg">where to look for the variable</param>
        /// <param name="name">the variable's name</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_get_string_buf(
            ref git_buf _out,
            git_config* cfg,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Get each value of a multivar in a foreach callback
        ///
        ///  <para>
        ///   The callback will be called on each variable found
        ///  </para>
        ///
        ///  <para>
        ///   The regular expression is applied case-sensitively on
        ///   the normalized form of the variable name: the section
        ///   and variable parts are lower-cased. The subsection is
        ///   left unchanged.
        ///  </para>
        /// </summary>
        /// <param name="cfg">where to look for the variable</param>
        /// <param name="name">the variable's name</param>
        /// <param name="regexp">
        ///  regular expression to filter which variables we're
        ///  interested in. Use NULL to indicate all
        /// </param>
        /// <param name="callback">
        ///  the function to be called on each value of the variable
        /// </param>
        /// <param name="payload">
        ///  opaque pointer to pass to the callback
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_get_multivar_foreach(
            git_config* cfg,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string regexp,
            git_config_foreach_cb callback,
            IntPtr payload);

        /// <summary>
        ///  Get each value of a multivar
        ///
        ///  <para>
        ///   The regular expression is applied case-sensitively on
        ///   the normalized form of the variable name: the section
        ///   and variable parts are lower-cased. The subsection is
        ///   left unchanged.
        ///  </para>
        /// </summary>
        /// <param name="_out">pointer to store the iterator</param>
        /// <param name="cfg">where to look for the variable</param>
        /// <param name="name">the variable's name</param>
        /// <param name="regexp">
        ///  regular expression to filter which variables we're
        ///  interested in. Use NULL to indicate all
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_multivar_iterator_new(
            out git_config_iterator* _out,
            git_config* cfg,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string regexp);

        /// <summary>
        ///  Return the current entry and advance the iterator
        ///
        ///  <para>
        ///   The pointers returned by this function are valid until
        ///   the next call to `git_config_next` or until the iterator
        ///   is freed.
        ///  </para>
        /// </summary>
        /// <param name="entry">pointer to store the entry</param>
        /// <param name="iter">the iterator</param>
        /// <returns>
        ///  0 or an error code. GIT_ITEROVER if the iteration has
        ///  completed
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_next(
            out git_config_entry* entry,
            ref git_config_iterator iter);

        /// <summary>
        ///  Free a config iterator
        /// </summary>
        /// <param name="iter">the iterator to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_config_iterator_free(
            ref git_config_iterator iter);

        /// <summary>
        ///  Set the value of an integer config variable in the config
        ///  file with the highest level (usually the local one).
        /// </summary>
        /// <param name="cfg">where to look for the variable</param>
        /// <param name="name">the variable's name</param>
        /// <param name="value">Integer value for the variable</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_set_int32(
            git_config* cfg,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            int value);

        /// <summary>
        ///  Set the value of a long integer config variable in the
        ///  config file with the highest level (usually the local one).
        /// </summary>
        /// <param name="cfg">where to look for the variable</param>
        /// <param name="name">the variable's name</param>
        /// <param name="value">Long integer value for the variable</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_set_int64(
            git_config* cfg,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            long value);

        /// <summary>
        ///  Set the value of a boolean config variable in the config
        ///  file with the highest level (usually the local one).
        /// </summary>
        /// <param name="cfg">where to look for the variable</param>
        /// <param name="name">the variable's name</param>
        /// <param name="value">the value to store</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_set_bool(
            git_config* cfg,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            int value);

        /// <summary>
        ///  Set the value of a string config variable in the config file
        ///  with the highest level (usually the local one).
        ///
        ///  <para>
        ///   A copy of the string is made and the user is free to use
        ///   it afterwards.
        ///  </para>
        /// </summary>
        /// <param name="cfg">where to look for the variable</param>
        /// <param name="name">the variable's name</param>
        /// <param name="value">the string to store.</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_set_string(
            git_config* cfg,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string value);

        /// <summary>
        ///  Set a multivar in the local config file.
        ///
        ///  <para>
        ///   The regular expression is applied case-sensitively on
        ///   the value.
        ///  </para>
        /// </summary>
        /// <param name="cfg">where to look for the variable</param>
        /// <param name="name">the variable's name</param>
        /// <param name="regexp">
        ///  a regular expression to indicate which values to replace
        /// </param>
        /// <param name="value">the new value.</param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_set_multivar(
            git_config* cfg,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string regexp,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string value);

        /// <summary>
        ///  Delete a config variable from the config file with the
        ///  highest level (usually the local one).
        /// </summary>
        /// <param name="cfg">the configuration</param>
        /// <param name="name">the variable to delete</param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_delete_entry(
            git_config* cfg,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Deletes one or several entries from a multivar in the local
        ///  config file.
        ///
        ///  <para>
        ///   The regular expression is applied case-sensitively on
        ///   the value.
        ///  </para>
        /// </summary>
        /// <param name="cfg">where to look for the variables</param>
        /// <param name="name">the variable's name</param>
        /// <param name="regexp">
        ///  a regular expression to indicate which values to delete
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_delete_multivar(
            git_config* cfg,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string regexp);

        /// <summary>
        ///  Perform an operation on each config variable.
        ///
        ///  <para>
        ///   The callback receives the normalized name and value of
        ///   each variable in the config backend, and the data
        ///   pointer passed to this function. If the callback returns
        ///   a non-zero value, the function stops iterating and
        ///   returns that value to the caller.
        ///  </para>
        ///
        ///  <para>
        ///   The pointers passed to the callback are only valid as
        ///   long as the iteration is ongoing.
        ///  </para>
        /// </summary>
        /// <param name="cfg">where to get the variables from</param>
        /// <param name="callback">
        ///  the function to call on each variable
        /// </param>
        /// <param name="payload">the data to pass to the callback</param>
        /// <returns>
        ///  0 on success, non-zero callback return value, or error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_foreach(
            git_config* cfg,
            git_config_foreach_cb callback,
            IntPtr payload);

        /// <summary>
        ///  Iterate over all the config variables
        ///
        ///  <para>
        ///   Use `git_config_next` to advance the iteration and
        ///   `git_config_iterator_free` when done.
        ///  </para>
        /// </summary>
        /// <param name="_out">pointer to store the iterator</param>
        /// <param name="cfg">where to get the variables from</param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_iterator_new(
            out git_config_iterator* _out,
            git_config* cfg);

        /// <summary>
        ///  Iterate over all the config variables whose name matches a
        ///  pattern
        ///
        ///  <para>
        ///   Use `git_config_next` to advance the iteration and
        ///   `git_config_iterator_free` when done.
        ///  </para>
        ///
        ///  <para>
        ///   The regular expression is applied case-sensitively on
        ///   the normalized form of the variable name: the section
        ///   and variable parts are lower-cased. The subsection is
        ///   left unchanged.
        ///  </para>
        /// </summary>
        /// <param name="_out">pointer to store the iterator</param>
        /// <param name="cfg">where to ge the variables from</param>
        /// <param name="regexp">
        ///  regular expression to match the names
        /// </param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_iterator_glob_new(
            out git_config_iterator* _out,
            git_config* cfg,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string regexp);

        /// <summary>
        ///  Perform an operation on each config variable matching a
        ///  regular expression.
        ///
        ///  <para>
        ///   This behaves like `git_config_foreach` with an
        ///   additional filter of a regular expression that filters
        ///   which config keys are passed to the callback.
        ///  </para>
        ///
        ///  <para>
        ///   The regular expression is applied case-sensitively on
        ///   the normalized form of the variable name: the section
        ///   and variable parts are lower-cased. The subsection is
        ///   left unchanged.
        ///  </para>
        ///
        ///  <para>
        ///   The regular expression is applied case-sensitively on
        ///   the normalized form of the variable name: the
        ///   case-insensitive parts are lower-case.
        ///  </para>
        /// </summary>
        /// <param name="cfg">where to get the variables from</param>
        /// <param name="regexp">
        ///  regular expression to match against config names
        /// </param>
        /// <param name="callback">
        ///  the function to call on each variable
        /// </param>
        /// <param name="payload">the data to pass to the callback</param>
        /// <returns>
        ///  0 or the return value of the callback which didn't return 0
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_foreach_match(
            git_config* cfg,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string regexp,
            git_config_foreach_cb callback,
            IntPtr payload);

        /// <summary>
        ///  Query the value of a config variable and return it mapped to
        ///  an integer constant.
        ///
        ///  <para>
        ///   This is a helper method to easily map different possible
        ///   values to a variable to integer constants that easily
        ///   identify them.
        ///  </para>
        ///
        ///  <para>
        ///   A mapping array looks as follows:
        ///  </para>
        ///
        ///  <para>
        ///   git_configmap autocrlf_mapping[] = { {GIT_CVAR_FALSE,
        ///   NULL, GIT_AUTO_CRLF_FALSE}, {GIT_CVAR_TRUE, NULL,
        ///   GIT_AUTO_CRLF_TRUE}, {GIT_CVAR_STRING, "input",
        ///   GIT_AUTO_CRLF_INPUT}, {GIT_CVAR_STRING, "default",
        ///   GIT_AUTO_CRLF_DEFAULT}};
        ///  </para>
        ///
        ///  <para>
        ///   On any "false" value for the variable (e.g. "false",
        ///   "FALSE", "no"), the mapping will store
        ///   `GIT_AUTO_CRLF_FALSE` in the `out` parameter.
        ///  </para>
        ///
        ///  <para>
        ///   The same thing applies for any "true" value such as
        ///   "true", "yes" or "1", storing the `GIT_AUTO_CRLF_TRUE`
        ///   variable.
        ///  </para>
        ///
        ///  <para>
        ///   Otherwise, if the value matches the string "input" (with
        ///   case insensitive comparison), the given constant will be
        ///   stored in `out`, and likewise for "default".
        ///  </para>
        ///
        ///  <para>
        ///   If not a single match can be made to store in `out`, an
        ///   error code will be returned.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  place to store the result of the mapping
        /// </param>
        /// <param name="cfg">config file to get the variables from</param>
        /// <param name="name">name of the config variable to lookup</param>
        /// <param name="maps">
        ///  array of `git_configmap` objects specifying the possible
        ///  mappings
        /// </param>
        /// <param name="map_n">number of mapping objects in `maps`</param>
        /// <returns>0 on success, error code otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_get_mapped(
            out int _out,
            git_config* cfg,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            ref git_configmap maps,
            UIntPtr map_n);

        /// <summary>
        ///  Maps a string value to an integer constant
        /// </summary>
        /// <param name="_out">
        ///  place to store the result of the parsing
        /// </param>
        /// <param name="maps">
        ///  array of `git_configmap` objects specifying the possible
        ///  mappings
        /// </param>
        /// <param name="map_n">number of mapping objects in `maps`</param>
        /// <param name="value">value to parse</param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_lookup_map_value(
            out int _out,
            ref git_configmap maps,
            UIntPtr map_n,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string value);

        /// <summary>
        ///  Parse a string value as a bool.
        ///
        ///  <para>
        ///   Valid values for true are: 'true', 'yes', 'on', 1 or any
        ///   number different from 0 Valid values for false are:
        ///   'false', 'no', 'off', 0
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  place to store the result of the parsing
        /// </param>
        /// <param name="value">value to parse</param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_parse_bool(
            out int _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string value);

        /// <summary>
        ///  Parse a string value as an int32.
        ///
        ///  <para>
        ///   An optional value suffix of 'k', 'm', or 'g' will cause
        ///   the value to be multiplied by 1024, 1048576, or
        ///   1073741824 prior to output.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  place to store the result of the parsing
        /// </param>
        /// <param name="value">value to parse</param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_parse_int32(
            out int _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string value);

        /// <summary>
        ///  Parse a string value as an int64.
        ///
        ///  <para>
        ///   An optional value suffix of 'k', 'm', or 'g' will cause
        ///   the value to be multiplied by 1024, 1048576, or
        ///   1073741824 prior to output.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  place to store the result of the parsing
        /// </param>
        /// <param name="value">value to parse</param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_parse_int64(
            out long _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string value);

        /// <summary>
        ///  Parse a string value as a path.
        ///
        ///  <para>
        ///   A leading '~' will be expanded to the global search path
        ///   (which defaults to the user's home directory but can be
        ///   overridden via `git_libgit2_opts()`.
        ///  </para>
        ///
        ///  <para>
        ///   If the value does not begin with a tilde, the input will
        ///   be returned.
        ///  </para>
        /// </summary>
        /// <param name="_out">placae to store the result of parsing</param>
        /// <param name="value">the path to evaluate</param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_parse_path(
            ref git_buf _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string value);

        /// <summary>
        ///  Perform an operation on each config variable in a given
        ///  config backend, matching a regular expression.
        ///
        ///  <para>
        ///   This behaves like `git_config_foreach_match` except that
        ///   only config entries from the given backend entry are
        ///   enumerated.
        ///  </para>
        ///
        ///  <para>
        ///   The regular expression is applied case-sensitively on
        ///   the normalized form of the variable name: the section
        ///   and variable parts are lower-cased. The subsection is
        ///   left unchanged.
        ///  </para>
        /// </summary>
        /// <param name="backend">where to get the variables from</param>
        /// <param name="regexp">
        ///  regular expression to match against config names (can be
        ///  NULL)
        /// </param>
        /// <param name="callback">
        ///  the function to call on each variable
        /// </param>
        /// <param name="payload">the data to pass to the callback</param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_backend_foreach_match(
            ref git_config_backend backend,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string regexp,
            git_config_foreach_cb callback,
            IntPtr payload);

        /// <summary>
        ///  Lock the backend with the highest priority
        ///
        ///  <para>
        ///   Locking disallows anybody else from writing to that
        ///   backend. Any updates made after locking will not be
        ///   visible to a reader until the file is unlocked.
        ///  </para>
        ///
        ///  <para>
        ///   You can apply the changes by calling
        ///   `git_transaction_commit()` before freeing the
        ///   transaction. Either of these actions will unlock the
        ///   config.
        ///  </para>
        /// </summary>
        /// <param name="tx">
        ///  the resulting transaction, use this to commit or undo the
        ///  changes
        /// </param>
        /// <param name="cfg">the configuration in which to lock</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_lock(
            git_transaction** tx,
            git_config* cfg);
    }
}
