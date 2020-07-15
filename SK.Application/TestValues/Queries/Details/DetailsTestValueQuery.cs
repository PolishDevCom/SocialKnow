using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SK.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.TestValues.Queries.Details
{
    public class DetailsTestValueQuery
    {
        public class Query : IRequest<TestValueDto> 
        { 
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, TestValueDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<TestValueDto> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.TestValues
                    .ProjectTo<TestValueDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(tv => tv.Id == request.Id);
            }
        }
    }
}
