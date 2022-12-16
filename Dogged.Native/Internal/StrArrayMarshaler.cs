using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using Dogged.Native.Services;

namespace Dogged.Native
{
    /// <summary>
    /// A custom marshaler suitable for marshaling git_strarray to/from string[]
    /// for use in libgit2.
    /// </summary>
    internal class StrArrayMarshaler : ICustomMarshaler
    {
        /// <summary>
        /// The "ToNative" cookie will provide an instance of the marshaler
        /// for use converting managed string[] to native git_strarray.
        /// When used as the CustomMarshaler for a PInvoke context, this
        /// marshaler will free the native memory after the native function
        /// returns.
        /// </summary>
        public const string ToNative = "ToNative";

        /// <summary>
        /// The "FromNative" cookie will provide an instance of the marshaler
        /// for use converting git_strarray to managed string[].
        /// When used as the CustomMarshaler for a PInvoke context, this
        /// marshaler will not free the native memory after the native
        /// function results, since the native string is owned by the
        /// library itself.
        /// </summary>
        public const string FromNative = "FromNative";

        private static readonly StrArrayMarshaler fromNativeInstance = new StrArrayMarshaler(false);
        private static readonly StrArrayMarshaler toNativeInstance = new StrArrayMarshaler(true);

        private readonly bool cleanup;

        public static ICustomMarshaler GetInstance(string cookie)
        {
            switch (cookie)
            {
                case ToNative:
                    return toNativeInstance;
                case FromNative:
                    return fromNativeInstance;
                default:
                    throw new ArgumentException("invalid encoding cookie");
            }
        }

        private StrArrayMarshaler(bool cleanup)
        {
            this.cleanup = cleanup;
        }

        public int GetNativeDataSize()
        {
            return -1;
        }

        public unsafe IntPtr MarshalManagedToNative(Object value)
        {
            if (value == null)
            {
                return IntPtr.Zero;
            }

            var managedStrArray = value as string[];
            if (managedStrArray == null)
            {
                throw new MarshalDirectiveException("Cannot marshal a non-string[]");
            }

            var nativeStrArray = (git_strarray*)Marshal.AllocHGlobal(sizeof(git_strarray)).ToPointer();
            nativeStrArray->strings = null;
            nativeStrArray->count = new UIntPtr((uint)managedStrArray.Length);
            
            if (managedStrArray.Length > 0)
            {
                nativeStrArray->strings = (IntPtr*)Marshal.AllocHGlobal(sizeof(IntPtr) * managedStrArray.Length).ToPointer();
                for (var i = 0; i < managedStrArray.Length; i++)
                {
                    nativeStrArray->strings[i] = Utf8Converter.ToNative(managedStrArray[i]);
                }
            }

            return new IntPtr(nativeStrArray);
        }

        public unsafe object MarshalNativeToManaged(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return null;
            }

            var nativeStrArray = (git_strarray*)ptr.ToPointer();
            var managedStrArray = new string[nativeStrArray->count.ToUInt32()];

            for (var i = 0; i < managedStrArray.Length; i++)
            {
                managedStrArray[i] = Utf8Converter.FromNative((byte*)nativeStrArray->strings[i]);
            }

            return managedStrArray;
        }

        public void CleanUpManagedData(object value)
        {
        }

        public unsafe virtual void CleanUpNativeData(IntPtr ptr)
        {
            if (ptr != IntPtr.Zero && cleanup)
            {
                var nativeStrArray = (git_strarray*)ptr.ToPointer();

                var count = nativeStrArray->count.ToUInt32();
                for (var i = 0; i < count; i++)
                {
                    Marshal.FreeHGlobal(nativeStrArray->strings[i]);
                }

                Marshal.FreeHGlobal(new IntPtr(nativeStrArray->strings));
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}
