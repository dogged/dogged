using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public sealed class TransactionHandle : SafeHandle
    {
        public TransactionHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe TransactionHandle(git_transaction* nativeTransaction) :
            base((IntPtr)nativeTransaction, true)
        {
        }

        public override bool IsInvalid
        {
            get
            {
                return handle == IntPtr.Zero;
            }
        }

        protected override unsafe bool ReleaseHandle()
        {
            libgit2.git_transaction_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Create a new transaction object
        ///
        ///  <para>
        ///   This does not lock anything, but sets up the transaction
        ///   object to know from which repository to lock.
        ///  </para>
        /// </summary>
        /// <param name="_out">the resulting transaction</param>
        /// <param name="repo">the repository in which to lock</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_transaction_new(
            out TransactionHandle _out,
            RepositoryHandle repo);

        /// <summary>
        ///  Lock a reference
        ///
        ///  <para>
        ///   Lock the specified reference. This is the first step to
        ///   updating a reference.
        ///  </para>
        /// </summary>
        /// <param name="tx">the transaction</param>
        /// <param name="refname">the reference to lock</param>
        /// <returns>0 or an error message</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_transaction_lock_ref(
            TransactionHandle tx,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string refname);

        /// <summary>
        ///  Set the target of a reference
        ///
        ///  <para>
        ///   Set the target of the specified reference. This
        ///   reference must be locked.
        ///  </para>
        /// </summary>
        /// <param name="tx">the transaction</param>
        /// <param name="refname">reference to update</param>
        /// <param name="target">target to set the reference to</param>
        /// <param name="sig">
        ///  signature to use in the reflog; pass NULL to read the
        ///  identity from the config
        /// </param>
        /// <param name="msg">message to use in the reflog</param>
        /// <returns>
        ///  0, GIT_ENOTFOUND if the reference is not among the locked
        ///  ones, or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_transaction_set_target(
            TransactionHandle tx,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string refname,
            ref git_oid target,
            ref git_signature sig,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string msg);

        /// <summary>
        ///  Set the target of a reference
        ///
        ///  <para>
        ///   Set the target of the specified reference. This
        ///   reference must be locked.
        ///  </para>
        /// </summary>
        /// <param name="tx">the transaction</param>
        /// <param name="refname">reference to update</param>
        /// <param name="target">target to set the reference to</param>
        /// <param name="sig">
        ///  signature to use in the reflog; pass NULL to read the
        ///  identity from the config
        /// </param>
        /// <param name="msg">message to use in the reflog</param>
        /// <returns>
        ///  0, GIT_ENOTFOUND if the reference is not among the locked
        ///  ones, or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_transaction_set_symbolic_target(
            TransactionHandle tx,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string refname,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string target,
            ref git_signature sig,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string msg);

        /// <summary>
        ///  Set the reflog of a reference
        ///
        ///  <para>
        ///   Set the specified reference's reflog. If this is
        ///   combined with setting the target, that update won't be
        ///   written to the reflog.
        ///  </para>
        /// </summary>
        /// <param name="tx">the transaction</param>
        /// <param name="refname">the reference whose reflog to set</param>
        /// <param name="reflog">
        ///  the reflog as it should be written out
        /// </param>
        /// <returns>
        ///  0, GIT_ENOTFOUND if the reference is not among the locked
        ///  ones, or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_transaction_set_reflog(
            TransactionHandle tx,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string refname,
            ReflogHandle reflog);

        /// <summary>
        ///  Remove a reference
        /// </summary>
        /// <param name="tx">the transaction</param>
        /// <param name="refname">the reference to remove</param>
        /// <returns>
        ///  0, GIT_ENOTFOUND if the reference is not among the locked
        ///  ones, or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_transaction_remove(
            TransactionHandle tx,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string refname);

        /// <summary>
        ///  Commit the changes from the transaction
        ///
        ///  <para>
        ///   Perform the changes that have been queued. The updates
        ///   will be made one by one, and the first failure will stop
        ///   the processing.
        ///  </para>
        /// </summary>
        /// <param name="tx">the transaction</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_transaction_commit(
            TransactionHandle tx);

        /// <summary>
        ///  Free the resources allocated by this transaction
        ///
        ///  <para>
        ///   If any references remain locked, they will be unlocked
        ///   without any changes made to them.
        ///  </para>
        /// </summary>
        /// <param name="tx">the transaction</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_transaction_free(
            IntPtr tx);
    }
}
