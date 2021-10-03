using MediatR;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Models;
using SK.Application.Common.Wrappers;
using SK.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Categories.Queries.ListCategory
{
    public class ListCategoryQueryHandler : IRequestHandler<ListCategoryQuery, PagedResponse<List<CategoryDto>>>
    {
        private readonly IPaginationService<Category, CategoryDto> _paginationService;

        public ListCategoryQueryHandler(IPaginationService<Category, CategoryDto> paginationService)
        {
            _paginationService = paginationService;
        }

        public async Task<PagedResponse<List<CategoryDto>>> Handle(ListCategoryQuery request, CancellationToken cancellationToken)
        {
            var route = request.Path;
            var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
            return await _paginationService.GetPagedData(validFilter, route, cancellationToken);
        }
    }
}