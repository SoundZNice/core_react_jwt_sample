using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core3.Application.Interfaces;
using Core3.Application.Interfaces.Services;
using Core3.Application.Models.Token;
using Core3.Application.Models.User;
using Core3.Common.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Core3.Services.Services
{
    public class TokenFactoryService : ITokenFactoryService
    {
        private readonly ISecurityService _securityService;
        private readonly IOptionsSnapshot<BearerTokenOptions> _configuration;
        private readonly ILogger<TokenFactoryService> _logger;

        public TokenFactoryService(
            ISecurityService securityService,
            IOptionsSnapshot<BearerTokenOptions> configuration,
            ILogger<TokenFactoryService> logger)
        {
            Guard.ArgumentNotNull(securityService, nameof(securityService));
            Guard.ArgumentNotNull(configuration, nameof(configuration));
            Guard.ArgumentNotNull(logger, nameof(logger));

            _securityService = securityService;
            _configuration = configuration;
            _logger = logger;
        }

        public Task<JwtTokensData> CreateJwtTokensAsync(UserDto user)
        {
            var (accessToken, claims) = CreateAccessToken(user);
            var (refreshTokenValue, refreshTokenSerial) = CreateRefreshToken();
            return Task.Run(() =>
            {
                return new JwtTokensData
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshTokenValue,
                    RefreshTokenSerial = refreshTokenSerial,
                    Claims = claims
                };
            });
        }

        public string GetRefreshTokenSerial(string refreshTokenValue)
        {
            string result = null;

            if (!string.IsNullOrWhiteSpace(refreshTokenValue))
            {
                ClaimsPrincipal decodedRefreshTokenPrincipal = null;
                try
                {
                    decodedRefreshTokenPrincipal = new JwtSecurityTokenHandler().ValidateToken(
                        refreshTokenValue,
                        new TokenValidationParameters
                        {
                            RequireExpirationTime = true,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            IssuerSigningKey =
                                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Value.Key)),
                            ValidateIssuerSigningKey = true,
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero
                        },
                        out _);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to validate refreshTokenValue {refreshTokenValue}, Exception: {ex.Message}");
                }

                result = decodedRefreshTokenPrincipal?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)
                    ?.Value;
            }

            return result;
        }

        private (string AccessToken, IReadOnlyCollection<Claim> Claims) CreateAccessToken(UserDto user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, _securityService.CreateCryptographicallySecureGuid().ToString(), ClaimValueTypes.String, _configuration.Value.Issuer),
                new Claim(JwtRegisteredClaimNames.Iss, _configuration.Value.Issuer, ClaimValueTypes.String, _configuration.Value.Issuer),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _configuration.Value.Issuer),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String, _configuration.Value.Issuer),
                new Claim(ClaimTypes.Name, user.UserName, ClaimValueTypes.String, _configuration.Value.Issuer),
                new Claim("DisplayName", user.DisplayName, ClaimValueTypes.String, _configuration.Value.Issuer),
                new Claim(ClaimTypes.SerialNumber, user.SerialNumber, ClaimValueTypes.String, _configuration.Value.Issuer),
                new Claim(ClaimTypes.UserData, user.Id.ToString(), ClaimValueTypes.String, _configuration.Value.Issuer)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Value.Key));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(
                _configuration.Value.Issuer,
                _configuration.Value.Audience,
                claims,
                DateTime.UtcNow,
                DateTime.Now.AddMinutes(_configuration.Value.AccessTokenExpirationMinutes),
                creds);

            return (new JwtSecurityTokenHandler().WriteToken(token), claims);
        }

        private (string RefreshTokenValue, string RefrechTokenSerial) CreateRefreshToken()
        {
            string refreshTokenSerial =
                _securityService.CreateCryptographicallySecureGuid().ToString().Replace("_", "");

            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, _securityService.CreateCryptographicallySecureGuid().ToString(), ClaimValueTypes.String, _configuration.Value.Issuer),
                new Claim(JwtRegisteredClaimNames.Iss, _configuration.Value.Issuer, ClaimValueTypes.String, _configuration.Value.Issuer),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _configuration.Value.Issuer),
                new Claim(ClaimTypes.SerialNumber, refreshTokenSerial, ClaimValueTypes.String, _configuration.Value.Issuer)
            };
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Value.Key));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(
                _configuration.Value.Issuer,
                _configuration.Value.Audience,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(_configuration.Value.RefreshTokenExpirationMinutes),
                creds);
            string refreshTokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return (refreshTokenValue, refreshTokenSerial);
        }
    }
}
