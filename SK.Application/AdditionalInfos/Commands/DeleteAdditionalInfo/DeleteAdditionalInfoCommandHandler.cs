using MediatR;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.AdditionalInfos;
using SK.Domain.Entities;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.AdditionalInfos.Commands.DeleteAdditionalInfo
{
    public class DeleteAdditionalInfoCommandHandler : IRequestHandler<DeleteAdditionalInfoCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<AdditionalInfosResource> _localizer;

        public DeleteAdditionalInfoCommandHandler(IApplicationDbContext context, IStringLocalizer<AdditionalInfosResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }
        public async Task<Unit> Handle(DeleteAdditionalInfoCommand request, CancellationToken cancellationToken)
        {
            var additionalInfoToDelete = await _context.AdditionalInfos.FindAsync(request.Id) ?? throw new NotFoundException(nameof(AdditionalInfo), request.Id);
            _context.AdditionalInfos.Remove(additionalInfoToDelete);
            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { Tag = _localizer["AditionalInfoSaveError"] });

        }
    }
}
