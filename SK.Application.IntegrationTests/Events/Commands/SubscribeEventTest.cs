using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Events.Commands.CreateEvent;
using SK.Application.Events.Commands.SubscribeEvent;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Events.Commands
{
    using static Testing;

    public class SubscribeEventTest : TestBase
    {
        [Test]
        public async Task ShouldSubscribeToEvent()
        {
            //arrange
            var eventId = Guid.NewGuid();
            var creatorUsername = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");
            var createCommand = new CreateEventCommand()
            {
                Id = eventId,
                Title = "Test Event",
                Date = DateTime.Now,
                Description = "Event now",
                Category = "webinar",
                City = "Internet",
                Venue = "Discord"
            };
            var createdEventId = await SendAsync(createCommand);
            var subscriberUsername = await RunAsDefaultUserAsync();

            //act
            var subscribe = await SendAsync(new SubscribeEventCommand() { Id = createdEventId });
            var testedEvents = FindUserEventsByEventGuidAsync(createdEventId);

            //assert
            testedEvents.Should().HaveCount(2);
            testedEvents.FirstOrDefault(ue => ue.IsHost).AppUser.UserName.Should().Be(creatorUsername);
            testedEvents.FirstOrDefault(ue => ue.AppUser.UserName == subscriberUsername).IsHost.Should().Be(false);
        }

        [Test]
        public async Task ShouldBeNotFoundExceptionWhenWrongEventId()
        {
            //arrange
            var eventId = Guid.NewGuid();
            var creatorUsername = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");
            var createCommand = new CreateEventCommand()
            {
                Id = eventId,
                Title = "Test Event",
                Date = DateTime.Now,
                Description = "Event now",
                Category = "webinar",
                City = "Internet",
                Venue = "Discord"
            };
            var createdEventId = await SendAsync(createCommand);
            var subscriberUsername = await RunAsDefaultUserAsync();

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(new SubscribeEventCommand() { Id = Guid.NewGuid() })).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldBeRestExceptionWhenAlreadySubscribingEvent()
        {
            //arrange
            var eventId = Guid.NewGuid();
            var creatorUsername = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");
            var createCommand = new CreateEventCommand()
            {
                Id = eventId,
                Title = "Test Event",
                Date = DateTime.Now,
                Description = "Event now",
                Category = "webinar",
                City = "Internet",
                Venue = "Discord"
            };
            var createdEventId = await SendAsync(createCommand);
            var subscriberUsername = await RunAsDefaultUserAsync();

            //act
            var subscribe = await SendAsync(new SubscribeEventCommand() { Id = createdEventId });

            //assert
            FluentActions.Invoking(() =>
                SendAsync(new SubscribeEventCommand() { Id = createdEventId })).Should().Throw<RestException>();
        }
        
    }
}
