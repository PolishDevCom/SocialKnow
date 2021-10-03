using MediatR;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Models;
using SK.Application.Common.Wrappers;
using SK.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Events.Queries.ListEvent
{
    public class ListEventQueryHandler : IRequestHandler<ListEventQuery, PagedResponse<List<EventDto>>>
    {
        private readonly IPaginationService<Event, EventDto> _paginationService;

        public ListEventQueryHandler(IPaginationService<Event, EventDto> paginationService)
        {
            _paginationService = paginationService;
        }

        public async Task<PagedResponse<List<EventDto>>> Handle(ListEventQuery request, CancellationToken cancellationToken)
        {
            var route = request.Path;
            var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
            return await _paginationService.GetPagedData(validFilter, route, cancellationToken, e => e.Date);
        }
    }
}