using MediatR;
using System;

namespace SK.Application.Events.Commands.UnsubscribeEvent
{
    public class UnsubscribeEventCommand : IRequest
    {
        public Guid Id { get; set; }

        public UnsubscribeEventCommand()
        {
        }

        public UnsubscribeEventCommand(Guid id)
        {
            Id = id;
        }
    }
}