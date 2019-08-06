using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Core3.Application.Interfaces;
using Core3.Application.Interfaces.Services;
using Core3.Common.Helpers;
using Core3.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Core3.Services.Services
{
    public class UserService : IUserService
    {
        private readonly ICore3DbContext _context;
        private readonly ISecurityService _securityService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
            ICore3DbContext context,
            ISecurityService securityService,
            IHttpContextAccessor httpContextAccessor)
        {
            Guard.ArgumentNotNull(context, nameof(context));
            Guard.ArgumentNotNull(securityService, nameof(securityService));
            Guard.ArgumentNotNull(httpContextAccessor, nameof(httpContextAccessor));

            _context = context;
            _securityService = securityService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetSerialNumberAsync(Guid userId)
        {
            User user = await FindUserAsync(userId);
            return user.SerialNumber;
        }

        public async Task<User> FindUserAsync(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task<User> FindUserAsync(string userName, string password)
        {
            string passwordHash = _securityService.GetSha256Hash(password);
            return await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == userName && x.Password == passwordHash);
        }

        public async Task<User> FindUserAsync(Guid userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task UpdateUserLastActivityDateAsync(Guid userId)
        {
            User user = await FindUserAsync(userId);
            bool needUpdate = true;

            if (user.LastLoggedIn != null)
            {
                TimeSpan updateLastActivityDate = TimeSpan.FromMinutes(2);
                DateTimeOffset currentUtc = DateTimeOffset.UtcNow;
                TimeSpan timeElapsed = currentUtc.Subtract(user.LastLoggedIn.Value);
                if (timeElapsed < updateLastActivityDate)
                    needUpdate = false;
            }

            if (needUpdate)
            {
                user.LastLoggedIn = DateTimeOffset.UtcNow;
                await _context.SaveChangesAsync(default);
            }
        }

        public async Task<User> GetCurrentUserAsync()
        {
            Guid userId = GetCurrentUserId();
            return await FindUserAsync(userId);
        }

        public Guid GetCurrentUserId()
        {
            ClaimsIdentity claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            Claim userDataClaim = claimsIdentity?.FindFirst(ClaimTypes.UserData);
            string userId = userDataClaim?.Value;
            return string.IsNullOrWhiteSpace(userId) ? Guid.Empty : Guid.Parse(userId);
        }

        public async Task<(bool succeeded, string error)> ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            string currentPasswordHash = _securityService.GetSha256Hash(currentPassword);
            if (user.Password != currentPasswordHash)
            {
                return (false, "Current pass is wrong.");
            }

            user.Password = _securityService.GetSha256Hash(newPassword);
            await _context.SaveChangesAsync(default);
            return (true, string.Empty);
        }
    }
}
