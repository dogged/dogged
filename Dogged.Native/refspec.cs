using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    ///  A refspec specifies the mapping between remote and local
    ///  reference names when fetch or pushing.
    /// </summary>
    public struct git_refspec
    {
    }

    public static partial class libgit2
    {
        /// <summary>
        ///  Parse a given refspec string
        /// </summary>
        /// <param name="refspec">
        ///  a pointer to hold the refspec handle
        /// </param>
        /// <param name="input">the refspec string</param>
        /// <param name="is_fetch">is this a refspec for a fetch</param>
        /// <returns>
        ///  0 if the refspec string could be parsed, -1 otherwise
        /// </returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_refspec_parse(
            git_refspec** refspec,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string input,
            int is_fetch);

        /// <summary>
        ///  Free a refspec object which has been created by
        ///  git_refspec_parse
        /// </summary>
        /// <param name="refspec">the refspec object</param>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void git_refspec_free(
            git_refspec* refspec);

        /// <summary>
        ///  Get the source specifier
        /// </summary>
        /// <param name="refspec">the refspec</param>
        /// <returns>the refspec's source specifier</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_refspec_src(
            git_refspec* refspec);

        /// <summary>
        ///  Get the destination specifier
        /// </summary>
        /// <param name="refspec">the refspec</param>
        /// <returns>the refspec's destination specifier</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_refspec_dst(
            git_refspec* refspec);

        /// <summary>
        ///  Get the refspec's string
        /// </summary>
        /// <param name="refspec">the refspec</param>
        /// <returns>the refspec's original string</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))]
        public static extern unsafe string git_refspec_string(
            git_refspec* refspec);

        /// <summary>
        ///  Get the force update setting
        /// </summary>
        /// <param name="refspec">the refspec</param>
        /// <returns>1 if force update has been set, 0 otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_refspec_force(
            git_refspec* refspec);

        /// <summary>
        ///  Get the refspec's direction.
        /// </summary>
        /// <param name="spec">refspec</param>
        /// <returns>GIT_DIRECTION_FETCH or GIT_DIRECTION_PUSH</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe git_direction git_refspec_direction(
            git_refspec* spec);

        /// <summary>
        ///  Check if a refspec's source descriptor matches a negative
        ///  reference
        /// </summary>
        /// <param name="refspec">the refspec</param>
        /// <param name="refname">the name of the reference to check</param>
        /// <returns>1 if the refspec matches, 0 otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_refspec_src_matches_negative(
            git_refspec* refspec,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string refname);

        /// <summary>
        ///  Check if a refspec's source descriptor matches a reference
        /// </summary>
        /// <param name="refspec">the refspec</param>
        /// <param name="refname">the name of the reference to check</param>
        /// <returns>1 if the refspec matches, 0 otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_refspec_src_matches(
            git_refspec* refspec,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string refname);

        /// <summary>
        ///  Check if a refspec's destination descriptor matches a
        ///  reference
        /// </summary>
        /// <param name="refspec">the refspec</param>
        /// <param name="refname">the name of the reference to check</param>
        /// <returns>1 if the refspec matches, 0 otherwise</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_refspec_dst_matches(
            git_refspec* refspec,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string refname);

        /// <summary>
        ///  Transform a reference to its target following the refspec's
        ///  rules
        /// </summary>
        /// <param name="_out">where to store the target name</param>
        /// <param name="spec">the refspec</param>
        /// <param name="name">
        ///  the name of the reference to transform
        /// </param>
        /// <returns>0, GIT_EBUFS or another error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_refspec_transform(
            ref git_buf _out,
            git_refspec* spec,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);

        /// <summary>
        ///  Transform a target reference to its source reference
        ///  following the refspec's rules
        /// </summary>
        /// <param name="_out">
        ///  where to store the source reference name
        /// </param>
        /// <param name="spec">the refspec</param>
        /// <param name="name">
        ///  the name of the reference to transform
        /// </param>
        /// <returns>0, GIT_EBUFS or another error</returns>
        [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int git_refspec_rtransform(
            ref git_buf _out,
            git_refspec* spec,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string name);
    }
}
