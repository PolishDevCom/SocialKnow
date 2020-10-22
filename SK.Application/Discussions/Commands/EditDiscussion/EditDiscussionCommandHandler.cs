using MediatR;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Discussions.Commands.EditDiscussion
{
    public class EditDiscussionCommandHandler : IRequestHandler<EditDiscussionCommand>
    {
        private readonly IApplicationDbContext _context;

        public EditDiscussionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(EditDiscussionCommand request, CancellationToken cancellationToken)
        {
            var discussionToFind = await _context.Discussions.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Discussion), request.Id);

            discussionToFind.Title = request.Title ?? discussionToFind.Title;
            discussionToFind.Description = request.Description ?? discussionToFind.Description;

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Discussion = "Problem saving changes" });
        }
    }
}
