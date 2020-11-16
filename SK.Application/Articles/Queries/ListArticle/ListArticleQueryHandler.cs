using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

        public ListArticleQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResponse<List<ArticleDto>>> Handle(ListArticleQuery request, CancellationToken cancellationToken)
        {
            var validFilter = new PaginationFilter(pageNumber: request.Filter.PageNumber, pageSize: request.Filter.PageSize);
            var pagedData = await _context.Articles
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ProjectTo<ArticleDto>(_mapper.ConfigurationProvider)
                .OrderByDescending(a => a.Created)
                .ToListAsync(cancellationToken);
            var totalRecords = await _context.Articles.CountAsync();

            return new PagedResponse<List<ArticleDto>>(pagedData, validFilter.PageNumber, validFilter.PageSize);

        }
    }
}