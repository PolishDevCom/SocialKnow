using MediatR;
using SK.Application.Common.Models;
using SK.Application.Common.Wrappers;
using System.Collections.Generic;

namespace SK.Application.AdditionalInfoDefinitions.Queries.ListAdditionalInfoDefinition
{
    public class ListAdditionalInfoDefinitionQuery : IRequest<PagedResponse<List<AdditionalInfoDefinitionDto>>>
    {
        public PaginationFilter Filter { get; set; }
        public string Path { get; set; }

        public ListAdditionalInfoDefinitionQuery(PaginationFilter filter, string path)
        {
            Filter = filter;
            Path = path;
        }
    }
}
