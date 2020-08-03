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
        public void ShouldRequireValidTodoListId()
        {
            //arrange
            var command = new DeleteTestValueCommand { Id = 99 };

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldDeleteTodoList()
        {
            //arrange
            var createdTestValueId = await SendAsync(new CreateTestValueCommand
            {
                Id = 1,
                Name = "New value"
            });

            //act
            await SendAsync(new DeleteTestValueCommand
            {
                Id = createdTestValueId
            });

            var actualTestValue = await FindAsync<TestValue>(createdTestValueId);

            //assert
            actualTestValue.Should().BeNull();
        }
    }
}
