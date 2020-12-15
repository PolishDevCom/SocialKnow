using AutoMapper;
using SK.Application.Common.Mapping;
using SK.Domain.Entities;
using System.Linq;

namespace SK.Application.Events.Queries
{
    public class AttendeeDto : IMapFrom<UserEvent>
    {
        /// <summary>
        /// Attendee username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Attendee main photo
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Is attendee host
        /// </summary>
        public bool IsHost { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserEvent, AttendeeDto>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser.UserName))
                .ForMember(d => d.Image, o => o.MapFrom(s => s.AppUser.Photos.FirstOrDefault(p => p.IsMain).Url));
        }
    }
}
