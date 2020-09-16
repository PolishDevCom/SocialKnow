using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SK.Domain.Entities
{
    public class AppUser : IdentityUser 
    {
        public virtual ICollection<UserEvent> UserEvents { get; set; }
    }
}
