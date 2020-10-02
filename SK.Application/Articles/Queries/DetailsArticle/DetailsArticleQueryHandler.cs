using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Articles.Queries.DetailsArticle
{ 
    public class DetailsArticleQueryHandler : IRequestHandler<DetailsArticleQuery, ArticleDto>
    {

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DetailsArticleQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ArticleDto> Handle(DetailsArticleQuery request, CancellationToken cancellationToken)
        {
            return await _context.Articles
                .ProjectTo<ArticleDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(a => a.Id == request.Id)
                ??
                throw new NotFoundException(nameof(Article), request.Id);
        }
    }
}
