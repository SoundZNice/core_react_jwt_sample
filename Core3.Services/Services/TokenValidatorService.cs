using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Core3.Application.Interfaces.Services;
using Core3.Common.Helpers;
using Core3.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Internal;

namespace Core3.Services.Services
{
    public class TokenValidatorService : ITokenValidatorService
    {
        private readonly IUserService _userService;
        private readonly ITokenStoreService _tokenStoreService;

        public TokenValidatorService(IUserService userService, ITokenStoreService tokenStoreService)
        {
            Guard.ArgumentNotNull(userService, nameof(userService));
            Guard.ArgumentNotNull(tokenStoreService, nameof(tokenStoreService));

            _userService = userService;
            _tokenStoreService = tokenStoreService;
        }

        public async Task ValidateAsync(TokenValidatedContext context)
        {
            ClaimsPrincipal userPrincipal = context.Principal;

            ClaimsIdentity claimsIdentity = context.Principal.Identity as ClaimsIdentity;
            if (claimsIdentity?.Claims == null || !claimsIdentity.Claims.Any())
            {
                context.Fail("No claims");
                return;
            }

            Claim serialNumberClaim = claimsIdentity.FindFirst(ClaimTypes.SerialNumber);
            if (serialNumberClaim == null)
            {
                context.Fail("No serial");
                return;
            }

            string userIdString = claimsIdentity.FindFirst(ClaimTypes.UserData).Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                context.Fail("No User id");
            }

            User user = await _userService.FindUserAsync(userId);
            if (user == null || user.SerialNumber != serialNumberClaim.Value || !user.IsActive)
            {
                context.Fail("This token is expired.Please login again.");
            }

            JwtSecurityToken accessToken = context.SecurityToken as JwtSecurityToken;
            if (accessToken == null || string.IsNullOrWhiteSpace(accessToken.RawData) ||
                !await _tokenStoreService.IsValidTokenAsync(accessToken.RawData, userId))
            {
                context.Fail("Token not found in db");
                return;
            }

            await _userService.UpdateUserLastActivityDateAsync(userId);
        }
    }
}
