using System;

namespace Dogged
{
    /// <summary>
    /// An identity of a person in a git repository.
    /// </summary>
    public class RepositoryIdentity
    {
        private readonly string name;
        private readonly string email;

        public RepositoryIdentity(string name, string email)
        {
            this.name = name;
            this.email = email;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string Email
        {
            get
            {
                return email;
            }
        }
    }
}
