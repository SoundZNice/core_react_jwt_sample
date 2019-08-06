using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core3.Application.Exceptions;
using Core3.Application.Interfaces;
using Core3.Application.ViewModels;
using Core3.Common.Helpers;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core3.Application.Queries.User
{
    public class UserInformationQuery : IRequest<UserViewModel>
    {
        public ClaimsPrincipal User { get; set; }

        public class UserInformationQueryHandler : IRequestHandler<UserInformationQuery, UserViewModel>
        {
            private readonly ICore3DbContext _context;
            private readonly IMapper _mapper;

            public UserInformationQueryHandler(ICore3DbContext context, IMapper mapper)
            {
                Guard.ArgumentNotNull(context, nameof(context));
                Guard.ArgumentNotNull(mapper, nameof(mapper));

                _context = context;
                _mapper = mapper;
            }

            public async Task<UserViewModel> Handle(UserInformationQuery request, CancellationToken cancellationToken)
            {
                ClaimsIdentity claimsIdentity = request.User.Identity as ClaimsIdentity;
                if (claimsIdentity == null)
                {
                    throw new NotFoundException(nameof(claimsIdentity), claimsIdentity);
                }

                UserViewModel user = await _context.Users.ProjectTo<UserViewModel>(_mapper.ConfigurationProvider)
                    .Where(u => string.Equals(u.UserName, claimsIdentity.Name))
                    .FirstOrDefaultAsync(cancellationToken);

                return user;
            }
        }

        public class UserInformationQueryValidator : AbstractValidator<UserInformationQuery>
        {
            public UserInformationQueryValidator()
            {
                RuleFor(x => x.User).NotNull().WithMessage(x => $"{nameof(x.User)} is missing");
            }
        }
    }
}
