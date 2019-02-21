using System;
using System.Runtime.InteropServices;

namespace Dogged.Native
{
    /// <summary>
    /// Function declaration for callbacks from the git_odb_foreach function.
    /// </summary>
    /// <param name="id">The object ID contained in the database</param>
    /// <param name="payload">The payload provided to the git_odb_foreach function</param>
    /// <returns>0 on success or non-zero to stop iteration</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int git_odb_foreach_cb(
        git_oid* id,
        UIntPtr payload);

    /// <summary>
    /// An object database stores the objects (commit, trees, blobs, tags,
    /// etc) for a repository.
    /// </summary>
    public struct git_odb
    {
    }
}
