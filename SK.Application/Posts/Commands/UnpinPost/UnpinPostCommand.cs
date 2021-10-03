using MediatR;
using System;

namespace SK.Application.Posts.Commands.UnpinPost
{
    public class UnpinPostCommand : IRequest
    {
        public Guid Id { get; set; }

        public UnpinPostCommand()
        {
        }

        public UnpinPostCommand(Guid id)
        {
            Id = id;
        }
    }
}