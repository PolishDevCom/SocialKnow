using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Events.Commands.CreateEvent;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
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
            var command = new Faker<CreateEventCommand>("en")
                .RuleFor(e => e.Id, f => f.Random.Guid())
                .RuleFor(e => e.Title, f => f.Lorem.Sentence())
                .RuleFor(e => e.Date, f => DateTime.UtcNow)
                .RuleFor(e => e.Description, f => f.Lorem.Sentence(5))
                .RuleFor(e => e.Category, f => f.Lorem.Word())
                .RuleFor(e => e.City, f => f.Lorem.Word())
                .RuleFor(e => e.Venue, f => f.Lorem.Sentence(1)).Generate();

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

        private static IEnumerable<TestCaseData> ShouldThrowValidationExceptionDuringCreatingEventTestCases
        {
            get
            {
                yield return new TestCaseData(null, new Faker("en").Lorem.Sentence(), new Faker("en").Lorem.Sentence(5), new Faker("en").Lorem.Word(), new Faker("en").Lorem.Word(), new Faker("en").Lorem.Sentence(1));
                yield return new TestCaseData(new Faker("en").Date.Future(), null, new Faker("en").Lorem.Sentence(5), new Faker("en").Lorem.Word(), new Faker("en").Lorem.Word(), new Faker("en").Lorem.Sentence(1));
                yield return new TestCaseData(new Faker("en").Date.Future(), new Faker("en").Lorem.Sentence(), null, new Faker("en").Lorem.Word(), new Faker("en").Lorem.Word(), new Faker("en").Lorem.Sentence(1));
                yield return new TestCaseData(new Faker("en").Date.Future(), new Faker("en").Lorem.Sentence(), new Faker("en").Lorem.Sentence(5), null, new Faker("en").Lorem.Word(), new Faker("en").Lorem.Sentence(1));
                yield return new TestCaseData(new Faker("en").Date.Future(), new Faker("en").Lorem.Sentence(), new Faker("en").Lorem.Sentence(5), new Faker("en").Lorem.Word(), null, new Faker("en").Lorem.Sentence(1));
                yield return new TestCaseData(new Faker("en").Date.Future(), new Faker("en").Lorem.Sentence(), new Faker("en").Lorem.Sentence(5), new Faker("en").Lorem.Word(), new Faker("en").Lorem.Word(), null);
                yield return new TestCaseData(new Faker("en").Date.Future(), new Faker("en").Lorem.Sentence(), new Faker("en").Lorem.Sentence(5), new Faker("en").Lorem.Word(), new Faker("en").Lorem.Word(), new Faker("en").Lorem.Sentence(50));
            }
        }

        [Test]
        [TestCaseSource(nameof(ShouldThrowValidationExceptionDuringCreatingEventTestCases))]
        public void ShouldThrowValidationExceptionDuringEventCreation(DateTime testDate, string testTitle, string testDescribtion, string testCategory, string testCity, string testVenue)
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
                SendAsync(command)).Should().Throw<Common.Exceptions.ValidationException>();
        }
    }
}
