using MediatR;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.TestValues.Commands.Delete
{
    public class DeleteTestValueCommand
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IApplicationDbContext _context;
            public Handler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var testValue = await _context.TestValues.FindAsync(request.Id);

                if (testValue == null)
                {
                    throw new NotFoundException(nameof(TestValue), request.Id);
                }

                _context.TestValues.Remove(testValue);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

        }
    }
}
