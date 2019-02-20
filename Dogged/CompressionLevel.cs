using System;
using System.Collections;
using System.Collections.Generic;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// Compression levels for objects.
    /// </summary>
    public enum CompressionLevel
    {
        /// <summary>
        /// Perform no compression.
        /// </summary>
        None = 0,

        /// <summary>
        /// Perform minimal compression and operate quickly.
        /// </summary>
        BestSpeed = 1,

        /// <summary>
        /// Perform maximal compression at the expense of speed.
        /// </summary>
        BestCompression = 9,

        /// <summary>
        /// Use the recommended default compression strategy.
        /// </summary>
        Default = -1
    }
}
