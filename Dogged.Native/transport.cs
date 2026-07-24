using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Callback for messages received by the transport.
    ///
    ///  <para>
    ///   Return a negative value to cancel the network operation.
    ///  </para>
    /// </summary>
    /// <param name="str">The message from the transport</param>
    /// <param name="len">The length of the message</param>
    /// <param name="payload">Payload provided by the caller</param>
    /// <returns>0 on success or an error code</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_transport_message_cb(
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string str,
        int len,
        IntPtr payload);

    /// <summary>
    ///  Signature of a function which creates a transport.
    /// </summary>
    /// <param name="_out">the transport generate</param>
    /// <param name="owner">the owner for the transport</param>
    /// <param name="param">the param to the transport creation</param>
    /// <returns>0 on success or an error code</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_transport_cb(
        out git_transport* _out,
        git_remote* owner,
        IntPtr param);
}
