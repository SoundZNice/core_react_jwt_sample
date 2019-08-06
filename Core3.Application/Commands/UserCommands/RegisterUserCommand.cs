using System.Threading;
using System.Threading.Tasks;
using Core3.Application.Interfaces;
using Core3.Application.Interfaces.Services;
using Core3.Common.Helpers;
using Core3.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Core3.Application.Commands.UserCommands
{
    public class RegisterUserCommand : IRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string DisplayName { get; set; }

        public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
        {
            private readonly IUserService _userService;
            private readonly ICore3DbContext _context;

            public RegisterUserCommandHandler(IUserService userService,
                ICore3DbContext context)
            {
                Guard.ArgumentNotNull(userService, nameof(userService));
                Guard.ArgumentNotNull(context, nameof(context));

                _userService = userService;
                _context = context;
            }

            public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
            {
                User user = new User
                {
                    UserName = request.UserName,
                    DisplayName = request.DisplayName,
                    Password = _userService.CreatePasswordForUser(request.Password),
                    IsActive = true,
                    SerialNumber = "SerialNumber"
                };

                await _context.Users.AddAsync(user, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }

        public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
        {
            public RegisterUserCommandValidator(IUserService userService)
            {
                Guard.ArgumentNotNull(userService, nameof(userService));

                RuleFor(x => x.UserName).NotEmpty().WithMessage(x => $"{nameof(x.UserName)} is missing");
                RuleFor(x => x.DisplayName).NotEmpty().WithMessage(x => $"{nameof(x.DisplayName)} is missing");
                RuleFor(x => x.Password).NotEmpty().WithMessage(x => $"{nameof(x.Password)} is missing");

                RuleFor(x => x.UserName).MustAsync(async (x, cancellationToken) =>
                {
                    User user = await userService.FindUserAsync(x);
                    return user == null;
                }).WithMessage(x => $"{nameof(x.UserName)} exists");
            }
        }
    }
}
