using MediatR;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Discussions;
using SK.Domain.Entities;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Discussions.Commands.EditDiscussion
{
    public class EditDiscussionCommandHandler : IRequestHandler<EditDiscussionCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<DiscussionsResource> _localizer;

        public EditDiscussionCommandHandler(IApplicationDbContext context, IStringLocalizer<DiscussionsResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(EditDiscussionCommand request, CancellationToken cancellationToken)
        {
            var discussionToFind = await _context.Discussions.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Discussion), request.Id);

            discussionToFind.Title = request.Title ?? discussionToFind.Title;
            discussionToFind.Description = request.Description ?? discussionToFind.Description;

            var category = await _context.Categories.FindAsync(request.CategoryId);
            if (category != null)
            {
                discussionToFind.Category = category;
            }

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Discussion = _localizer["DiscussionSaveError"] });
        }
    }
}