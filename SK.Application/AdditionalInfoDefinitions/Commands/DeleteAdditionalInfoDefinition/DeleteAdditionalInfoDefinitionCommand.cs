using MediatR;
using System;

namespace SK.Application.AdditionalInfoDefinitions.Commands.DeleteAdditionalInfoDefinition
{
    public class DeleteAdditionalInfoDefinitionCommand : IRequest
    {
        public Guid Id { get; set; }
        public DeleteAdditionalInfoDefinitionCommand() { }
        public DeleteAdditionalInfoDefinitionCommand(Guid id)
        {
            Id = id;
        }
    }
}

