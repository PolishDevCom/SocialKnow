using MediatR;
using Microsoft.AspNetCore.Mvc;
using SK.Application.Photos.Commands.AddPhoto;
using SK.Application.Photos.Commands.DeletePhoto;
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

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(string id)
        {
            return await Mediator.Send(new DeletePhotoCommand(id));
        }
    }
}
