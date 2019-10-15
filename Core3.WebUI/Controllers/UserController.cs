using System.Threading.Tasks;
using Core3.Application.Queries.UserQueries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core3.WebUI.Controllers
{
    [ApiController]
    public class UserController : BaseController
    {
        [Authorize]
        [Route("api/userinfo")]
        [HttpGet]
        public async Task<IActionResult> UserInfo()
        {
            return Ok(await Mediator.Send(new UserInformationQuery()));
        }
    }
}