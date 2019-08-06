using System;
using System.Collections.Generic;
using System.Text;

namespace Core3.Application.Interfaces.Services
{
    public interface ISecurityService
    {
        string GetSha256Hash(string input);

        Guid CreateCryptographicallySecureGuid();
    }
}
