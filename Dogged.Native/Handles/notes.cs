using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public sealed class NoteIteratorHandle : SafeHandle
    {
        public NoteIteratorHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe NoteIteratorHandle(git_note_iterator* nativeNoteIterator) :
            base((IntPtr)nativeNoteIterator, true)
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
            libgit2.git_note_iterator_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public sealed class NoteHandle : SafeHandle
    {
        public NoteHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe NoteHandle(git_note* nativeNote) :
            base((IntPtr)nativeNote, true)
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
            libgit2.git_note_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Creates a new iterator for notes
        ///
        ///  <para>
        ///   The iterator must be freed manually by the user.
        ///  </para>
        /// </summary>
        /// <param name="_out">pointer to the iterator</param>
        /// <param name="repo">repository where to look up the note</param>
        /// <param name="notes_ref">
        ///  canonical name of the reference to use (optional); defaults
        ///  to "refs/notes/commits"
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_note_iterator_new(
            out NoteIteratorHandle _out,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string notes_ref);

        /// <summary>
        ///  Creates a new iterator for notes from a commit
        ///
        ///  <para>
        ///   The iterator must be freed manually by the user.
        ///  </para>
        /// </summary>
        /// <param name="_out">pointer to the iterator</param>
        /// <param name="notes_commit">
        ///  a pointer to the notes commit object
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_note_commit_iterator_new(
            out NoteIteratorHandle _out,
            CommitHandle notes_commit);

        /// <summary>
        ///  Frees an git_note_iterator
        /// </summary>
        /// <param name="it">pointer to the iterator</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_note_iterator_free(
            IntPtr it);

        /// <summary>
        ///  Return the current item (note_id and annotated_id) and
        ///  advance the iterator internally to the next value
        /// </summary>
        /// <param name="note_id">id of blob containing the message</param>
        /// <param name="annotated_id">
        ///  id of the git object being annotated
        /// </param>
        /// <param name="it">pointer to the iterator</param>
        /// <returns>
        ///  0 (no error), GIT_ITEROVER (iteration is done) or an error
        ///  code (negative value)
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_note_next(
            ref git_oid note_id,
            ref git_oid annotated_id,
            NoteIteratorHandle it);

        /// <summary>
        ///  Read the note for an object
        ///
        ///  <para>
        ///   The note must be freed manually by the user.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  pointer to the read note; NULL in case of error
        /// </param>
        /// <param name="repo">repository where to look up the note</param>
        /// <param name="notes_ref">
        ///  canonical name of the reference to use (optional); defaults
        ///  to "refs/notes/commits"
        /// </param>
        /// <param name="oid">
        ///  OID of the git object to read the note from
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_note_read(
            out NoteHandle _out,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string notes_ref,
            ref git_oid oid);

        /// <summary>
        ///  Read the note for an object from a note commit
        ///
        ///  <para>
        ///   The note must be freed manually by the user.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  pointer to the read note; NULL in case of error
        /// </param>
        /// <param name="repo">repository where to look up the note</param>
        /// <param name="notes_commit">
        ///  a pointer to the notes commit object
        /// </param>
        /// <param name="oid">
        ///  OID of the git object to read the note from
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_note_commit_read(
            out NoteHandle _out,
            RepositoryHandle repo,
            CommitHandle notes_commit,
            ref git_oid oid);

        /// <summary>
        ///  Get the note author
        /// </summary>
        /// <param name="note">the note</param>
        /// <returns>the author</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_signature* git_note_author(
            NoteHandle note);

        /// <summary>
        ///  Get the note committer
        /// </summary>
        /// <param name="note">the note</param>
        /// <returns>the committer</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_signature* git_note_committer(
            NoteHandle note);

        /// <summary>
        ///  Get the note message
        /// </summary>
        /// <param name="note">the note</param>
        /// <returns>the note message</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_note_message(
            NoteHandle note);

        /// <summary>
        ///  Get the note object's id
        /// </summary>
        /// <param name="note">the note</param>
        /// <returns>the note object's id</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_note_id(
            NoteHandle note);

        /// <summary>
        ///  Add a note for an object
        /// </summary>
        /// <param name="_out">
        ///  pointer to store the OID (optional); NULL in case of error
        /// </param>
        /// <param name="repo">repository where to store the note</param>
        /// <param name="notes_ref">
        ///  canonical name of the reference to use (optional); defaults
        ///  to "refs/notes/commits"
        /// </param>
        /// <param name="author">
        ///  signature of the notes commit author
        /// </param>
        /// <param name="committer">
        ///  signature of the notes commit committer
        /// </param>
        /// <param name="oid">OID of the git object to decorate</param>
        /// <param name="note">
        ///  Content of the note to add for object oid
        /// </param>
        /// <param name="force">Overwrite existing note</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_note_create(
            ref git_oid _out,
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string notes_ref,
            ref git_signature author,
            ref git_signature committer,
            ref git_oid oid,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string note,
            int force);

        /// <summary>
        ///  Add a note for an object from a commit
        ///
        ///  <para>
        ///   This function will create a notes commit for a given
        ///   object, the commit is a dangling commit, no reference is
        ///   created.
        ///  </para>
        /// </summary>
        /// <param name="notes_commit_out">
        ///  pointer to store the commit (optional); NULL in case of
        ///  error
        /// </param>
        /// <param name="notes_blob_out">
        ///  a point to the id of a note blob (optional)
        /// </param>
        /// <param name="repo">repository where the note will live</param>
        /// <param name="parent">
        ///  Pointer to parent note or NULL if this shall start a new
        ///  notes tree
        /// </param>
        /// <param name="author">
        ///  signature of the notes commit author
        /// </param>
        /// <param name="committer">
        ///  signature of the notes commit committer
        /// </param>
        /// <param name="oid">OID of the git object to decorate</param>
        /// <param name="note">
        ///  Content of the note to add for object oid
        /// </param>
        /// <param name="allow_note_overwrite">
        ///  Overwrite existing note
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_note_commit_create(
            ref git_oid notes_commit_out,
            ref git_oid notes_blob_out,
            RepositoryHandle repo,
            CommitHandle parent,
            ref git_signature author,
            ref git_signature committer,
            ref git_oid oid,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string note,
            int allow_note_overwrite);

        /// <summary>
        ///  Remove the note for an object
        /// </summary>
        /// <param name="repo">repository where the note lives</param>
        /// <param name="notes_ref">
        ///  canonical name of the reference to use (optional); defaults
        ///  to "refs/notes/commits"
        /// </param>
        /// <param name="author">
        ///  signature of the notes commit author
        /// </param>
        /// <param name="committer">
        ///  signature of the notes commit committer
        /// </param>
        /// <param name="oid">
        ///  OID of the git object to remove the note from
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_note_remove(
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string notes_ref,
            ref git_signature author,
            ref git_signature committer,
            ref git_oid oid);

        /// <summary>
        ///  Remove the note for an object
        /// </summary>
        /// <param name="notes_commit_out">
        ///  pointer to store the new notes commit (optional); NULL in
        ///  case of error. When removing a note a new tree containing
        ///  all notes sans the note to be removed is created and a new
        ///  commit pointing to that tree is also created. In the case
        ///  where the resulting tree is an empty tree a new commit
        ///  pointing to this empty tree will be returned.
        /// </param>
        /// <param name="repo">repository where the note lives</param>
        /// <param name="notes_commit">
        ///  a pointer to the notes commit object
        /// </param>
        /// <param name="author">
        ///  signature of the notes commit author
        /// </param>
        /// <param name="committer">
        ///  signature of the notes commit committer
        /// </param>
        /// <param name="oid">
        ///  OID of the git object to remove the note from
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_note_commit_remove(
            ref git_oid notes_commit_out,
            RepositoryHandle repo,
            CommitHandle notes_commit,
            ref git_signature author,
            ref git_signature committer,
            ref git_oid oid);

        /// <summary>
        ///  Free a git_note object
        /// </summary>
        /// <param name="note">git_note object</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_note_free(
            IntPtr note);

        /// <summary>
        ///  Get the default notes reference for a repository
        /// </summary>
        /// <param name="_out">
        ///  buffer in which to store the name of the default notes
        ///  reference
        /// </param>
        /// <param name="repo">The Git repository</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_note_default_ref(
            ref git_buf _out,
            RepositoryHandle repo);

        /// <summary>
        ///  Loop over all the notes within a specified namespace and
        ///  issue a callback for each one.
        /// </summary>
        /// <param name="repo">Repository where to find the notes.</param>
        /// <param name="notes_ref">
        ///  Reference to read from (optional); defaults to
        ///  "refs/notes/commits".
        /// </param>
        /// <param name="note_cb">
        ///  Callback to invoke per found annotation. Return non-zero to
        ///  stop looping.
        /// </param>
        /// <param name="payload">
        ///  Extra parameter to callback function.
        /// </param>
        /// <returns>
        ///  0 on success, non-zero callback return value, or error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_note_foreach(
            RepositoryHandle repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string notes_ref,
            git_note_foreach_cb note_cb,
            IntPtr payload);
    }
}
