using MediatR;
using System;

namespace SK.Application.Discussions.Commands.EditDiscussion
{
    public class EditDiscussionCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public EditDiscussionCommand()
        {
        }

        public EditDiscussionCommand(DiscussionEditDto request)
        {
            Id = request.Id;
            CategoryId = request.CategoryId;
            Title = request.Title;
            Description = request.Description;
        }
    }
}