using MediatR;
using Microsoft.EntityFrameworkCore;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
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

        public UnsubscribeEventCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
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
                throw new RestException(HttpStatusCode.NotFound, new { Attendance = "You cannot remove yourself as host" });
            }

            _context.UserEvents.Remove(subscription);

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Event = "Problem saving changes" });

        }
    }
}
