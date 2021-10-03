using SK.Domain.Enums;

namespace SK.Application.Profiles.Commands
{
    public class ProfileCreateOrEditDto
    {
        public string Nickname { get; set; }
        public Gender UserGender { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public string ShortBio { get; set; }
    }
}