using MediatR;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Helpers;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.AdditionalInfos;
using SK.Domain.Entities;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.AdditionalInfos.Commands.CreateAdditionalInfo
{
    public class CreateAdditionalInfoCommandHandler : IRequestHandler<CreateAdditionalInfoCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<AdditionalInfosResource> _localizer;

        public CreateAdditionalInfoCommandHandler(IApplicationDbContext context, IStringLocalizer<AdditionalInfosResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Guid> Handle(CreateAdditionalInfoCommand request, CancellationToken cancellationToken)
        {
            var additionalInfo = new AdditionalInfo()
            {
                Id = request.Id,
                InfoName = request.InfoName,
                InfoType = ConvertHelper.ConvertTypeOfFieldEnumToStringType(request.TypeOfField)
            };

            _context.AdditionalInfos.Add(additionalInfo);
            var succes = await _context.SaveChangesAsync(cancellationToken) > 0;
            if (succes)
            {
                return additionalInfo.Id;
            }
            throw new RestException(HttpStatusCode.BadRequest, new { AdditionalInfo = _localizer["AdditionalInfoSaveError"]});
        }
    }
}
