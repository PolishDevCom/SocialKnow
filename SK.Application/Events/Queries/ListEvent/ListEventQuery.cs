using MediatR;
using SK.Application.Common.Models;
using SK.Application.Common.Wrappers;
using System.Collections.Generic;

namespace SK.Application.Events.Queries.ListEvent
{
    public class ListEventQuery : IRequest<PagedResponse<List<EventDto>>>
    {
        public PaginationFilter Filter { get; set; }
        public string Path { get; set; }

        public ListEventQuery(PaginationFilter filter, string path)
        {
            Filter = filter;
            Path = path;
        }
    }
}