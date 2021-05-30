using SK.Domain.Enums;
using System;

namespace SK.Application.AdditionalInfos.Queries
{
    public class AdditionalInfoDto
    {
        public Guid Id { get; set; }
        public string InfoName { get; set; }
        public TypeOfField TypeOfField { get; set; }
    }
}
