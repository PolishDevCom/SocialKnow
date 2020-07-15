using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SK.Application.Common.Exceptions;
using SK.Application.TestValues.Commands.Create;
using SK.Application.TestValues.Commands.Edit;
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
            var testValueId = await SendAsync(new CreateTestValueCommand.Command
            {
                Id = 1,
                Name = "New Value"
            });

            //act
            var command = new EditTestValueCommand.Command
            {
                Id = testValueId,
                Name = "Updated Value"
            };
            await SendAsync(command);
            var actualTestValue = await FindAsync<TestValue>(testValueId);

            //assert
            actualTestValue.Name.Should().Be(command.Name);
            actualTestValue.LastModifiedBy.Should().NotBeNull();
            actualTestValue.LastModifiedBy.Should().Be("MADO");
            actualTestValue.LastModified.Should().NotBeNull();
            actualTestValue.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
        }

        [Test]
        public void ShouldRequireValidTestValue()
        {
            //arrange
            var command = new EditTestValueCommand.Command
            {
                Id = 99,
                Name = "New Title"
            };

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldRequireNameTestValue()
        {

            //arrange
            var testValueId = await SendAsync(new CreateTestValueCommand.Command
            {
                Id = 1,
                Name = "New Value"
            });

            var command = new EditTestValueCommand.Command
            {
                Id = 1
            };

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }
    }
}
