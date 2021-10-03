using MediatR;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Discussions;
using SK.Domain.Entities;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Discussions.Commands.PinDiscussion
{
    public class PinDiscussionCommandHandler : IRequestHandler<PinDiscussionCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<DiscussionsResource> _localizer;

        public PinDiscussionCommandHandler(IApplicationDbContext context, IStringLocalizer<DiscussionsResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(PinDiscussionCommand request, CancellationToken cancellationToken)
        {
            var discussionToPin = await _context.Discussions.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Discussion), request.Id);

            if (discussionToPin.IsPinned)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Discussion = _localizer["DiscussionPinError"] });
            }

            discussionToPin.IsPinned = true;

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Discussion = _localizer["DiscussionSaveError"] });
        }
    }
}