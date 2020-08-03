using MediatR;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.TestValues.Commands.DeleteTestValue
{
    public class DeleteTestValueCommandHandler : IRequestHandler<DeleteTestValueCommand>
    {
            private readonly IApplicationDbContext _context;
            public DeleteTestValueCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteTestValueCommand request, CancellationToken cancellationToken)
            {
                var testValue = await _context.TestValues.FindAsync(request.Id) ?? throw new NotFoundException(nameof(TestValue), request.Id); ;

                _context.TestValues.Remove(testValue);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
    }
}
