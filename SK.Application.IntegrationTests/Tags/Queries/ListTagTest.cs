using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Models;
using SK.Application.Tags.Queries.ListTag;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Tags.Queries
{
    using static Testing;

    public class ListTagTest : TestBase
    {
        [Test]
        public async Task ShouldReturnAllTagsAsAList()
        {
            int numberOfTags = 3;
            await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");
            for (int i = 0; i < numberOfTags; i++)
            {
                await AddAsync(new Faker<Tag>("en")
                    .RuleFor(a => a.Id, f => f.Random.Guid())
                    .RuleFor(a => a.Title, f => f.Lorem.Sentence())
                    .Generate());
            }

            var filter = new PaginationFilter();
            var path = String.Empty;

            var query = new ListTagQuery(filter, path);

            var result = await SendAsync(query);

            result.FirstPage.Should().BeNull();
            result.LastPage.Should().BeNull();
            result.NextPage.Should().BeNull();
            result.PreviousPage.Should().BeNull();

            result.Succeeded.Should().BeTrue();
            result.Errors.Should().BeNull();
            result.TotalRecords.Should().Be(numberOfTags);

            result.Data.Should().HaveCount(numberOfTags > filter.PageSize ? filter.PageSize : numberOfTags);
        }
    }
}
