using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Discussions.Commands.CloseDiscussion;
using SK.Application.Discussions.Commands.CreateDiscussion;
using SK.Application.Discussions.Commands.OpenDiscussion;
using SK.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Discussions.Commands
{
    using static Testing;

    public class OpenDiscussionTest : TestBase
    {
        [Test]
        public async Task ShouldOpenDiscussion()
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

            var openCommand = new OpenDiscussionCommand() { Id = createdDiscussionId };

            //act
            await SendAsync(openCommand);

            var openedDiscussion = await FindByGuidAsync<Discussion>(createdDiscussionId);

            //assert
            openedDiscussion.Should().NotBeNull();
            openedDiscussion.IsClosed.Should().Be(false);
            openedDiscussion.LastModified.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0,0,1));
            openedDiscussion.LastModifiedBy.Should().Be(loggedUser);
        }

        [Test]
        public void ShouldThrowNotFoundException()
        {
            //arrange
            var openCommand = new OpenDiscussionCommand() { Id = Guid.NewGuid() };

            //assert
            FluentActions.Invoking(() =>
                SendAsync(openCommand)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task ShouldThrowRestExceptionWhenDiscussionIsAlreadyOpened()
        {
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var createdDiscussionId = await SendAsync(new Faker<CreateDiscussionCommand>("en")
                .RuleFor(d => d.Id, f => f.Random.Guid())
                .RuleFor(d => d.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(d => d.Description, f => f.Lorem.Sentences(sentenceCount: 2))
                .RuleFor(d => d.PostBody, f => f.Lorem.Paragraph()).Generate());

            var openCommand = new OpenDiscussionCommand() { Id = createdDiscussionId };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(openCommand)).Should().ThrowAsync<RestException>();
        }
    }
}