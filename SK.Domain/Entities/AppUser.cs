using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SK.Domain.Entities
{
    public class AppUser : IdentityUser 
    {
        public string Bio { get; set; }
        public virtual ICollection<UserEvent> UserEvents { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
}
