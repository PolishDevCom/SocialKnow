using SK.Domain.Enums;
using System;

namespace SK.Application.AdditionalInfoDefinitions.Commands
{
    public class AdditionalInfoDefinitionCreateOrEditDto
    {
        public Guid Id { get; set; }
        public string InfoName { get; set; }
        public TypeOfField TypeOfField { get; set; }
    }
}