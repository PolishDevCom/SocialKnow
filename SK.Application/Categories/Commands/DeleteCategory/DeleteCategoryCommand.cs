using MediatR;
using System;

namespace SK.Application.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommand : IRequest
    {
        public Guid Id { get; set; }

        public DeleteCategoryCommand()
        {
        }

        public DeleteCategoryCommand(Guid id)
        {
            Id = id;
        }
    }
}