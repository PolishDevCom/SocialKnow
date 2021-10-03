using Microsoft.AspNetCore.Http;
using SK.Application.Photos.Commands.AddPhoto;

namespace SK.Application.Common.Interfaces
{
    public interface IPhotoService
    {
        PhotoUploadResult AddPhoto(IFormFile file);

        string DeletePhoto(string publicId);
    }
}