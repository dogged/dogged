using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public sealed class CommitHandle : ObjectHandle
    {
        public CommitHandle() : base()
        {
        }

        public unsafe CommitHandle(git_commit* nativeCommit) :
            base((git_object*)nativeCommit)
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
            libgit2.git_commit_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Lookup a commit object from a repository.
        ///
        ///  <para>
        ///   The returned object should be released with
        ///   `git_commit_free` when no longer needed.
        ///  </para>
        /// </summary>
        /// <param name="commit">pointer to the looked up commit</param>
        /// <param name="repo">
        ///  the repo to use when locating the commit.
        /// </param>
        /// <param name="id">
        ///  identity of the commit to locate. If the object is an
        ///  annotated tag it will be peeled back to the commit.
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_lookup(
            out CommitHandle commit,
            RepositoryHandle repo,
            ref git_oid id);

        /// <summary>
        ///  Lookup a commit object from a repository, given a prefix of
        ///  its identifier (short id).
        ///
        ///  <para>
        ///   The returned object should be released with
        ///   `git_commit_free` when no longer needed.
        ///  </para>
        /// </summary>
        /// <param name="commit">pointer to the looked up commit</param>
        /// <param name="repo">
        ///  the repo to use when locating the commit.
        /// </param>
        /// <param name="id">
        ///  identity of the commit to locate. If the object is an
        ///  annotated tag it will be peeled back to the commit.
        /// </param>
        /// <param name="len">the length of the short identifier</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_lookup_prefix(
            CommitHandle commit,
            RepositoryHandle repo,
            ref git_oid id,
            UIntPtr len);

        /// <summary>
        ///  Close an open commit
        ///
        ///  <para>
        ///   This is a wrapper around git_object_free()
        ///  </para>
        ///
        ///  <para>
        ///   IMPORTANT: It *is* necessary to call this method when
        ///   you stop using a commit. Failure to do so will cause a
        ///   memory leak.
        ///  </para>
        /// </summary>
        /// <param name="commit">the commit to close</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_commit_free(
            IntPtr commit);

        /// <summary>
        ///  Get the id of a commit.
        /// </summary>
        /// <param name="commit">a previously loaded commit.</param>
        /// <returns>object identity for the commit.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_commit_id(
            CommitHandle commit);

        /// <summary>
        ///  Get the repository that contains the commit.
        /// </summary>
        /// <param name="commit">A previously loaded commit.</param>
        /// <returns>Repository that contains this commit.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_repository* git_commit_owner(
            CommitHandle commit);

        /// <summary>
        ///  Get the encoding for the message of a commit, as a string
        ///  representing a standard encoding name.
        ///
        ///  <para>
        ///   The encoding may be NULL if the `encoding` header in the
        ///   commit is missing; in that case UTF-8 is assumed.
        ///  </para>
        /// </summary>
        /// <param name="commit">a previously loaded commit.</param>
        /// <returns>NULL, or the encoding</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_commit_message_encoding(
            CommitHandle commit);

        /// <summary>
        ///  Get the full message of a commit.
        ///
        ///  <para>
        ///   The returned message will be slightly prettified by
        ///   removing any potential leading newlines.
        ///  </para>
        /// </summary>
        /// <param name="commit">a previously loaded commit.</param>
        /// <returns>the message of a commit</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_commit_message(
            CommitHandle commit);

        /// <summary>
        ///  Get the full raw message of a commit.
        /// </summary>
        /// <param name="commit">a previously loaded commit.</param>
        /// <returns>the raw message of a commit</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_commit_message_raw(
            CommitHandle commit);

        /// <summary>
        ///  Get the short "summary" of the git commit message.
        ///
        ///  <para>
        ///   The returned message is the summary of the commit,
        ///   comprising the first paragraph of the message with
        ///   whitespace trimmed and squashed.
        ///  </para>
        /// </summary>
        /// <param name="commit">a previously loaded commit.</param>
        /// <returns>the summary of a commit or NULL on error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_commit_summary(
            CommitHandle commit);

        /// <summary>
        ///  Get the long "body" of the git commit message.
        ///
        ///  <para>
        ///   The returned message is the body of the commit,
        ///   comprising everything but the first paragraph of the
        ///   message. Leading and trailing whitespaces are trimmed.
        ///  </para>
        /// </summary>
        /// <param name="commit">a previously loaded commit.</param>
        /// <returns>
        ///  the body of a commit or NULL when no the message only
        ///  consists of a summary
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_commit_body(
            CommitHandle commit);

        /// <summary>
        ///  Get the commit time (i.e. committer time) of a commit.
        /// </summary>
        /// <param name="commit">a previously loaded commit.</param>
        /// <returns>the time of a commit</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe long git_commit_time(
            CommitHandle commit);

        /// <summary>
        ///  Get the commit timezone offset (i.e. committer's preferred
        ///  timezone) of a commit.
        /// </summary>
        /// <param name="commit">a previously loaded commit.</param>
        /// <returns>
        ///  positive or negative timezone offset, in minutes from UTC
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_time_offset(
            CommitHandle commit);

        /// <summary>
        ///  Get the committer of a commit.
        /// </summary>
        /// <param name="commit">a previously loaded commit.</param>
        /// <returns>the committer of a commit</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_signature* git_commit_committer(
            CommitHandle commit);

        /// <summary>
        ///  Get the author of a commit.
        /// </summary>
        /// <param name="commit">a previously loaded commit.</param>
        /// <returns>the author of a commit</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_signature* git_commit_author(
            CommitHandle commit);

        /// <summary>
        ///  Get the committer of a commit, using the mailmap to map
        ///  names and email addresses to canonical real names and email
        ///  addresses.
        ///
        ///  <para>
        ///   Call `git_signature_free` to free the signature.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  a pointer to store the resolved signature.
        /// </param>
        /// <param name="commit">a previously loaded commit.</param>
        /// <param name="mailmap">
        ///  the mailmap to resolve with. (may be NULL)
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_committer_with_mailmap(
            out git_signature* _out,
            CommitHandle commit,
            MailmapHandle mailmap);

        /// <summary>
        ///  Get the author of a commit, using the mailmap to map names
        ///  and email addresses to canonical real names and email
        ///  addresses.
        ///
        ///  <para>
        ///   Call `git_signature_free` to free the signature.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  a pointer to store the resolved signature.
        /// </param>
        /// <param name="commit">a previously loaded commit.</param>
        /// <param name="mailmap">
        ///  the mailmap to resolve with. (may be NULL)
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_author_with_mailmap(
            out git_signature* _out,
            CommitHandle commit,
            MailmapHandle mailmap);

        /// <summary>
        ///  Get the full raw text of the commit header.
        /// </summary>
        /// <param name="commit">a previously loaded commit</param>
        /// <returns>the header text of the commit</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_commit_raw_header(
            CommitHandle commit);

        /// <summary>
        ///  Get the tree pointed to by a commit.
        /// </summary>
        /// <param name="tree_out">
        ///  pointer where to store the tree object
        /// </param>
        /// <param name="commit">a previously loaded commit.</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_tree(
            out TreeHandle tree_out,
            CommitHandle commit);

        /// <summary>
        ///  Get the id of the tree pointed to by a commit. This differs
        ///  from `git_commit_tree` in that no attempts are made to fetch
        ///  an object from the ODB.
        /// </summary>
        /// <param name="commit">a previously loaded commit.</param>
        /// <returns>the id of tree pointed to by commit.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_commit_tree_id(
            CommitHandle commit);

        /// <summary>
        ///  Get the number of parents of this commit
        /// </summary>
        /// <param name="commit">a previously loaded commit.</param>
        /// <returns>integer of count of parents</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe uint git_commit_parentcount(
            CommitHandle commit);

        /// <summary>
        ///  Get the specified parent of the commit.
        /// </summary>
        /// <param name="_out">
        ///  Pointer where to store the parent commit
        /// </param>
        /// <param name="commit">a previously loaded commit.</param>
        /// <param name="n">
        ///  the position of the parent (from 0 to `parentcount`)
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_parent(
            out CommitHandle _out,
            CommitHandle commit,
            uint n);

        /// <summary>
        ///  Get the oid of a specified parent for a commit. This is
        ///  different from `git_commit_parent`, which will attempt to
        ///  load the parent commit from the ODB.
        /// </summary>
        /// <param name="commit">a previously loaded commit.</param>
        /// <param name="n">
        ///  the position of the parent (from 0 to `parentcount`)
        /// </param>
        /// <returns>the id of the parent, NULL on error.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_commit_parent_id(
            CommitHandle commit,
            uint n);

        /// <summary>
        ///  Get the commit object that is the <n>th generation ancestor
        ///  of the named commit object, following only the first
        ///  parents. The returned commit has to be freed by the caller.
        ///
        ///  <para>
        ///   Passing `0` as the generation number returns another
        ///   instance of the base commit itself.
        ///  </para>
        /// </summary>
        /// <param name="ancestor">
        ///  Pointer where to store the ancestor commit
        /// </param>
        /// <param name="commit">a previously loaded commit.</param>
        /// <param name="n">the requested generation</param>
        /// <returns>
        ///  0 on success; GIT_ENOTFOUND if no matching ancestor exists
        ///  or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_nth_gen_ancestor(
            CommitHandle ancestor,
            CommitHandle commit,
            uint n);

        /// <summary>
        ///  Get an arbitrary header field
        /// </summary>
        /// <param name="_out">
        ///  the buffer to fill; existing content will be overwritten
        /// </param>
        /// <param name="commit">the commit to look in</param>
        /// <param name="field">the header field to return</param>
        /// <returns>
        ///  0 on succeess, GIT_ENOTFOUND if the field does not exist, or
        ///  an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_header_field(
            ref git_buf _out,
            CommitHandle commit,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string field);

        /// <summary>
        ///  Extract the signature from a commit
        ///
        ///  <para>
        ///   If the id is not for a commit, the error class will be
        ///   `GIT_ERROR_INVALID`. If the commit does not have a
        ///   signature, the error class will be `GIT_ERROR_OBJECT`.
        ///  </para>
        /// </summary>
        /// <param name="signature">
        ///  the signature block; existing content will be overwritten
        /// </param>
        /// <param name="signed_data">
        ///  signed data; this is the commit contents minus the signature
        ///  block; existing content will be overwritten
        /// </param>
        /// <param name="repo">
        ///  the repository in which the commit exists
        /// </param>
        /// <param name="commit_id">
        ///  the commit from which to extract the data
        /// </param>
        /// <param name="field">
        ///  the name of the header field containing the signature block;
        ///  pass `NULL` to extract the default 'gpgsig'
        /// </param>
        /// <returns>
        ///  0 on success, GIT_ENOTFOUND if the id is not for a commit or
        ///  the commit does not have a signature.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_extract_signature(
            ref git_buf signature,
            ref git_buf signed_data,
            RepositoryHandle repo,
            ref git_oid commit_id,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string field);

        /// <summary>
        ///  Commits the staged changes in the repository; this is a near
        ///  analog to `git commit -m message`.
        ///
        ///  <para>
        ///   By default, empty commits are not allowed.
        ///  </para>
        /// </summary>
        /// <param name="id">
        ///  pointer to store the new commit's object id
        /// </param>
        /// <param name="repo">repository to commit changes in</param>
        /// <param name="message">the commit message</param>
        /// <param name="opts">options for creating the commit</param>
        /// <returns>
        ///  0 on success, GIT_EUNCHANGED if there were no changes to
        ///  commit, or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_create_from_stage(
            ref git_oid id,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string message,
            ref git_commit_create_options opts);

        /// <summary>
        ///  Create a new commit object and write it to the object
        ///  database.
        ///
        ///  <para>
        ///   Note that the message will _not_ be cleaned up
        ///   automatically. You may wish to use the
        ///   `git_message_prettify` API to clean it up.
        ///  </para>
        ///
        ///  <para>
        ///   This API provides the most control for generating commit
        ///   objects; this is useful if (for example) you have a tree
        ///   object already. If you have staged changes, and just
        ///   want to emulate `git commit` on the command line, then
        ///   `git_commit_create_from_stage` may be a simpler option.
        ///  </para>
        /// </summary>
        /// <param name="id_out">
        ///  pointer to store the new commit's object id
        /// </param>
        /// <param name="repo">repository to store the commit in</param>
        /// <param name="author">
        ///  The name of the author and time of authorship
        /// </param>
        /// <param name="committer">
        ///  The name of the committer and time of commit
        /// </param>
        /// <param name="message">Full message for this commit</param>
        /// <param name="tree">
        ///  The `git_tree` that will be used as the tree for this commit
        /// </param>
        /// <param name="parent_count">
        ///  Number of parents for this commit
        /// </param>
        /// <param name="parents">
        ///  Array pointers to `git_commit` objects that will be used as
        ///  parents for this commit.
        /// </param>
        /// <param name="opts">
        ///  Additional options for creating this commit, or `NULL` for
        ///  defaults
        /// </param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_create_ext(
            ref git_oid id_out,
            RepositoryHandle repo,
            ref git_signature author,
            ref git_signature committer,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string message,
            TreeHandle tree,
            UIntPtr parent_count,
            git_commit  parents,
            ref git_commit_create_ext_options opts);

        /// <summary>
        ///  Create new commit in the repository from a list of
        ///  `git_object` pointers
        ///
        ///  <para>
        ///   The message will **not** be cleaned up automatically.
        ///   You can do that with the `git_message_prettify()`
        ///   function.
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
        public static extern unsafe int git_commit_create(
            ref git_oid id,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string update_ref,
            ref git_signature author,
            ref git_signature committer,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string message_encoding,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string message,
            TreeHandle tree,
            UIntPtr parent_count,
            CommitHandle parents);

        /// <summary>
        ///  Create new commit in the repository using a variable
        ///  argument list.
        ///
        ///  <para>
        ///   The message will **not** be cleaned up automatically.
        ///   You can do that with the `git_message_prettify()`
        ///   function.
        ///  </para>
        ///
        ///  <para>
        ///   The parents for the commit are specified as a variable
        ///   list of pointers to `const git_commit *`. Note that this
        ///   is a convenience method which may not be safe to export
        ///   for certain languages or compilers
        ///  </para>
        ///
        ///  <para>
        ///   All other parameters remain the same as
        ///   `git_commit_create()`.
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
        /// <returns>
        ///  0 or an error code The created commit will be written to the
        ///  Object Database and the given reference will be updated to
        ///  point to it
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_create_v(
            ref git_oid id,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string update_ref,
            ref git_signature author,
            ref git_signature committer,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string message_encoding,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string message,
            TreeHandle tree,
            UIntPtr parent_count);

        /// <summary>
        ///  A "batteries included" commit creation from a tree using
        ///  defaults.
        ///
        ///  <para>
        ///   By default, empty commits are not allowed.
        ///  </para>
        /// </summary>
        /// <param name="id">
        ///  pointer to store the new commit's object id
        /// </param>
        /// <param name="repo">repository to commit changes in</param>
        /// <param name="tree">tree to point the commit to</param>
        /// <param name="message">the commit message</param>
        /// <param name="opts">options for creating the commit</param>
        /// <returns>
        ///  0 on success, GIT_EUNCHANGED if there were no changes to
        ///  commit, or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_create_from_tree(
            ref git_oid id,
            RepositoryHandle repo,
            TreeHandle tree,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string message,
            ref git_commit_create_options opts);

        /// <summary>
        ///  Amends the HEAD commit in the repository using the staged
        ///  changes; this is a near analog to `git commit --amend -m
        ///  message`.
        /// </summary>
        /// <param name="id">
        ///  pointer to store the new commit's object id
        /// </param>
        /// <param name="repo">repository to commit changes in</param>
        /// <param name="message">the commit message</param>
        /// <param name="opts">options for creating the commit</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_amend_from_stage(
            ref git_oid id,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string message,
            ref git_commit_create_options opts);

        /// <summary>
        ///  Amends the HEAD commit in the repository using the given
        ///  tree.
        /// </summary>
        /// <param name="id">
        ///  pointer to store the new commit's object id
        /// </param>
        /// <param name="repo">repository to commit changes in</param>
        /// <param name="tree">tree to point the commit to</param>
        /// <param name="message">the commit message</param>
        /// <param name="opts">options for creating the commit</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_amend_from_tree(
            ref git_oid id,
            RepositoryHandle repo,
            TreeHandle tree,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string message,
            ref git_commit_create_options opts);

        /// <summary>
        ///  Amend an existing commit by replacing only non-NULL values.
        ///
        ///  <para>
        ///   This creates a new commit that is exactly the same as
        ///   the old commit, except that any non-NULL values will be
        ///   updated. The new commit has the same parents as the old
        ///   commit.
        ///  </para>
        ///
        ///  <para>
        ///   The `update_ref` value works as in the regular
        ///   `git_commit_create()`, updating the ref to point to the
        ///   newly rewritten commit. If you want to amend a commit
        ///   that is not currently the tip of the branch and then
        ///   rewrite the following commits to reach a ref, pass this
        ///   as NULL and update the rest of the commit chain and ref
        ///   separately.
        ///  </para>
        ///
        ///  <para>
        ///   Unlike `git_commit_create()`, the `author`, `committer`,
        ///   `message`, `message_encoding`, and `tree` parameters can
        ///   be NULL in which case this will use the values from the
        ///   original `commit_to_amend`.
        ///  </para>
        ///
        ///  <para>
        ///   All parameters have the same meanings as in
        ///   `git_commit_create()`.
        ///  </para>
        /// </summary>
        /// <param name="id">
        ///  Pointer in which to store the OID of the newly created
        ///  commit
        /// </param>
        /// <param name="commit_to_amend">The commit to amend</param>
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
        ///  Signature with committer and commit time of commit
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
        /// <returns>
        ///  0 or an error code The created commit will be written to the
        ///  Object Database and the given reference will be updated to
        ///  point to it
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_amend(
            ref git_oid id,
            CommitHandle commit_to_amend,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string update_ref,
            ref git_signature author,
            ref git_signature committer,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string message_encoding,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string message,
            TreeHandle tree);

        /// <summary>
        ///  Create a commit and write it into a buffer
        ///
        ///  <para>
        ///   Create a commit as with `git_commit_create()` but
        ///   instead of writing it to the objectdb, write the
        ///   contents of the object into a buffer.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  the buffer into which to write the commit object content
        /// </param>
        /// <param name="repo">
        ///  Repository where the referenced tree and parents live
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
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_create_buffer(
            ref git_buf _out,
            RepositoryHandle repo,
            ref git_signature author,
            ref git_signature committer,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string message_encoding,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string message,
            TreeHandle tree,
            UIntPtr parent_count,
            CommitHandle parents);

        /// <summary>
        ///  Create a commit object from the given buffer and signature
        ///
        ///  <para>
        ///   Given the unsigned commit object's contents, its
        ///   signature and the header field in which to store the
        ///   signature, attach the signature to the commit and write
        ///   it into the given repository.
        ///  </para>
        /// </summary>
        /// <param name="_out">the resulting commit id</param>
        /// <param name="repo">
        ///  the repository to create the commit in.
        /// </param>
        /// <param name="commit_content">
        ///  the content of the unsigned commit object
        /// </param>
        /// <param name="signature">
        ///  the signature to add to the commit. Leave `NULL` to create a
        ///  commit without adding a signature field.
        /// </param>
        /// <param name="signature_field">
        ///  which header field should contain this signature. Leave
        ///  `NULL` for the default of "gpgsig"
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_create_with_signature(
            ref git_oid _out,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string commit_content,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string signature,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string signature_field);

        /// <summary>
        ///  Create an in-memory copy of a commit. The copy must be
        ///  explicitly free'd or it will leak.
        /// </summary>
        /// <param name="_out">
        ///  Pointer to store the copy of the commit
        /// </param>
        /// <param name="source">Original commit to copy</param>
        /// <returns>0</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_commit_dup(
            out CommitHandle _out,
            CommitHandle source);
    }
}
