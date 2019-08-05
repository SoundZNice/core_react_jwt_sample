using System;
using System.Collections.Generic;

namespace Core3.Domain.Entities
{
    public sealed class User
    {
        public User()
        {
            UserTokens = new HashSet<UserToken>();
        }

        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string DisplayName { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset? LastLoggedIn { get; set; }

        public string SerialNumber { get; set; }

        public ICollection<UserToken> UserTokens { get; set; }
    }
}
