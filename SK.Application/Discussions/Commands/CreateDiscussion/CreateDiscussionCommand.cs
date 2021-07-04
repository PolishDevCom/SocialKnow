using MediatR;
using SK.Application.Common.Mapping;
using SK.Domain.Entities;
using System;

namespace SK.Application.Discussions.Commands.CreateDiscussion
{
    public class CreateDiscussionCommand : IRequest<Guid>, IMapTo<Discussion>
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PostBody { get; set; }

        public CreateDiscussionCommand() { }
        public CreateDiscussionCommand(DiscussionCreateDto request)
        {
            Id = request.Id;
            CategoryId = request.CategoryId;
            Title = request.Title;
            Description = request.Description;
            PostBody = request.PostBody;
        }
    }
}
