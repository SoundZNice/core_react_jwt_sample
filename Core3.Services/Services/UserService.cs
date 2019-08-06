using System;
using System.Threading.Tasks;
using Core3.Application.Interfaces.Services;
using Core3.Application.Models.User;

namespace Core3.Services.Services
{
    public class UserService : IUserService
    {
        public Task<string> GetSerialNumberAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> FindUserAsync(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> FIndUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserLastActivityDateAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> GetCurrentUserAsync()
        {
            throw new NotImplementedException();
        }

        public Guid GetCurrentUserId()
        {
            throw new NotImplementedException();
        }

        public Task<(bool succeeded, string error)> ChangePasswordAsync(UserDto user, string currentPassword, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}
