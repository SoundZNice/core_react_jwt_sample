using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core3.Application.Models.Note;
using Core3.Application.Queries.Notes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core3.WebUI.Controllers
{
    [ApiController]
    public class NoteController : BaseController
    {
        [Route("api/notes")]
        [HttpGet]
        public async Task<ActionResult<IList<NoteDto>>> GetNotes()
        {
            return Ok(await Mediator.Send(new GetNotesQuery()));
        }
    }
}