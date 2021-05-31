using MediatR;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Helpers;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.AdditionalInfoDefinitions;
using SK.Domain.Entities;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.AdditionalInfoDefinitions.Commands.EditAdditionalInfoDefinition
{
    public class EditAdditionalInfoDefinitionCommandHandler : IRequestHandler<EditAdditionalInfoDefinitionCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<AdditionalInfoDefinitionsResource> _localizer;

        public EditAdditionalInfoDefinitionCommandHandler(IApplicationDbContext context, IStringLocalizer<AdditionalInfoDefinitionsResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Guid> Handle(EditAdditionalInfoDefinitionCommand request, CancellationToken cancellationToken)
        {
            var additionalInfoDefinition = await _context.AdditionalInfoDefinitions.FindAsync(request.Id) ?? throw new NotFoundException(nameof(AdditionalInfoDefinition), request.Id);

            additionalInfoDefinition.InfoName = request.InfoName;
            additionalInfoDefinition.InfoType = ConvertHelper.ConvertTypeOfFieldEnumToStringType(request.TypeOfField);

            _context.AdditionalInfoDefinitions.Add(additionalInfoDefinition);
            var succes = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (succes)
            {
                return additionalInfoDefinition.Id;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { AdditionalInfoDefinition = _localizer["AdditionalInfoDefinitionSaveError"]});
        }
    }
}
