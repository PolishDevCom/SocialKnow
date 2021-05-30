using MediatR;
using SK.Domain.Enums;
using System;

namespace SK.Application.AdditionalInfos.Commands.CreateAdditionalInfo
{
    public class CreateAdditionalInfoCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string InfoName { get; set; }
        public TypeOfField TypeOfField { get; set; }

        public CreateAdditionalInfoCommand() { }
        public CreateAdditionalInfoCommand(AdditionalInfoCreateOrEditDto additionalInfoCreateOrEditDto)
        {
            Id = additionalInfoCreateOrEditDto.Id;
            InfoName = additionalInfoCreateOrEditDto.InfoName;
            TypeOfField = additionalInfoCreateOrEditDto.TypeOfField;
        }
    }
}
