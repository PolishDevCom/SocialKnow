using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.AdditionalInfoDefinitions.Queries.ListAdditionalInfoDefinition;
using SK.Application.Common.Models;
using SK.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.AdditionalInfoDefinitions.Queries
{
    using static Testing;

    public class ListAdditionalInfoDefinitionsTest : TestBase
    {
        [Test]
        public async Task ShouldReturnAllAdditionalInfoDefinitionsAsAList()
        {
            int numberOfAdditionalInfoDefinitions = 3;
            await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");
            for (int i = 0; i < numberOfAdditionalInfoDefinitions; i++)
            {
                await AddAsync(new Faker<AdditionalInfoDefinition>("en")
                    .RuleFor(a => a.Id, f => f.Random.Guid())
                    .RuleFor(a => a.InfoName, f => f.Lorem.Word())
                    .RuleFor(a => a.InfoType, f => "string")
                    .Generate());
            }

            var filter = new PaginationFilter();
            var path = String.Empty;

            var query = new ListAdditionalInfoDefinitionQuery(filter, path);

            var result = await SendAsync(query);

            result.FirstPage.Should().BeNull();
            result.LastPage.Should().BeNull();
            result.NextPage.Should().BeNull();
            result.PreviousPage.Should().BeNull();

            result.Succeeded.Should().BeTrue();
            result.Errors.Should().BeNull();
            result.TotalRecords.Should().Be(numberOfAdditionalInfoDefinitions);

            result.Data.Should().HaveCount(numberOfAdditionalInfoDefinitions > filter.PageSize ? filter.PageSize : numberOfAdditionalInfoDefinitions);
        }
    }
}