using MediatR;
using SK.Domain.Enums;
using System;

namespace SK.Application.AdditionalInfoDefinitions.Commands.CreateAdditionalInfoDefinition
{
    public class CreateAdditionalInfoDefinitionCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string InfoName { get; set; }
        public TypeOfField TypeOfField { get; set; }

        public CreateAdditionalInfoDefinitionCommand() { }
        public CreateAdditionalInfoDefinitionCommand(AdditionalInfoDefinitionCreateOrEditDto additionalInfoDefinitionCreateOrEditDto)
        {
            Id = additionalInfoDefinitionCreateOrEditDto.Id;
            InfoName = additionalInfoDefinitionCreateOrEditDto.InfoName;
            TypeOfField = additionalInfoDefinitionCreateOrEditDto.TypeOfField;
        }
    }
}
