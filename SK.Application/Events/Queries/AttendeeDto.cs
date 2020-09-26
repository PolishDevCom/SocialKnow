using AutoMapper;
using SK.Application.Common.Mapping;
using SK.Domain.Entities;

namespace SK.Application.Events.Queries
{
    public class AttendeeDto : IMapFrom<UserEvent>
    {
        public string Username { get; set; }
        public string Image { get; set; }
        public bool IsHost { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserEvent, AttendeeDto>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser.UserName));
        }
    }
}
