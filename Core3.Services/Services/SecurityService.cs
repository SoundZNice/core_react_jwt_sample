using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Core3.Application.Interfaces.Services;

namespace Core3.Services.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly RandomNumberGenerator _rand = RandomNumberGenerator.Create();
        
        public string GetSha256Hash(string input)
        {
            using (SHA256CryptoServiceProvider hashAlgorithm = new SHA256CryptoServiceProvider())
            {
                byte[] byteValue = Encoding.UTF8.GetBytes(input);
                byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);
                return Convert.ToBase64String(byteHash);
            }
        }

        public Guid CreateCryptographicallySecureGuid()
        {
            byte[] bytes = new byte[16];
            _rand.GetBytes(bytes);
            return new Guid(bytes);
        }
    }
}
