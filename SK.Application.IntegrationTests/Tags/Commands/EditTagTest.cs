using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Tags.Commands;
using SK.Application.Tags.Commands.CreateTag;
using SK.Application.Tags.Commands.EditTag;
using SK.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Tags.Commands
{
    using static Testing;

    public class EditTagTest : TestBase
    {
        [Test]
        public async Task ShouldUpdateTag()
        {
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var tagId = await CreateTag();

            var tagToModify = new Faker<TagCreateOrEditDto>("en")
                .RuleFor(a => a.Id, f => tagId)
                .RuleFor(a => a.Title, f => f.Lorem.Sentence())
                .Generate();

            var command = new EditTagCommand(tagToModify);
            await SendAsync(command);

            var modifiedTag = await FindByGuidAsync<Tag>(tagId);

            modifiedTag.Id.Should().Be(tagToModify.Id);
            modifiedTag.Title.Should().Be(tagToModify.Title);
            modifiedTag.LastModifiedBy.Should().Be(loggedUser);
            modifiedTag.LastModified.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0,0,1));
        }

        [Test]
        public void ShouldRequireValidTagId()
        {
            var tagToEdit = new Faker<TagCreateOrEditDto>("en")
               .RuleFor(a => a.Id, f => f.Random.Guid())
               .RuleFor(a => a.Title, f => f.Lorem.Sentence())
               .Generate();

            var command = new EditTagCommand(tagToEdit);

            Func<Task> act = async () => await SendAsync(command);

            act.Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task ShouldNotEditTagIfTitleIsEmpty()
        {
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var tagId = await CreateTag();

            var tagToCreate = new Faker<TagCreateOrEditDto>("en")
                .RuleFor(a => a.Id, f => tagId)
                .Generate();
            tagToCreate.Title = null;

            var command = new EditTagCommand(tagToCreate);

            Func<Task> act = async () => await SendAsync(command);

            act.Should().ThrowAsync<Common.Exceptions.ValidationException>();
        }

        private async Task<Guid> CreateTag()
        {
            var tagToCreate = new Faker<TagCreateOrEditDto>("en")
                .RuleFor(a => a.Id, f => f.Random.Guid())
                .RuleFor(a => a.Title, f => f.Lorem.Sentence())
                .Generate();

            return await SendAsync(new CreateTagCommand(tagToCreate));
        }
    }
}