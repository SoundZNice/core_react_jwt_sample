using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core3.Application.Models.Notes;
using MediatR;

namespace Core3.Application.Queries.Notes
{
    public class GetUserNotesQuery : IRequest<IReadOnlyCollection<NoteViewModel>>
    {
        public class GetUserNotesQueryHandler : IRequestHandler<GetUserNotesQuery, IReadOnlyCollection<NoteViewModel>>
        {
            public Task<IReadOnlyCollection<NoteViewModel>> Handle(GetUserNotesQuery request, CancellationToken cancellationToken)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
