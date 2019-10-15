using System.Collections.Generic;
using Core3.Application.Models.Notes;
using MediatR;

namespace Core3.Application.Queries.Notes
{
    public class GetNotesQuery : IRequest<IReadOnlyCollection<NoteViewModel>>
    {
    }
}
