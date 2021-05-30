using MediatR;
using SK.Domain.Enums;
using System;

namespace SK.Application.AdditionalInfos.Commands.EditAdditionalInfo
{
    public class EditAdditionalInfoCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string InfoName { get; set; }
        public TypeOfField TypeOfField { get; set; }

        public EditAdditionalInfoCommand() { }
        public EditAdditionalInfoCommand(AdditionalInfoCreateOrEditDto additionalInfoCreateOrEditDto)
        {
            Id = additionalInfoCreateOrEditDto.Id;
            InfoName = additionalInfoCreateOrEditDto.InfoName;
            TypeOfField = additionalInfoCreateOrEditDto.TypeOfField;
        }
    }
}
