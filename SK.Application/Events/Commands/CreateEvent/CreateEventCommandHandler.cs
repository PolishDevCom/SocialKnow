using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Events;
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
        private readonly IStringLocalizer<EventsResource> _localizer;
        private readonly IMapper _mapper;

        public CreateEventCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, IDateTime dateTime, IStringLocalizer<EventsResource> localizer,
            IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _dateTime = dateTime;
            _localizer = localizer;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var newEvent = _mapper.Map<Event>(request);
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
            throw new RestException(HttpStatusCode.BadRequest, new { Event = _localizer["EventSaveError"] });
        }
    }
}
