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

namespace SK.Application.Articles.Queries.ListArticle
{ 
    public class ListArticleQueryHandler : IRequestHandler<ListArticleQuery, PagedResponse<List<ArticleDto>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public ListArticleQueryHandler(IApplicationDbContext context, IMapper mapper, IUriService uriService)
        {
            _context = context;
            _mapper = mapper;
            _uriService = uriService;
        }

        public async Task<PagedResponse<List<ArticleDto>>> Handle(ListArticleQuery request, CancellationToken cancellationToken)
        {
            var route = request.Path;
            var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
            var pagedData = await _context.Articles
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ProjectTo<ArticleDto>(_mapper.ConfigurationProvider)
                .OrderByDescending(a => a.Created)
                .ToListAsync(cancellationToken);
            var totalRecords = await _context.Articles.CountAsync();
            var pagedResponse = PaginationHelper.CreatePagedReponse<ArticleDto>(pagedData, validFilter, totalRecords, _uriService, route);

            return pagedResponse;

        }
    }
}