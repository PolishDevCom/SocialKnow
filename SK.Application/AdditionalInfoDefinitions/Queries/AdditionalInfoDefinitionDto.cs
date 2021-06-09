using SK.Domain.Enums;
using System;

namespace SK.Application.AdditionalInfoDefinitions.Queries
{
    public class AdditionalInfoDefinitionDto
    {
        public Guid Id { get; set; }
        public string InfoName { get; set; }
        public TypeOfField TypeOfField { get; set; }
    }
}
