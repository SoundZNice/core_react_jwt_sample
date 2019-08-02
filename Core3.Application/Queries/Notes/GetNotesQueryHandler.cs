using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core3.Application.Interfaces;
using Core3.Application.Models.Note;
using Core3.Common.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core3.Application.Queries.Notes
{
    public class GetNotesQueryHandler : IRequestHandler<GetNotesQuery, List<NoteDto>>
    {
        private readonly ICore3DbContext _context;
        private readonly IMapper _mapper;

        public GetNotesQueryHandler(ICore3DbContext context, IMapper mapper)
        {
            Guard.ArgumentNotNull(context, nameof(context));
            Guard.ArgumentNotNull(mapper, nameof(mapper));

            _context = context;
            _mapper = mapper;
        }

        public async Task<List<NoteDto>> Handle(GetNotesQuery request, CancellationToken cancellationToken)
        {
            throw new Exception("Smth went wrong");
            return await _context.Notes.ProjectTo<NoteDto>(_mapper.ConfigurationProvider)
                .OrderBy(n => n.DateModified)
                .ToListAsync(cancellationToken);
        }
    }
}
