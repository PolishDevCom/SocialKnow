using SK.Domain.Enums;
using System.Collections.Generic;

namespace SK.Domain.Entities
{
    public class AdditionalUserContent
    {
        public int Id { get; set; }
        //basic additional informations
        public string Nickname { get; set; }
        public Gender UserGender { get; set; }
        public string Bio { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public VoivoideshipPoland Voivoideship { get; set; }

        //climatic information
        public SexualPreference SexualPreference { get; set; }
        public List<Kink> Kinks { get; set; }
    }
}
