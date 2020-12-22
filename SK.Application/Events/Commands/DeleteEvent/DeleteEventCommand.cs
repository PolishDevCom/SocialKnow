using MediatR;
using System;

namespace SK.Application.Events.Commands.DeleteEvent
{
    public class DeleteEventCommand : IRequest
    {
        public Guid Id { get; set; }

        public DeleteEventCommand(){}
        public DeleteEventCommand(Guid id)
        {
            Id = id;
        }
    }
}
