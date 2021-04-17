using SK.Application.Common.Mapping;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SK.Application.Tags.Queries
{
    public class TagDto : IMapFrom<Tag>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
    }
}
