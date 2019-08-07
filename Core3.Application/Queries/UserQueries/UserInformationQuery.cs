using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core3.Application.Interfaces;
using Core3.Application.Interfaces.Services;
using Core3.Application.Models.Users;
using Core3.Common.Helpers;
using Core3.Domain.Entities;
using MediatR;

namespace Core3.Application.Queries.UserQueries
{
    public class UserInformationQuery : IRequest<UserViewModel>
    {
        public class UserInformationQueryHandler : IRequestHandler<UserInformationQuery, UserViewModel>
        {
            private readonly ICore3DbContext _context;
            private readonly IMapper _mapper;
            private readonly IUserService _userService;

            public UserInformationQueryHandler(
                ICore3DbContext context, 
                IMapper mapper,
                IUserService userService)
            {
                Guard.ArgumentNotNull(context, nameof(context));
                Guard.ArgumentNotNull(mapper, nameof(mapper));
                Guard.ArgumentNotNull(userService, nameof(userService));

                _context = context;
                _mapper = mapper;
                _userService = userService;
            }

            public async Task<UserViewModel> Handle(UserInformationQuery request, CancellationToken cancellationToken)
            {
                User user = await _userService.GetCurrentUserAsync();

                UserViewModel userViewModel = _mapper.Map<UserViewModel>(user);

                return userViewModel;
            }
        }
    }
}
