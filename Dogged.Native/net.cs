using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Direction of the connection.
    ///
    ///  <para>
    ///   We need this because we need to know whether we should call
    ///   git-upload-pack or git-receive-pack on the remote end when
    ///   get_refs gets called.
    ///  </para>
    /// </summary>
    public enum git_direction
    {
        GIT_DIRECTION_FETCH = 0,
        GIT_DIRECTION_PUSH = 1,
    }

    /// <summary>
    ///  Description of a reference advertised by a remote server, given
    ///  out on `ls` calls.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct git_remote_head
    {
        public int local;

        public git_oid oid;

        public git_oid loid;

        public IntPtr name;

        /// <summary>
        ///  If the server send a symref mapping for this ref, this will
        ///  point to the target.
        /// </summary>
        public IntPtr symref_target;
    }
}
