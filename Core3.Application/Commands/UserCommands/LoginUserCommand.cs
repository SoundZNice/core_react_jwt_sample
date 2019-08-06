using System.Threading;
using System.Threading.Tasks;
using Core3.Application.Exceptions;
using Core3.Application.Interfaces;
using Core3.Application.Interfaces.Services;
using Core3.Application.Models.Token;
using Core3.Application.ViewModels;
using Core3.Common.Helpers;
using FluentValidation;
using MediatR;

namespace Core3.Application.Commands.UserCommands
{
    public class LoginUserCommand : IRequest<TokenViewModel>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, TokenViewModel>
        {
            private readonly ICore3DbContext _context;
            private readonly IUserService _userService;
            private readonly ITokenFactoryService _tokenFactoryService;
            private readonly ITokenStoreService _tokenStoreService;

            public LoginUserCommandHandler(ICore3DbContext context, IUserService userService,
                ITokenFactoryService tokenFactoryService, ITokenStoreService tokenStoreService)
            {
                Guard.ArgumentNotNull(context, nameof(context));
                Guard.ArgumentNotNull(userService, nameof(userService));
                Guard.ArgumentNotNull(tokenFactoryService, nameof(tokenFactoryService));
                Guard.ArgumentNotNull(tokenStoreService, nameof(tokenStoreService));

                _context = context;
                _userService = userService;
                _tokenFactoryService = tokenFactoryService;
                _tokenStoreService = tokenStoreService;
            }

            public async Task<TokenViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
            {
                Domain.Entities.User user = await _userService.FindUserAsync(request.Username, request.Password);
                if (user == null || !user.IsActive)
                {
                    throw new UnauthrizedException($"No active user with login {request.Username} found!");
                }

                JwtTokensData tokens = await _tokenFactoryService.CreateJwtTokensAsync(user);
                await _tokenStoreService.AddUserTokenAsync(user, tokens.RefreshTokenSerial, tokens.AccessToken, null);
                await _context.SaveChangesAsync(cancellationToken);

                return new TokenViewModel
                {
                    AccessToken = tokens.AccessToken,
                    RefreshToken = tokens.RefreshToken
                };
            }
        }

        public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
        {
            public LoginUserCommandValidator(IUserService userService)
            {
                Guard.ArgumentNotNull(userService, nameof(userService));

                RuleFor(x => x.Username).Must(text => !string.IsNullOrWhiteSpace(text))
                    .WithMessage(x => $"{nameof(x.Username)} is empty!");
                RuleFor(x => x.Password).Must(text => !string.IsNullOrWhiteSpace(text))
                    .WithMessage(x => $"{nameof(x.Password)} is empty!");
            }
        }
    }
}
