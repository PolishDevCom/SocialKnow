using Microsoft.AspNetCore.Mvc;
using SK.Application.User;
using SK.Application.User.Queries;
using System.Threading.Tasks;

namespace SK.API.Controllers
{
    public class UserController : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<User>> CurrentUser()
        {
            return await Mediator.Send(new GetCurrentUserQuery());
        }
    }
}
