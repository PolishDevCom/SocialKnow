using MediatR;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Events.Commands.DeleteEvent
{
    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteEventCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            var eventToDelete = await _context.Events.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Event), request.Id);

            _context.Events.Remove(eventToDelete);

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Discussion = "Problem saving changes" });

        }
    }
}
