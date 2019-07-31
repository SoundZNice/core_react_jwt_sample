using System;
using System.Collections.Generic;
using System.Text;
using Core3.Application.Models.Note;
using MediatR;

namespace Core3.Application.Queries.Notes
{
    public class GetNotesQuery : IRequest<List<NoteDto>>
    {
    }
}
