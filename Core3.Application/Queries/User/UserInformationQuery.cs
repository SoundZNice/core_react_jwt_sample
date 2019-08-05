using System;
using System.Collections.Generic;
using System.Text;
using Core3.Application.Models.User;
using MediatR;

namespace Core3.Application.Queries.User
{
    public class UserInformationQuery : IRequest<UserDTO>
    {
    }
}
