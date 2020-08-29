using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SK.Application.User;
using SK.Application.User.Commands.RegisterUser;
using SK.Application.User.Queries.GetCurrentUser;
using SK.Application.User.Queries.LoginUser;
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

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterUserCommand command)
        {
            return await Mediator.Send(command);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(LoginUserQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}
