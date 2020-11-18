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
            await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");
            for (int i = 0; i < 3; i++)
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

            var query = new ListArticleQuery(filter,path);

            //act
            var result = await SendAsync(query);

            //assert
            result.Data.Should().HaveCount(3);
        }
    }
}
