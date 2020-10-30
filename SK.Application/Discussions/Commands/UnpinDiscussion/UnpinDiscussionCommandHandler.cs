using MediatR;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using SK.Application.Common.Resources.Discussions;
using Microsoft.Extensions.Localization;

namespace SK.Application.Discussions.Commands.UnpinDiscussion
{
    public class UnpinDiscussionCommandHandler : IRequestHandler<UnpinDiscussionCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<DiscussionsResource> _localizer;

        public UnpinDiscussionCommandHandler(IApplicationDbContext context, IStringLocalizer<DiscussionsResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(UnpinDiscussionCommand request, CancellationToken cancellationToken)
        {
            var discussionToUnpin = await _context.Discussions.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Discussion), request.Id);

            if (!discussionToUnpin.IsPinned)
            {
                throw new RestException(HttpStatusCode.BadRequest, new { Discussion = _localizer["DiscussionUnpinError"] });
            }

            discussionToUnpin.IsPinned = false;

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Discussion = _localizer["DiscussionSaveError"] });
        }
    }
}
