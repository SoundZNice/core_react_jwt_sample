using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core3.Application.Interfaces;
using Core3.Application.Interfaces.Services;
using Core3.Application.Models.Token;
using Core3.Common.Helpers;
using Core3.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Core3.Services.Services
{
    public class TokenStoreService : ITokenStoreService
    {
        private readonly ISecurityService _securityService;
        private readonly ICore3DbContext _context;
        private readonly IOptionsSnapshot<BearerTokenOptions> _configuration;
        private readonly ITokenFactoryService _tokenFactoryService;

        public TokenStoreService(
            ISecurityService securityService,
            ICore3DbContext context, 
            IOptionsSnapshot<BearerTokenOptions> configuration,
            ITokenFactoryService tokenFactoryService)
        {
            Guard.ArgumentNotNull(securityService, nameof(SecurityToken));
            Guard.ArgumentNotNull(context, nameof(context));
            Guard.ArgumentNotNull(configuration, nameof(configuration));
            Guard.ArgumentNotNull(tokenFactoryService, nameof(tokenFactoryService));

            _securityService = securityService;
            _context = context;
            _configuration = configuration;
            _tokenFactoryService = tokenFactoryService;
        }

        public async Task AddUserTokenAsync(UserToken userToken)
        {
            if (!_configuration.Value.AllowMultipleLoginFromTheSameUser)
            {
                await InvalidateUserTokensAsync(userToken.UserId);
            }

            await DeleteTokensWithSameRefreshTokenSourceAsync(userToken.RefreshTokenIdHashSource);
            await _context.UserTokens.AddAsync(userToken);
        }

        public async Task AddUserTokenAsync(User user, string refreshTOkenSerial, string accessToken, string refreshTokenSourceSerial)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            UserToken token = new UserToken
            {
                UserId = user.Id,
                RefreshTokenIdHash = _securityService.GetSha256Hash(refreshTOkenSerial),
                RefreshTokenIdHashSource = string.IsNullOrWhiteSpace(refreshTokenSourceSerial)
                    ? null
                    : _securityService.GetSha256Hash(refreshTokenSourceSerial),
                AccessTokenHash = _securityService.GetSha256Hash(accessToken),
                RefreshTokenExpiresDateTIme = now.AddMinutes(_configuration.Value.RefreshTokenExpirationMinutes),
                AccessTokenExpiresDateTime = now.AddMinutes(_configuration.Value.AccessTokenExpirationMinutes)
            };
            await AddUserTokenAsync(token);
        }

        public async Task<bool> IsValidTokenAsync(string accessToken, Guid userId)
        {
            string accessTokenHash = _securityService.GetSha256Hash(accessToken);
            UserToken token =
                await _context.UserTokens.FirstOrDefaultAsync(x =>
                    x.AccessTokenHash == accessTokenHash && x.UserId == userId);

            return token?.AccessTokenExpiresDateTime >= DateTimeOffset.UtcNow;
        }

        public async Task DeleteExpiredTokensAsync()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            await _context.UserTokens.Where(x => x.RefreshTokenExpiresDateTIme < now)
                .ForEachAsync(token => _context.UserTokens.Remove(token));
        }

        public async Task<UserToken> FindTokenAsync(string refreshTokenValue)
        {
            UserToken result = null;

            if (!string.IsNullOrWhiteSpace(refreshTokenValue))
            {
                string refreshTokenSerial = _tokenFactoryService.GetRefreshTokenSerial(refreshTokenValue);
                if (!string.IsNullOrWhiteSpace(refreshTokenSerial))
                {
                    string refreshTokenIdHash = _securityService.GetSha256Hash(refreshTokenSerial);
                    result = await _context.UserTokens
                        .Include(x => x.User)
                        .FirstOrDefaultAsync(x => x.RefreshTokenIdHash == refreshTokenIdHash);
                }
            }

            return result;
        }

        public async Task DeleteTokenAsync(string refreshTokenValue)
        {
            UserToken userToken = await FindTokenAsync(refreshTokenValue);
            if (userToken != null)
                await _context.UserTokens.Where(t => t.Id == userToken.Id)
                    .ForEachAsync(token => _context.UserTokens.Remove(token));
        }

        public async Task DeleteTokensWithSameRefreshTokenSourceAsync(string refreshTokenIdRefreshSource)
        {
            if (!string.IsNullOrWhiteSpace(refreshTokenIdRefreshSource))
            {
                await _context.UserTokens.Where(t => t.RefreshTokenIdHashSource == refreshTokenIdRefreshSource)
                    .ForEachAsync(token => _context.UserTokens.Remove(token));
            }
        }

        public async Task InvalidateUserTokensAsync(Guid userId)
        {
            await _context.UserTokens.Where(x => x.UserId == userId)
                .ForEachAsync(userToken => _context.UserTokens.Remove(userToken));
        }

        public async Task RevokeUserBearerTokensAsync(string userIdValue, string refreshTokenValue)
        {
            if (!string.IsNullOrWhiteSpace(userIdValue) && Guid.TryParse(userIdValue, out Guid userId))
            {
                if (_configuration.Value.AllowSignoutAllUserActiveClients)
                    await InvalidateUserTokensAsync(userId);
            }

            if (!string.IsNullOrWhiteSpace(refreshTokenValue))
            {
                string refreshTokenIdHashSource = _tokenFactoryService.GetRefreshTokenSerial(refreshTokenValue);
                await DeleteTokensWithSameRefreshTokenSourceAsync(refreshTokenIdHashSource);
            }

            await DeleteExpiredTokensAsync();
        }
    }
}
