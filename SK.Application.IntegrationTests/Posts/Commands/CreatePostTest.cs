using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Posts.Commands.CreatePost;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Posts.Commands
{
    using static Testing;

    public class CreatePostTest : TestBase
    {
        [Test]
        public async Task ShouldCreatePostInDiscussion()
        {
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var discussionToAdd = new Faker<Discussion>("en")
                .RuleFor(d => d.Id, f => f.Random.Guid())
                .RuleFor(d => d.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(d => d.Description, f => f.Lorem.Sentences(sentenceCount: 2)).Generate();

            await AddAsync(discussionToAdd);

            var createPostCommand = new Faker<CreatePostCommand>("en")
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Body, f => f.Lorem.Sentences(5))
                .RuleFor(p => p.DiscussionId, f => discussionToAdd.Id).Generate();

            //act
            var createdPostId = await SendAsync(createPostCommand);
            var discussionPostsList = FindPostsByDiscussionGuidAsync(discussionToAdd.Id);

            //assert
            discussionPostsList.Last().Id.Should().Be(createPostCommand.Id);
            discussionPostsList.Last().Body.Should().Be(createPostCommand.Body);
            discussionPostsList.Last().IsPinned.Should().Be(false);
            discussionPostsList.Last().Created.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0,0,1));
            discussionPostsList.Last().CreatedBy.Should().Be(loggedUser);
        }

        [Test]
        public async Task ShouldThrowNotFoundWhenInvalidDiscussionId()
        {
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var discussionToAdd = new Faker<Discussion>("en")
                .RuleFor(d => d.Id, f => f.Random.Guid())
                .RuleFor(d => d.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(d => d.Description, f => f.Lorem.Sentences(sentenceCount: 2)).Generate();

            await AddAsync(discussionToAdd);

            var createPostCommand = new Faker<CreatePostCommand>("en")
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Body, f => f.Lorem.Sentences(5))
                .RuleFor(p => p.DiscussionId, f => f.Random.Guid()).Generate();

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(createPostCommand)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task ShouldThrowRestEexceptionWhenDiscussionIsClosed()
        {
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var discussionToAdd = new Faker<Discussion>("en")
                .RuleFor(d => d.Id, f => f.Random.Guid())
                .RuleFor(d => d.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(d => d.IsClosed, f => true)
                .RuleFor(d => d.Description, f => f.Lorem.Sentences(sentenceCount: 2)).Generate();

            await AddAsync(discussionToAdd);

            var createPostCommand = new Faker<CreatePostCommand>("en")
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Body, f => f.Lorem.Sentences(5))
                .RuleFor(p => p.DiscussionId, f => discussionToAdd.Id).Generate();

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(createPostCommand)).Should().ThrowAsync<RestException>();
        }

        private static IEnumerable<TestCaseData> ShouldThrowValidationExceptionDuringCreatingPostTestCases
        {
            get
            {
                yield return new TestCaseData(null, new Faker("en").Random.Guid()).SetName("PostBodyMissingTest");
                yield return new TestCaseData(new Faker("en").Lorem.Sentence(wordCount: 3), null).SetName("PostDiscussionIdMissingTest");
            }
        }

        [Test]
        [TestCaseSource(nameof(ShouldThrowValidationExceptionDuringCreatingPostTestCases))]
        public void ShouldThrowValidationExceptionDuringDiscussionCreation(string testBody, Guid testDiscussionId)
        {
            //arrange
            var createPostCommand = new CreatePostCommand()
            {
                Id = Guid.NewGuid(),
                Body = testBody,
                DiscussionId = testDiscussionId
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(createPostCommand)).Should().ThrowAsync<Common.Exceptions.ValidationException>();
        }
    }
}