using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public static partial class libgit2
    {
        /// <summary>
        ///  Free a credential.
        ///
        ///  <para>
        ///   This is only necessary if you own the object; that is,
        ///   if you are a transport.
        ///  </para>
        /// </summary>
        /// <param name="cred">the object to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_credential_free(
            IntPtr cred);
    }
}
