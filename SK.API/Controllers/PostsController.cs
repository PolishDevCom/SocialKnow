using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SK.Application.Posts.Commands;
using SK.Application.Posts.Commands.CreatePost;
using SK.Application.Posts.Commands.DeletePost;
using SK.Application.Posts.Commands.EditPost;
using SK.Application.Posts.Commands.PinPost;
using SK.Application.Posts.Commands.UnpinPost;
using System;
using System.Threading.Tasks;

namespace SK.API.Controllers
{
    [Authorize]
    public class PostsController : ApiController
    {
        /// <summary>
        /// Adds new post.
        /// </summary>
        /// <param name="newPost">New post</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] PostCreateDto newPost)
        {
            return Ok(await Mediator.Send(new CreatePostCommand(newPost)));
        }

        /// <summary>
        /// Deletes a post with selected id.
        /// </summary>
        /// <param name="id" example="3fa85f64-5717-4562-b3fc-2c963f66afa6">Post ID</param>
        /// <returns></returns>
        [Authorize(Policy = "IsPostOwner")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeletePostCommand(id));
            return NoContent();
        }

        /// <summary>
        /// Updates an existing post by id.
        /// </summary>
        /// <param name="id" example="3fa85f64-5717-4562-b3fc-2c963f66afa6">Post ID</param>
        /// <param name="editedPost">Edited post</param>
        /// <returns></returns>
        [Authorize(Policy = "IsPostOwner")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(Guid id, [FromBody] PostEditDto editedPost)
        {
            var command = new EditPostCommand(editedPost)
            {
                Id = id
            };
            await Mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Pins an existing post selected by id.
        /// </summary>
        /// <param name="id" example="3fa85f64-5717-4562-b3fc-2c963f66afa6">Post ID</param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Moderator")]
        [HttpPut("{id}/pin")]
        public async Task<ActionResult> Pin(Guid id)
        {
            await Mediator.Send(new PinPostCommand(id));
            return NoContent();
        }

        /// <summary>
        /// Unpins an existing post selected by id.
        /// </summary>
        /// <param name="id" example="3fa85f64-5717-4562-b3fc-2c963f66afa6">Post ID</param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Moderator")]
        [HttpPut("{id}/unpin")]
        public async Task<ActionResult> Unpin(Guid id)
        {
            await Mediator.Send(new UnpinPostCommand(id));
            return NoContent();
        }
    }
}