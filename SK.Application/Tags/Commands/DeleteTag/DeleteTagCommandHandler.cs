using MediatR;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Tags;
using SK.Domain.Entities;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Tags.Commands.DeleteTag
{
    public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<TagsResource> _localizer;

        public DeleteTagCommandHandler(IApplicationDbContext context, IStringLocalizer<TagsResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            var tagToDelete = await _context.Tags.FindAsync(request.Id) ?? throw new NotFoundException(nameof(Tag), request.Id);
            _context.Tags.Remove(tagToDelete);
            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Tag = _localizer["TagSaveError"] });
        }
    }
}