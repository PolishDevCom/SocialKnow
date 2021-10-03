using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Posts.Commands.PinPost;
using SK.Application.Posts.Commands.UnpinPost;
using SK.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Posts.Commands
{
    using static Testing;

    public class UnpinPostTest : TestBase
    {
        [Test]
        public async Task ShouldUnpinPost()
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

            var unpinCommand = new UnpinPostCommand() { Id = postToAdd.Id };

            //act
            await SendAsync(unpinCommand);

            var unpinnedPost = await FindByGuidAsync<Post>(postToAdd.Id);

            //assert
            unpinnedPost.Should().NotBeNull();
            unpinnedPost.IsPinned.Should().Be(false);
            unpinnedPost.LastModified.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0,0,1));
            unpinnedPost.LastModifiedBy.Should().Be(loggedUser);
        }

        [Test]
        public void ShouldThrowNotFoundException()
        {
            //arrange
            var pinCommand = new UnpinPostCommand() { Id = Guid.NewGuid() };

            //assert
            FluentActions.Invoking(() =>
                SendAsync(pinCommand)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task ShouldThrowRestExceptionWhenPostIsAlreadyUnpinned()
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

            var unpinCommand = new UnpinPostCommand() { Id = postToAdd.Id };
            await SendAsync(unpinCommand);

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(unpinCommand)).Should().ThrowAsync<RestException>();
        }
    }
}