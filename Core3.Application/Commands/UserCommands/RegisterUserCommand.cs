using System.Threading;
using System.Threading.Tasks;
using Core3.Application.Interfaces.Services;
using Core3.Common.Helpers;
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
            public Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
            {
                throw new System.NotImplementedException();
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
                    Domain.Entities.User user = await userService.FindUserAsync(x);
                    return user == null;
                }).WithMessage(x => $"{nameof(x.UserName)} exists");
            }
        }
    }
}
