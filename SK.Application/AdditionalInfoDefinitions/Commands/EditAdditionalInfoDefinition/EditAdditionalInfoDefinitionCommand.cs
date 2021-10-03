using MediatR;
using SK.Domain.Enums;
using System;

namespace SK.Application.AdditionalInfoDefinitions.Commands.EditAdditionalInfoDefinition
{
    public class EditAdditionalInfoDefinitionCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string InfoName { get; set; }
        public TypeOfField TypeOfField { get; set; }

        public EditAdditionalInfoDefinitionCommand()
        {
        }

        public EditAdditionalInfoDefinitionCommand(AdditionalInfoDefinitionCreateOrEditDto additionalInfoDefinitionCreateOrEditDto)
        {
            Id = additionalInfoDefinitionCreateOrEditDto.Id;
            InfoName = additionalInfoDefinitionCreateOrEditDto.InfoName;
            TypeOfField = additionalInfoDefinitionCreateOrEditDto.TypeOfField;
        }
    }
}