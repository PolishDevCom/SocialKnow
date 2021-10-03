using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.AdditionalInfoDefinitions.Commands.CreateAdditionalInfoDefinition;
using SK.Application.AdditionalInfoDefinitions.Commands.DeleteAdditionalInfoDefinition;
using SK.Application.Common.Exceptions;
using SK.Domain.Entities;
using SK.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.AdditionalInfoDefinitions.Commands
{
    using static Testing;

    public class DeleteAdditionalInfoDefinitionsTests : TestBase
    {
        [Test]
        public void ShouldRequireValidAdditionalInfoDefinitionId()
        {
            //arrange
            var command = new DeleteAdditionalInfoDefinitionCommand { Id = Guid.NewGuid() };

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task ShouldDeleteAdditionalInfoDefinition()
        {
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var command = new Faker<CreateAdditionalInfoDefinitionCommand>("en")
                    .RuleFor(a => a.Id, f => f.Random.Guid())
                    .RuleFor(a => a.InfoName, f => f.Lorem.Word())
                    .RuleFor(a => a.TypeOfField, f => TypeOfField.Text)
                    .Generate();

            var createdAdditionalInfoDefinitionId = await SendAsync(command);

            //act
            var result = await SendAsync(new DeleteAdditionalInfoDefinitionCommand
            {
                Id = createdAdditionalInfoDefinitionId
            });

            var actualAdditionalInfoDefinition = await FindByGuidAsync<AdditionalInfoDefinition>(createdAdditionalInfoDefinitionId);

            //assert
            actualAdditionalInfoDefinition.Should().BeNull();
        }
    }
}
