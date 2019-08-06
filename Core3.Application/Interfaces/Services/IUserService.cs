using System;
using System.Threading.Tasks;
using Core3.Domain.Entities;

namespace Core3.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<string> GetSerialNumberAsync(Guid userId);

        Task<User> FindUserAsync(string userName);

        Task<User> FindUserAsync(string userName, string password);

        Task<User> FindUserAsync(Guid userId);

        Task UpdateUserLastActivityDateAsync(Guid userId);

        Task<User> GetCurrentUserAsync();

        Guid GetCurrentUserId();

        Task<(bool succeeded, string error)> ChangePasswordAsync(User user, string currentPassword,
            string newPassword);
    }
}
