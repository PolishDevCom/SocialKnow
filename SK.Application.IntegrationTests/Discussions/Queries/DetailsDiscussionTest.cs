using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Models;
using SK.Application.Discussions.Queries.DetailsDiscussion;
using SK.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Discussions.Queries
{
    using static Testing;

    public class DetailsDiscussionTest : TestBase
    {
        [Test]
        public async Task ShouldReturnDetailsOfDiscussion()
        {
            //arrange
            var filter = new PaginationFilter();
            var path = String.Empty;
            int numberOfPosts = 10;
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var discussionToAdd1 = new Faker<Discussion>("en")
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(p => p.Description, f => f.Lorem.Sentences(sentenceCount: 2)).Generate();

            var discussionToAdd2 = new Faker<Discussion>("en")
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(p => p.Description, f => f.Lorem.Sentences(sentenceCount: 2)).Generate();

            await AddAsync(discussionToAdd1);
            await AddAsync(discussionToAdd2);

            for (int i = 0; i < numberOfPosts; i++)
            {
                var postToAdd = new Faker<Post>("en")
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Body, f => f.Lorem.Sentences(5))
                .RuleFor(p => p.DiscussionId, f => discussionToAdd2.Id).Generate();

                await AddAsync(postToAdd);
            }

            var detailsDiscussionQuery = new DetailsDiscussionQuery(discussionToAdd2.Id, filter, path);

            //act
            var result = await SendAsync(detailsDiscussionQuery);

            //assert
            result.Id.Should().Be(discussionToAdd2.Id);
            result.IsClosed.Should().BeFalse();
            result.IsPinned.Should().BeFalse();
            result.Description.Should().Be(discussionToAdd2.Description);
            result.Title.Should().Be(discussionToAdd2.Title);
            result.NumberOfPosts.Should().Be(numberOfPosts);
            result.Posts.Data.Should().HaveCount(numberOfPosts > filter.PageSize? filter.PageSize : numberOfPosts);
        }

        [Test]
        public async Task ShouldReturnDetailsOfDiscussionWithPostsInCorrectOrder()
        {
            //arrange
            var filter = new PaginationFilter();
            var path = String.Empty;
            int numberOfPosts = 10;
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var discussionToAdd1 = new Faker<Discussion>("en")
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(p => p.Description, f => f.Lorem.Sentences(sentenceCount: 2)).Generate();

            var discussionToAdd2 = new Faker<Discussion>("en")
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(p => p.Description, f => f.Lorem.Sentences(sentenceCount: 2)).Generate();

            await AddAsync(discussionToAdd1);
            await AddAsync(discussionToAdd2);

            for (int i = 0; i < numberOfPosts; i++)
            {
                var postToAdd = new Faker<Post>("en")
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Body, f => f.Lorem.Sentences(5))
                .RuleFor(p => p.DiscussionId, f => discussionToAdd2.Id).Generate();

                await AddAsync(postToAdd);
            }

            var detailsDiscussionQuery = new DetailsDiscussionQuery(discussionToAdd2.Id, filter, path);

            //act
            var result = await SendAsync(detailsDiscussionQuery);

            //assert
            result.NumberOfPosts.Should().Be(numberOfPosts);
            result.Posts.Data.Should().HaveCount(numberOfPosts > filter.PageSize ? filter.PageSize : numberOfPosts);
            result.Posts.Data.Should().BeInDescendingOrder(p => p.Created);
        }

        [Test]
        public async Task ShouldReturnDetailsOfDiscussionWithPostsWherePinnedIsFirst()
        {
            //arrange
            var filter = new PaginationFilter();
            var path = String.Empty;
            int numberOfPosts = 5;
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var discussionToAdd1 = new Faker<Discussion>("en")
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(p => p.Description, f => f.Lorem.Sentences(sentenceCount: 2)).Generate();

            var discussionToAdd2 = new Faker<Discussion>("en")
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(p => p.Description, f => f.Lorem.Sentences(sentenceCount: 2)).Generate();

            await AddAsync(discussionToAdd1);
            await AddAsync(discussionToAdd2);

            for (int i = 0; i < numberOfPosts; i++)
            {
                var postToAdd = new Faker<Post>("en")
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Body, f => f.Lorem.Sentences(5))
                .RuleFor(p => p.DiscussionId, f => discussionToAdd2.Id).Generate();

                await AddAsync(postToAdd);
            }

            var postPinned = new Faker<Post>("en")
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Body, f => f.Lorem.Sentences(5))
                .RuleFor(p => p.DiscussionId, f => discussionToAdd2.Id)
                .RuleFor(p => p.IsPinned, f => true).Generate();

            await AddAsync(postPinned);

            var detailsDiscussionQuery = new DetailsDiscussionQuery(discussionToAdd2.Id, filter, path);

            //act
            var result = await SendAsync(detailsDiscussionQuery);

            //assert
            result.NumberOfPosts.Should().Be(numberOfPosts+1);
            result.Posts.Data.Should().HaveCount(numberOfPosts + 1 > filter.PageSize ? filter.PageSize : numberOfPosts + 1);
            result.Posts.Data.First().Id.Should().Be(postPinned.Id);
            result.Posts.Data.Skip(1).Should().BeInDescendingOrder(p => p.Created);
        }

        [Test]
        public async Task ShouldReturnDetailsOfDiscussionWithPostsAndPinnedPostsInCorrectOrder()
        {
            //arrange
            int numberOfPosts = 5;
            var filter = new PaginationFilter();
            var path = String.Empty;
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var discussionToAdd1 = new Faker<Discussion>("en")
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(p => p.Description, f => f.Lorem.Sentences(sentenceCount: 2)).Generate();

            var discussionToAdd2 = new Faker<Discussion>("en")
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(p => p.Description, f => f.Lorem.Sentences(sentenceCount: 2)).Generate();

            await AddAsync(discussionToAdd1);
            await AddAsync(discussionToAdd2);

            for (int i = 0; i < numberOfPosts; i++)
            {
                var postToAdd = new Faker<Post>("en")
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Body, f => f.Lorem.Sentences(5))
                .RuleFor(p => p.DiscussionId, f => discussionToAdd2.Id).Generate();

                await AddAsync(postToAdd);
            }

            var postPinned1 = new Faker<Post>("en")
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Body, f => f.Lorem.Sentences(5))
                .RuleFor(p => p.DiscussionId, f => discussionToAdd2.Id)
                .RuleFor(p => p.IsPinned, f => true).Generate();

            await AddAsync(postPinned1);

            var postPinned2 = new Faker<Post>("en")
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Body, f => f.Lorem.Sentences(5))
                .RuleFor(p => p.DiscussionId, f => discussionToAdd2.Id)
                .RuleFor(p => p.IsPinned, f => true).Generate();

            await AddAsync(postPinned2);

            var detailsDiscussionQuery = new DetailsDiscussionQuery(discussionToAdd2.Id, filter, path);

            //act
            var result = await SendAsync(detailsDiscussionQuery);

            //assert
            result.NumberOfPosts.Should().Be(numberOfPosts + 2);
            result.Posts.Data.Should().HaveCount(numberOfPosts + 2 > filter.PageSize ? filter.PageSize : numberOfPosts + 2);
            result.Posts.Data.Take(2).Should().NotContain(p => p.IsPinned == false);
            result.Posts.Data.Take(2).Should().BeInDescendingOrder(p => p.Created);
            result.Posts.Data.Skip(2).Should().NotContain(p => p.IsPinned == true);
            result.Posts.Data.Skip(2).Should().BeInDescendingOrder(p => p.Created);
        }

        [Test]
        public async Task ShouldThrowNotFoundExceptionIfEventDoesNotExist()
        {
            //arrange
            var filter = new PaginationFilter();
            var path = String.Empty;
            Guid notExistingId = Guid.NewGuid();

            await AddAsync(new Faker<Discussion>("en")
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(p => p.Description, f => f.Lorem.Sentences(sentenceCount: 2)).Generate());

            var query = new DetailsDiscussionQuery(notExistingId, filter, path);

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(query)).Should().ThrowAsync<NotFoundException>();
        }
    }
}
