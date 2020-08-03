using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SK.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.TestValues.Queries.DetailsTestValue
{
    public class DetailsTestValueQueryHandler : IRequestHandler<DetailsTestValueQuery, TestValueDto>
    {

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DetailsTestValueQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TestValueDto> Handle(DetailsTestValueQuery request, CancellationToken cancellationToken)
        {
            return await _context.TestValues
                .ProjectTo<TestValueDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(tv => tv.Id == request.Id);
        }
    }
}
