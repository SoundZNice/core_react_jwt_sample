﻿using System;
using System.Collections.Generic;
using System.Text;
using Core3.Application.Models.Token;

namespace Core3.Application.Models.User
{
    public class UserDto
    {
        public UserDto()
        {
            UserTokens = new HashSet<UserTokenDto>();
        }

        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string DisplayName { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset? LastLoggedIn { get; set; }

        public string SerialNumber { get; set; }

        public ICollection<UserTokenDto> UserTokens { get; set; }
    }
}
