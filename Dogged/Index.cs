using System;
using System.Collections;
using System.Collections.Generic;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// An index is a map of paths in the repository to object contents.
    /// The repository has an index which acts as the staging area for
    /// changes to be committed and a cache of in-working tree files.
    /// </summary>
    public class Index : NativeDisposable
    {
        private unsafe git_index* nativeIndex;

        private unsafe Index(git_index* nativeIndex)
        {
            Ensure.ArgumentNotNull(nativeIndex, "index");
            this.nativeIndex = nativeIndex;
        }

        internal unsafe static Index FromNative(git_index* nativeIndex)
        {
            return new Index(nativeIndex);
        }

        internal unsafe override bool IsDisposed
        {
            get
            {
                return (nativeIndex == null);
            }
        }

        internal unsafe override void Dispose(bool disposing)
        {
            if (nativeIndex != null)
            {
                libgit2.git_index_free(nativeIndex);
                nativeIndex = null;
            }
        }
    }
}
