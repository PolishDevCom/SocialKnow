using SK.Application.Common.Mapping;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SK.Application.Categories.Queries
{
    public class CategoryDto : IMapFrom<Category>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}
