using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public sealed class MailmapHandle : SafeHandle
    {
        public MailmapHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe MailmapHandle(git_mailmap* nativeMailmap) :
            base((IntPtr)nativeMailmap, true)
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
            libgit2.git_mailmap_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Allocate a new mailmap object.
        ///
        ///  <para>
        ///   This object is empty, so you'll have to add a mailmap
        ///   file before you can do anything with it. The mailmap
        ///   must be freed with 'git_mailmap_free'.
        ///  </para>
        /// </summary>
        /// <param name="_out">pointer to store the new mailmap</param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_mailmap_new(
            out MailmapHandle _out);

        /// <summary>
        ///  Free the mailmap and its associated memory.
        /// </summary>
        /// <param name="mm">the mailmap to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_mailmap_free(
            IntPtr mm);

        /// <summary>
        ///  Add a single entry to the given mailmap object. If the entry
        ///  already exists, it will be replaced with the new entry.
        /// </summary>
        /// <param name="mm">mailmap to add the entry to</param>
        /// <param name="real_name">the real name to use, or NULL</param>
        /// <param name="real_email">the real email to use, or NULL</param>
        /// <param name="replace_name">the name to replace, or NULL</param>
        /// <param name="replace_email">the email to replace</param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_mailmap_add_entry(
            MailmapHandle mm,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string real_name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string real_email,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string replace_name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string replace_email);

        /// <summary>
        ///  Create a new mailmap instance containing a single mailmap
        ///  file
        /// </summary>
        /// <param name="_out">pointer to store the new mailmap</param>
        /// <param name="buf">buffer to parse the mailmap from</param>
        /// <param name="len">the length of the input buffer</param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_mailmap_from_buffer(
            out MailmapHandle _out,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string buf,
            UIntPtr len);

        /// <summary>
        ///  Create a new mailmap instance from a repository, loading
        ///  mailmap files based on the repository's configuration.
        ///
        ///  <para>
        ///   Mailmaps are loaded in the following order: 1.
        ///   '.mailmap' in the root of the repository's working
        ///   directory, if present. 2. The blob object identified by
        ///   the 'mailmap.blob' config entry, if set. [NOTE:
        ///   'mailmap.blob' defaults to 'HEAD:.mailmap' in bare
        ///   repositories] 3. The path in the 'mailmap.file' config
        ///   entry, if set.
        ///  </para>
        /// </summary>
        /// <param name="_out">pointer to store the new mailmap</param>
        /// <param name="repo">
        ///  repository to load mailmap information from
        /// </param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_mailmap_from_repository(
            out MailmapHandle _out,
            RepositoryHandle repo);

        /// <summary>
        ///  Resolve a name and email to the corresponding real name and
        ///  email.
        ///
        ///  <para>
        ///   The lifetime of the strings are tied to `mm`, `name`,
        ///   and `email` parameters.
        ///  </para>
        /// </summary>
        /// <param name="real_name">pointer to store the real name</param>
        /// <param name="real_email">pointer to store the real email</param>
        /// <param name="mm">
        ///  the mailmap to perform a lookup with (may be NULL)
        /// </param>
        /// <param name="name">the name to look up</param>
        /// <param name="email">the email to look up</param>
        /// <returns>0 on success, or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_mailmap_resolve(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))] out string real_name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))] out string real_email,
            MailmapHandle mm,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string email);

        /// <summary>
        ///  Resolve a signature to use real names and emails with a
        ///  mailmap.
        ///
        ///  <para>
        ///   Call `git_signature_free()` to free the data.
        ///  </para>
        /// </summary>
        /// <param name="_out">new signature</param>
        /// <param name="mm">mailmap to resolve with</param>
        /// <param name="sig">signature to resolve</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_mailmap_resolve_signature(
            out git_signature* _out,
            MailmapHandle mm,
            ref git_signature sig);
    }
}
