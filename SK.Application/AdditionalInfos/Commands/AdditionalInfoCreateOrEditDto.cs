using SK.Domain.Enums;
using System;

namespace SK.Application.AdditionalInfos.Commands
{
    public class AdditionalInfoCreateOrEditDto
    {
        public Guid Id { get; set; }
        public string InfoName { get; set; }
        public TypeOfField TypeOfField { get; set; }
    }
}
