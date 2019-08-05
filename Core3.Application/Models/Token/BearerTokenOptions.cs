using System;
using System.Collections.Generic;
using System.Text;

namespace Core3.Application.Models.Token
{
    public class BearerTokenOptions
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationMinutes { get; set; }
        public bool AllowMultipleLoginFromTheSameUser { get; set; }
        public bool AllowSignoutAllUserActiveClients { get; set; }
    }
}
