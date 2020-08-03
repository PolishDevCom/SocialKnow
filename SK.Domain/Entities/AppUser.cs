using Microsoft.AspNetCore.Identity;
using SK.Domain.Enums;
using System.Collections.Generic;

namespace SK.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        //Common part
        public string Nickname { get; set; }
        public Gender UserGender { get; set; }
        public int Age { get; set; }
        public VoivoideshipPoland Voivoideship { get; set; }

        //Topic dependent part
        public SexualPreference SexualPreference { get; set; }
        public List<Kink> Kinks { get; set; }
    }
}
