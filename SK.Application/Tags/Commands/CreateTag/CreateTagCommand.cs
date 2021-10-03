using MediatR;
using SK.Application.Common.Mapping;
using SK.Domain.Entities;
using System;

namespace SK.Application.Tags.Commands.CreateTag
{
    public class CreateTagCommand : IRequest<Guid>, IMapTo<Tag>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public CreateTagCommand()
        {
        }

        public CreateTagCommand(TagCreateOrEditDto tagCreateOrEditDto)
        {
            Id = tagCreateOrEditDto.Id;
            Title = tagCreateOrEditDto.Title;
        }
    }
}