using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Events.Queries.DetailsEvent
{
    public class DetailsEventQueryHandler : IRequestHandler<DetailsEventQuery, EventDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DetailsEventQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<EventDto> Handle(DetailsEventQuery request, CancellationToken cancellationToken)
        {
            return await _context.Events
                .ProjectTo<EventDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.Id == request.Id) 
                ??
                throw new NotFoundException(nameof(Event), request.Id);
        }
    }
}
