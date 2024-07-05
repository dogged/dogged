using System;

namespace Dogged
{
    /// <summary>
    /// A class that can attest to its disposal state.
    /// </summary>
    public abstract class KnownDisposable : IDisposable
    {
        internal abstract bool IsDisposed { get; }
        public abstract void Dispose();
    }
}
