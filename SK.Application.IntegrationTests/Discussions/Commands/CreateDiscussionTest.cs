using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Categories.Commands.CreateCategory;
using SK.Application.Discussions.Commands.CreateDiscussion;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Discussions.Commands
{
    using static Testing;

    public class CreateDiscussionTest : TestBase
    {
        [Test]
        public async Task ShouldCreateDiscussionWithFirstPost()
        {
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var categoryId = Guid.NewGuid();
            var createCategoryCommand = new Faker<CreateCategoryCommand>("en")
                .RuleFor(c => c.Id, f => f.PickRandomParam(categoryId))
                .RuleFor(c => c.Title, f => f.Lorem.Sentence(wordCount: 3)).Generate();
            await SendAsync(createCategoryCommand);

            var createDiscussionCommand = new Faker<CreateDiscussionCommand>("en")
                .RuleFor(d => d.Id, f => f.Random.Guid())
                .RuleFor(d => d.CategoryId, f => f.PickRandom(categoryId))
                .RuleFor(d => d.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(d => d.Description, f => f.Lorem.Sentences(sentenceCount: 2))
                .RuleFor(d => d.PostBody, f => f.Lorem.Paragraph()).Generate();

            var createdDiscussionId = await SendAsync(createDiscussionCommand);

            var createdDiscussion = await FirstOrDefaultWithIncludeAsync<Discussion, Category>(x => x.Category);

            var createdFirstPost = FindPostsByDiscussionGuidAsync(createdDiscussionId);

            createdDiscussion.Should().NotBeNull();
            createdDiscussion.Id.Should().Be(createDiscussionCommand.Id);
            createdDiscussion.Title.Should().Be(createDiscussionCommand.Title);
            createdDiscussion.Description.Should().Be(createDiscussionCommand.Description);
            createdDiscussion.IsClosed.Should().Be(false);
            createdDiscussion.IsPinned.Should().Be(false);
            createdDiscussion.Created.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0,0,1));
            createdDiscussion.CreatedBy.Should().Be(loggedUser);
            createdDiscussion.Category.Id.Should().Be(categoryId);

            createdFirstPost.Should().HaveCount(1);
            createdFirstPost.First().IsPinned.Should().Be(false);
            createdFirstPost.First().Body.Should().Be(createDiscussionCommand.PostBody);
        }

        private static IEnumerable<TestCaseData> ShouldThrowValidationExceptionDuringCreatingDiscussionTestCases
        {
            get
            {
                yield return new TestCaseData(null, new Faker("en").Lorem.Sentences(sentenceCount: 2), new Faker("en").Lorem.Paragraph())
                    .SetName("DiscussionTitleMissingTest");
                yield return new TestCaseData(new Faker("en").Lorem.Sentence(wordCount: 3), null, new Faker("en").Lorem.Paragraph())
                    .SetName("DiscussionDescriptionMissingTest");
                yield return new TestCaseData(new Faker("en").Lorem.Sentence(wordCount: 3), new Faker("en").Lorem.Sentences(sentenceCount: 2), null)
                    .SetName("DiscussionPostBodyMissingTest");
            }
        }

        [Test]
        [TestCaseSource(nameof(ShouldThrowValidationExceptionDuringCreatingDiscussionTestCases))]
        public void ShouldThrowValidationExceptionDuringDiscussionCreation(string testTitle, string testDescription, string testPostBody)
        {
            //arrange
            var command = new CreateDiscussionCommand()
            {
                Id = Guid.NewGuid(),
                Title = testTitle,
                Description = testDescription,
                PostBody = testPostBody
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<Common.Exceptions.ValidationException>();
        }
    }
}