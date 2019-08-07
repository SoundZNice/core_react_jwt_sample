using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core3.Application.Interfaces;
using Core3.Application.Models.Notes;
using Core3.Common.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core3.Application.Queries.Notes
{
    public class GetNotesQueryHandler : IRequestHandler<GetNotesQuery, IReadOnlyCollection<NoteViewModel>>
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

        public async Task<IReadOnlyCollection<NoteViewModel>> Handle(GetNotesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Notes.ProjectTo<NoteViewModel>(_mapper.ConfigurationProvider)
                .OrderBy(n => n.DateModified)
                .ToListAsync(cancellationToken);
        }
    }
}
