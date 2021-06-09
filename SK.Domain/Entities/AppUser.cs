using Microsoft.AspNetCore.Identity;
using SK.Domain.Enums;
using System.Collections.Generic;

namespace SK.Domain.Entities
{
    public class AppUser : IdentityUser 
    {
        public string Nickname { get; set; }
        public Gender UserGender { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public string ShortBio { get; set; }
        public virtual ICollection<UserEvent> UserEvents { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
}
