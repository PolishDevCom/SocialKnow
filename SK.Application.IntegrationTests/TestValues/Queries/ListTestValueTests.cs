using FluentAssertions;
using NUnit.Framework;
using SK.Application.TestValues.Queries.ListTestValue;
using SK.Domain.Entities;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.TestValues.Queries
{

    using static Testing;

    public class ListTestValueTests : TestBase
    {
        [Test]
        public async Task ShouldReturnAllTestValueAsAList()
        {
            //arrange
            await AddAsync(new Article
            {
                Id = 1,
                Name = "Test1"
            });
            await AddAsync(new Article
            {
                Id = 2,
                Name = "Test2"
            });
            await AddAsync(new Article
            {
                Id = 3,
                Name = "Test3"
            });

            var query = new ListArticleQuery();

            //act
            var result = await SendAsync(query);

            //assert
            result.Should().HaveCount(3);
        }
    }
}
