using MediatR;
using SK.Application.Common.Mapping;
using SK.Domain.Entities;
using System;

namespace SK.Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<Guid>, IMapTo<Category>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public CreateCategoryCommand()
        {
        }

        public CreateCategoryCommand(CategoryCreateOrEditDto categoryCreateOrEditDto)
        {
            Id = categoryCreateOrEditDto.Id;
            Title = categoryCreateOrEditDto.Title;
        }
    }
}