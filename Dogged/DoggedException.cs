using System;
using Dogged.Native;

namespace Dogged
{
    /// <summary>
    /// This is a base class for exceptions from the Dogged library.
    /// </summary>
    public class DoggedException : Exception
    {
        internal DoggedException(string message) : base(message) { }
    }

    /// <summary>
    /// This is a base class for exceptions that occurred in libgit2
    /// or the Dogged.Native proxy layer.
    /// </summary>
    public class NativeException : DoggedException
    {
        internal NativeException(string message) : base(message) { }
    }

    /// <summary>
    /// A user-specified callback indicated that execution of the
    /// current function should stop.
    /// </summary>
    public class UserCancelledException : NativeException
    {
        internal UserCancelledException(string message) : base(message) { }
    }

    /// <summary>
    /// The operation cannot be performed on a bare repository.
    /// </summary>
    public class BareRepositoryException : NativeException
    {
        internal BareRepositoryException(string message) : base(message) { }
    }

    /// <summary>
    /// The operation cannot be performed since an item already exists.
    /// </summary>
    public class ItemExistsException : NativeException
    {
        internal ItemExistsException(string message) : base(message) { }
    }

    /// <summary>
    /// The name or specification provided is not in the valid format.
    /// </summary>
    public class InvalidSpecificationException : NativeException
    {
        internal InvalidSpecificationException(string message) : base(message) { }
    }

    /// <summary>
    /// The operation cannot be performed since unmerged (conflicting) index
    /// entries exist.
    /// </summary>
    public class UnmergedIndexEntriesException : NativeException
    {
        internal UnmergedIndexEntriesException(string message) : base(message) { }
    }

    /// <summary>
    /// The reference is not fast-forwardable.
    /// </summary>
    public class NonFastForwardException : NativeException
    {
        internal NonFastForwardException(string message) : base(message) { }
    }

    /// <summary>
    /// The checkout cannot be performed since the working tree has
    /// changes that conflict with the checkout target.
    /// </summary>
    public class CheckoutConflictException : NativeException
    {
        internal CheckoutConflictException(string message) : base(message) { }
    }

    /// <summary>
    /// A file is locked.
    /// </summary>
    public class LockedFileException : NativeException
    {
        internal LockedFileException(string message) : base(message) { }
    }

    /// <summary>
    /// The requested object was not found.
    /// </summary>
    public class NotFoundException : NativeException
    {
        internal NotFoundException(string message) : base(message) { }
    }

    /// <summary>
    /// The object cannot be peeled.
    /// </summary>
    public class CannotBePeeledException : NativeException
    {
        internal CannotBePeeledException(string message) : base(message) { }
    }
}
