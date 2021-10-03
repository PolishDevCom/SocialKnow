using MediatR;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Discussions;
using SK.Domain.Entities;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Discussions.Commands.DeleteDiscussion
{
    public class DeleteDiscussionCommandHandler : IRequestHandler<DeleteDiscussionCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<DiscussionsResource> _localizer;

        public DeleteDiscussionCommandHandler(IApplicationDbContext context, IStringLocalizer<DiscussionsResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(DeleteDiscussionCommand request, CancellationToken cancellationToken)
        {
            var discussionToDelete = await _context.Discussions.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Discussion), request.Id);

            _context.Discussions.Remove(discussionToDelete);

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Discussion = _localizer["DiscussionSaveError"] });
        }
    }
}