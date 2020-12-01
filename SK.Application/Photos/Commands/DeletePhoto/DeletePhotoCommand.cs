using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SK.Application.Photos.Commands.DeletePhoto
{
    public class DeletePhotoCommand : IRequest
    {
        public DeletePhotoCommand() {}

        public DeletePhotoCommand(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}
