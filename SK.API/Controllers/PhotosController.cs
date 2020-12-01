using Microsoft.AspNetCore.Mvc;
using SK.Application.Photos.Commands.AddPhoto;
using SK.Domain.Entities;
using System.Threading.Tasks;

namespace SK.API.Controllers
{
    public class PhotosController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<Photo>> Add([FromForm] AddPhotoCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
