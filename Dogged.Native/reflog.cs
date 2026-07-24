using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Representation of a reference log
    /// </summary>
    public struct git_reflog
    {
    }

    /// <summary>
    ///  Representation of a reference log entry
    /// </summary>
    public struct git_reflog_entry
    {
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
            out git_reflog* _out,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Write an existing in-memory reflog object back to disk using
        ///  an atomic file lock.
        /// </summary>
        /// <param name="reflog">an existing reflog object</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reflog_write(
            git_reflog* reflog);

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
            git_reflog* reflog,
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
            git_repository* repo,
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
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Get the number of log entries in a reflog
        /// </summary>
        /// <param name="reflog">the previously loaded reflog</param>
        /// <returns>the number of log entries</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe UIntPtr git_reflog_entrycount(
            git_reflog* reflog);

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
            git_reflog* reflog,
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
            git_reflog* reflog,
            UIntPtr idx,
            int rewrite_previous_entry);

        /// <summary>
        ///  Get the old oid
        /// </summary>
        /// <param name="entry">a reflog entry</param>
        /// <returns>the old oid</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_reflog_entry_id_old(
            git_reflog_entry* entry);

        /// <summary>
        ///  Get the new oid
        /// </summary>
        /// <param name="entry">a reflog entry</param>
        /// <returns>the new oid at this time</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_reflog_entry_id_new(
            git_reflog_entry* entry);

        /// <summary>
        ///  Get the committer of this entry
        /// </summary>
        /// <param name="entry">a reflog entry</param>
        /// <returns>the committer</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_signature* git_reflog_entry_committer(
            git_reflog_entry* entry);

        /// <summary>
        ///  Get the log message
        /// </summary>
        /// <param name="entry">a reflog entry</param>
        /// <returns>the log msg</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_reflog_entry_message(
            git_reflog_entry* entry);

        /// <summary>
        ///  Free the reflog
        /// </summary>
        /// <param name="reflog">reflog to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_reflog_free(
            git_reflog* reflog);
    }
}
