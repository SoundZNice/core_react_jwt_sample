using System;
using System.Collections.Generic;
using System.Text;
using Core3.Application.Models.User;

namespace Core3.Application.Models.Token
{
    public class UserTokenDto
    {
        public int Id { get; set; }

        public string AccessTokenHash { get; set; }

        public DateTimeOffset AccessTokenExpiresDateTime { get; set; }

        public string RefreshTokenIdHash { get; set; }

        public string RefreshTokenIdHashSource { get; set; }

        public DateTimeOffset RefreshTokenExpiresDateTime { get; set; }

        public int UserId { get; set; }

        public UserDto User { get; set; }
    }
}
