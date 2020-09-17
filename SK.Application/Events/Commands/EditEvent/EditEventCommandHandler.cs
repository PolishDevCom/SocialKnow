using MediatR;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Events.Commands.EditEvent
{
    public class EditEventCommandHandler : IRequestHandler<EditEventCommand>
    {
        private readonly IApplicationDbContext _context;

        public EditEventCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(EditEventCommand request, CancellationToken cancellationToken)
        {
            var eventToChange = await _context.Events.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Event), request.Id);

            eventToChange.Title = request.Title ?? eventToChange.Title;
            eventToChange.Description = request.Description ?? eventToChange.Description;
            eventToChange.Category = request.Category ?? eventToChange.Category;
            eventToChange.Date = request.Date ?? eventToChange.Date;
            eventToChange.City = request.City ?? eventToChange.City;
            eventToChange.Venue = request.Venue ?? eventToChange.Venue;

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Event = "Problem saving changes" });

        }
    }
}
