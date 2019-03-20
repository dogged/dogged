using System;
using System.Collections;
using System.Collections.Generic;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// Accessors for the Git references (branches, tags) in a repository.
    /// </summary>
    public class ReferenceCollection
    {
        private Repository repository;

        internal ReferenceCollection(Repository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Lookup and return the reference as specified by the given name.
        /// </summary>
        /// <param name="name">The reference to lookup</param>
        /// <returns>The reference specified by the given name</returns>
        public unsafe Reference Lookup(string name)
        {
            Ensure.ArgumentNotNull(name, "name");

            git_reference* reference = null;

            Ensure.NativeSuccess(() => libgit2.git_reference_lookup(out reference, repository.NativeRepository, name), repository);
            Ensure.NativePointerNotNull(reference);

            return Reference.FromNative(reference);
        }
    }
}
