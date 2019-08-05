using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core3.Application.Interfaces;
using Core3.Application.Interfaces.Services;
using Core3.Common.Helpers;
using FluentValidation;
using MediatR;

namespace Core3.Application.Commands.User
{
    public class LogoutCommand : IRequest<bool>
    {
        public string RefreshToken { get; set; }
        public ClaimsPrincipal User { get; set; }

        public class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
        {
            private readonly ITokenStoreService _tokenStoreService;
            private readonly ICore3DbContext _context;

            public LogoutCommandHandler(ITokenStoreService tokenStoreService, ICore3DbContext context)
            {
                Guard.ArgumentNotNull(tokenStoreService, nameof(tokenStoreService));
                Guard.ArgumentNotNull(context, nameof(context));

                _tokenStoreService = tokenStoreService;
                _context = context;
            }

            public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
            {
                bool result = false;

                ClaimsIdentity claimsIdentity = request.User.Identity as ClaimsIdentity;
                if (claimsIdentity != null)
                {
                    string userIdValue = claimsIdentity.FindFirst(ClaimTypes.UserData)?.Value;
                    await _tokenStoreService.RevokeUserBearerTokensAsync(userIdValue, request.RefreshToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    result = true;
                }

                return result;
            }
        }

        public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
        {
            public LogoutCommandValidator()
            {
                RuleFor(x => x.RefreshToken).NotEmpty().WithMessage(x => $"{nameof(x.RefreshToken)} is missing");
                RuleFor(x => x.User).NotNull().WithMessage(x => $"{nameof(x.User)} is missing");
            }
        }
    }
}
