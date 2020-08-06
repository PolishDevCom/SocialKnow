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
            var testValueId = await SendAsync(new CreateTestValueCommand(1, "New Value"));

            //act
            var command = new EditTestValueCommand(testValueId, "Updated Value");
            await SendAsync(command);
            var actualTestValue = await FindAsync<TestValue>(testValueId);

            //assert
            actualTestValue.Name.Should().Be(command.Name);
            actualTestValue.LastModifiedBy.Should().NotBeNull();
            actualTestValue.LastModified.Should().NotBeNull();
            actualTestValue.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
        }

        [Test]
        public void ShouldRequireValidTestValue()
        {
            //arrange
            var command = new EditTestValueCommand(99, "New Title");

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldRequireNameTestValue()
        {

            //arrange
            var testValueId = await SendAsync(new CreateTestValueCommand(1, "New Value"));

            var command = new EditTestValueCommand(1, null);

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }
    }
}
