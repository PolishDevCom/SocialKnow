using System;
using System.Collections.Generic;
using System.Text;

namespace SK.Application.Categories.Commands
{
    public class CategoryCreateOrEditDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}
