using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SK.Application.Common.Exceptions;
using SK.Application.TestValues.Commands.CreateTestValue;
using SK.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.TestValues.Commands
{
    using static Testing;

    public class CreateTestValueTest : TestBase
    {
        [Test]
        public async Task ShouldCreateTestValue()
        {
            //arrange
            var command = new CreateArticleCommand(1, "Test");

            //act
            var createdId = await SendAsync(command);
            var createdTestValue = await FindAsync<Article>(createdId);

            //assert
            createdTestValue.Should().NotBeNull();
            createdTestValue.Id.Should().Be(command.Id);
            createdTestValue.Name.Should().Be(command.Name);
            createdTestValue.Created.Should().BeCloseTo(DateTime.UtcNow, 1000);
        }

        [Test]
        public void ShouldRequireMinimumFields()
        {
            //arrange
            var command = new CreateArticleCommand();

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }
    }
}
