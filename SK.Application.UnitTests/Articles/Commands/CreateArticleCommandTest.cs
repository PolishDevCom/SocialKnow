using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.Articles.Commands;
using SK.Application.Articles.Commands.CreateArticle;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Articles;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Articles.Commands.CreateArticle
{
    public class CreateArticleCommandTest
    {
        private readonly Mock<DbSet<Article>> dbSetArticle;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IStringLocalizer<ArticlesResource>> stringLocalizer;
        private readonly Mock<IMapper> mapper;

        public CreateArticleCommandTest()
        {
            dbSetArticle = new Mock<DbSet<Article>>();
            context = new Mock<IApplicationDbContext>();
            stringLocalizer = new Mock<IStringLocalizer<ArticlesResource>>();
            mapper = new Mock<IMapper>();
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            //Arrange
            var id = new Guid();
            var articleDto = new ArticleCreateOrEditDto { Id = id };
            var article = new Article { Id = id };

            context.Setup(x => x.Articles).Returns(dbSetArticle.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            CreateArticleCommandHandler createArticleCommandHandler = new CreateArticleCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            CreateArticleCommand createArticleCommand = new CreateArticleCommand(articleDto);

            mapper.Setup(x => x.Map<Article>(createArticleCommand)).Returns(article);

            //Act
            var result = await createArticleCommandHandler.Handle(createArticleCommand, new CancellationToken());

            //Assert
            result.Should().Be(id);
        }

        [Test]
        public void ShouldNotCallHandleIfNotSavedChanges()
        {
            //Arrange
            context.Setup(x => x.Articles).Returns(dbSetArticle.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            CreateArticleCommandHandler createArticleCommandHandler = new CreateArticleCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            CreateArticleCommand createArticleCommand = new CreateArticleCommand();

            mapper.Setup(x => x.Map<Article>(createArticleCommand)).Returns(new Article());

            //Act
            Func<Task> act = async () => await createArticleCommandHandler.Handle(createArticleCommand, new CancellationToken());

            //Assert
            act.Should().Throw<RestException>();
        }
    }
}
