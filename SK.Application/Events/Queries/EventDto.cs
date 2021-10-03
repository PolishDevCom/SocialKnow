using SK.Application.Common.Mapping;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SK.Application.Events.Queries
{
    public class EventDto : IMapFrom<Event>
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Event title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Event description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Category of event
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Event date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Event city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Event venue
        /// </summary>
        public string Venue { get; set; }

        /// <summary>
        /// Event attendees collection
        /// </summary>
        [JsonPropertyName("attendees")]
        public ICollection<AttendeeDto> UserEvents { get; set; }
    }
}