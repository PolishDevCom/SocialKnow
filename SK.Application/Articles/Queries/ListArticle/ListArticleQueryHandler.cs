using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SK.Application.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Articles.Queries.ListArticle
{ 
    public class ListArticleQueryHandler : IRequestHandler<ListArticleQuery, List<ArticleDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ListArticleQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ArticleDto>> Handle(ListArticleQuery request, CancellationToken cancellationToken)
        {
            return await _context.Articles
                .ProjectTo<ArticleDto>(_mapper.ConfigurationProvider)
                .OrderByDescending(a => a.Created)
                .ToListAsync(cancellationToken);
        }
    }
}