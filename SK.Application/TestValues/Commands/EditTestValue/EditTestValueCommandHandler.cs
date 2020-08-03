using MediatR;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.TestValues.Commands.EditTestValue
{
    public class EditTestValueCommandHandler : IRequestHandler<EditTestValueCommand>
    {
        private readonly IApplicationDbContext _context;

        public EditTestValueCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(EditTestValueCommand request, CancellationToken cancellationToken)
        {
            var testValue = await _context.TestValues.FindAsync(request.Id) ?? throw new NotFoundException(nameof(TestValue), request.Id);

            testValue.Id = request.Id;
            testValue.Name = request.Name;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

    }
}
