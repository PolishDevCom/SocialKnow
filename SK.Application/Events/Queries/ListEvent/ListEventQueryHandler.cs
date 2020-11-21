using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SK.Application.Common.Helpers;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Models;
using SK.Application.Common.Wrappers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Events.Queries.ListEvent
{
    public class ListEventQueryHandler : IRequestHandler<ListEventQuery, PagedResponse<List<EventDto>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public ListEventQueryHandler(IApplicationDbContext context, IMapper mapper, IUriService uriService)
        {
            _context = context;
            _mapper = mapper;
            _uriService = uriService;
        }

        public async Task<PagedResponse<List<EventDto>>> Handle(ListEventQuery request, CancellationToken cancellationToken)
        {
            var route = request.Path;
            var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
            var pagedData = await _context.Events
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ProjectTo<EventDto>(_mapper.ConfigurationProvider)
                .OrderByDescending(e => e.Date)
                .ToListAsync(cancellationToken);
            var totalRecords = await _context.Events.CountAsync();
            var pagedResponse = PaginationHelper.CreatePagedReponse<EventDto>(pagedData, validFilter, totalRecords, _uriService, route);

            return pagedResponse;
        }
    }
}
