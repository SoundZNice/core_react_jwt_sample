using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core3.Application.Interfaces;
using Core3.Common.Helpers;
using Core3.Domain.Entities;
using MediatR;

namespace Core3.Application.Commands.Notes
{
    public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand>
    {
        private readonly ICore3DbContext _context;

        public CreateNoteCommandHandler(ICore3DbContext context)
        {
            Guard.ArgumentNotNull(context, nameof(context));

            _context = context;
        }

        public async Task<Unit> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
        {
            Note note = new Note
            {
                Text = request.Text
            };

            _context.Notes.Add(note);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
