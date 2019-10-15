using System;
using System.Threading.Tasks;
using Core3.Domain.Entities;

namespace Core3.Application.Interfaces.Services
{
    public interface ITokenStoreService
    {
        Task AddUserTokenAsync(UserToken userToken);

        Task AddUserTokenAsync(User user, string refreshTOkenSerial, string accessToken,
            string refreshTokenSourceSerial);

        Task<bool> IsValidTokenAsync(string accessToken, Guid userId);

        Task DeleteExpiredTokensAsync();

        Task<UserToken> FindTokenAsync(string refreshTokenValue);

        Task DeleteTokenAsync(string refreshTokenValue);

        Task DeleteTokensWithSameRefreshTokenSourceAsync(string refreshTokenIdHashSource);

        Task InvalidateUserTokensAsync(Guid userId);

        Task RevokeUserBearerTokensAsync(string userIdValue, string refreshTokenValue);
    }
}
