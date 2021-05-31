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

namespace SK.Application.AdditionalInfoDefinitions.Commands.CreateAdditionalInfoDefinition
{
    public class CreateAdditionalInfoDefinitionCommandHandler : IRequestHandler<CreateAdditionalInfoDefinitionCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<AdditionalInfoDefinitionsResource> _localizer;

        public CreateAdditionalInfoDefinitionCommandHandler(IApplicationDbContext context, IStringLocalizer<AdditionalInfoDefinitionsResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Guid> Handle(CreateAdditionalInfoDefinitionCommand request, CancellationToken cancellationToken)
        {
            var additionalInfoDefinition = new AdditionalInfoDefinition()
            {
                Id = request.Id,
                InfoName = request.InfoName,
                InfoType = ConvertHelper.ConvertTypeOfFieldEnumToStringType(request.TypeOfField)
            };

            _context.AdditionalInfoDefinitions.Add(additionalInfoDefinition);
            var succes = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (succes)
            {
                return additionalInfoDefinition.Id;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { AdditionalInfoDefinition = _localizer["AdditionalInfoDefinitionSaveError"] });
        }
    }
}
