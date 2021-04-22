using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SK.Application.Tags.Commands.DeleteTag
{
    public class DeleteTagCommand : IRequest
    {
        public Guid Id { get; set; }
        public DeleteTagCommand() { }
        public DeleteTagCommand(Guid id)
        {
            Id = id;
        }
    }
}
