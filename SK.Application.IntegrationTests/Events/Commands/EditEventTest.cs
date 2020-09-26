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
        public void ShouldRequireValidEvent()
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

        [Test]
        public void ShouldRequireFieldTitle()
        {
            //arrange
            var command = new EditEventCommand()
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                Description = "Event now",
                Category = "webinar",
                City = "Internet",
                Venue = "Discord"
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public void ShouldRequireFieldDate()
        {
            //arrange
            var command = new EditEventCommand()
            {
                Id = Guid.NewGuid(),
                Title = "Test Event",
                Description = "Event now",
                Category = "webinar",
                City = "Internet",
                Venue = "Discord"
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public void ShouldRequireFieldDescription()
        {
            //arrange
            var command = new EditEventCommand()
            {
                Id = Guid.NewGuid(),
                Title = "Test Event",
                Date = DateTime.Now,
                Category = "webinar",
                City = "Internet",
                Venue = "Discord"
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public void ShouldRequireFieldCategory()
        {
            //arrange
            var command = new EditEventCommand()
            {
                Id = Guid.NewGuid(),
                Title = "Test Event",
                Date = DateTime.Now,
                Description = "Event now",
                City = "Internet",
                Venue = "Discord"
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public void ShouldRequireFieldCity()
        {
            //arrange
            var command = new EditEventCommand()
            {
                Id = Guid.NewGuid(),
                Title = "Test Event",
                Date = DateTime.Now,
                Description = "Event now",
                Category = "webinar",
                Venue = "Discord"
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public void ShouldRequireFieldVenue()
        {
            //arrange
            var command = new EditEventCommand()
            {
                Id = Guid.NewGuid(),
                Title = "Test Event",
                Date = DateTime.Now,
                Description = "Event now",
                Category = "webinar",
                City = "Internet",
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public void ShouldThrowExceptionIfTitleLongerThan200()
        {
            //arrange
            var command = new EditEventCommand()
            {
                Id = Guid.NewGuid(),
                Title = "Test Event Title Which Is Very Very Long And Is Above Two Hundred Characters. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris vulputate euismod dapibus. Suspendisse potenti. Donec tristique mi id quam imperdiet semper. Sed ac ullamcorper lorem, vitae tincidunt magna. Vestibulum id tortor turpis. Cras at dolor non magna pharetra molestie eget sed augue. Sed vitae eros augue. Nullam vestibulum malesuada consectetur. Sed lacinia vitae velit et pharetra viverra.",
                Date = DateTime.Now,
                Description = "Event now",
                Category = "webinar",
                City = "Internet",
                Venue = "Discord"
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }
    }
}
