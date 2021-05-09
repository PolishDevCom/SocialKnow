using MediatR;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Models;
using SK.Application.Common.Wrappers;
using SK.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Tags.Queries.ListTag
{
    public class ListTagQueryHandler : IRequestHandler<ListTagQuery, PagedResponse<List<TagDto>>>
    {
        private readonly IPaginationService<Tag, TagDto> _paginationService;

        public ListTagQueryHandler(IPaginationService<Tag, TagDto> paginationService)
        {
            _paginationService = paginationService;
        }

        public async Task<PagedResponse<List<TagDto>>> Handle(ListTagQuery request, CancellationToken cancellationToken)
        {
            var route = request.Path;
            var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
            return await _paginationService.GetPagedData(validFilter, route, cancellationToken);
        }
    }
}
