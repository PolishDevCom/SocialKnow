using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Posts.Commands.EditPost;
using SK.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Posts.Commands
{
    using static Testing;
    public class EditPostTest : TestBase
    {
        [Test]
        public async Task ShouldUpdatePost()
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

            var editCommand = new Faker<EditPostCommand>("en")
                .RuleFor(p => p.Id, f => postToAdd.Id)
                .RuleFor(p => p.Body, f => f.Lorem.Sentences(5)).Generate();

            //act
            await SendAsync(editCommand);

            var editedPost = await FindByGuidAsync<Post>(postToAdd.Id);

            //assert
            editedPost.Should().NotBeNull();
            editedPost.Id.Should().Be(editCommand.Id);
            editedPost.Body.Should().Be(editCommand.Body);
            editedPost.IsPinned.Should().Be(false);
            editedPost.LastModified.Should().BeCloseTo(DateTime.UtcNow, 1000);
            editedPost.LastModifiedBy.Should().Be(loggedUser);
        }

        [Test]
        public void ShouldThrowNotFoundException()
        {
            //arrange
            var editCommand = new Faker<EditPostCommand>("en")
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Body, f => f.Lorem.Sentences(5)).Generate();

            //assert
            FluentActions.Invoking(() =>
                SendAsync(editCommand)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldThrowValidationExceptionDuringDiscussionEdition()
        {
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

            var editCommand = new Faker<EditPostCommand>("en")
                .RuleFor(p => p.Id, f => postToAdd.Id)
                .RuleFor(p => p.Body, f => null).Generate();

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(editCommand)).Should().Throw<Common.Exceptions.ValidationException>();
        }
    }
}
