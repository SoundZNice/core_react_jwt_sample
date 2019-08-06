using System.Threading.Tasks;
using Core3.Application.Models.Token;
using Core3.Domain.Entities;

namespace Core3.Application.Interfaces.Services
{
    public interface ITokenFactoryService
    {
        Task<JwtTokensData> CreateJwtTokensAsync(User user);

        string GetRefreshTokenSerial(string refreshTokenValue);
    }
}
