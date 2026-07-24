using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
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
            ConfigHandle cfg,
            ref git_config_backend file,
            git_config_level_t level,
            RepositoryHandle repo,
            int force);
    }
}
