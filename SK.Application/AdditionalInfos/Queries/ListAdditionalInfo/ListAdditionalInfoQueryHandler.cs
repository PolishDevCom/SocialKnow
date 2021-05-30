using MediatR;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Models;
using SK.Application.Common.Wrappers;
using SK.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.AdditionalInfos.Queries.ListAdditionalInfo
{
    public class ListAdditionalInfoQueryHandler : IRequestHandler<ListAdditionalInfoQuery, PagedResponse<List<AdditionalInfoDto>>>
    {
        private readonly IPaginationService<AdditionalInfo, AdditionalInfoDto> _paginationService;

        public ListAdditionalInfoQueryHandler(IPaginationService<AdditionalInfo, AdditionalInfoDto> paginationService)
        {
            _paginationService = paginationService;
        }

        public async Task<PagedResponse<List<AdditionalInfoDto>>> Handle(ListAdditionalInfoQuery request, CancellationToken cancellationToken)
        {
            var route = request.Path;
            var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
            return await _paginationService.GetPagedData(validFilter, route, cancellationToken);
        }
    }
}
