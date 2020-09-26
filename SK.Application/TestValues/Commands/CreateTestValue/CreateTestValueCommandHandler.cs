using MediatR;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.TestValues.Commands.CreateTestValue
{
    public class CreateTestValueCommandHandler : IRequestHandler<CreateTestValueCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateTestValueCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(CreateTestValueCommand request, CancellationToken cancellationToken)
        {
            var testValue = new TestValue
            {
                Id = request.Id,
                Name = request.Name
            };

            _context.TestValues.Add(testValue);
            await _context.SaveChangesAsync(cancellationToken);

            return testValue.Id;
        }
    }
}