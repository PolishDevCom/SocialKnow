using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Articles.Commands.CreateArticle;
using SK.Application.Articles.Commands.DeleteArticle;
using SK.Application.Common.Exceptions;
using SK.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Articles.Commands
{
    using static Testing;

    public class DeleteTestValueTest : TestBase
    {
        [Test]
        public void ShouldRequireValidArticleId()
        {
            //arrange
            var command = new DeleteArticleCommand { Id = Guid.NewGuid() };

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task ShouldDeleteArticle()
        {
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var command = new Faker<CreateArticleCommand>("en")
                .RuleFor(a => a.Id, f => f.Random.Guid())
                .RuleFor(a => a.Title, f => f.Lorem.Sentence())
                .RuleFor(a => a.Abstract, f => f.Lorem.Paragraph())
                .RuleFor(a => a.Image, f => null)
                .RuleFor(a => a.Content, f => f.Lorem.Paragraphs(5))
                .Generate();

            var createdArticleId = await SendAsync(command);

            //act
            var result = await SendAsync(new DeleteArticleCommand
            {
                Id = createdArticleId
            });

            var actualEvent = await FindByGuidAsync<Article>(createdArticleId);

            //assert
            actualEvent.Should().BeNull();
        }
    }
}