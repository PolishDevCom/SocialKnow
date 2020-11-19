using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Models;
using SK.Application.Events.Queries.ListEvent;
using SK.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Events.Queries
{
    using static Testing;

    public class ListEventTest : TestBase
    {
        [Test]
        public async Task ShouldReturnAllEventsAsAList()
        {
            //arrange
            var event1 = new Faker<Event>("en")
                .RuleFor(e => e.Id, f => f.Random.Guid())
                .RuleFor(e => e.Title, f => f.Lorem.Sentence())
                .RuleFor(e => e.Date, f => DateTime.Now)
                .RuleFor(e => e.Description, f => f.Lorem.Sentence(5))
                .RuleFor(e => e.Category, f => f.Lorem.Word())
                .RuleFor(e => e.City, f => f.Lorem.Word())
                .RuleFor(e => e.Venue, f => f.Lorem.Sentence(1)).Generate();

            var event2 = new Faker<Event>("en")
                .RuleFor(e => e.Id, f => f.Random.Guid())
                .RuleFor(e => e.Title, f => f.Lorem.Sentence())
                .RuleFor(e => e.Date, f => DateTime.Now.AddDays(1))
                .RuleFor(e => e.Description, f => f.Lorem.Sentence(5))
                .RuleFor(e => e.Category, f => f.Lorem.Word())
                .RuleFor(e => e.City, f => f.Lorem.Word())
                .RuleFor(e => e.Venue, f => f.Lorem.Sentence(1)).Generate();

            var event3 = new Faker<Event>("en")
                .RuleFor(e => e.Id, f => f.Random.Guid())
                .RuleFor(e => e.Title, f => f.Lorem.Sentence())
                .RuleFor(e => e.Date, f => DateTime.Now.AddDays(-1))
                .RuleFor(e => e.Description, f => f.Lorem.Sentence(5))
                .RuleFor(e => e.Category, f => f.Lorem.Word())
                .RuleFor(e => e.City, f => f.Lorem.Word())
                .RuleFor(e => e.Venue, f => f.Lorem.Sentence(1)).Generate();

            var filter = new PaginationFilter();
            var path = String.Empty;

            await AddAsync(event1);
            await AddAsync(event2);
            await AddAsync(event3);

            var query = new ListEventQuery(filter, path);

            //act
            var result = await SendAsync(query);

            //assert
            result.FirstPage.Should().BeNull();
            result.LastPage.Should().BeNull();
            result.NextPage.Should().BeNull();
            result.PreviousPage.Should().BeNull();

            result.Succeeded.Should().BeTrue();
            result.Errors.Should().BeNull();

            result.TotalRecords.Should().Be(3);

            result.Data.Should().HaveCount(3);
            result.Data[0].Id.Should().Be(event2.Id);
            result.Data[1].Id.Should().Be(event1.Id);
            result.Data[2].Id.Should().Be(event3.Id);
        }
    }
}
