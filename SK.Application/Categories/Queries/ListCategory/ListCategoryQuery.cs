﻿using MediatR;
using SK.Application.Common.Models;
using SK.Application.Common.Wrappers;
using System.Collections.Generic;

namespace SK.Application.Categories.Queries.ListCategory
{
    public class ListCategoryQuery : IRequest<PagedResponse<List<CategoryDto>>>
    {
        public PaginationFilter Filter { get; set; }
        public string Path { get; set; }

        public ListCategoryQuery(PaginationFilter filter, string path)
        {
            Filter = filter;
            Path = path;
        }
    }
}