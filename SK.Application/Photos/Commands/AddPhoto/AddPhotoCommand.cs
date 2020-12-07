using MediatR;
using Microsoft.AspNetCore.Http;
using SK.Domain.Entities;

namespace SK.Application.Photos.Commands.AddPhoto
{
    public class AddPhotoCommand : IRequest<Photo>
    {
        public IFormFile File { get; set; }
    }
}
