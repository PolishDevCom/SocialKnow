using MediatR;
using System;

namespace SK.Application.Posts.Commands.DeletePost
{
    public class DeletePostCommand : IRequest
    {
        public Guid Id { get; set; }

        public DeletePostCommand()
        {
        }

        public DeletePostCommand(Guid id)
        {
            Id = id;
        }
    }
}