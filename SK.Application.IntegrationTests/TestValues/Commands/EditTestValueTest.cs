using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SK.Application.Common.Exceptions;
using SK.Application.TestValues.Commands.CreateTestValue;
using SK.Application.TestValues.Commands.EditTestValue;
using SK.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.TestValues.Commands
{
    using static Testing;

    public class EditTestValueTest : TestBase
    {
        [Test]
        public async Task ShouldUpdateTestValue()
        {
            //arrange
            var testValueId = await SendAsync(new CreateArticleCommand(1, "New Value"));

            //act
            var command = new EditArticleCommand(testValueId, "Updated Value");
            await SendAsync(command);
            var actualTestValue = await FindAsync<Article>(testValueId);

            //assert
            actualTestValue.Name.Should().Be(command.Name);
            actualTestValue.LastModified.Should().BeCloseTo(DateTime.UtcNow, 1000);
        }

        [Test]
        public void ShouldRequireValidTestValue()
        {
            //arrange
            var command = new EditArticleCommand(99, "New Title");

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldRequireNameTestValue()
        {

            //arrange
            var testValueId = await SendAsync(new CreateArticleCommand(1, "New Value"));

            var command = new EditArticleCommand(1, null);

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }
    }
}
