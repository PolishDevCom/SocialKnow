using SK.Application.Common.Mapping;
using SK.Domain.Entities;
using System;

namespace SK.Application.Tags.Queries
{
    public class TagDto : IMapFrom<Tag>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}