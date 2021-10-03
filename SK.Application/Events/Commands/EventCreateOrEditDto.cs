using System;

namespace SK.Application.Events.Commands
{
    public class EventCreateOrEditDto
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
    }
}