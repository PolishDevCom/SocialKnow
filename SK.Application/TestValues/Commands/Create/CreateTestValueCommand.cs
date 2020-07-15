using MediatR;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.TestValues.Commands.Create
{
    public class CreateTestValueCommand
    {
        public class Command : IRequest<int>
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var testValue = new TestValue();

                testValue.Id = request.Id;
                testValue.Name = request.Name;

                _context.TestValues.Add(testValue);
                await _context.SaveChangesAsync(cancellationToken);

                return testValue.Id;
            }
        }
    }
}