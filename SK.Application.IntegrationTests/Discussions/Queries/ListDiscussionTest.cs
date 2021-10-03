using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Models;
using SK.Application.Discussions.Commands.PinDiscussion;
using SK.Application.Discussions.Queries.ListDiscussion;
using SK.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Discussions.Queries
{
    using static Testing;

    public class ListDiscussionTest : TestBase
    {
        [Test]
        public async Task ShouldReturnAllDiscussionsAsAList()
        {
            //arrange
            int numberOfDiscussions = 10;
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            for (int i = 0; i < numberOfDiscussions; i++)
            {
                var discussionToAdd = new Faker<Discussion>("en")
                .RuleFor(d => d.Id, f => f.Random.Guid())
                .RuleFor(d => d.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(d => d.Description, f => f.Lorem.Sentences(sentenceCount: 2)).Generate();

                await AddAsync(discussionToAdd);
            }

            var filter = new PaginationFilter();
            var path = String.Empty;
            var listQuery = new ListDiscussionQuery(filter, path);

            //act
            var result = await SendAsync(listQuery);

            //assert
            result.Data.Should().HaveCount(numberOfDiscussions);
        }

        [Test]
        public async Task ShouldReturnAllDiscussionsAsAListWithCorrectOrdering()
        {
            //arrange
            int numberOfDiscussions = 10;
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            for (int i = 0; i < numberOfDiscussions; i++)
            {
                var discussionToAdd = new Faker<Discussion>("en")
                .RuleFor(d => d.Id, f => f.Random.Guid())
                .RuleFor(d => d.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(d => d.Description, f => f.Lorem.Sentences(sentenceCount: 2)).Generate();

                await AddAsync(discussionToAdd);
            }

            var filter = new PaginationFilter();
            var path = String.Empty;
            var listQuery = new ListDiscussionQuery(filter, path);

            //act
            var result = await SendAsync(listQuery);

            //assert
            result.Data.Should().BeInDescendingOrder(d => d.Created);
        }

        [Test]
        public async Task ShouldReturnAllDiscussionsAsAListWithPinnedOnBegining()
        {
            //arrange
            int numberOfDiscussions = 10;
            var filter = new PaginationFilter();
            var path = String.Empty;
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            for (int i = 0; i < numberOfDiscussions; i++)
            {
                var discussionToAdd = new Faker<Discussion>("en")
                .RuleFor(d => d.Id, f => f.Random.Guid())
                .RuleFor(d => d.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(d => d.Description, f => f.Lorem.Sentences(sentenceCount: 2)).Generate();

                await AddAsync(discussionToAdd);
            }

            var result = await SendAsync(new ListDiscussionQuery(filter, path));
            var discussionToPinId = result.Data.Last().Id;

            await SendAsync(new PinDiscussionCommand() { Id = discussionToPinId });

            //act
            var actResult = await SendAsync(new ListDiscussionQuery(filter, path));

            //assert
            actResult.Data.First().Id.Should().Be(discussionToPinId);
        }

        [Test]
        public async Task ShouldReturnAllDiscussionsAsAListWithPinnedOnBeginingInDescendingOrder()
        {
            //arrange
            int numberOfDiscussions = 10;
            var filter = new PaginationFilter();
            var path = String.Empty;
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            for (int i = 0; i < numberOfDiscussions; i++)
            {
                var discussionToAdd = new Faker<Discussion>("en")
                .RuleFor(d => d.Id, f => f.Random.Guid())
                .RuleFor(d => d.Title, f => f.Lorem.Sentence(wordCount: 3))
                .RuleFor(d => d.Description, f => f.Lorem.Sentences(sentenceCount: 2)).Generate();

                await AddAsync(discussionToAdd);
            }

            var result = await SendAsync(new ListDiscussionQuery(filter, path));
            var discussionToPin1Id = result.Data[3].Id;
            var discussionToPin2Id = result.Data[6].Id;
            var discussionToPin3Id = result.Data.Last().Id;

            await SendAsync(new PinDiscussionCommand() { Id = discussionToPin1Id });
            await SendAsync(new PinDiscussionCommand() { Id = discussionToPin2Id });
            await SendAsync(new PinDiscussionCommand() { Id = discussionToPin3Id });

            //act
            var actResult = await SendAsync(new ListDiscussionQuery(filter, path));

            //assert
            actResult.Data[0].Id.Should().Be(discussionToPin1Id);
            actResult.Data[1].Id.Should().Be(discussionToPin2Id);
            actResult.Data[2].Id.Should().Be(discussionToPin3Id);
        }
    }
}