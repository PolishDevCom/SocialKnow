using MediatR;
using Microsoft.AspNetCore.Mvc;
using SK.Application.Photos.Commands.AddPhoto;
using SK.Application.Photos.Commands.DeletePhoto;
using SK.Application.Photos.Commands.SetMainPhoto;
using SK.Domain.Entities;
using System.Threading.Tasks;

namespace SK.API.Controllers
{
    public class PhotosController : ApiController
    {
        /// <summary>
        /// Adds new photo.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Photo>> Add([FromForm] AddPhotoCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Deletes a photo with selected id.
        /// </summary>
        /// <param name="id">Photo ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await Mediator.Send(new DeletePhotoCommand(id));
            return NoContent();
        }

        /// <summary>
        /// Sets photo with selected id as a main photo.
        /// </summary>
        /// <param name="id">PhotoID</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> SetMain(string id)
        {
            await Mediator.Send(new SetMainPhotoCommand(id));
            return NoContent();
        }
    }
}
