using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Posts.Commands.PinPost;
using SK.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Posts.Commands
{
    using static Testing;
    public class PinPostTest : TestBase
    {
        [Test]
        public async Task ShouldPinPost()
        {
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var discussionToAdd = new Faker<Discussion>("en")
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(p => p.Description, f => f.Lorem.Sentences(sentenceCount: 2)).Generate();

            await AddAsync(discussionToAdd);

            var postToAdd = new Faker<Post>("en")
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Body, f => f.Lorem.Sentences(5))
                .RuleFor(p => p.DiscussionId, f => discussionToAdd.Id).Generate();

            await AddAsync(postToAdd);

            var pinCommand = new PinPostCommand() { Id = postToAdd.Id };

            //act
            await SendAsync(pinCommand);

            var pinnedPost= await FindByGuidAsync<Post>(postToAdd.Id);

            //assert
            pinnedPost.Should().NotBeNull();
            pinnedPost.IsPinned.Should().Be(true);
            pinnedPost.LastModified.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(1000));
            pinnedPost.LastModifiedBy.Should().Be(loggedUser);
        }

        [Test]
        public void ShouldThrowNotFoundException()
        {
            //arrange
            var pinCommand = new PinPostCommand() { Id = Guid.NewGuid() };

            //assert
            FluentActions.Invoking(() =>
                SendAsync(pinCommand)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task ShouldThrowRestExceptionWhenPostIsAlreadyPinned()
        {
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var discussionToAdd = new Faker<Discussion>("en")
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(p => p.Description, f => f.Lorem.Sentences(sentenceCount: 2)).Generate();

            await AddAsync(discussionToAdd);

            var postToAdd = new Faker<Post>("en")
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Body, f => f.Lorem.Sentences(5))
                .RuleFor(p => p.DiscussionId, f => discussionToAdd.Id).Generate();

            await AddAsync(postToAdd);

            var pinCommand = new PinPostCommand() { Id = postToAdd.Id };
            await SendAsync(pinCommand);

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(pinCommand)).Should().ThrowAsync<RestException>();
        }
    }
}
