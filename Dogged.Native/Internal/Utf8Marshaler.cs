using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using Dogged.Native.Services;

namespace Dogged.Native
{
    /// <summary>
    /// A custom marshaler suitable for marshaling managed strings to and
    /// from UTF8 byte arrays suitable for use in libgit2.
    /// </summary>
    internal class Utf8Marshaler : ICustomMarshaler
    {
        /// <summary>
        /// The "ToNative" cookie will provide an instance of the marshaler
        /// for use converting managed strings to native UTF8 byte arrays.
        /// When used as the CustomMarshaler for a PInvoke context, this
        /// marshaler will free the native memory after the native function
        /// returns.
        /// </summary>
        public const string ToNative = "ToNative";

        /// <summary>
        /// The "FromNative" cookie will provide an instance of the marshaler
        /// for use converting native UTF8 byte arrays to managed strings.
        /// When used as the CustomMarshaler for a PInvoke context, this
        /// marshaler will not free the native memory after the native
        /// function results, since the native string is owned by the
        /// library itself.
        /// </summary>
        public const string FromNative = "FromNative";

        private static readonly Utf8Marshaler fromNativeInstance = new Utf8Marshaler(false);
        private static readonly Utf8Marshaler toNativeInstance = new Utf8Marshaler(true);

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

        private Utf8Marshaler(bool cleanup)
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

            var str = value as string;

            if (str == null)
            {
                throw new MarshalDirectiveException("Cannot marshal a non-string");
            }

            return Utf8Converter.ToNative(str);
        }

        public unsafe object MarshalNativeToManaged(IntPtr ptr)
        {
            return Utf8Converter.FromNative((byte*)ptr);
        }

        public void CleanUpManagedData(object value)
        {
        }

        public virtual void CleanUpNativeData(IntPtr ptr)
        {
            if (ptr != IntPtr.Zero && cleanup)
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}
