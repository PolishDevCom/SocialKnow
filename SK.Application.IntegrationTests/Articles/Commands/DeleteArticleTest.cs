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
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldDeleteArticle()
        {
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");
            var command = new CreateArticleCommand()
            {
                Id = Guid.NewGuid(),
                Title = "Article Title",
                Abstract = "Article Abstract",
                Image = null,
                Content = "Article Content"
            };
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
