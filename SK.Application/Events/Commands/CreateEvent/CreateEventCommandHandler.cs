using MediatR;
using Microsoft.EntityFrameworkCore;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Events.Commands.CreateEvent
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;
        private readonly IDateTime _dateTime;

        public CreateEventCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, IDateTime dateTime)
        {
            _context = context;
            _currentUser = currentUser;
            _dateTime = dateTime;
        }

        public async Task<Guid> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var newEvent = new Event
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                Category = request.Category,
                Date = request.Date,
                City = request.City,
                Venue = request.Venue
            };

            _context.Events.Add(newEvent);

            var hostUser = await _context.Users.SingleOrDefaultAsync(u => u.UserName == _currentUser.Username);

            var attendee = new UserEvent
            {
                AppUser = hostUser,
                Event = newEvent,
                IsHost = true,
                DateJoined = _dateTime.Now
            };

            _context.UserEvents.Add(attendee);

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return newEvent.Id;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Event = "Problem saving changes" });
        }
    }
}
