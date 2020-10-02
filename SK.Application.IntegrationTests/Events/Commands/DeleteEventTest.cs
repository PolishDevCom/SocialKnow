using Bogus;
using FluentAssertions;
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
            
            var command = new Faker<CreateEventCommand>("en")
                .RuleFor(e => e.Id, f => f.Random.Guid())
                .RuleFor(e => e.Title, f => f.Lorem.Sentence())
                .RuleFor(e => e.Date, f => f.Date.Future())
                .RuleFor(e => e.Description, f => f.Lorem.Sentence(5))
                .RuleFor(e => e.Category, f => f.Lorem.Word())
                .RuleFor(e => e.City, f => f.Lorem.Word())
                .RuleFor(e => e.Venue, f => f.Lorem.Sentence(1)).Generate();

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
