using FluentAssertions;
using NUnit.Framework;
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
            await AddAsync(new Event
            {
                Id = Guid.NewGuid(),
                Title = "Test Event1",
                Date = DateTime.Now,
                Description = "Event now",
                Category = "webinar",
                City = "Internet",
                Venue = "Discord"
            });
            await AddAsync(new Event
            {
                Id = Guid.NewGuid(),
                Title = "Test Event2",
                Date = DateTime.Now.AddDays(1),
                Description = "Event now",
                Category = "webinar",
                City = "Internet",
                Venue = "Discord"
            });
            await AddAsync(new Event
            {
                Id = Guid.NewGuid(),
                Title = "Test Event3",
                Date = DateTime.Now.AddDays(-1),
                Description = "Event now",
                Category = "webinar",
                City = "Internet",
                Venue = "Discord"
            });

            var query = new ListEventQuery();

            //act
            var result = await SendAsync(query);

            //assert
            result.Should().HaveCount(3);
            result[0].Title.Should().Be("Test Event2");
            result[1].Title.Should().Be("Test Event1");
            result[2].Title.Should().Be("Test Event3");
        }
    }
}
