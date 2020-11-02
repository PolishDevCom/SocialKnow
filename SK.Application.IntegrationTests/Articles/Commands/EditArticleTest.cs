using Bogus;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SK.Application.Articles.Commands.CreateArticle;
using SK.Application.Articles.Commands.EditArticle;
using SK.Application.Articles.Queries;
using SK.Application.Common.Exceptions;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
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
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var articleToCreate = new Faker<ArticleDto>("en")
                .RuleFor(a => a.Id, f => f.Random.Guid())
                .RuleFor(a => a.Title, f => f.Lorem.Sentence())
                .RuleFor(a => a.Abstract, f => f.Lorem.Paragraph())
                .RuleFor(a => a.Image, f => null)
                .RuleFor(a => a.Content, f => f.Lorem.Paragraphs(5))
                .Generate();

            var articleId = await SendAsync(new CreateArticleCommand(articleToCreate));

            var articleToModify = new Faker<ArticleDto>("en")
                .RuleFor(a => a.Id, f => articleToCreate.Id)
                .RuleFor(a => a.Title, f => f.Lorem.Sentence())
                .RuleFor(a => a.Abstract, f => f.Lorem.Paragraph())
                .RuleFor(a => a.Image, f => null)
                .RuleFor(a => a.Content, f => f.Lorem.Paragraphs(5))
                .Generate();

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

        private static IEnumerable<TestCaseData> ShouldRequireFieldAndThrowValidationExceptionDuringUpdateArticleTestCases
        {
            get
            {
                yield return new TestCaseData(null, new Faker("en").Lorem.Paragraph(), new Faker("en").Lorem.Paragraphs(5))
                    .SetName("ArticleTitleMissingTest");
                yield return new TestCaseData(new Faker("en").Lorem.Sentence(), null, new Faker("en").Lorem.Paragraphs(5))
                    .SetName("ArticleAbstactMissingTest");
                yield return new TestCaseData(new Faker("en").Lorem.Sentence(), new Faker("en").Lorem.Paragraph(), null)
                    .SetName("ArticleContentMissingTest");
            }
        }

        [Test]
        [TestCaseSource(nameof(ShouldRequireFieldAndThrowValidationExceptionDuringUpdateArticleTestCases))]
        public void ShouldRequireFieldAndThrowValidationExceptionDuringArticleEditing(string testTitle, string testAbstract, string testContent)
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
                SendAsync(command)).Should().Throw<Common.Exceptions.ValidationException>();
        }

        [Test]
        public void ShouldRequireValidArticleIdAndThrowValidationException()
        {
            //arrange
            var command = new Faker<EditArticleCommand>("en")
                .RuleFor(a => a.Id, f => f.Random.Guid())
                .RuleFor(a => a.Title, f => f.Lorem.Sentence())
                .RuleFor(a => a.Abstract, f => f.Lorem.Paragraph())
                .RuleFor(a => a.Image, f => null)
                .RuleFor(a => a.Content, f => f.Lorem.Paragraphs(5))
                .Generate();

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }
    }
}
