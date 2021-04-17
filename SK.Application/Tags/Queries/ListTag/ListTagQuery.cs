using MediatR;
using SK.Application.Common.Models;
using SK.Application.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SK.Application.Tags.Queries.ListTag
{
    public class ListTagQuery : IRequest<PagedResponse<List<TagDto>>>
    {
        public PaginationFilter Filter { get; set; }
        public string Path { get; set; }

        public ListTagQuery(PaginationFilter filter, string path)
        {
            Filter = filter;
            Path = path;
        }
    }
}
