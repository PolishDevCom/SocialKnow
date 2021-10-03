using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.AdditionalInfoDefinitions.Commands;
using SK.Application.AdditionalInfoDefinitions.Commands.CreateAdditionalInfoDefinition;
using SK.Application.AdditionalInfoDefinitions.Commands.EditAdditionalInfoDefinition;
using SK.Application.Common.Exceptions;
using SK.Domain.Entities;
using SK.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.AdditionalInfoDefinitions.Commands
{
    using static Testing;

    public class EditAdditionalInfoDefinitionsTests : TestBase
    {
        [Test]
        public async Task ShouldUpdateAdditionalInfoDefinition()
        {
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var additionalInfoDefinitionToCreate = new Faker<AdditionalInfoDefinitionCreateOrEditDto>("en")
                .RuleFor(a => a.Id, f => f.Random.Guid())
                .RuleFor(a => a.InfoName, f => f.Lorem.Word())
                .RuleFor(a => a.TypeOfField, f => TypeOfField.Text)
                .Generate();

            var additionalInfoDefinitionId = await SendAsync(new CreateAdditionalInfoDefinitionCommand(additionalInfoDefinitionToCreate));

            var additionalInfoDefinitionToModify = new Faker<AdditionalInfoDefinitionCreateOrEditDto>("en")
                .RuleFor(a => a.Id, f => additionalInfoDefinitionId)
                .RuleFor(a => a.InfoName, f => f.Lorem.Word())
                .RuleFor(a => a.TypeOfField, f => TypeOfField.Number)
                .Generate();

            //act
            var command = new EditAdditionalInfoDefinitionCommand(additionalInfoDefinitionToModify);
            await SendAsync(command);

            var modifiedAdditionalInfoDefinition = await FindByGuidAsync<AdditionalInfoDefinition>(additionalInfoDefinitionId);

            //assert
            modifiedAdditionalInfoDefinition.Id.Should().Be(additionalInfoDefinitionToModify.Id);
            modifiedAdditionalInfoDefinition.InfoName.Should().Be(additionalInfoDefinitionToModify.InfoName);
            modifiedAdditionalInfoDefinition.InfoType.Should().Be("int");
            modifiedAdditionalInfoDefinition.LastModifiedBy.Should().Be(loggedUser);
            modifiedAdditionalInfoDefinition.LastModified.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0,0,1));
        }

        [Test]
        public void ShouldRequireFieldAndThrowValidationExceptionDuringAdditionalInfoDefinitionEditing()
        {
            //arrange
            var command = new Faker<EditAdditionalInfoDefinitionCommand>("en")
                .RuleFor(a => a.Id, f => Guid.NewGuid())
                .RuleFor(a => a.InfoName, f => null)
                .RuleFor(a => a.TypeOfField, f => TypeOfField.Number)
                .Generate();

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<Common.Exceptions.ValidationException>();
        }

        [Test]
        public void ShouldRequireValidArticleIdAndThrowValidationException()
        {
            //arrange
            var command = new Faker<EditAdditionalInfoDefinitionCommand>("en")
                .RuleFor(a => a.Id, f => Guid.NewGuid())
                .RuleFor(a => a.InfoName, f => f.Lorem.Word())
                .RuleFor(a => a.TypeOfField, f => TypeOfField.Number)
                .Generate();

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<NotFoundException>();
        }
    }
}