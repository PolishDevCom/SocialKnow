using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Events.Commands.CreateEvent;
using SK.Application.Events.Commands.SubscribeEvent;
using SK.Application.Events.Commands.UnsubscribeEvent;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Events.Commands
{
    using static Testing;

    public class UnsubscribeEventTest : TestBase
    {
        [Test]
        public async Task ShouldUnsubscribeEvent()
        {
            //arrange
            var eventId = Guid.NewGuid();

            var creatorUsername = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var createCommand = new Faker<CreateEventCommand>("en")
                .RuleFor(e => e.Id, f => f.Random.Guid())
                .RuleFor(e => e.Title, f => f.Lorem.Sentence())
                .RuleFor(e => e.Date, f => f.Date.Future())
                .RuleFor(e => e.Description, f => f.Lorem.Sentence(5))
                .RuleFor(e => e.Category, f => f.Lorem.Word())
                .RuleFor(e => e.City, f => f.Lorem.Word())
                .RuleFor(e => e.Venue, f => f.Lorem.Sentence(1)).Generate();

            var createdEventId = await SendAsync(createCommand);

            var subscriberUsername = await RunAsDefaultUserAsync();

            var subscribe = await SendAsync(new SubscribeEventCommand() { Id = createdEventId });

            //act
            var unsubscribe = await SendAsync(new UnsubscribeEventCommand() { Id = createdEventId });

            var testedEvents = FindUserEventsByEventGuidAsync(createdEventId);

            //assert
            testedEvents.Should().HaveCount(1);
            testedEvents.FirstOrDefault(ue => ue.IsHost).AppUser.UserName.Should().Be(creatorUsername);
            testedEvents.FirstOrDefault(ue => ue.AppUser.UserName == subscriberUsername).Should().BeNull();
        }

        [Test]
        public async Task ShouldBeNotFoundExceptionWhenWrongEventId()
        {
            //arrange
            var eventId = Guid.NewGuid();

            var creatorUsername = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var createCommand = new Faker<CreateEventCommand>("en")
                .RuleFor(e => e.Id, f => f.Random.Guid())
                .RuleFor(e => e.Title, f => f.Lorem.Sentence())
                .RuleFor(e => e.Date, f => f.Date.Future())
                .RuleFor(e => e.Description, f => f.Lorem.Sentence(5))
                .RuleFor(e => e.Category, f => f.Lorem.Word())
                .RuleFor(e => e.City, f => f.Lorem.Word())
                .RuleFor(e => e.Venue, f => f.Lorem.Sentence(1)).Generate();
            var createdEventId = await SendAsync(createCommand);

            var subscriberUsername = await RunAsDefaultUserAsync();

            var subscribe = await SendAsync(new SubscribeEventCommand() { Id = createdEventId });

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(new UnsubscribeEventCommand() { Id = Guid.NewGuid() })).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task ShouldBeNotRestExceptionWhenUserWantToUnsubscribeEventWhereIsHost()
        {
            //arrange
            var eventId = Guid.NewGuid();

            var creatorUsername = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var createCommand = new Faker<CreateEventCommand>("en")
                .RuleFor(e => e.Id, f => f.Random.Guid())
                .RuleFor(e => e.Title, f => f.Lorem.Sentence())
                .RuleFor(e => e.Date, f => f.Date.Future())
                .RuleFor(e => e.Description, f => f.Lorem.Sentence(5))
                .RuleFor(e => e.Category, f => f.Lorem.Word())
                .RuleFor(e => e.City, f => f.Lorem.Word())
                .RuleFor(e => e.Venue, f => f.Lorem.Sentence(1)).Generate();

            var createdEventId = await SendAsync(createCommand);

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(new UnsubscribeEventCommand() { Id = createdEventId })).Should().ThrowAsync<RestException>();
        }
    }
}