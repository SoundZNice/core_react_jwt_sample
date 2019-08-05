using System.Threading.Tasks;
using Core3.Application.Commands.User;
using Core3.Application.Models.Token;
using Core3.Application.Queries.User;
using Core3.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Core3.WebUI.Controllers
{
    public class AuthController : BaseController
    {
        [IgnoreAntiforgeryToken]
        [Route("api/login")]
        [HttpPost]
        public async Task<ActionResult<TokenViewModel>> Login([FromBody] LoginUserCommand loginUserCommand)
        {
            return Ok(await Mediator.Send(loginUserCommand));
        }

        [Route("api/register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand registerUserCommand)
        {
            return Ok(await Mediator.Send(registerUserCommand));
        }

        [Route("api/refresh")]
        [HttpPost]
        public async Task<ActionResult<TokenViewModel>> RefreshToken([FromBody] RefreshTokenCommand refreshTokenCommand)
        {
            return Ok(await Mediator.Send(refreshTokenCommand));
        }

        [Route("api/logout")]
        [HttpPost]
        public async Task<ActionResult<bool>> Logout([FromBody] LogoutCommand logoutCommand)
        {
            return Ok(await Mediator.Send(logoutCommand));
        }

        [Route("api/check")]
        [HttpPost, HttpGet]
        public bool IsAuthenticated()
        {
            return User.Identity.IsAuthenticated;
        }

        [Route("api/userinfo")]
        [HttpGet]
        public async Task<IActionResult> UserInfo()
        {
            return Ok(await Mediator.Send(new UserInformationQuery()));
        }
    }
}