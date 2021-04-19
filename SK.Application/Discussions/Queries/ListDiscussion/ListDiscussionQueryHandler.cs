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

namespace SK.Application.Discussions.Queries.ListDiscussion
{
    public class ListDiscussionQueryHandler : IRequestHandler<ListDiscussionQuery, PagedResponse<List<DiscussionDto>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public ListDiscussionQueryHandler(IApplicationDbContext context, IMapper mapper, IUriService uriService)
        {
            _context = context;
            _mapper = mapper;
            _uriService = uriService;
        }
        public async Task<PagedResponse<List<DiscussionDto>>> Handle(ListDiscussionQuery request, CancellationToken cancellationToken)
        {
            var route = request.Path;
            var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);

            var pagedData = await _context.Discussions
                .OrderByDescending(d => d.IsPinned)
                .ThenByDescending(d => d.Created)
                .ProjectTo<DiscussionDto>(_mapper.ConfigurationProvider)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync(cancellationToken);

            pagedData.ForEach(d => d.NumberOfPosts = d.Posts?.Count ?? 0);

            var totalRecords = await _context.Discussions.CountAsync();
            var pagedResponse = PaginationHelper.CreatePagedReponse<DiscussionDto>(pagedData, validFilter, totalRecords, _uriService, route);

            return pagedResponse;
        }
    }
}
