using SK.Domain.Entities;
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
        /// User main photo
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// User short bio
        /// </summary>
        public string Bio { get; set; }

        /// <summary>
        /// User photos collection
        /// </summary>
        public ICollection<Photo> Photos { get; set; }
    }
}
