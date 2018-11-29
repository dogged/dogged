using System;

using Dogged.Native;
using Dogged.Native.Services;

namespace Dogged
{
    /// <summary>
    /// The signature portion of a git commit, containing the author or
    /// committer information including name, email address and time.
    /// </summary>
    public class Signature
    {
        internal unsafe static Signature FromNative(git_signature* nativeSignature)
        {
            return new Signature() {
                Name = Utf8Converter.FromNative(nativeSignature->name),
                Email = Utf8Converter.FromNative(nativeSignature->email),
                When = DateTimeOffset.FromUnixTimeSeconds(nativeSignature->when.time).ToOffset(TimeSpan.FromMinutes(nativeSignature->when.offset))
            };
        }

        /// <summary>
        /// Gets the full name of the author or committer.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the email address of the author or committer.
        /// </summary>
        public string Email
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the time the commit was authored or committed.
        /// </summary>
        public DateTimeOffset When
        {
            get;
            private set;
        }
    }
}
