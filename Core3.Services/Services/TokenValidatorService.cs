using System.Threading.Tasks;
using Core3.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Core3.Services.Services
{
    public class TokenValidatorService : ITokenValidatorService
    {
        public Task ValidateAsync(TokenValidatedContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
