using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Discussions.Commands.CreateDiscussion;
using SK.Application.Discussions.Commands.PinDiscussion;
using SK.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Discussions.Commands
{
    using static Testing;
    public class PinDiscussionTest : TestBase
    {
        [Test]
        public async Task ShouldPinDiscussion()
        {
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var createdDiscussionId = await SendAsync(new Faker<CreateDiscussionCommand>("en")
                .RuleFor(d => d.Id, f => f.Random.Guid())
                .RuleFor(d => d.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(d => d.Description, f => f.Lorem.Sentences(sentenceCount: 2))
                .RuleFor(d => d.PostBody, f => f.Lorem.Paragraph()).Generate());

            var pinCommand = new PinDiscussionCommand() { Id = createdDiscussionId };

            //act
            await SendAsync(pinCommand);

            var pinnedDiscussion = await FindByGuidAsync<Discussion>(createdDiscussionId);

            //assert
            pinnedDiscussion.Should().NotBeNull();
            pinnedDiscussion.IsPinned.Should().Be(true);
            pinnedDiscussion.LastModified.Should().BeCloseTo(DateTime.UtcNow, 1000);
            pinnedDiscussion.LastModifiedBy.Should().Be(loggedUser);
        }

        [Test]
        public void ShouldThrowNotFoundException()
        {
            //arrange
            var pinCommand = new PinDiscussionCommand() { Id = Guid.NewGuid() };

            //assert
            FluentActions.Invoking(() =>
                SendAsync(pinCommand)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldThrowRestExceptionWhenDiscussionIsAlreadyPinned()
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

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(pinCommand)).Should().Throw<RestException>();
        }
    }
}
