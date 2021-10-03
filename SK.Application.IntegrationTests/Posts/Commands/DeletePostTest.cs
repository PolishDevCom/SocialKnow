using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Posts.Commands.DeletePost;
using SK.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Posts.Commands
{
    using static Testing;
    public class DeletePostTest : TestBase
    {
        [Test]
        public void ShouldThrowNotFoundWhenPostIdIsIncorrect()
        {
            //arrange
            var deletepostCommand = new DeletePostCommand { Id = Guid.NewGuid() };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(deletepostCommand)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task ShouldDeletePost()
        {
            //arrange
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

            //act
            var result = await SendAsync(new DeletePostCommand
            {
                Id = postToAdd.Id
            });

            var actualPost = await FindByGuidAsync<Post>(postToAdd.Id);

            //assert
            actualPost.Should().BeNull();
        }
    }
}
