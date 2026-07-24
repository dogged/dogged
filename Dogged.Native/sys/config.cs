using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  An entry in a configuration backend. This is provided so that
    ///  backend implementors can have a mechanism to free their data.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_config_backend_entry
    {
        /// <summary>
        ///  The base configuration entry
        /// </summary>
        public git_config_entry entry;

        /// <summary>
        ///  Free function for this entry; for internal purposes. Callers
        ///  should call `git_config_entry_free` to free data.
        /// </summary>
        public IntPtr free;
    }

    /// <summary>
    ///  Every iterator must have this struct as its first element, so
    ///  the API can talk to it. You'd define your iterator as
    ///
    ///  <para>
    ///   struct my_iterator { git_config_iterator parent; ... }
    ///  </para>
    ///
    ///  <para>
    ///   and assign `iter->parent.backend` to your
    ///   `git_config_backend`.
    ///  </para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_config_iterator
    {
        public IntPtr backend;

        public uint flags;

        /// <summary>
        ///  Return the current entry and advance the iterator. The
        ///  memory belongs to the library.
        /// </summary>
        public IntPtr next;

        /// <summary>
        ///  Free the iterator
        /// </summary>
        public IntPtr free;
    }

    /// <summary>
    ///  Generic backend that implements the interface to access a
    ///  configuration file
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_config_backend
    {
        public uint version;

        /// <summary>
        ///  True if this backend is for a snapshot
        /// </summary>
        public int _readonly;

        public IntPtr cfg;

        public IntPtr open;

        public IntPtr get;

        public IntPtr set;

        public IntPtr set_multivar;

        public IntPtr del;

        public IntPtr del_multivar;

        public IntPtr iterator;

        /// <summary>
        ///  Produce a read-only version of this backend
        /// </summary>
        public IntPtr snapshot;

        /// <summary>
        ///  Lock this backend.
        ///
        ///  <para>
        ///   Prevent any writes to the data store backing this
        ///   backend. Any updates must not be visible to any other
        ///   readers.
        ///  </para>
        /// </summary>
        public IntPtr _lock;

        /// <summary>
        ///  Unlock the data store backing this backend. If success is
        ///  true, the changes should be committed, otherwise rolled
        ///  back.
        /// </summary>
        public IntPtr unlock;

        public IntPtr free;
    }

    /// <summary>
    ///  Options for in-memory configuration backends.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_config_backend_memory_options
    {
        /// <summary>
        /// Current version of the options structure.
        /// </summary>
        public static readonly uint GIT_CONFIG_BACKEND_MEMORY_OPTIONS_VERSION = 1;


        public uint version;

        /// <summary>
        ///  The type of this backend (eg, "command line"). If this is
        ///  NULL, then this will be "in-memory".
        /// </summary>
        public IntPtr backend_type;

        /// <summary>
        ///  The path to the origin; if this is NULL then it will be left
        ///  unset in the resulting configuration entries.
        /// </summary>
        public IntPtr origin_path;
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Initializes a `git_config_backend` with default values.
        ///  Equivalent to creating an instance with
        ///  GIT_CONFIG_BACKEND_INIT.
        /// </summary>
        /// <param name="backend">
        ///  the `git_config_backend` struct to initialize.
        /// </param>
        /// <param name="version">
        ///  Version of struct; pass `GIT_CONFIG_BACKEND_VERSION`
        /// </param>
        /// <returns>Zero on success; -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_init_backend(
            ref git_config_backend backend,
            uint version);

        /// <summary>
        ///  Add a generic config file instance to an existing config
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
        /// <param name="file">
        ///  the configuration file (backend) to add
        /// </param>
        /// <param name="level">the priority level of the backend</param>
        /// <param name="repo">
        ///  optional repository to allow parsing of conditional includes
        /// </param>
        /// <param name="force">
        ///  if a config file already exists for the given priority
        ///  level, replace it
        /// </param>
        /// <returns>
        ///  0 on success, GIT_EEXISTS when adding more than one file for
        ///  a given priority level (and force_replace set to 0), or
        ///  error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_add_backend(
            git_config* cfg,
            ref git_config_backend file,
            git_config_level_t level,
            git_repository* repo,
            int force);

        /// <summary>
        ///  Initialize git_config_backend_memory_options structure
        ///
        ///  <para>
        ///   Initialize a `git_config_backend_memory_options` with
        ///   default values. Equivalent to creating an instance with
        ///   GIT_CONFIG_BACKEND_MEMORY_OPTIONS_INIT.
        ///  </para>
        /// </summary>
        /// <param name="opts">
        ///  The `git_config_backend_memory_options` struct to
        ///  initialize.
        /// </param>
        /// <param name="version">
        ///  The struct version; pass
        ///  `GIT_CONFIG_BACKEND_MEMORY_OPTIONS_VERSION`
        /// </param>
        /// <returns>0 on success or -1 on failure.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_backend_memory_options_init(
            ref git_config_backend_memory_options opts,
            uint version);

        /// <summary>
        ///  Create an in-memory configuration backend from a string in
        ///  standard git configuration file format.
        /// </summary>
        /// <param name="_out">the new backend</param>
        /// <param name="cfg">the configuration that is to be parsed</param>
        /// <param name="len">
        ///  the length of the string pointed to by `cfg`
        /// </param>
        /// <param name="opts">
        ///  the options to initialize this backend with, or NULL
        /// </param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_backend_from_string(
            out git_config_backend* _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string cfg,
            UIntPtr len,
            ref git_config_backend_memory_options opts);

        /// <summary>
        ///  Create an in-memory configuration backend from a list of
        ///  name/value pairs.
        /// </summary>
        /// <param name="_out">the new backend</param>
        /// <param name="values">
        ///  the configuration values to set (in "key=value" format)
        /// </param>
        /// <param name="len">the length of the values array</param>
        /// <param name="opts">
        ///  the options to initialize this backend with, or NULL
        /// </param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_config_backend_from_values(
            out git_config_backend* _out,
            char** values,
            UIntPtr len,
            ref git_config_backend_memory_options opts);
    }
}
