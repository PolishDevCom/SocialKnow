using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.AdditionalInfoDefinitions.Commands;
using SK.Application.AdditionalInfoDefinitions.Commands.CreateAdditionalInfoDefinition;
using SK.Domain.Entities;
using SK.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.AdditionalInfoDefinitions.Commands
{
    using static Testing;

    public class CreateAdditionalInfoDefinitionsTests : TestBase
    {
        [Test]
        public async Task ShouldCreateAdditionalInfoDefinition()
        {
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var additionalInfoDefinitionToCreate = new Faker<AdditionalInfoDefinitionCreateOrEditDto>("en")
                    .RuleFor(a => a.Id, f => f.Random.Guid())
                    .RuleFor(a => a.InfoName, f => f.Lorem.Word())
                    .RuleFor(a => a.TypeOfField, f => TypeOfField.Text)
                    .Generate();

            var command = new CreateAdditionalInfoDefinitionCommand(additionalInfoDefinitionToCreate);

            var additionalInfoDefinitionId = await SendAsync(command);
            var createdAdditionalInfoDefinition = await FindByGuidAsync<AdditionalInfoDefinition>(additionalInfoDefinitionId);

            createdAdditionalInfoDefinition.Id.Should().Be(additionalInfoDefinitionId);
            createdAdditionalInfoDefinition.InfoType.Should().Be("string");
            createdAdditionalInfoDefinition.InfoName.Should().Be(additionalInfoDefinitionToCreate.InfoName);
            createdAdditionalInfoDefinition.CreatedBy.Should().Be(loggedUser);
            createdAdditionalInfoDefinition.Created.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0, 0, 1));
        }

        private static IEnumerable<TestCaseData> ShouldRequireFieldAndThrowValidationExceptionDuringCreatingAdditionalInfoDefinitionTestCases
        {
            get
            {
                yield return new TestCaseData(null, TypeOfField.Text)
                    .SetName("AdditionalInfoDefinitionInfoNameMissingTest");
            }
        }

        [Test]
        [TestCaseSource(nameof(ShouldRequireFieldAndThrowValidationExceptionDuringCreatingAdditionalInfoDefinitionTestCases))]
        public async Task ShouldRequireFieldAndThrowValidationExceptionDuringArticleCreating(string infoName, TypeOfField typeOfField)
        {
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var additionalInfoDefinitionToCreate = new Faker<AdditionalInfoDefinitionCreateOrEditDto>("en")
                    .RuleFor(a => a.Id, f => f.Random.Guid())
                    .RuleFor(a => a.InfoName, f => infoName)
                    .RuleFor(a => a.TypeOfField, f => typeOfField)
                    .Generate();

            var command = new CreateAdditionalInfoDefinitionCommand(additionalInfoDefinitionToCreate);

            Func<Task> act = async () => await SendAsync(command);

            act.Should().ThrowAsync<Common.Exceptions.ValidationException>();
        }
    }
}