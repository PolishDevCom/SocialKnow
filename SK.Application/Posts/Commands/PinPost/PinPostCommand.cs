using MediatR;
using System;

namespace SK.Application.Posts.Commands.PinPost
{
    public class PinPostCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
