using Bogus;
using FluentAssertions;
using NUnit.Framework;
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
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var createDiscussionCommand = new Faker<CreateDiscussionCommand>("en")
                .RuleFor(d => d.Id, f => f.Random.Guid())
                .RuleFor(d => d.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(d => d.Description, f => f.Lorem.Sentences(sentenceCount: 2))
                .RuleFor(d => d.PostBody, f => f.Lorem.Paragraph()).Generate();

            //act
            var createdDiscussionId = await SendAsync(createDiscussionCommand);

            var createdDiscussion = await FindByGuidAsync<Discussion>(createdDiscussionId);

            var createdFirstPost = FindPostsByDiscussionGuidAsync(createdDiscussionId);

            //assert
            createdDiscussion.Should().NotBeNull();
            createdDiscussion.Id.Should().Be(createDiscussionCommand.Id);
            createdDiscussion.Title.Should().Be(createDiscussionCommand.Title);
            createdDiscussion.Description.Should().Be(createDiscussionCommand.Description);
            createdDiscussion.IsClosed.Should().Be(false);
            createdDiscussion.IsPinned.Should().Be(false);
            createdDiscussion.Created.Should().BeCloseTo(DateTime.UtcNow, 1000);
            createdDiscussion.CreatedBy.Should().Be(loggedUser);

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
                SendAsync(command)).Should().Throw<Common.Exceptions.ValidationException>();
        }
    }
}
