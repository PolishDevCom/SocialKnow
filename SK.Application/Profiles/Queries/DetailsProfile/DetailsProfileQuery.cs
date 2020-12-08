
using MediatR;

namespace SK.Application.Profiles.Queries.DetailsProfile
{
    public class DetailsProfileQuery : IRequest<ProfileDto>
    {
        public string Username { get; set; }
    }
}
