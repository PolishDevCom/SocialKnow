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

            await AddAsync(new Event
            {
                Id = Guid.NewGuid(),
                Title = "Test Event1",
                Date = DateTime.Now,
                Description = "Event now1",
                Category = "webinar1",
                City = "Internet1",
                Venue = "Discord1"
            });
            await AddAsync(new Event
            {
                Id = expectedId,
                Title = "Test Event2",
                Date = DateTime.Now.AddDays(1),
                Description = "Event now2",
                Category = "webinar2",
                City = "Internet2",
                Venue = "Discord2"
            });
            await AddAsync(new Event
            {
                Id = Guid.NewGuid(),
                Title = "Test Event3",
                Date = DateTime.Now.AddDays(-1),
                Description = "Event now3",
                Category = "webinar3",
                City = "Internet3",
                Venue = "Discord3"
            });

            

            var query = new DetailsEventQuery() { Id = expectedId };

            //act
            var result = await SendAsync(query);

            //assert
            result.Id.Should().Be(expectedId);
            result.Title.Should().Be("Test Event2");
            result.Date.Should().BeCloseTo(DateTime.Now.AddDays(1), 1000);
            result.Description.Should().Be("Event now2");
            result.Category.Should().Be("webinar2");
            result.City.Should().Be("Internet2");
            result.Venue.Should().Be("Discord2");
        }

        [Test]
        public async Task ShouldThrowNotFoundExceptionIfEventDoesNotExist()
        {
            //arrange
            Guid notExistingId = Guid.NewGuid();

            await AddAsync(new Event
            {
                Id = Guid.NewGuid(),
                Title = "Test Event1",
                Date = DateTime.Now,
                Description = "Event now1",
                Category = "webinar1",
                City = "Internet1",
                Venue = "Discord1"
            });

            var query = new DetailsEventQuery() { Id = notExistingId };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(query)).Should().Throw<NotFoundException>();
        }
    }
}
