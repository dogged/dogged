using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
        /// <summary>
        ///  Initialize the OpenSSL locks
        ///
        ///  <para>
        ///   OpenSSL requires the application to determine how it
        ///   performs locking.
        ///  </para>
        ///
        ///  <para>
        ///   This is a last-resort convenience function which libgit2
        ///   provides for allocating and initializing the locks as
        ///   well as setting the locking function to use the system's
        ///   native locking functions.
        ///  </para>
        ///
        ///  <para>
        ///   The locking function will be cleared and the memory will
        ///   be freed when you call git_threads_sutdown().
        ///  </para>
        ///
        ///  <para>
        ///   If your programming language has an OpenSSL
        ///   package/bindings, it likely sets up locking. You should
        ///   very strongly prefer that over this function.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  0 on success, -1 if there are errors or if libgit2 was not
        ///  built with OpenSSL and threading support.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_openssl_set_locking();
    }
}
