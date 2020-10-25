using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Discussions.Commands.CreateDiscussion;
using SK.Application.Discussions.Commands.DeleteDiscussion;
using SK.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Discussions.Commands
{
    using static Testing;
    public class DeleteDiscussionTest : TestBase
    {
        [Test]
        public void ShouldRequireValidDiscussionId()
        {
            //arrange
            var deleteDiscussioncommand = new DeleteDiscussionCommand { Id = Guid.NewGuid() };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(deleteDiscussioncommand)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldDeleteDiscussion()
        {
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var createDiscussionCommand = new Faker<CreateDiscussionCommand>("en")
                .RuleFor(d => d.Id, f => f.Random.Guid())
                .RuleFor(d => d.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(d => d.Description, f => f.Lorem.Sentences(sentenceCount: 2))
                .RuleFor(d => d.PostBody, f => f.Lorem.Paragraph()).Generate();

            var createdDiscussionId = await SendAsync(createDiscussionCommand);

            //act
            var result = await SendAsync(new DeleteDiscussionCommand
            {
                Id = createdDiscussionId
            });

            var actualDiscussion = await FindByGuidAsync<Discussion>(createdDiscussionId);
            var actualDiscussionPosts = FindPostsByDiscussionGuidAsync(createdDiscussionId);

            //assert
            actualDiscussion.Should().BeNull();
            actualDiscussionPosts.Should().BeNullOrEmpty();
        }
    }
}
