using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core3.Application.Models.Token;
using Core3.Application.Models.User;
using Core3.Domain.Entities;

namespace Core3.Application.Interfaces.Services
{
    public interface ITokenStoreService
    {
        Task AddUserTokenAsync(UserTokenDto userToken);

        Task AddUserTokenAsync(UserDto user, string refreshTOkenSerial, string accessToken,
            string refreshTokenSourceSerial);

        Task<bool> IsValidTokenAsync(string accessToken, Guid userId);

        Task DeleteExpiredTokensAsync();

        Task<UserTokenDto> FindTokenAsync(string refreshTokenValue);

        Task DeleteTokenAsync(string refreshTokenValue);

        Task DeleteTokensWithSameRefreshTokenSourceAsync(string refreshTokenIdHashSource);

        Task InvalidateUserTokensAsync(Guid userId);

        Task RevokeUserBearerTokensAsync(string userIdValue, string refreshTokenValue);
    }
}
