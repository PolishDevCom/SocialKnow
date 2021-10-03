using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Categories.Commands.CreateCategory;
using SK.Application.Common.Exceptions;
using SK.Application.Discussions.Commands.CreateDiscussion;
using SK.Application.Discussions.Commands.EditDiscussion;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Discussions.Commands
{
    using static Testing;

    public class EditDiscussionTest : TestBase
    {
        [Test]
        public async Task ShouldUpdateDiscussion()
        {
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var categoryId = Guid.NewGuid();
            var createCategoryCommand = new Faker<CreateCategoryCommand>("en")
                .RuleFor(c => c.Id, f => f.PickRandomParam(categoryId))
                .RuleFor(c => c.Title, f => f.Lorem.Sentence(wordCount: 3)).Generate();
            await SendAsync(createCategoryCommand);

            var createdDiscussionId = await SendAsync(new Faker<CreateDiscussionCommand>("en")
                .RuleFor(d => d.Id, f => f.Random.Guid())
                .RuleFor(d => d.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(d => d.Description, f => f.Lorem.Sentences(sentenceCount: 2))
                .RuleFor(d => d.PostBody, f => f.Lorem.Paragraph()).Generate());

            var editCommand = new Faker<EditDiscussionCommand>("en")
                .RuleFor(d => d.Id, f => createdDiscussionId)
                .RuleFor(d => d.CategoryId, f => f.PickRandom(categoryId))
                .RuleFor(d => d.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(d => d.Description, f => f.Lorem.Sentences(sentenceCount: 2)).Generate();

            await SendAsync(editCommand);

            var editedDiscussion = await FirstOrDefaultWithIncludeAsync<Discussion, Category>(x => x.Category);

            editedDiscussion.Should().NotBeNull();
            editedDiscussion.Id.Should().Be(editCommand.Id);
            editedDiscussion.Title.Should().Be(editCommand.Title);
            editedDiscussion.Description.Should().Be(editCommand.Description);
            editedDiscussion.IsClosed.Should().Be(false);
            editedDiscussion.IsPinned.Should().Be(false);
            editedDiscussion.LastModified.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0,0,1));
            editedDiscussion.LastModifiedBy.Should().Be(loggedUser);
            editedDiscussion.Category.Id.Should().Be(categoryId);
        }

        [Test]
        public void ShouldThrowNotFoundException()
        {
            //arrange
            var editCommand = new Faker<EditDiscussionCommand>("en")
                .RuleFor(d => d.Id, f => f.Random.Guid())
                .RuleFor(d => d.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(d => d.Description, f => f.Lorem.Sentences(sentenceCount: 2)).Generate();

            //assert
            FluentActions.Invoking(() =>
                SendAsync(editCommand)).Should().ThrowAsync<NotFoundException>();
        }

        private static IEnumerable<TestCaseData> ShouldThrowValidationExceptionDuringEditingDiscussionTestCases
        {
            get
            {
                yield return new TestCaseData(null, new Faker("en").Lorem.Sentences(sentenceCount: 2))
                    .SetName("DiscussionTitleMissingTest");
                yield return new TestCaseData(new Faker("en").Lorem.Sentence(wordCount: 3), null)
                    .SetName("DiscussionDescriptionMissingTest");
            }
        }

        [Test]
        [TestCaseSource(nameof(ShouldThrowValidationExceptionDuringEditingDiscussionTestCases))]
        public async Task ShouldThrowValidationExceptionDuringDiscussionCreation(string testTitle, string testDescription)
        {
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var createdDiscussionId = await SendAsync(new Faker<CreateDiscussionCommand>("en")
                .RuleFor(d => d.Id, f => f.Random.Guid())
                .RuleFor(d => d.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(d => d.Description, f => f.Lorem.Sentences(sentenceCount: 2))
                .RuleFor(d => d.PostBody, f => f.Lorem.Paragraph()).Generate());

            var editCommand = new CreateDiscussionCommand()
            {
                Id = createdDiscussionId,
                Title = testTitle,
                Description = testDescription
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(editCommand)).Should().ThrowAsync<Common.Exceptions.ValidationException>();
        }
    }
}