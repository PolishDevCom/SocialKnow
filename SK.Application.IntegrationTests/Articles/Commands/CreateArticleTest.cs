using Bogus;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SK.Application.Articles.Commands.CreateArticle;
using SK.Application.Articles.Queries;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Articles.Commands
{
    using static Testing;

    public class CreateArticleTest : TestBase
    {
        [Test]
        public async Task ShouldCreateArticle()
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

            var command = new CreateArticleCommand(articleToCreate);

            //act
            var createdId = await SendAsync(command);
            var createdArticle = await FindByGuidAsync<Article>(createdId);

            //assert
            createdArticle.Id.Should().Be(createdId);
            createdArticle.Title.Should().Be(articleToCreate.Title);
            createdArticle.Abstract.Should().Be(articleToCreate.Abstract);
            createdArticle.Image.Should().BeNull();
            createdArticle.Content.Should().Be(articleToCreate.Content);
            createdArticle.CreatedBy.Should().Be(loggedUser);
            createdArticle.Created.Should().BeCloseTo(DateTime.UtcNow, 1000);
        }

        private static IEnumerable<TestCaseData> ShouldRequireFieldAndThrowValidationExceptionDuringCreatingArticleTestCases
        {
            get
            {
                yield return new TestCaseData(null, new Faker("en").Lorem.Paragraph(), new Faker("en").Lorem.Paragraphs(5)).SetName("ArticleTitleMissingTest");
                yield return new TestCaseData(new Faker("en").Lorem.Sentence(), null, new Faker("en").Lorem.Paragraphs(5)).SetName("ArticleAbstactMissingTest");
                yield return new TestCaseData(new Faker("en").Lorem.Sentence(), new Faker("en").Lorem.Paragraph(), null).SetName("ArticleContentMissingTest");
            }
        }

        [Test]
        [TestCaseSource(nameof(ShouldRequireFieldAndThrowValidationExceptionDuringCreatingArticleTestCases))]
        public void ShouldRequireFieldAndThrowValidationExceptionDuringArticleCreating(string testTitle, string testAbstract, string testContent)
        {
            //arrange
            var command = new CreateArticleCommand()
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
    }
}
