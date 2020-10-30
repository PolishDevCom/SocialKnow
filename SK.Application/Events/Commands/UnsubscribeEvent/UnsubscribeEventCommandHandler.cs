using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Events;
using SK.Domain.Entities;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Events.Commands.UnsubscribeEvent
{
    public class UnsubscribeEventCommandHandler : IRequestHandler<UnsubscribeEventCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<EventsResource> _localizer;

        public UnsubscribeEventCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IStringLocalizer<EventsResource> localizer)
        {
            _context = context;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(UnsubscribeEventCommand request, CancellationToken cancellationToken)
        {
            var eventToUnsubscribe = await _context.Events.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Event), request.Id);
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == _currentUserService.Username);

            var subscription = await _context.UserEvents.SingleOrDefaultAsync(x => x.EventId == eventToUnsubscribe.Id && x.AppUserId == user.Id);
            if (subscription == null)
            {
                return Unit.Value;
            }

            if (subscription.IsHost)
            {
                throw new RestException(HttpStatusCode.NotFound, new { Attendance = _localizer["EventUnsubscribeHostError"] });
            }

            _context.UserEvents.Remove(subscription);

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Event = _localizer["EventSaveError"] });

        }
    }
}
