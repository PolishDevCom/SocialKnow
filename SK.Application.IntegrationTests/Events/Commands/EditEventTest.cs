using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SK.Application.Common.Exceptions;
using SK.Application.Events.Commands.CreateEvent;
using SK.Application.Events.Commands.EditEvent;
using SK.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Events.Commands
{
    using static Testing;

    public class EditEventTest
    {
        [Test]
        public async Task ShouldUpdateTestValue()
        {
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");
            var eventId = await SendAsync(new CreateEventCommand()
            {
                Id = Guid.NewGuid(),
                Title = "Test Event",
                Date = DateTime.Now,
                Description = "Event now",
                Category = "webinar",
                City = "Internet",
                Venue = "Discord"
            });

            //act
            var command = new EditEventCommand()
            {
                Id = eventId,
                Title = "Edited Test Event",
                Date = DateTime.Now.AddDays(1),
                Description = "Event later",
                Category = "webinar edited",
                City = "Internet edited",
                Venue = "Discord edited"
            };
            await SendAsync(command);
            var actualEvent = await FindByGuidAsync<Event>(eventId);

            //assert
            actualEvent.Should().NotBeNull();
            actualEvent.Id.Should().Be(command.Id);
            actualEvent.Title.Should().Be(command.Title);
            actualEvent.Date.Should().BeCloseTo(command.Date.GetValueOrDefault(), 1000);
            actualEvent.Description.Should().Be(command.Description);
            actualEvent.Category.Should().Be(command.Category);
            actualEvent.City.Should().Be(command.City);
            actualEvent.Venue.Should().Be(command.Venue);
            actualEvent.Created.Should().BeCloseTo(DateTime.UtcNow, 1000);
        }

        [Test]
        [TestCase("2020-8-18", null, "Event now", "webinar", "Internet", "Discord")]
        [TestCase("2020-8-18", "Test Event", null, "webinar", "Internet", "Discord")]
        [TestCase("2020-8-18", "Test Event", "Event now", null, "Internet", "Discord")]
        [TestCase("2020-8-18", "Test Event", "Event now", "webinar", null, "Discord")]
        [TestCase("2020-8-18", "Test Event", "Event now", "webinar", "Internet", null)]
        [TestCase("2020-8-18", "Test Event Title Which Is Very Very Long And Is Above Two Hundred Characters. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris vulputate euismod dapibus. Suspendisse potenti. Donec tristique mi id quam imperdiet semper. Sed ac ullamcorper lorem, vitae tincidunt magna. Vestibulum id tortor turpis. Cras at dolor non magna pharetra molestie eget sed augue. Sed vitae eros augue. Nullam vestibulum malesuada consectetur. Sed lacinia vitae velit et pharetra viverra.", "Event now", "webinar", "Internet", "Discord")]
        public void ShouldThrowValidationException(DateTime testDate, string testTitle, string testDescribtion, string testCategory, string testCity, string testVenue)
        {
            //arrange
            var command = new EditEventCommand()
            {
                Id = Guid.NewGuid(),
                Date = testDate,
                Title = testTitle,
                Description = testDescribtion,
                Category = testCategory,
                City = testCity,
                Venue = testVenue
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public void ShouldThrowNotFoundException()
        {
            //arrange
            var command = new EditEventCommand()
            {
                Id = Guid.NewGuid(),
                Title = "Edited Test Event",
                Date = DateTime.Now.AddDays(1),
                Description = "Event later",
                Category = "webinar edited",
                City = "Internet edited",
                Venue = "Discord edited"
            };

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }
    }
}
