using System;
using System.Collections;
using System.Collections.Generic;
using Dogged.Native;

namespace Dogged
{
    public class Diff : NativeDisposable
    {
        private unsafe git_diff* nativeDiff;

        private unsafe Diff(git_diff* nativeDiff)
        {
            Ensure.NativePointerNotNull(nativeDiff);
            this.nativeDiff = nativeDiff;
        }

        internal unsafe static Diff FromNative(git_diff* nativeDiff)
        {
            return new Diff(nativeDiff);
        }

        internal unsafe git_diff* NativeDiff
        {
            get
            {
                return nativeDiff;
            }
        }

        internal unsafe override bool IsDisposed
        {
            get
            {
                return (nativeDiff == null);
            }
        }

        internal unsafe override void Dispose(bool disposing)
        {
            if (nativeDiff != null)
            {
                libgit2.git_diff_free(nativeDiff);
                nativeDiff = null;
            }
        }

        /// <summary>
        /// Match a pathspec against files in a diff.
        /// </summary>
        /// <param name="pathSpec">The pathspec to match.</param>
        /// <returns>True if any matches found, otherwise, false</returns>
        public unsafe bool IsMatch(PathSpec pathSpec)
        {
            return IsMatch(pathSpec, PathSpecFlags.Default);
        }

        /// <summary>
        /// Match a pathspec against files in a diff.
        /// </summary>
        /// <param name="pathSpec">The pathspec to match.</param>
        /// <param name="flags">Options to control the match.</param>
        /// <returns>True if any matches found, otherwise, false</returns>
        public unsafe bool IsMatch(PathSpec pathSpec, PathSpecFlags flags)
        {
            flags |= PathSpecFlags.NoMatchError;

            git_pathspec_match_list* matchList = null;
            int ret = Ensure.NativeCall(() => libgit2.git_pathspec_match_diff(ref matchList, NativeDiff, (git_pathspec_flag_t)flags, pathSpec.NativePathspec), this);
            return ret == 0;
        }
    }
}
