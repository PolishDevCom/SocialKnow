using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SK.Application.Common.Helpers;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Models;
using SK.Application.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Tags.Queries.ListTag
{
    public class ListTagQueryHandler : IRequestHandler<ListTagQuery, PagedResponse<List<TagDto>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public ListTagQueryHandler(IApplicationDbContext context, IMapper mapper, IUriService uriService)
        {
            _context = context;
            _mapper = mapper;
            _uriService = uriService;
        }

        public async Task<PagedResponse<List<TagDto>>> Handle(ListTagQuery request, CancellationToken cancellationToken)
        {
            var route = request.Path;
            var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
            var pagedData = await _context.Tags
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ProjectTo<TagDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            var totalRecords = await _context.Tags.CountAsync();
            var pagedResponse = PaginationHelper.CreatePagedReponse<TagDto>(pagedData, validFilter, totalRecords, _uriService, route);

            return pagedResponse;
        }
    }
}
