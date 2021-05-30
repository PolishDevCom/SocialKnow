using MediatR;
using SK.Application.Common.Models;
using SK.Application.Common.Wrappers;
using System.Collections.Generic;

namespace SK.Application.AdditionalInfos.Queries.ListAdditionalInfo
{
    public class ListAdditionalInfoQuery : IRequest<PagedResponse<List<AdditionalInfoDto>>>
    {
        public PaginationFilter Filter { get; set; }
        public string Path { get; set; }

        public ListAdditionalInfoQuery(PaginationFilter filter, string path)
        {
            Filter = filter;
            Path = path;
        }
    }
}
