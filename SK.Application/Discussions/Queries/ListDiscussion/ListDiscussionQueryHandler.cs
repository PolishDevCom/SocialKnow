using MediatR;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Models;
using SK.Application.Common.Wrappers;
using SK.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Discussions.Queries.ListDiscussion
{
    public class ListDiscussionQueryHandler : IRequestHandler<ListDiscussionQuery, PagedResponse<List<DiscussionDto>>>
    {
        private readonly IPaginationService<Discussion, DiscussionDto> _paginationService;

        public ListDiscussionQueryHandler(IPaginationService<Discussion, DiscussionDto> paginationService)
        {
            _paginationService = paginationService;
        }

        public async Task<PagedResponse<List<DiscussionDto>>> Handle(ListDiscussionQuery request, CancellationToken cancellationToken)
        {
            var route = request.Path;
            var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);

            var pagedData = await _paginationService.GetPagedData(validFilter, route, cancellationToken, d => d.IsPinned);
            pagedData.Data.ForEach(d => d.NumberOfPosts = d.Posts?.Count ?? 0);

            return pagedData;
        }
    }
}