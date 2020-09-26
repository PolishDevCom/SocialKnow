using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.TestValues.Commands.CreateTestValue;
using SK.Application.TestValues.Commands.DeleteTestValue;
using SK.Domain.Entities;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.TestValues.Commands
{

    using static Testing;

    public class DeleteTestValueTest : TestBase
    {
        [Test]
        public void ShouldRequireValidTestValueId()
        {
            //arrange
            var command = new DeleteArticleCommand { Id = 99 };

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldDeleteTestValue()
        {
            //arrange
            var createdTestValueId = await SendAsync(new CreateArticleCommand(1, "New value"));

            //act
            await SendAsync(new DeleteArticleCommand
            {
                Id = createdTestValueId
            });

            var actualTestValue = await FindAsync<Article>(createdTestValueId);

            //assert
            actualTestValue.Should().BeNull();
        }
    }
}
