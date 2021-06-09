using MediatR;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Models;
using SK.Application.Common.Wrappers;
using SK.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.AdditionalInfoDefinitions.Queries.ListAdditionalInfoDefinition
{
    public class ListAdditionalInfoQueryHandler : IRequestHandler<ListAdditionalInfoDefinitionQuery, PagedResponse<List<AdditionalInfoDefinitionDto>>>
    {
        private readonly IPaginationService<AdditionalInfoDefinition, AdditionalInfoDefinitionDto> _paginationService;

        public ListAdditionalInfoQueryHandler(IPaginationService<AdditionalInfoDefinition, AdditionalInfoDefinitionDto> paginationService)
        {
            _paginationService = paginationService;
        }

        public async Task<PagedResponse<List<AdditionalInfoDefinitionDto>>> Handle(ListAdditionalInfoDefinitionQuery request, CancellationToken cancellationToken)
        {
            var route = request.Path;
            var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
            return await _paginationService.GetPagedData(validFilter, route, cancellationToken);
        }
    }
}
