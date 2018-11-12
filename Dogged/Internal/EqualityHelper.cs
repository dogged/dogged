using System;

namespace Dogged
{
    /// <summary>
    /// Provides object equality and hash code support based on object
    /// member values.
    /// </summary>
    internal class EqualityHelper<T>
    {
        private readonly Func<T, object>[] contributorAccessors;

        /// <summary>
        /// Create an equality helper for an object.
        /// </summary>
        /// <param name="accessors"/>List of accessors that contribute to the object's equality</param>
        public EqualityHelper(params Func<T, object>[] accessors)
        {
            this.contributorAccessors = accessors;
        }

        /// <summary>
        /// Provide a hash code implementation based on the values of
        /// the contributor accessors.
        /// </summary>
        public int GetHashCode(T instance)
        {
            int hashCode = GetType().GetHashCode();

            unchecked
            {
                foreach (Func<T, object> accessor in contributorAccessors)
                {
                    object item = accessor(instance);
                    hashCode = (hashCode * 397) ^ (item != null ? item.GetHashCode() : 0);
                }
            }

            return hashCode;
        }

        /// <summary>
        /// Provide an equality implementation based on the values of the
        /// contributor accessors of the two values.
        /// </summary>
        public bool Equals(T instance, T other)
        {
            if (ReferenceEquals(null, instance) || ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(instance, other))
            {
                return true;
            }

            if (instance.GetType() != other.GetType())
            {
                return false;
            }

            foreach (Func<T, object> accessor in contributorAccessors)
            {
                if (!Equals(accessor(instance), accessor(other)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
