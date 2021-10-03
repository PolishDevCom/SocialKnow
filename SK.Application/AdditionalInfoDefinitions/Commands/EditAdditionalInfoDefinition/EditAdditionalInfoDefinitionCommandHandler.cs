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

namespace SK.Application.AdditionalInfoDefinitions.Commands.EditAdditionalInfoDefinition
{
    public class EditAdditionalInfoDefinitionCommandHandler : IRequestHandler<EditAdditionalInfoDefinitionCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<AdditionalInfoDefinitionsResource> _localizer;
        private readonly IMapper _mapper;

        public EditAdditionalInfoDefinitionCommandHandler(IApplicationDbContext context, IStringLocalizer<AdditionalInfoDefinitionsResource> localizer, IMapper mapper)
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(EditAdditionalInfoDefinitionCommand request, CancellationToken cancellationToken)
        {
            var additionalInfoDefinition = await _context.AdditionalInfoDefinitions.FindAsync(request.Id) ?? throw new NotFoundException(nameof(AdditionalInfoDefinition), request.Id);

            _mapper.Map(request, additionalInfoDefinition);
            var succes = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (succes)
            {
                return additionalInfoDefinition.Id;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { AdditionalInfoDefinition = _localizer["AdditionalInfoDefinitionSaveError"] });
        }
    }
}