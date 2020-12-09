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
        [HttpGet("{username}")]
        public async Task<ActionResult<ProfileDto>> Details(string username)
        {
            return await Mediator.Send(new DetailsProfileQuery(username));
        }
    }
}
