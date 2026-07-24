using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Callback function to return parents for commit.
    ///
    ///  <para>
    ///   This is invoked with the count of the number of parents
    ///   processed so far along with the user supplied payload. This
    ///   should return a git_oid of the next parent or NULL if all
    ///   parents have been provided.
    ///  </para>
    /// </summary>
    /// <param name="idx">the index of the parent</param>
    /// <param name="payload">the user-specified payload</param>
    /// <returns>
    ///  the object id of the parent, or NULL if there are no further
    ///  parents
    /// </returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate git_oid* git_commit_parent_callback(
        UIntPtr idx,
        IntPtr payload);

    public static partial class libgit2
    {
        /// <summary>
        ///  Create new commit in the repository from a list of `git_oid`
        ///  values.
        ///
        ///  <para>
        ///   See documentation for `git_commit_create()` for
        ///   information about the parameters, as the meaning is
        ///   identical excepting that `tree` and `parents` now take
        ///   `git_oid`. This is a dangerous API in that nor the
        ///   `tree`, neither the `parents` list of `git_oid`s are
        ///   checked for validity.
        ///  </para>
        /// </summary>
        /// <param name="id">
        ///  Pointer in which to store the OID of the newly created
        ///  commit
        /// </param>
        /// <param name="repo">Repository where to store the commit</param>
        /// <param name="update_ref">
        ///  If not NULL, name of the reference that will be updated to
        ///  point to this commit. If the reference is not direct, it
        ///  will be resolved to a direct reference. Use "HEAD" to update
        ///  the HEAD of the current branch and make it point to this
        ///  commit. If the reference doesn't exist yet, it will be
        ///  created. If it does exist, the first parent must be the tip
        ///  of this branch.
        /// </param>
        /// <param name="author">
        ///  Signature with author and author time of commit
        /// </param>
        /// <param name="committer">
        ///  Signature with committer and * commit time of commit
        /// </param>
        /// <param name="message_encoding">
        ///  The encoding for the message in the commit, represented with
        ///  a standard encoding name. E.g. "UTF-8". If NULL, no encoding
        ///  header is written and UTF-8 is assumed.
        /// </param>
        /// <param name="message">Full message for this commit</param>
        /// <param name="tree">
        ///  An instance of a `git_tree` object that will be used as the
        ///  tree for the commit. This tree object must also be owned by
        ///  the given `repo`.
        /// </param>
        /// <param name="parent_count">
        ///  Number of parents for this commit
        /// </param>
        /// <param name="parents">
        ///  Array of `parent_count` pointers to `git_commit` objects
        ///  that will be used as the parents for this commit. This array
        ///  may be NULL if `parent_count` is 0 (root commit). All the
        ///  given commits must be owned by the `repo`.
        /// </param>
        /// <returns>
        ///  0 or an error code The created commit will be written to the
        ///  Object Database and the given reference will be updated to
        ///  point to it
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_create_from_ids(
            ref git_oid id,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string update_ref,
            ref git_signature author,
            ref git_signature committer,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string message_encoding,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string message,
            ref git_oid tree,
            UIntPtr parent_count,
            IntPtr parents);

        /// <summary>
        ///  Create a new commit in the repository with an callback to
        ///  supply parents.
        ///
        ///  <para>
        ///   See documentation for `git_commit_create()` for
        ///   information about the parameters, as the meaning is
        ///   identical excepting that `tree` takes a `git_oid` and
        ///   doesn't check for validity, and `parent_cb` is invoked
        ///   with `parent_payload` and should return `git_oid` values
        ///   or NULL to indicate that all parents are accounted for.
        ///  </para>
        /// </summary>
        /// <param name="id">
        ///  Pointer in which to store the OID of the newly created
        ///  commit
        /// </param>
        /// <param name="repo">Repository where to store the commit</param>
        /// <param name="update_ref">
        ///  If not NULL, name of the reference that will be updated to
        ///  point to this commit. If the reference is not direct, it
        ///  will be resolved to a direct reference. Use "HEAD" to update
        ///  the HEAD of the current branch and make it point to this
        ///  commit. If the reference doesn't exist yet, it will be
        ///  created. If it does exist, the first parent must be the tip
        ///  of this branch.
        /// </param>
        /// <param name="author">
        ///  Signature with author and author time of commit
        /// </param>
        /// <param name="committer">
        ///  Signature with committer and * commit time of commit
        /// </param>
        /// <param name="message_encoding">
        ///  The encoding for the message in the commit, represented with
        ///  a standard encoding name. E.g. "UTF-8". If NULL, no encoding
        ///  header is written and UTF-8 is assumed.
        /// </param>
        /// <param name="message">Full message for this commit</param>
        /// <param name="tree">
        ///  An instance of a `git_tree` object that will be used as the
        ///  tree for the commit. This tree object must also be owned by
        ///  the given `repo`.
        /// </param>
        /// <param name="parent_cb">
        ///  Callback to invoke to obtain parent information
        /// </param>
        /// <param name="parent_payload">
        ///  User-specified payload to provide to the callback
        /// </param>
        /// <returns>
        ///  0 or an error code The created commit will be written to the
        ///  Object Database and the given reference will be updated to
        ///  point to it
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_create_from_callback(
            ref git_oid id,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string update_ref,
            ref git_signature author,
            ref git_signature committer,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string message_encoding,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string message,
            ref git_oid tree,
            git_commit_parent_callback parent_cb,
            IntPtr parent_payload);
    }
}
