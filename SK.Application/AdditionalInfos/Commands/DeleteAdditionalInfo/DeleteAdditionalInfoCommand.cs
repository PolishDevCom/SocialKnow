using MediatR;
using System;

namespace SK.Application.AdditionalInfos.Commands.DeleteAdditionalInfo
{
    public class DeleteAdditionalInfoCommand : IRequest
    {
        public Guid Id { get; set; }
        public DeleteAdditionalInfoCommand() { }
        public DeleteAdditionalInfoCommand(Guid id)
        {
            Id = id;
        }
    }
}

