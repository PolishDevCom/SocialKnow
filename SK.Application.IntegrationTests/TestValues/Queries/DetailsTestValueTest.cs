using NUnit.Framework;
using SK.Application.TestValues.Queries.DetailsTestValue;
using SK.Domain.Entities;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.TestValues.Queries
{
    using static Testing;

    public class DetailsTestValueTest : TestBase
    {
        [Test]
        public async Task ShouldReturnDetailsOfObject()
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

            int expectedId = 2;

            var query = new DetailsArticleQuery() { Id = expectedId };

            //act
            var result = await SendAsync(query);
            int actualId = result.Id;

            //assert
            Assert.AreEqual(expectedId, actualId);
        }
    }
}
