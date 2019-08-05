using System;

namespace Core3.Domain.Entities
{
    public class UserToken
    {
        public Guid Id { get; set; }

        public string AccessTokenHash { get; set; }

        public DateTimeOffset AccessTokenExpiresDateTime { get; set; }

        public string RefreshTokenIdHash { get; set; }

        public string RefreshTokenHashSource { get; set; }

        public DateTimeOffset RefreshTokenExpiresDateTIme { get; set; }

        public Guid UserId { get; set; }

        public virtual User User { get; set; }
    }
}
