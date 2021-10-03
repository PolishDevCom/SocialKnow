using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Articles.Queries.DetailsArticle;
using SK.Application.Common.Exceptions;
using SK.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Articles.Queries
{
    using static Testing;

    public class DetailsArticleTest : TestBase
    {
        [Test]
        public async Task ShouldReturnDetailsOfObject()
        {
            //arrange
            Guid expectedId = Guid.NewGuid();
            await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");
            for (int i = 0; i < 2; i++)
            {
                await AddAsync(new Faker<Article>("en")
                    .RuleFor(a => a.Id, f => f.Random.Guid())
                    .RuleFor(a => a.Title, f => f.Lorem.Sentence())
                    .RuleFor(a => a.Abstract, f => f.Lorem.Paragraph())
                    .RuleFor(a => a.Image, f => null)
                    .RuleFor(a => a.Content, f => f.Lorem.Paragraphs(5))
                    .Generate());
            }

            var detailedArticle = new Faker<Article>("en")
                    .RuleFor(a => a.Id, f => expectedId)
                    .RuleFor(a => a.Title, f => f.Lorem.Sentence())
                    .RuleFor(a => a.Abstract, f => f.Lorem.Paragraph())
                    .RuleFor(a => a.Image, f => null)
                    .RuleFor(a => a.Content, f => f.Lorem.Paragraphs(5))
                    .Generate();

            await AddAsync(detailedArticle);

            var query = new DetailsArticleQuery() { Id = expectedId };

            //act
            var result = await SendAsync(query);

            //assert
            result.Id.Should().Be(expectedId);
            result.Title.Should().Be(detailedArticle.Title);
            result.Abstract.Should().Be(detailedArticle.Abstract);
            result.Image.Should().BeNull();
            result.Content.Should().Be(detailedArticle.Content);
            result.CreatedBy.Should().Be("scott101@localhost");
            result.Created.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0,0,1));
        }

        [Test]
        public async Task ShouldThrowNotFoundExceptionIfArticleDoesNotExist()
        {
            //arrange
            Guid notExistingId = Guid.NewGuid();

            await AddAsync(new Faker<Article>("en")
                    .RuleFor(a => a.Id, f => f.Random.Guid())
                    .RuleFor(a => a.Title, f => f.Lorem.Sentence())
                    .RuleFor(a => a.Abstract, f => f.Lorem.Paragraph())
                    .RuleFor(a => a.Image, f => null)
                    .RuleFor(a => a.Content, f => f.Lorem.Paragraphs(5))
                    .Generate());

            var query = new DetailsArticleQuery() { Id = notExistingId };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(query)).Should().ThrowAsync<NotFoundException>();
        }
    }
}