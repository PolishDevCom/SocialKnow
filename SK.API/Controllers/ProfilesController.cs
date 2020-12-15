using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SK.Application.Profiles.Queries;
using SK.Application.Profiles.Queries.DetailsProfile;
using System.Threading.Tasks;

namespace SK.API.Controllers
{
    [Authorize]
    public class ProfilesController : ApiController
    {
        /// <summary>
        /// Fetches a single user profile by username.
        /// </summary>
        /// <param name="username" example="thor123">Username</param>
        /// <returns></returns>
        [HttpGet("{username}")]
        public async Task<ActionResult<ProfileDto>> Details(string username)
        {
            return Ok(await Mediator.Send(new DetailsProfileQuery(username)));
        }
    }
}
