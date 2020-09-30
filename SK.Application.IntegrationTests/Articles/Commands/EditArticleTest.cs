using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SK.Application.Articles.Commands.CreateArticle;
using SK.Application.Articles.Commands.EditArticle;
using SK.Application.Articles.Queries;
using SK.Application.Common.Exceptions;
using SK.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Articles.Commands
{
    using static Testing;

    public class EditArticleTest : TestBase
    {
        [Test]
        public async Task ShouldUpdateArticle()
        {
            //arrange
            var articleToCreate = new ArticleDto
            {
                Id = Guid.NewGuid(),
                Title = "Article Title",
                Abstract = "Article Abstract",
                Image = null,
                Content = "Article Content"
            };
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");
            var articleId = await SendAsync(new CreateArticleCommand(articleToCreate));

            var articleToModify = new ArticleDto
            {
                Id = articleToCreate.Id,
                Title = "Modified Article Title",
                Abstract = " ModifiedArticle Abstract",
                Image = null,
                Content = "Modified Article Content"
            };

            //act
            var command = new EditArticleCommand(articleToModify);
            await SendAsync(command);
            var modifiedArticle= await FindByGuidAsync<Article>(articleId);

            //assert
            modifiedArticle.Id.Should().Be(articleToModify.Id);
            modifiedArticle.Title.Should().Be(articleToModify.Title);
            modifiedArticle.Abstract.Should().Be(articleToModify.Abstract);
            modifiedArticle.Image.Should().BeNull();
            modifiedArticle.Content.Should().Be(articleToModify.Content);
            modifiedArticle.LastModifiedBy.Should().Be(loggedUser);
            modifiedArticle.LastModified.Should().BeCloseTo(DateTime.UtcNow, 1000);
        }

        [Test]
        [TestCase(null, "Article Abstract", "Article Content")]
        [TestCase("Article Title", null, "Article Content")]
        [TestCase("Article Title", "Article Abstract", null)]
        public void ShouldRequireFieldAndThrowValidationException(string testTitle, string testAbstract, string testContent)
        {
            //arrange
            var command = new EditArticleCommand()
            {
                Id = Guid.NewGuid(),
                Title = testTitle,
                Abstract = testAbstract,
                Image = null,
                Content = testContent
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public void ShouldRequireValidArticleIdAndThrowValidationException()
        {
            //arrange
            var command = new EditArticleCommand()
            {
                Id = Guid.NewGuid(),
                Title = "Article Title",
                Abstract = "Article Abstract",
                Image = null,
                Content = "Article Content"
            };

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }
    }
}
