using SK.Domain.Entities;
using SK.Domain.Enums;
using System.Collections.Generic;

namespace SK.Application.Profiles.Queries
{
    public class ProfileDto
    {
        /// <summary>
        /// User username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// User nickname
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// User main photo
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// User gender
        /// </summary>
        public Gender UserGender { get; set; }

        /// <summary>
        /// Age
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// User short bio
        /// </summary>
        public string ShortBio { get; set; }

        /// <summary>
        /// User photos collection
        /// </summary>
        public ICollection<Photo> Photos { get; set; }
    }
}
