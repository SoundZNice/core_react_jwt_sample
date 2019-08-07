using System.Collections.Generic;
using System.Threading.Tasks;
using Core3.Application.Commands.Notes;
using Core3.Application.Models.Notes;
using Core3.Application.Queries.Notes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core3.WebUI.Controllers
{
    [ApiController]
    public class NoteController : BaseController
    {
        [Route("api/notes")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<NoteViewModel>>> GetList()
        {
            return Ok(await Mediator.Send(new GetNotesQuery()));
        }

        [Authorize]
        [Route("api/user-notes")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<NoteViewModel>>> GetUserList()
        {
            return Ok(await Mediator.Send(new GetUserNotesQuery()));
        }

        [Route("api/notes")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNoteCommand command)
        {
            await Mediator.Send(command);

            return NoContent();
        }
    }
}