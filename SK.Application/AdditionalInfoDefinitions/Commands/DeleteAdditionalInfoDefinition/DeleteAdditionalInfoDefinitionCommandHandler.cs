using MediatR;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.AdditionalInfoDefinitions;
using SK.Domain.Entities;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.AdditionalInfoDefinitions.Commands.DeleteAdditionalInfoDefinition
{
    public class DeleteAdditionalInfoDefinitionCommandHandler : IRequestHandler<DeleteAdditionalInfoDefinitionCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<AdditionalInfoDefinitionsResource> _localizer;

        public DeleteAdditionalInfoDefinitionCommandHandler(IApplicationDbContext context, IStringLocalizer<AdditionalInfoDefinitionsResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(DeleteAdditionalInfoDefinitionCommand request, CancellationToken cancellationToken)
        {
            var additionalInfoDefinitionToDelete = await _context.AdditionalInfoDefinitions.FindAsync(request.Id) ?? throw new NotFoundException(nameof(AdditionalInfoDefinition), request.Id);
            _context.AdditionalInfoDefinitions.Remove(additionalInfoDefinitionToDelete);
            var success = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (success)
            {
                return Unit.Value;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { AdditionalInfoDefinition = _localizer["AditionalInfoDefinitionSaveError"] });
        }
    }
}