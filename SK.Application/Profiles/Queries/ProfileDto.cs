using SK.Domain.Entities;
using System.Collections.Generic;

namespace SK.Application.Profiles.Queries
{
    public class ProfileDto
    {
        public string Username { get; set; }
        public string Image { get; set; }
        public string Bio { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}
