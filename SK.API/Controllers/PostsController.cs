using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using SK.Application.Posts.Commands.CreatePost;

namespace SK.API.Controllers
{
    public class PostsController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreatePostCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
