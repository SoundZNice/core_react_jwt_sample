using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core3.Application.Models.Token;
using Core3.Application.Models.User;

namespace Core3.Application.Interfaces.Services
{
    public interface ITokenFactoryService
    {
        Task<JwtTokensData> CreateJwtTokensAsync(UserDto user);

        string GetRefreshTokenSerial(string refreshTokenValue);
    }
}
