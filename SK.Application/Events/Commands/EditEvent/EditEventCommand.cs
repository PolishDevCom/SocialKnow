using MediatR;
using SK.Application.Common.Mapping;
using SK.Domain.Entities;
using System;

namespace SK.Application.Events.Commands.EditEvent
{
    public class EditEventCommand : IRequest, IMapTo<Event>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime? Date { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }
        public EditEventCommand() { }
        public EditEventCommand(EventCreateOrEditDto request)
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
