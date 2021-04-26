using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SK.Application.Common.Helpers;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Models;
using SK.Application.Common.Wrappers;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.Tags.Queries.ListTag
{
    public class ListTagQueryHandler : IRequestHandler<ListTagQuery, PagedResponse<List<TagDto>>>
    {
        private readonly IPaginationService<Tag, TagDto> paginationService;

        public ListTagQueryHandler(IPaginationService<Tag, TagDto> paginationService)
        {
            this.paginationService = paginationService;
        }

        public async Task<PagedResponse<List<TagDto>>> Handle(ListTagQuery request, CancellationToken cancellationToken)
        {
            var route = request.Path;
            var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
            return await paginationService.GetPagedData(validFilter, route, cancellationToken);
        }
    }
}
