using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Events.Queries.DetailsEvent;
using SK.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Events.Queries
{
    using static Testing;

    public class DetailsEventTest : TestBase
    {
        [Test]
        public async Task ShouldReturnDetailsOfObject()
        {
            //arrange
            Guid expectedId = Guid.NewGuid();

            var event1 = new Faker<Event>("en")
                .RuleFor(e => e.Id, f => f.Random.Guid())
                .RuleFor(e => e.Title, f => f.Lorem.Sentence())
                .RuleFor(e => e.Date, f => f.Date.Future())
                .RuleFor(e => e.Description, f => f.Lorem.Sentence(5))
                .RuleFor(e => e.Category, f => f.Lorem.Word())
                .RuleFor(e => e.City, f => f.Lorem.Word())
                .RuleFor(e => e.Venue, f => f.Lorem.Sentence(1)).Generate();

            var event2 = new Faker<Event>("en")
                .RuleFor(e => e.Id, f => expectedId)
                .RuleFor(e => e.Title, f => f.Lorem.Sentence())
                .RuleFor(e => e.Date, f => f.Date.Future())
                .RuleFor(e => e.Description, f => f.Lorem.Sentence(5))
                .RuleFor(e => e.Category, f => f.Lorem.Word())
                .RuleFor(e => e.City, f => f.Lorem.Word())
                .RuleFor(e => e.Venue, f => f.Lorem.Sentence(1)).Generate();

            var event3 = new Faker<Event>("en")
                .RuleFor(e => e.Id, f => f.Random.Guid())
                .RuleFor(e => e.Title, f => f.Lorem.Sentence())
                .RuleFor(e => e.Date, f => f.Date.Future())
                .RuleFor(e => e.Description, f => f.Lorem.Sentence(5))
                .RuleFor(e => e.Category, f => f.Lorem.Word())
                .RuleFor(e => e.City, f => f.Lorem.Word())
                .RuleFor(e => e.Venue, f => f.Lorem.Sentence(1)).Generate();

            await AddAsync(event1);
            await AddAsync(event2);
            await AddAsync(event3);

            var query = new DetailsEventQuery() { Id = expectedId };

            //act
            var result = await SendAsync(query);

            //assert
            result.Id.Should().Be(expectedId);
            result.Title.Should().Be(event2.Title);
            result.Date.Should().BeCloseTo(event2.Date, 1000);
            result.Description.Should().Be(event2.Description);
            result.Category.Should().Be(event2.Category);
            result.City.Should().Be(event2.City);
            result.Venue.Should().Be(event2.Venue);
        }

        [Test]
        public async Task ShouldThrowNotFoundExceptionIfEventDoesNotExist()
        {
            //arrange
            Guid notExistingId = Guid.NewGuid();

            await AddAsync(new Faker<Event>("en")
                .RuleFor(e => e.Id, f => f.Random.Guid())
                .RuleFor(e => e.Title, f => f.Lorem.Sentence())
                .RuleFor(e => e.Date, f => f.Date.Future())
                .RuleFor(e => e.Description, f => f.Lorem.Sentence(5))
                .RuleFor(e => e.Category, f => f.Lorem.Word())
                .RuleFor(e => e.City, f => f.Lorem.Word())
                .RuleFor(e => e.Venue, f => f.Lorem.Sentence(1)).Generate());

            var query = new DetailsEventQuery() { Id = notExistingId };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(query)).Should().Throw<NotFoundException>();
        }
    }
}
