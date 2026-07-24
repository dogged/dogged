using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  Callback used to iterate over references
    /// </summary>
    /// <param name="reference">The reference object</param>
    /// <param name="payload">
    ///  Payload passed to git_reference_foreach
    /// </param>
    /// <returns>non-zero to terminate the iteration</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_reference_foreach_cb(
        git_reference* reference,
        IntPtr payload);

    /// <summary>
    ///  Callback used to iterate over reference names
    /// </summary>
    /// <param name="name">The reference name</param>
    /// <param name="payload">
    ///  Payload passed to git_reference_foreach_name
    /// </param>
    /// <returns>non-zero to terminate the iteration</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_reference_foreach_name_cb(
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
        IntPtr payload);

    /// <summary>
    ///  Normalization options for reference lookup
    /// </summary>
    public enum git_reference_format_t
    {
        /// <summary>
        ///  No particular normalization.
        /// </summary>
        GIT_REFERENCE_FORMAT_NORMAL = 0,

        /// <summary>
        ///  Control whether one-level refnames are accepted (i.e.,
        ///  refnames that do not contain multiple /-separated
        ///  components). Those are expected to be written only using
        ///  uppercase letters and underscore (FETCH_HEAD, ...)
        /// </summary>
        GIT_REFERENCE_FORMAT_ALLOW_ONELEVEL = 1,

        /// <summary>
        ///  Interpret the provided name as a reference pattern for a
        ///  refspec (as used with remote repositories). If this option
        ///  is enabled, the name is allowed to contain a single * (
        ///  <star>) in place of a one full pathname component (e.g.,
        ///  foo/ <star>/bar but not foo/bar <star>).
        /// </summary>
        GIT_REFERENCE_FORMAT_REFSPEC_PATTERN = 2,

        /// <summary>
        ///  Interpret the name as part of a refspec in shorthand form so
        ///  the `ONELEVEL` naming rules aren't enforced and 'master'
        ///  becomes a valid name.
        /// </summary>
        GIT_REFERENCE_FORMAT_REFSPEC_SHORTHAND = 4,
    }

    /// <summary>
    ///  In-memory representation of a reference.
    /// </summary>
    public struct git_reference
    {
    }

    /// <summary>
    ///  Basic type of any Git reference.
    /// </summary>
    public enum git_reference_t
    {
        /// <summary>
        ///  Invalid reference
        /// </summary>
        GIT_REFERENCE_INVALID = 0,

        /// <summary>
        ///  A reference that points at an object id
        /// </summary>
        GIT_REFERENCE_DIRECT = 1,

        /// <summary>
        ///  A reference that points at another reference
        /// </summary>
        GIT_REFERENCE_SYMBOLIC = 2,

        GIT_REFERENCE_ALL = 3,
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Lookup a reference by name in a repository.
        ///
        ///  <para>
        ///   The returned reference must be freed by the user.
        ///  </para>
        ///
        ///  <para>
        ///   The name will be checked for validity. See
        ///   `git_reference_symbolic_create()` for rules about valid
        ///   names.
        ///  </para>
        /// </summary>
        /// <param name="_out">pointer to the looked-up reference</param>
        /// <param name="repo">
        ///  the repository to look up the reference
        /// </param>
        /// <param name="name">
        ///  the long name for the reference (e.g. HEAD,
        ///  refs/heads/master, refs/tags/v0.1.0, ...)
        /// </param>
        /// <returns>
        ///  0 on success, GIT_ENOTFOUND, GIT_EINVALIDSPEC or an error
        ///  code.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_lookup(
            out git_reference* _out,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Lookup a reference by name and resolve immediately to OID.
        ///
        ///  <para>
        ///   This function provides a quick way to resolve a
        ///   reference name straight through to the object id that it
        ///   refers to. This avoids having to allocate or free any
        ///   `git_reference` objects for simple situations.
        ///  </para>
        ///
        ///  <para>
        ///   The name will be checked for validity. See
        ///   `git_reference_symbolic_create()` for rules about valid
        ///   names.
        ///  </para>
        /// </summary>
        /// <param name="_out">Pointer to oid to be filled in</param>
        /// <param name="repo">
        ///  The repository in which to look up the reference
        /// </param>
        /// <param name="name">
        ///  The long name for the reference (e.g. HEAD,
        ///  refs/heads/master, refs/tags/v0.1.0, ...)
        /// </param>
        /// <returns>
        ///  0 on success, GIT_ENOTFOUND, GIT_EINVALIDSPEC or an error
        ///  code.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_name_to_id(
            ref git_oid _out,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Lookup a reference by DWIMing its short name
        ///
        ///  <para>
        ///   Apply the git precedence rules to the given shorthand to
        ///   determine which reference the user is referring to.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  pointer in which to store the reference
        /// </param>
        /// <param name="repo">the repository in which to look</param>
        /// <param name="shorthand">the short name for the reference</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_dwim(
            out git_reference* _out,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string shorthand);

        /// <summary>
        ///  Conditionally create a new symbolic reference.
        ///
        ///  <para>
        ///   A symbolic reference is a reference name that refers to
        ///   another reference name. If the other name moves, the
        ///   symbolic name will move, too. As a simple example, the
        ///   "HEAD" reference might refer to "refs/heads/master"
        ///   while on the "master" branch of a repository.
        ///  </para>
        ///
        ///  <para>
        ///   The symbolic reference will be created in the repository
        ///   and written to the disk. The generated reference object
        ///   must be freed by the user.
        ///  </para>
        ///
        ///  <para>
        ///   Valid reference names must follow one of two patterns:
        ///  </para>
        ///
        ///  <para>
        ///   1. Top-level names must contain only capital letters and
        ///   underscores, and must begin and end with a letter. (e.g.
        ///   "HEAD", "ORIG_HEAD"). 2. Names prefixed with "refs/" can
        ///   be almost anything. You must avoid the characters '~',
        ///   '^', ':', ' \ ', '?', '[', and '*', and the sequences
        ///   ".." and "@{" which have special meaning to revparse.
        ///  </para>
        ///
        ///  <para>
        ///   This function will return an error if a reference
        ///   already exists with the given name unless `force` is
        ///   true, in which case it will be overwritten.
        ///  </para>
        ///
        ///  <para>
        ///   The message for the reflog will be ignored if the
        ///   reference does not belong in the standard set (HEAD,
        ///   branches and remote-tracking branches) and it does not
        ///   have a reflog.
        ///  </para>
        ///
        ///  <para>
        ///   It will return GIT_EMODIFIED if the reference's value at
        ///   the time of updating does not match the one passed
        ///   through `current_value` (i.e. if the ref has changed
        ///   since the user read it).
        ///  </para>
        ///
        ///  <para>
        ///   If `current_value` is all zeros, this function will
        ///   return GIT_EMODIFIED if the ref already exists.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Pointer to the newly created reference
        /// </param>
        /// <param name="repo">
        ///  Repository where that reference will live
        /// </param>
        /// <param name="name">The name of the reference</param>
        /// <param name="target">The target of the reference</param>
        /// <param name="force">Overwrite existing references</param>
        /// <param name="current_value">
        ///  The expected value of the reference when updating
        /// </param>
        /// <param name="log_message">
        ///  The one line long message to be appended to the reflog
        /// </param>
        /// <returns>
        ///  0 on success, GIT_EEXISTS, GIT_EINVALIDSPEC, GIT_EMODIFIED
        ///  or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_symbolic_create_matching(
            out git_reference* _out,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string target,
            int force,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string current_value,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string log_message);

        /// <summary>
        ///  Create a new symbolic reference.
        ///
        ///  <para>
        ///   A symbolic reference is a reference name that refers to
        ///   another reference name. If the other name moves, the
        ///   symbolic name will move, too. As a simple example, the
        ///   "HEAD" reference might refer to "refs/heads/master"
        ///   while on the "master" branch of a repository.
        ///  </para>
        ///
        ///  <para>
        ///   The symbolic reference will be created in the repository
        ///   and written to the disk. The generated reference object
        ///   must be freed by the user.
        ///  </para>
        ///
        ///  <para>
        ///   Valid reference names must follow one of two patterns:
        ///  </para>
        ///
        ///  <para>
        ///   1. Top-level names must contain only capital letters and
        ///   underscores, and must begin and end with a letter. (e.g.
        ///   "HEAD", "ORIG_HEAD"). 2. Names prefixed with "refs/" can
        ///   be almost anything. You must avoid the characters '~',
        ///   '^', ':', ' \ ', '?', '[', and '*', and the sequences
        ///   ".." and "@{" which have special meaning to revparse.
        ///  </para>
        ///
        ///  <para>
        ///   This function will return an error if a reference
        ///   already exists with the given name unless `force` is
        ///   true, in which case it will be overwritten.
        ///  </para>
        ///
        ///  <para>
        ///   The message for the reflog will be ignored if the
        ///   reference does not belong in the standard set (HEAD,
        ///   branches and remote-tracking branches) and it does not
        ///   have a reflog.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Pointer to the newly created reference
        /// </param>
        /// <param name="repo">
        ///  Repository where that reference will live
        /// </param>
        /// <param name="name">The name of the reference</param>
        /// <param name="target">The target of the reference</param>
        /// <param name="force">Overwrite existing references</param>
        /// <param name="log_message">
        ///  The one line long message to be appended to the reflog
        /// </param>
        /// <returns>
        ///  0 on success, GIT_EEXISTS, GIT_EINVALIDSPEC or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_symbolic_create(
            out git_reference* _out,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string target,
            int force,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string log_message);

        /// <summary>
        ///  Create a new direct reference.
        ///
        ///  <para>
        ///   A direct reference (also called an object id reference)
        ///   refers directly to a specific object id (a.k.a. OID or
        ///   SHA) in the repository. The id permanently refers to the
        ///   object (although the reference itself can be moved). For
        ///   example, in libgit2 the direct ref "refs/tags/v0.17.0"
        ///   refers to OID 5b9fac39d8a76b9139667c26a63e6b3f204b3977.
        ///  </para>
        ///
        ///  <para>
        ///   The direct reference will be created in the repository
        ///   and written to the disk. The generated reference object
        ///   must be freed by the user.
        ///  </para>
        ///
        ///  <para>
        ///   Valid reference names must follow one of two patterns:
        ///  </para>
        ///
        ///  <para>
        ///   1. Top-level names must contain only capital letters and
        ///   underscores, and must begin and end with a letter. (e.g.
        ///   "HEAD", "ORIG_HEAD"). 2. Names prefixed with "refs/" can
        ///   be almost anything. You must avoid the characters '~',
        ///   '^', ':', ' \ ', '?', '[', and '*', and the sequences
        ///   ".." and "@{" which have special meaning to revparse.
        ///  </para>
        ///
        ///  <para>
        ///   This function will return an error if a reference
        ///   already exists with the given name unless `force` is
        ///   true, in which case it will be overwritten.
        ///  </para>
        ///
        ///  <para>
        ///   The message for the reflog will be ignored if the
        ///   reference does not belong in the standard set (HEAD,
        ///   branches and remote-tracking branches) and it does not
        ///   have a reflog.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Pointer to the newly created reference
        /// </param>
        /// <param name="repo">
        ///  Repository where that reference will live
        /// </param>
        /// <param name="name">The name of the reference</param>
        /// <param name="id">
        ///  The object id pointed to by the reference.
        /// </param>
        /// <param name="force">Overwrite existing references</param>
        /// <param name="log_message">
        ///  The one line long message to be appended to the reflog
        /// </param>
        /// <returns>
        ///  0 on success, GIT_EEXISTS, GIT_EINVALIDSPEC or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_create(
            out git_reference* _out,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            ref git_oid id,
            int force,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string log_message);

        /// <summary>
        ///  Conditionally create new direct reference
        ///
        ///  <para>
        ///   A direct reference (also called an object id reference)
        ///   refers directly to a specific object id (a.k.a. OID or
        ///   SHA) in the repository. The id permanently refers to the
        ///   object (although the reference itself can be moved). For
        ///   example, in libgit2 the direct ref "refs/tags/v0.17.0"
        ///   refers to OID 5b9fac39d8a76b9139667c26a63e6b3f204b3977.
        ///  </para>
        ///
        ///  <para>
        ///   The direct reference will be created in the repository
        ///   and written to the disk. The generated reference object
        ///   must be freed by the user.
        ///  </para>
        ///
        ///  <para>
        ///   Valid reference names must follow one of two patterns:
        ///  </para>
        ///
        ///  <para>
        ///   1. Top-level names must contain only capital letters and
        ///   underscores, and must begin and end with a letter. (e.g.
        ///   "HEAD", "ORIG_HEAD"). 2. Names prefixed with "refs/" can
        ///   be almost anything. You must avoid the characters '~',
        ///   '^', ':', ' \ ', '?', '[', and '*', and the sequences
        ///   ".." and "@{" which have special meaning to revparse.
        ///  </para>
        ///
        ///  <para>
        ///   This function will return an error if a reference
        ///   already exists with the given name unless `force` is
        ///   true, in which case it will be overwritten.
        ///  </para>
        ///
        ///  <para>
        ///   The message for the reflog will be ignored if the
        ///   reference does not belong in the standard set (HEAD,
        ///   branches and remote-tracking branches) and it does not
        ///   have a reflog.
        ///  </para>
        ///
        ///  <para>
        ///   It will return GIT_EMODIFIED if the reference's value at
        ///   the time of updating does not match the one passed
        ///   through `current_id` (i.e. if the ref has changed since
        ///   the user read it).
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Pointer to the newly created reference
        /// </param>
        /// <param name="repo">
        ///  Repository where that reference will live
        /// </param>
        /// <param name="name">The name of the reference</param>
        /// <param name="id">
        ///  The object id pointed to by the reference.
        /// </param>
        /// <param name="force">Overwrite existing references</param>
        /// <param name="current_id">
        ///  The expected value of the reference at the time of update
        /// </param>
        /// <param name="log_message">
        ///  The one line long message to be appended to the reflog
        /// </param>
        /// <returns>
        ///  0 on success, GIT_EMODIFIED if the value of the reference
        ///  has changed, GIT_EEXISTS, GIT_EINVALIDSPEC or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_create_matching(
            out git_reference* _out,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            ref git_oid id,
            int force,
            ref git_oid current_id,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string log_message);

        /// <summary>
        ///  Get the OID pointed to by a direct reference.
        ///
        ///  <para>
        ///   Only available if the reference is direct (i.e. an
        ///   object id reference, not a symbolic one).
        ///  </para>
        ///
        ///  <para>
        ///   To find the OID of a symbolic ref, call
        ///   `git_reference_resolve()` and then this function (or
        ///   maybe use `git_reference_name_to_id()` to directly
        ///   resolve a reference name all the way through to an OID).
        ///  </para>
        /// </summary>
        /// <param name="_ref">The reference</param>
        /// <returns>
        ///  a pointer to the oid if available, NULL otherwise
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_reference_target(
            git_reference* _ref);

        /// <summary>
        ///  Return the peeled OID target of this reference.
        ///
        ///  <para>
        ///   This peeled OID only applies to direct references that
        ///   point to a hard Tag object: it is the result of peeling
        ///   such Tag.
        ///  </para>
        /// </summary>
        /// <param name="_ref">The reference</param>
        /// <returns>
        ///  a pointer to the oid if available, NULL otherwise
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_oid* git_reference_target_peel(
            git_reference* _ref);

        /// <summary>
        ///  Get full name to the reference pointed to by a symbolic
        ///  reference.
        ///
        ///  <para>
        ///   Only available if the reference is symbolic.
        ///  </para>
        /// </summary>
        /// <param name="_ref">The reference</param>
        /// <returns>
        ///  a pointer to the name if available, NULL otherwise
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_reference_symbolic_target(
            git_reference* _ref);

        /// <summary>
        ///  Get the type of a reference.
        ///
        ///  <para>
        ///   Either direct (GIT_REFERENCE_DIRECT) or symbolic
        ///   (GIT_REFERENCE_SYMBOLIC)
        ///  </para>
        /// </summary>
        /// <param name="_ref">The reference</param>
        /// <returns>the type</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_reference_t git_reference_type(
            git_reference* _ref);

        /// <summary>
        ///  Get the full name of a reference.
        ///
        ///  <para>
        ///   See `git_reference_symbolic_create()` for rules about
        ///   valid names.
        ///  </para>
        /// </summary>
        /// <param name="_ref">The reference</param>
        /// <returns>the full name for the ref</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_reference_name(
            git_reference* _ref);

        /// <summary>
        ///  Resolve a symbolic reference to a direct reference.
        ///
        ///  <para>
        ///   This method iteratively peels a symbolic reference until
        ///   it resolves to a direct reference to an OID.
        ///  </para>
        ///
        ///  <para>
        ///   The peeled reference is returned in the `resolved_ref`
        ///   argument, and must be freed manually once it's no longer
        ///   needed.
        ///  </para>
        ///
        ///  <para>
        ///   If a direct reference is passed as an argument, a copy
        ///   of that reference is returned. This copy must be
        ///   manually freed too.
        ///  </para>
        /// </summary>
        /// <param name="_out">Pointer to the peeled reference</param>
        /// <param name="_ref">The reference</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_resolve(
            out git_reference* _out,
            git_reference* _ref);

        /// <summary>
        ///  Get the repository where a reference resides.
        /// </summary>
        /// <param name="_ref">The reference</param>
        /// <returns>a pointer to the repo</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_repository* git_reference_owner(
            git_reference* _ref);

        /// <summary>
        ///  Create a new reference with the same name as the given
        ///  reference but a different symbolic target. The reference
        ///  must be a symbolic reference, otherwise this will fail.
        ///
        ///  <para>
        ///   The new reference will be written to disk, overwriting
        ///   the given reference.
        ///  </para>
        ///
        ///  <para>
        ///   The target name will be checked for validity. See
        ///   `git_reference_symbolic_create()` for rules about valid
        ///   names.
        ///  </para>
        ///
        ///  <para>
        ///   The message for the reflog will be ignored if the
        ///   reference does not belong in the standard set (HEAD,
        ///   branches and remote-tracking branches) and it does not
        ///   have a reflog.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Pointer to the newly created reference
        /// </param>
        /// <param name="_ref">The reference</param>
        /// <param name="target">The new target for the reference</param>
        /// <param name="log_message">
        ///  The one line long message to be appended to the reflog
        /// </param>
        /// <returns>
        ///  0 on success, GIT_EINVALIDSPEC or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_symbolic_set_target(
            out git_reference* _out,
            git_reference* _ref,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string target,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string log_message);

        /// <summary>
        ///  Conditionally create a new reference with the same name as
        ///  the given reference but a different OID target. The
        ///  reference must be a direct reference, otherwise this will
        ///  fail.
        ///
        ///  <para>
        ///   The new reference will be written to disk, overwriting
        ///   the given reference.
        ///  </para>
        /// </summary>
        /// <param name="_out">
        ///  Pointer to the newly created reference
        /// </param>
        /// <param name="_ref">The reference</param>
        /// <param name="id">The new target OID for the reference</param>
        /// <param name="log_message">
        ///  The one line long message to be appended to the reflog
        /// </param>
        /// <returns>
        ///  0 on success, GIT_EMODIFIED if the value of the reference
        ///  has changed since it was read, or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_set_target(
            out git_reference* _out,
            git_reference* _ref,
            ref git_oid id,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string log_message);

        /// <summary>
        ///  Rename an existing reference.
        ///
        ///  <para>
        ///   This method works for both direct and symbolic
        ///   references.
        ///  </para>
        ///
        ///  <para>
        ///   The new name will be checked for validity. See
        ///   `git_reference_symbolic_create()` for rules about valid
        ///   names.
        ///  </para>
        ///
        ///  <para>
        ///   If the `force` flag is not enabled, and there's already
        ///   a reference with the given name, the renaming will fail.
        ///  </para>
        ///
        ///  <para>
        ///   IMPORTANT: The user needs to write a proper reflog entry
        ///   if the reflog is enabled for the repository. We only
        ///   rename the reflog if it exists.
        ///  </para>
        /// </summary>
        /// <param name="new_ref">The new reference</param>
        /// <param name="_ref">The reference to rename</param>
        /// <param name="new_name">The new name for the reference</param>
        /// <param name="force">Overwrite an existing reference</param>
        /// <param name="log_message">
        ///  The one line long message to be appended to the reflog
        /// </param>
        /// <returns>
        ///  0 on success, GIT_EINVALIDSPEC, GIT_EEXISTS or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_rename(
            out git_reference* new_ref,
            git_reference* _ref,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string new_name,
            int force,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string log_message);

        /// <summary>
        ///  Delete an existing reference.
        ///
        ///  <para>
        ///   This method works for both direct and symbolic
        ///   references. The reference will be immediately removed on
        ///   disk but the memory will not be freed. Callers must call
        ///   `git_reference_free`.
        ///  </para>
        ///
        ///  <para>
        ///   This function will return an error if the reference has
        ///   changed from the time it was looked up.
        ///  </para>
        /// </summary>
        /// <param name="_ref">The reference to remove</param>
        /// <returns>0, GIT_EMODIFIED or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_delete(
            git_reference* _ref);

        /// <summary>
        ///  Delete an existing reference by name
        ///
        ///  <para>
        ///   This method removes the named reference from the
        ///   repository without looking at its old value.
        ///  </para>
        /// </summary>
        /// <param name="repo">
        ///  The repository to remove the reference from
        /// </param>
        /// <param name="name">The reference to remove</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_remove(
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Fill a list with all the references that can be found in a
        ///  repository.
        ///
        ///  <para>
        ///   The string array will be filled with the names of all
        ///   references; these values are owned by the user and
        ///   should be free'd manually when no longer needed, using
        ///   `git_strarray_free()`.
        ///  </para>
        /// </summary>
        /// <param name="array">
        ///  Pointer to a git_strarray structure where the reference
        ///  names will be stored
        /// </param>
        /// <param name="repo">Repository where to find the refs</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_list(
            ref git_strarray array,
            git_repository* repo);

        /// <summary>
        ///  Perform a callback on each reference in the repository.
        ///
        ///  <para>
        ///   The `callback` function will be called for each
        ///   reference in the repository, receiving the reference
        ///   object and the `payload` value passed to this method.
        ///   Returning a non-zero value from the callback will
        ///   terminate the iteration.
        ///  </para>
        ///
        ///  <para>
        ///   Note that the callback function is responsible to call
        ///   `git_reference_free` on each reference passed to it.
        ///  </para>
        /// </summary>
        /// <param name="repo">Repository where to find the refs</param>
        /// <param name="callback">
        ///  Function which will be called for every listed ref
        /// </param>
        /// <param name="payload">
        ///  Additional data to pass to the callback
        /// </param>
        /// <returns>
        ///  0 on success, non-zero callback return value, or error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_foreach(
            git_repository* repo,
            git_reference_foreach_cb callback,
            IntPtr payload);

        /// <summary>
        ///  Perform a callback on the fully-qualified name of each
        ///  reference.
        ///
        ///  <para>
        ///   The `callback` function will be called for each
        ///   reference in the repository, receiving the name of the
        ///   reference and the `payload` value passed to this method.
        ///   Returning a non-zero value from the callback will
        ///   terminate the iteration.
        ///  </para>
        /// </summary>
        /// <param name="repo">Repository where to find the refs</param>
        /// <param name="callback">
        ///  Function which will be called for every listed ref name
        /// </param>
        /// <param name="payload">
        ///  Additional data to pass to the callback
        /// </param>
        /// <returns>
        ///  0 on success, non-zero callback return value, or error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_foreach_name(
            git_repository* repo,
            git_reference_foreach_name_cb callback,
            IntPtr payload);

        /// <summary>
        ///  Create a copy of an existing reference.
        ///
        ///  <para>
        ///   Call `git_reference_free` to free the data.
        ///  </para>
        /// </summary>
        /// <param name="dest">pointer where to store the copy</param>
        /// <param name="source">object to copy</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_dup(
            git_reference** dest,
            git_reference* source);

        /// <summary>
        ///  Free the given reference.
        /// </summary>
        /// <param name="_ref">git_reference</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_reference_free(
            git_reference* _ref);

        /// <summary>
        ///  Compare two references.
        /// </summary>
        /// <param name="ref1">The first git_reference</param>
        /// <param name="ref2">The second git_reference</param>
        /// <returns>
        ///  0 if the same, else a stable but meaningless ordering.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_cmp(
            git_reference* ref1,
            git_reference* ref2);

        /// <summary>
        ///  Create an iterator for the repo's references
        /// </summary>
        /// <param name="_out">
        ///  pointer in which to store the iterator
        /// </param>
        /// <param name="repo">the repository</param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_iterator_new(
            out git_reference_iterator* _out,
            git_repository* repo);

        /// <summary>
        ///  Create an iterator for the repo's references that match the
        ///  specified glob
        /// </summary>
        /// <param name="_out">
        ///  pointer in which to store the iterator
        /// </param>
        /// <param name="repo">the repository</param>
        /// <param name="glob">
        ///  the glob to match against the reference names
        /// </param>
        /// <returns>0 or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_iterator_glob_new(
            out git_reference_iterator* _out,
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string glob);

        /// <summary>
        ///  Get the next reference
        /// </summary>
        /// <param name="_out">
        ///  pointer in which to store the reference
        /// </param>
        /// <param name="iter">the iterator</param>
        /// <returns>
        ///  0, GIT_ITEROVER if there are no more; or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_next(
            out git_reference* _out,
            ref git_reference_iterator iter);

        /// <summary>
        ///  Get the next reference's name
        ///
        ///  <para>
        ///   This function is provided for convenience in case only
        ///   the names are interesting as it avoids the allocation of
        ///   the `git_reference` object which `git_reference_next()`
        ///   needs.
        ///  </para>
        /// </summary>
        /// <param name="_out">pointer in which to store the string</param>
        /// <param name="iter">the iterator</param>
        /// <returns>
        ///  0, GIT_ITEROVER if there are no more; or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_next_name(
            char** _out,
            ref git_reference_iterator iter);

        /// <summary>
        ///  Free the iterator and its associated resources
        /// </summary>
        /// <param name="iter">the iterator to free</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_reference_iterator_free(
            ref git_reference_iterator iter);

        /// <summary>
        ///  Perform a callback on each reference in the repository whose
        ///  name matches the given pattern.
        ///
        ///  <para>
        ///   This function acts like `git_reference_foreach()` with
        ///   an additional pattern match being applied to the
        ///   reference name before issuing the callback function. See
        ///   that function for more information.
        ///  </para>
        ///
        ///  <para>
        ///   The pattern is matched using fnmatch or "glob" style
        ///   where a '*' matches any sequence of letters, a '?'
        ///   matches any letter, and square brackets can be used to
        ///   define character ranges (such as "[0-9]" for digits).
        ///  </para>
        /// </summary>
        /// <param name="repo">Repository where to find the refs</param>
        /// <param name="glob">
        ///  Pattern to match (fnmatch-style) against reference name.
        /// </param>
        /// <param name="callback">
        ///  Function which will be called for every listed ref
        /// </param>
        /// <param name="payload">
        ///  Additional data to pass to the callback
        /// </param>
        /// <returns>
        ///  0 on success, GIT_EUSER on non-zero callback, or error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_foreach_glob(
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string glob,
            git_reference_foreach_name_cb callback,
            IntPtr payload);

        /// <summary>
        ///  Check if a reflog exists for the specified reference.
        /// </summary>
        /// <param name="repo">the repository</param>
        /// <param name="refname">the reference's name</param>
        /// <returns>
        ///  0 when no reflog can be found, 1 when it exists; otherwise
        ///  an error code.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_has_log(
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string refname);

        /// <summary>
        ///  Ensure there is a reflog for a particular reference.
        ///
        ///  <para>
        ///   Make sure that successive updates to the reference will
        ///   append to its log.
        ///  </para>
        /// </summary>
        /// <param name="repo">the repository</param>
        /// <param name="refname">the reference's name</param>
        /// <returns>0 or an error code.</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_ensure_log(
            git_repository* repo,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string refname);

        /// <summary>
        ///  Check if a reference is a local branch.
        /// </summary>
        /// <param name="_ref">A git reference</param>
        /// <returns>
        ///  1 when the reference lives in the refs/heads namespace; 0
        ///  otherwise.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_is_branch(
            git_reference* _ref);

        /// <summary>
        ///  Check if a reference is a remote tracking branch
        /// </summary>
        /// <param name="_ref">A git reference</param>
        /// <returns>
        ///  1 when the reference lives in the refs/remotes namespace; 0
        ///  otherwise.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_is_remote(
            git_reference* _ref);

        /// <summary>
        ///  Check if a reference is a tag
        /// </summary>
        /// <param name="_ref">A git reference</param>
        /// <returns>
        ///  1 when the reference lives in the refs/tags namespace; 0
        ///  otherwise.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_is_tag(
            git_reference* _ref);

        /// <summary>
        ///  Check if a reference is a note
        /// </summary>
        /// <param name="_ref">A git reference</param>
        /// <returns>
        ///  1 when the reference lives in the refs/notes namespace; 0
        ///  otherwise.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_is_note(
            git_reference* _ref);

        /// <summary>
        ///  Normalize reference name and check validity.
        ///
        ///  <para>
        ///   This will normalize the reference name by removing any
        ///   leading slash '/' characters and collapsing runs of
        ///   adjacent slashes between name components into a single
        ///   slash.
        ///  </para>
        ///
        ///  <para>
        ///   Once normalized, if the reference name is valid, it will
        ///   be returned in the user allocated buffer.
        ///  </para>
        ///
        ///  <para>
        ///   See `git_reference_symbolic_create()` for rules about
        ///   valid names.
        ///  </para>
        /// </summary>
        /// <param name="buffer_out">
        ///  User allocated buffer to store normalized name
        /// </param>
        /// <param name="buffer_size">Size of buffer_out</param>
        /// <param name="name">Reference name to be checked.</param>
        /// <param name="flags">
        ///  Flags to constrain name validation rules - see the
        ///  GIT_REFERENCE_FORMAT constants above.
        /// </param>
        /// <returns>
        ///  0 on success, GIT_EBUFS if buffer is too small,
        ///  GIT_EINVALIDSPEC or an error code.
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_normalize_name(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string buffer_out,
            UIntPtr buffer_size,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name,
            uint flags);

        /// <summary>
        ///  Recursively peel reference until object of the specified
        ///  type is found.
        ///
        ///  <para>
        ///   The retrieved `peeled` object is owned by the repository
        ///   and should be closed with the `git_object_free` method.
        ///  </para>
        ///
        ///  <para>
        ///   If you pass `GIT_OBJECT_ANY` as the target type, then
        ///   the object will be peeled until a non-tag object is met.
        ///  </para>
        /// </summary>
        /// <param name="_out">Pointer to the peeled git_object</param>
        /// <param name="_ref">The reference to be processed</param>
        /// <param name="type">
        ///  The type of the requested object (GIT_OBJECT_COMMIT,
        ///  GIT_OBJECT_TAG, GIT_OBJECT_TREE, GIT_OBJECT_BLOB or
        ///  GIT_OBJECT_ANY).
        /// </param>
        /// <returns>
        ///  0 on success, GIT_EAMBIGUOUS, GIT_ENOTFOUND or an error code
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_peel(
            out git_object* _out,
            git_reference* _ref,
            git_object_t type);

        /// <summary>
        ///  Ensure the reference name is well-formed.
        ///
        ///  <para>
        ///   Valid reference names must follow one of two patterns:
        ///  </para>
        ///
        ///  <para>
        ///   1. Top-level names must contain only capital letters and
        ///   underscores, and must begin and end with a letter. (e.g.
        ///   "HEAD", "ORIG_HEAD"). 2. Names prefixed with "refs/" can
        ///   be almost anything. You must avoid the characters '~',
        ///   '^', ':', ' \ ', '?', '[', and '*', and the sequences
        ///   ".." and "@{" which have special meaning to revparse.
        ///  </para>
        /// </summary>
        /// <param name="valid">
        ///  output pointer to set with validity of given reference name
        /// </param>
        /// <param name="refname">name to be checked.</param>
        /// <returns>0 on success or an error code</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_reference_name_is_valid(
            int* valid,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string refname);

        /// <summary>
        ///  Get the reference's short name
        ///
        ///  <para>
        ///   This will transform the reference name into a name
        ///   "human-readable" version. If no shortname is
        ///   appropriate, it will return the full name.
        ///  </para>
        ///
        ///  <para>
        ///   The memory is owned by the reference and must not be
        ///   freed.
        ///  </para>
        /// </summary>
        /// <param name="_ref">a reference</param>
        /// <returns>the human-readable version of the name</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_reference_shorthand(
            git_reference* _ref);
    }
}
