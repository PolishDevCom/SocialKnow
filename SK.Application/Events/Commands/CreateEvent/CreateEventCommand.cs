using MediatR;
using SK.Application.Events.Queries;
using System;

namespace SK.Application.Events.Commands.CreateEvent
{
    public class CreateEventCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }
        public CreateEventCommand() {}
        public CreateEventCommand(EventDto request)
        {
            Id = request.Id;
            Title = request.Title;
            Description = request.Description;
            Category = request.Category;
            Date = request.Date;
            City = request.City;
            Venue = request.Venue;
        }
    }
}
