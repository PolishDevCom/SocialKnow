using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SK.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.TestValues.Queries.DetailsTestValue
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
            return await _context.TestValues
                .ProjectTo<ArticleDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(tv => tv.Id == request.Id);
        }
    }
}
