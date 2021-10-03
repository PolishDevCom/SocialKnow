using MediatR;
using SK.Application.Common.Mapping;
using SK.Domain.Entities;
using SK.Domain.Enums;

namespace SK.Application.Profiles.Commands.EditProfile
{
    public class EditProfileCommand : IRequest, IMapTo<AppUser>
    {
        public string Nickname { get; set; }
        public Gender UserGender { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public string ShortBio { get; set; }

        public EditProfileCommand()
        {
        }

        public EditProfileCommand(ProfileCreateOrEditDto profileCreateOrEditDto)
        {
            Nickname = profileCreateOrEditDto.Nickname;
            UserGender = profileCreateOrEditDto.UserGender;
            Age = profileCreateOrEditDto.Age;
            City = profileCreateOrEditDto.City;
            ShortBio = profileCreateOrEditDto.ShortBio;
        }
    }
}