using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core3.Application.Models.User;

namespace Core3.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<string> GetSerialNumberAsync(Guid userId);

        Task<UserDto> FindUserAsync(string userName, string password);

        Task<UserDto> FIndUserAsync(Guid userId);

        Task UpdateUserLastActivityDateAsync(Guid userId);

        Task<UserDto> GetCurrentUserAsync();

        Guid GetCurrentUserId();

        Task<(bool succeeded, string error)> ChangePasswordAsync(UserDto user, string currentPassword,
            string newPassword);
    }
}
