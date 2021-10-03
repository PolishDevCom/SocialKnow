using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Articles.Queries.ListArticle;
using SK.Application.Common.Models;
using SK.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Articles.Queries
{
    using static Testing;

    public class ListArticleTests : TestBase
    {
        [Test]
        public async Task ShouldReturnAllArticlesAsAList()
        {
            //arrange
            int numberOfArticles = 3;
            await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");
            for (int i = 0; i < numberOfArticles; i++)
            {
                await AddAsync(new Faker<Article>("en")
                    .RuleFor(a => a.Id, f => f.Random.Guid())
                    .RuleFor(a => a.Title, f => f.Lorem.Sentence())
                    .RuleFor(a => a.Abstract, f => f.Lorem.Paragraph())
                    .RuleFor(a => a.Image, f => null)
                    .RuleFor(a => a.Content, f => f.Lorem.Paragraphs(5))
                    .Generate());
            }

            var filter = new PaginationFilter();
            var path = String.Empty;

            var query = new ListArticleQuery(filter, path);

            //act
            var result = await SendAsync(query);

            //assert
            result.FirstPage.Should().BeNull();
            result.LastPage.Should().BeNull();
            result.NextPage.Should().BeNull();
            result.PreviousPage.Should().BeNull();

            result.Succeeded.Should().BeTrue();
            result.Errors.Should().BeNull();
            result.TotalRecords.Should().Be(numberOfArticles);

            result.Data.Should().HaveCount(numberOfArticles > filter.PageSize ? filter.PageSize : numberOfArticles);
        }
    }
}