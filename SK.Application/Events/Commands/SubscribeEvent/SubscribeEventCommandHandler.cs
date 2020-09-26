using MediatR;
using Microsoft.EntityFrameworkCore;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Events.Commands.SubscribeEvent
{
    public class SubscribeEventCommandHandler : IRequestHandler<SubscribeEventCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public SubscribeEventCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IDateTime dateTime)
        {
            _context = context;
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }
        public async Task<Unit> Handle(SubscribeEventCommand request, CancellationToken cancellationToken)
        {
            var eventToSubscribe = await _context.Events.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Event), request.Id);
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == _currentUserService.Username);

            var subscription = await _context.UserEvents.SingleOrDefaultAsync(x => x.EventId == eventToSubscribe.Id && x.AppUserId == user.Id);
            if (subscription != null)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Attendance = "Already subscribing this event" });
            }

            subscription = new UserEvent
            {
                Event = eventToSubscribe,
                AppUser = user,
                IsHost = false,
                DateJoined = _dateTime.Now
            };

            _context.UserEvents.Add(subscription);

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Event = "Problem saving changes" });
        }
    }
}
