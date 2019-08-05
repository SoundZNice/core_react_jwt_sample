using System;
using System.Collections.Generic;
using System.Text;

namespace Core3.Application.Models.ApiSettings
{
    public class ApiSettings
    {
        public string LoginPath { get; set; }
        public string LogoutPath { get; set; }
        public string RefreshTokenPaht { get; set; }
        public string AccessTokenObjectKey { get; set; }
        public string RefreshTokenObjectKey { get; set; }
        public string AdminRoleName { get; set; }
    }
}
