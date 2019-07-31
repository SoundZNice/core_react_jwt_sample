using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core3.Application.Models.Note;
using MediatR;

namespace Core3.Application.Queries.Notes
{
    public class GetNotesQueryHandler : IRequestHandler<GetNotesQuery, List<NoteDto>>
    {
        public Task<List<NoteDto>> Handle(GetNotesQuery request, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                Thread.Sleep(1000);
                return new List<NoteDto>
                {
                    new NoteDto
                    {
                        Id = Guid.NewGuid(),
                        Text = "Text 1"
                    },
                    new NoteDto
                    {
                        Id = Guid.NewGuid(),
                        Text = "Text 2"
                    }
                };
            }, cancellationToken);
        }
    }
}
