using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Exceptions;
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
        private readonly IMapper _mapper;

        public CreateAdditionalInfoDefinitionCommandHandler(IApplicationDbContext context, IStringLocalizer<AdditionalInfoDefinitionsResource> localizer, IMapper mapper)
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateAdditionalInfoDefinitionCommand request, CancellationToken cancellationToken)
        {
            var additionalInfoDefinition = _mapper.Map<AdditionalInfoDefinition>(request);

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