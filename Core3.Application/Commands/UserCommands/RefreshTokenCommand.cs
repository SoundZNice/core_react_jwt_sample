using System.Threading;
using System.Threading.Tasks;
using Core3.Application.Exceptions;
using Core3.Application.Interfaces;
using Core3.Application.Interfaces.Services;
using Core3.Application.Models.Token;
using Core3.Common.Helpers;
using Core3.Domain.Entities;
using FluentValidation;
using MediatR;
using Newtonsoft.Json.Linq;

namespace Core3.Application.Commands.UserCommands
{
    public class RefreshTokenCommand : IRequest<TokenViewModel>
    {
        public JToken JsonBody { get; set; }

        public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenViewModel>
        {
            private readonly ITokenStoreService _tokenStoreService;
            private readonly ITokenFactoryService _tokenFactoryService;
            private readonly ICore3DbContext _context;

            public RefreshTokenCommandHandler(ICore3DbContext context, ITokenStoreService tokenStoreService,
                ITokenFactoryService tokenFactoryService)
            {
                Guard.ArgumentNotNull(context, nameof(context));
                Guard.ArgumentNotNull(tokenStoreService, nameof(tokenStoreService));
                Guard.ArgumentNotNull(tokenFactoryService, nameof(tokenFactoryService));

                _tokenStoreService = tokenStoreService;
                _tokenFactoryService = tokenFactoryService;
                _context = context;
            }

            public async Task<TokenViewModel> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
            {
                string refreshTokenValue = request.JsonBody.Value<string>("refreshToken");

                UserToken token = await _tokenStoreService.FindTokenAsync(refreshTokenValue);
                if (token == null)
                    throw new UnauthrizedException();

                JwtTokensData result = await _tokenFactoryService.CreateJwtTokensAsync(token.User);
                await _tokenStoreService.AddUserTokenAsync(token.User, result.RefreshTokenSerial, result.AccessToken,
                    _tokenFactoryService.GetRefreshTokenSerial(refreshTokenValue));

                await _context.SaveChangesAsync(cancellationToken);

                return new TokenViewModel
                {
                    AccessToken = result.AccessToken,
                    RefreshToken = result.RefreshToken
                };
            }
        }

        public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
        {
            public RefreshTokenCommandValidator()
            {
                RuleFor(x => x).Must(x =>
                {
                    bool result = !(x == null || x.JsonBody == null);

                    string refreshTokenValue = x.JsonBody.Value<string>("refreshToken");
                    if (string.IsNullOrWhiteSpace(refreshTokenValue))
                        result = false;

                    return result;
                }).WithMessage(x => $"{nameof(x.JsonBody)} is missing!");
            }
        }
    }
}
