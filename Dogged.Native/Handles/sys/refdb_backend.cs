using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    public sealed class ReferenceIteratorHandle : SafeHandle
    {
        public ReferenceIteratorHandle() : base(IntPtr.Zero, true)
        {
        }

        public unsafe ReferenceIteratorHandle(git_reference_iterator* nativeReferenceIterator) :
            base((IntPtr)nativeReferenceIterator, true)
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
            libgit2.git_reference_iterator_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Constructors for default filesystem-based refdb backend
        ///
        ///  <para>
        ///   Under normal usage, this is called for you when the
        ///   repository is opened / created, but you can use this to
        ///   explicitly construct a filesystem refdb backend for a
        ///   repository.
        ///  </para>
        /// </summary>
        /// <param name="backend_out">
        ///  Output pointer to the git_refdb_backend object
        /// </param>
        /// <param name="repo">Git repository to access</param>
        /// <returns>0 on success, < 0 error code on failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_refdb_backend_fs(
            out git_refdb_backend* backend_out,
            RepositoryHandle repo);

        /// <summary>
        ///  Constructor for the reftable-based refdb backend
        ///
        ///  <para>
        ///   Under normal usage, this is called for you when the
        ///   repository is opened / created that uses the reftable
        ///   format, but you can use this to explicitly construct a
        ///   reftable refdb backend for a repository.
        ///  </para>
        /// </summary>
        /// <param name="backend_out">
        ///  Output pointer to the git_refdb_backend object
        /// </param>
        /// <param name="repo">Git repository to access</param>
        /// <returns>0 on success, < 0 error code on failure</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_refdb_backend_reftable(
            out git_refdb_backend* backend_out,
            RepositoryHandle repo);

        /// <summary>
        ///  Sets the custom backend to an existing reference DB
        ///
        ///  <para>
        ///   The `git_refdb` will take ownership of the
        ///   `git_refdb_backend` so you should NOT free it after
        ///   calling this function.
        ///  </para>
        /// </summary>
        /// <param name="refdb">database to add the backend to</param>
        /// <param name="backend">
        ///  pointer to a git_refdb_backend instance
        /// </param>
        /// <returns>0 on success; error code otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_refdb_set_backend(
            RefdbHandle refdb,
            ref git_refdb_backend backend);
    }
}
