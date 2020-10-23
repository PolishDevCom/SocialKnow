using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Discussions.Commands.CloseDiscussion;
using SK.Application.Discussions.Commands.CreateDiscussion;
using SK.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Discussions.Commands
{
    using static Testing;
    public class CloseDiscussionTest : TestBase
    {
        [Test]
        public async Task ShouldCloseDiscussion()
        {
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var createdDiscussionId = await SendAsync(new Faker<CreateDiscussionCommand>("en")
                .RuleFor(d => d.Id, f => f.Random.Guid())
                .RuleFor(d => d.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(d => d.Description, f => f.Lorem.Sentences(sentenceCount: 2))
                .RuleFor(d => d.PostBody, f => f.Lorem.Paragraph()).Generate());

            var closeCommand = new CloseDiscussionCommand() { Id = createdDiscussionId };

            //act
            await SendAsync(closeCommand);

            var closedDiscussion = await FindByGuidAsync<Discussion>(createdDiscussionId);

            //assert
            closedDiscussion.Should().NotBeNull();
            closedDiscussion.IsClosed.Should().Be(true);
            closedDiscussion.LastModified.Should().BeCloseTo(DateTime.UtcNow, 1000);
            closedDiscussion.LastModifiedBy.Should().Be(loggedUser);
        }

        [Test]
        public void ShouldThrowNotFoundException()
        {
            //arrange
            var closeCommand = new CloseDiscussionCommand() { Id = Guid.NewGuid() };

            //assert
            FluentActions.Invoking(() =>
                SendAsync(closeCommand)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldThrowRestExceptionWhenDiscussionIsAlreadyClosed()
        {
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var createdDiscussionId = await SendAsync(new Faker<CreateDiscussionCommand>("en")
                .RuleFor(d => d.Id, f => f.Random.Guid())
                .RuleFor(d => d.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(d => d.Description, f => f.Lorem.Sentences(sentenceCount: 2))
                .RuleFor(d => d.PostBody, f => f.Lorem.Paragraph()).Generate());

            var closeCommand = new CloseDiscussionCommand() { Id = createdDiscussionId };
            await SendAsync(closeCommand);

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(closeCommand)).Should().Throw<RestException>();
        }
    }
}
