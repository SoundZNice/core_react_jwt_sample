using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Core3.Application.Commands.Note
{
    public class CreateNoteCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public class Handler : IRequestHandler<CreateNoteCommand, Unit>
        {
            public async Task<Unit> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
            {
                return Unit.Value;
            }
        }
    }
}
