using MediatR;
using SK.Application.Common.Mapping;
using SK.Domain.Entities;
using System;

namespace SK.Application.Tags.Commands.EditTag
{
    public class EditTagCommand : IRequest, IMapTo<Tag>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public EditTagCommand()
        {
        }

        public EditTagCommand(TagCreateOrEditDto tagCreateOrEditDto)
        {
            Id = tagCreateOrEditDto.Id;
            Title = tagCreateOrEditDto.Title;
        }
    }
}