using Bogus;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SK.Application.Common.Exceptions;
using SK.Application.Events.Commands.CreateEvent;
using SK.Application.Events.Commands.EditEvent;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
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

            var eventId = await SendAsync(new Faker<CreateEventCommand>("en")
                .RuleFor(e => e.Id, f => f.Random.Guid())
                .RuleFor(e => e.Title, f => f.Lorem.Sentence())
                .RuleFor(e => e.Date, f => f.Date.Future())
                .RuleFor(e => e.Description, f => f.Lorem.Sentence(5))
                .RuleFor(e => e.Category, f => f.Lorem.Word())
                .RuleFor(e => e.City, f => f.Lorem.Word())
                .RuleFor(e => e.Venue, f => f.Lorem.Sentence(1)).Generate());

            //act
            var command = new Faker<EditEventCommand>("en")
                .RuleFor(e => e.Id, f => eventId)
                .RuleFor(e => e.Title, f => f.Lorem.Sentence())
                .RuleFor(e => e.Date, f => f.Date.Future())
                .RuleFor(e => e.Description, f => f.Lorem.Sentence(5))
                .RuleFor(e => e.Category, f => f.Lorem.Word())
                .RuleFor(e => e.City, f => f.Lorem.Word())
                .RuleFor(e => e.Venue, f => f.Lorem.Sentence(1)).Generate();
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

        private static IEnumerable<TestCaseData> ShouldThrowValidationExceptionDuringEditingEventTestCases
        {
            get
            {
                yield return new TestCaseData(new Faker("en").Date.Future(), null, new Faker("en").Lorem.Sentence(5), new Faker("en").Lorem.Word(), new Faker("en").Lorem.Word(), new Faker("en").Lorem.Sentence(1))
                    .SetName("EventTitleMissingTest");
                yield return new TestCaseData(new Faker("en").Date.Future(), new Faker("en").Lorem.Sentence(), null, new Faker("en").Lorem.Word(), new Faker("en").Lorem.Word(), new Faker("en").Lorem.Sentence(1))
                    .SetName("EventDescriptionMissingTest");
                yield return new TestCaseData(new Faker("en").Date.Future(), new Faker("en").Lorem.Sentence(), new Faker("en").Lorem.Sentence(5), null, new Faker("en").Lorem.Word(), new Faker("en").Lorem.Sentence(1))
                    .SetName("EventCategoryMissingTest");
                yield return new TestCaseData(new Faker("en").Date.Future(), new Faker("en").Lorem.Sentence(), new Faker("en").Lorem.Sentence(5), new Faker("en").Lorem.Word(), null, new Faker("en").Lorem.Sentence(1))
                    .SetName("EventCityMissingTest");
                yield return new TestCaseData(new Faker("en").Date.Future(), new Faker("en").Lorem.Sentence(), new Faker("en").Lorem.Sentence(5), new Faker("en").Lorem.Word(), new Faker("en").Lorem.Word(), null)
                    .SetName("EventVenueMissingTest");
                yield return new TestCaseData(new Faker("en").Date.Future(), new Faker("en").Lorem.Sentence(50), new Faker("en").Lorem.Sentence(5), new Faker("en").Lorem.Word(), new Faker("en").Lorem.Word(), new Faker("en").Lorem.Sentence(1))
                    .SetName("EventTooLongTitleTest");
            }
        }

        [Test]
        [TestCaseSource(nameof(ShouldThrowValidationExceptionDuringEditingEventTestCases))]
        public async Task ShouldThrowValidationExceptionDuringEventEditing(DateTime testDate, string testTitle, string testDescribtion, string testCategory, string testCity, string testVenue)
        {
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var eventId = await SendAsync(new Faker<CreateEventCommand>("en")
                .RuleFor(e => e.Id, f => f.Random.Guid())
                .RuleFor(e => e.Title, f => f.Lorem.Sentence())
                .RuleFor(e => e.Date, f => f.Date.Future())
                .RuleFor(e => e.Description, f => f.Lorem.Sentence(5))
                .RuleFor(e => e.Category, f => f.Lorem.Word())
                .RuleFor(e => e.City, f => f.Lorem.Word())
                .RuleFor(e => e.Venue, f => f.Lorem.Sentence(1)).Generate());

            var command = new EditEventCommand()
            {
                Id = eventId,
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

        [Test]
        public async Task ShouldThrowValidationExceptionWhenEventDateMissing()
        {
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var eventId = await SendAsync(new Faker<CreateEventCommand>("en")
                .RuleFor(e => e.Id, f => f.Random.Guid())
                .RuleFor(e => e.Title, f => f.Lorem.Sentence())
                .RuleFor(e => e.Date, f => f.Date.Future())
                .RuleFor(e => e.Description, f => f.Lorem.Sentence(5))
                .RuleFor(e => e.Category, f => f.Lorem.Word())
                .RuleFor(e => e.City, f => f.Lorem.Word())
                .RuleFor(e => e.Venue, f => f.Lorem.Sentence(1)).Generate());

            var command = new Faker<EditEventCommand>("en")
                .RuleFor(e => e.Id, f => eventId)
                .RuleFor(e => e.Title, f => f.Lorem.Sentence())
                .RuleFor(e => e.Date, f => null)
                .RuleFor(e => e.Description, f => f.Lorem.Sentence(5))
                .RuleFor(e => e.Category, f => f.Lorem.Word())
                .RuleFor(e => e.City, f => f.Lorem.Word())
                .RuleFor(e => e.Venue, f => f.Lorem.Sentence(1)).Generate();

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<Common.Exceptions.ValidationException>();
        }

        [Test]
        public void ShouldThrowNotFoundException()
        {
            //arrange
            var command = new Faker<EditEventCommand>("en")
                .RuleFor(e => e.Id, f => f.Random.Guid())
                .RuleFor(e => e.Title, f => f.Lorem.Sentence())
                .RuleFor(e => e.Date, f => f.Date.Future())
                .RuleFor(e => e.Description, f => f.Lorem.Sentence(5))
                .RuleFor(e => e.Category, f => f.Lorem.Word())
                .RuleFor(e => e.City, f => f.Lorem.Word())
                .RuleFor(e => e.Venue, f => f.Lorem.Sentence(1)).Generate();

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }
    }
}
