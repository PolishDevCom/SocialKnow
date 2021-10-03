using MediatR;
using SK.Application.Common.Mapping;
using SK.Domain.Entities;
using System;

namespace SK.Application.Categories.Commands.EditCategory
{
    public class EditCategoryCommand : IRequest, IMapTo<Category>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public EditCategoryCommand()
        {
        }

        public EditCategoryCommand(CategoryCreateOrEditDto categoryCreateOrEditDto)
        {
            Id = categoryCreateOrEditDto.Id;
            Title = categoryCreateOrEditDto.Title;
        }
    }
}