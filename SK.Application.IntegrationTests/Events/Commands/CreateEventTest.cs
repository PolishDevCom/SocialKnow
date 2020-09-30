using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Events.Commands.CreateEvent;
using SK.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Events.Commands
{
    using static Testing;
    public class CreateEventTest : TestBase
    {
        [Test]
        public async Task ShouldCreateEvent()
        {
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");
            var command = new CreateEventCommand()
            {
                Id = Guid.NewGuid(),
                Title = "Test Event",
                Date = DateTime.Now,
                Description = "Event now",
                Category = "webinar",
                City = "Internet",
                Venue = "Discord"
            };

            //act
            var createdTestEventId = await SendAsync(command);
            var createdTestEvent = await FindByGuidAsync<Event>(createdTestEventId);

            //assert
            createdTestEvent.Should().NotBeNull();
            createdTestEvent.Id.Should().Be(command.Id);
            createdTestEvent.Title.Should().Be(command.Title);
            createdTestEvent.Date.Should().BeCloseTo(command.Date, 1000);
            createdTestEvent.Description.Should().Be(command.Description);
            createdTestEvent.Category.Should().Be(command.Category);
            createdTestEvent.City.Should().Be(command.City);
            createdTestEvent.Venue.Should().Be(command.Venue);
            createdTestEvent.Created.Should().BeCloseTo(DateTime.UtcNow, 1000);
        }

        [Test]
        [TestCase(null, "Test Event", "Event now", "webinar", "Internet", "Discord")]
        [TestCase("2020-8-18", null, "Event now", "webinar", "Internet", "Discord")]
        [TestCase("2020-8-18", "Test Event", null, "webinar", "Internet", "Discord")]
        [TestCase("2020-8-18", "Test Event", "Event now", null, "Internet", "Discord")]
        [TestCase("2020-8-18", "Test Event", "Event now", "webinar", null, "Discord")]
        [TestCase("2020-8-18", "Test Event", "Event now", "webinar", "Internet", null)]
        [TestCase("2020-8-18", "Test Event Title Which Is Very Very Long And Is Above Two Hundred Characters. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris vulputate euismod dapibus. Suspendisse potenti. Donec tristique mi id quam imperdiet semper. Sed ac ullamcorper lorem, vitae tincidunt magna. Vestibulum id tortor turpis. Cras at dolor non magna pharetra molestie eget sed augue. Sed vitae eros augue. Nullam vestibulum malesuada consectetur. Sed lacinia vitae velit et pharetra viverra.", "Event now", "webinar", "Internet", "Discord")]
        public void ShouldThrowValidationException(DateTime testDate, string testTitle, string testDescribtion, string testCategory, string testCity, string testVenue)
        {
            //arrange
            var command = new CreateEventCommand()
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
    }
}
