using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public sealed class ReflogHandle : SafeHandle
    {
        public ReflogHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe ReflogHandle(git_reflog* nativeReflog) :
            base((IntPtr)nativeReflog, true)
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
            libgit2.git_reflog_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Read the reflog for the given reference
        ///
        ///  <para>
        ///   If there is no reflog file for the given reference yet,
        ///   an empty reflog object will be returned.
        ///  </para>
        ///
        ///  <para>
        ///   The reflog must be freed manually by using
        ///   git_reflog_free().
        ///  </para>
        /// </summary>
        /// <param name="_out">pointer to reflog</param>
        /// <param name="repo">the repository</param>
        /// <param name="name">reference to look up</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reflog_read(
            out ReflogHandle _out,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Write an existing in-memory reflog object back to disk using
        ///  an atomic file lock.
        /// </summary>
        /// <param name="reflog">an existing reflog object</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reflog_write(
            ReflogHandle reflog);

        /// <summary>
        ///  Add a new entry to the in-memory reflog.
        ///
        ///  <para>
        ///   `msg` is optional and can be NULL.
        ///  </para>
        /// </summary>
        /// <param name="reflog">an existing reflog object</param>
        /// <param name="id">
        ///  the OID the reference is now pointing to
        /// </param>
        /// <param name="committer">the signature of the committer</param>
        /// <param name="msg">the reflog message</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reflog_append(
            ReflogHandle reflog,
            ref git_oid id,
            ref git_signature committer,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string msg);

        /// <summary>
        ///  Rename a reflog
        ///
        ///  <para>
        ///   The reflog to be renamed is expected to already exist
        ///  </para>
        ///
        ///  <para>
        ///   The new name will be checked for validity. See
        ///   `git_reference_create_symbolic()` for rules about valid
        ///   names.
        ///  </para>
        /// </summary>
        /// <param name="repo">the repository</param>
        /// <param name="old_name">the old name of the reference</param>
        /// <param name="name">the new name of the reference</param>
        /// <returns>
        ///  0 on success, GIT_EINVALIDSPEC or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reflog_rename(
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string old_name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Delete the reflog for the given reference
        /// </summary>
        /// <param name="repo">the repository</param>
        /// <param name="name">the reflog to delete</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reflog_delete(
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Get the number of log entries in a reflog
        /// </summary>
        /// <param name="reflog">the previously loaded reflog</param>
        /// <returns>the number of log entries</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_reflog_entrycount(
            ReflogHandle reflog);

        /// <summary>
        ///  Lookup an entry by its index
        ///
        ///  <para>
        ///   Requesting the reflog entry with an index of 0 (zero)
        ///   will return the most recently created entry.
        ///  </para>
        /// </summary>
        /// <param name="reflog">a previously loaded reflog</param>
        /// <param name="idx">
        ///  the position of the entry to lookup. Should be greater than
        ///  or equal to 0 (zero) and less than
        ///  `git_reflog_entrycount()`.
        /// </param>
        /// <returns>the entry; NULL if not found</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_reflog_entry* git_reflog_entry_byindex(
            ReflogHandle reflog,
            UIntPtr idx);

        /// <summary>
        ///  Remove an entry from the reflog by its index
        ///
        ///  <para>
        ///   To ensure there's no gap in the log history, set
        ///   `rewrite_previous_entry` param value to 1. When deleting
        ///   entry `n`, member old_oid of entry `n-1` (if any) will
        ///   be updated with the value of member new_oid of entry
        ///   `n+1`.
        ///  </para>
        /// </summary>
        /// <param name="reflog">a previously loaded reflog.</param>
        /// <param name="idx">
        ///  the position of the entry to remove. Should be greater than
        ///  or equal to 0 (zero) and less than
        ///  `git_reflog_entrycount()`.
        /// </param>
        /// <param name="rewrite_previous_entry">
        ///  1 to rewrite the history; 0 otherwise.
        /// </param>
        /// <returns>
        ///  0 on success, GIT_ENOTFOUND if the entry doesn't exist or an
        ///  error code.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reflog_drop(
            ReflogHandle reflog,
            UIntPtr idx,
            int rewrite_previous_entry);

        /// <summary>
        ///  Free the reflog
        /// </summary>
        /// <param name="reflog">reflog to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_reflog_free(
            IntPtr reflog);
    }
}
