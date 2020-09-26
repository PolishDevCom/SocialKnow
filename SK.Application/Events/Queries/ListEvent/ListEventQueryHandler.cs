using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SK.Application.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Events.Queries.ListEvent
{
    public class ListEventQueryHandler : IRequestHandler<ListEventQuery, List<EventDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ListEventQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<EventDto>> Handle(ListEventQuery request, CancellationToken cancellationToken)
        {
            return await _context.Events
                .ProjectTo<EventDto>(_mapper.ConfigurationProvider)
                .OrderByDescending(e => e.Date)
                .ToListAsync(cancellationToken);
        }
    }
}
