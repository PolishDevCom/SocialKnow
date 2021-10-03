using MediatR;
using System;

namespace SK.Application.Events.Commands.SubscribeEvent
{
    public class SubscribeEventCommand : IRequest
    {
        public Guid Id { get; set; }

        public SubscribeEventCommand()
        {
        }

        public SubscribeEventCommand(Guid id)
        {
            Id = id;
        }
    }
}