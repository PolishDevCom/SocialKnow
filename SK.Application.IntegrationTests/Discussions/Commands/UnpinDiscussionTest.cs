using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Discussions.Commands.CreateDiscussion;
using SK.Application.Discussions.Commands.PinDiscussion;
using SK.Application.Discussions.Commands.UnpinDiscussion;
using SK.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Discussions.Commands
{
    using static Testing;

    public class UnpinDiscussionTest : TestBase
    {
        [Test]
        public async Task ShouldUnpinDiscussion()
        {
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var createdDiscussionId = await SendAsync(new Faker<CreateDiscussionCommand>("en")
                .RuleFor(d => d.Id, f => f.Random.Guid())
                .RuleFor(d => d.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(d => d.Description, f => f.Lorem.Sentences(sentenceCount: 2))
                .RuleFor(d => d.PostBody, f => f.Lorem.Paragraph()).Generate());

            var pinCommand = new PinDiscussionCommand() { Id = createdDiscussionId };
            await SendAsync(pinCommand);

            var unpinCommand = new UnpinDiscussionCommand() { Id = createdDiscussionId };

            //act
            await SendAsync(unpinCommand);

            var unpinnedDiscussion = await FindByGuidAsync<Discussion>(createdDiscussionId);

            //assert
            unpinnedDiscussion.Should().NotBeNull();
            unpinnedDiscussion.IsPinned.Should().Be(false);
            unpinnedDiscussion.LastModified.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0,0,1));
            unpinnedDiscussion.LastModifiedBy.Should().Be(loggedUser);
        }

        [Test]
        public void ShouldThrowNotFoundException()
        {
            //arrange
            var unpinCommand = new UnpinDiscussionCommand() { Id = Guid.NewGuid() };

            //assert
            FluentActions.Invoking(() =>
                SendAsync(unpinCommand)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task ShouldThrowRestExceptionWhenDiscussionIsAlreadyUnpinned()
        {
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var createdDiscussionId = await SendAsync(new Faker<CreateDiscussionCommand>("en")
                .RuleFor(d => d.Id, f => f.Random.Guid())
                .RuleFor(d => d.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(d => d.Description, f => f.Lorem.Sentences(sentenceCount: 2))
                .RuleFor(d => d.PostBody, f => f.Lorem.Paragraph()).Generate());

            var pinCommand = new PinDiscussionCommand() { Id = createdDiscussionId };
            await SendAsync(pinCommand);

            var unpinCommand = new UnpinDiscussionCommand() { Id = createdDiscussionId };
            await SendAsync(unpinCommand);

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(unpinCommand)).Should().ThrowAsync<RestException>();
        }
    }
}