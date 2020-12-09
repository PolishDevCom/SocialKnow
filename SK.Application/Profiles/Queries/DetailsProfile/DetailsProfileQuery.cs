
using MediatR;

namespace SK.Application.Profiles.Queries.DetailsProfile
{
    public class DetailsProfileQuery : IRequest<ProfileDto>
    {
        public DetailsProfileQuery(string username)
        {
            Username = username;
        }
        public string Username { get; set; }
    }
}
