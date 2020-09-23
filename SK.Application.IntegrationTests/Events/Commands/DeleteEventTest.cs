using FluentAssertions;
using MediatR;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Events.Commands.CreateEvent;
using SK.Application.Events.Commands.DeleteEvent;
using SK.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Events.Commands
{
    using static Testing;
    class DeleteEventTest : TestBase
    {
        [Test]
        public void ShouldRequireValidEventId()
        {
            //arrange
            var command = new DeleteEventCommand { Id = Guid.NewGuid() };

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldDeleteEvent()
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
            var createdEventId = await SendAsync(command);

            //act
            var result = await SendAsync(new DeleteEventCommand
            {
                Id = createdEventId
            });

            var actualEvent = await FindByGuidAsync<Event>(createdEventId);
            //assert
            actualEvent.Should().BeNull();
        }
    }
}
