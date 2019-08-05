using System;
using System.Collections.Generic;
using System.Text;

namespace Core3.Application.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset? LastLoggedIn { get; set; }

        public string SerialNumber { get; set; }
    }
}
