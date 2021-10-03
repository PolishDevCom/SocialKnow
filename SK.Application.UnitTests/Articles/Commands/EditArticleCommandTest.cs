using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.Articles.Commands;
using SK.Application.Articles.Commands.EditArticle;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Articles;
using SK.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Articles.Commands
{
    public class EditArticleCommandTest
    {
        private readonly Guid id;
        private readonly Mock<DbSet<Article>> dbSetArticle;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IStringLocalizer<ArticlesResource>> stringLocalizer;
        private readonly Mock<IMapper> mapper;

        private readonly Article article;
        private readonly ArticleCreateOrEditDto articleDto;

        public EditArticleCommandTest()
        {
            id = new Guid();
            dbSetArticle = new Mock<DbSet<Article>>();
            context = new Mock<IApplicationDbContext>();
            stringLocalizer = new Mock<IStringLocalizer<ArticlesResource>>();
            mapper = new Mock<IMapper>();

            article = new Article { Id = id };
            articleDto = new ArticleCreateOrEditDto { Id = id };
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            dbSetArticle.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Article>(Task.FromResult(article)));
            context.Setup(x => x.Articles).Returns(dbSetArticle.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            EditArticleCommandHandler editArticleCommandHandler = new EditArticleCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            EditArticleCommand editArticleCommand = new EditArticleCommand(articleDto);

            var result = await editArticleCommandHandler.Handle(editArticleCommand, new CancellationToken());

            mapper.Verify(x => x.Map(editArticleCommand, article), Times.Once);
            result.Should().Be(Unit.Value);
        }

        [Test]
        public void ShouldNotCallHandleIfNotSavedChanges()
        {
            dbSetArticle.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Article>(Task.FromResult(new Article { Id = id })));
            context.Setup(x => x.Articles).Returns(dbSetArticle.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            EditArticleCommandHandler editArticleCommandHandler = new EditArticleCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            EditArticleCommand editArticleCommand = new EditArticleCommand(articleDto);

            Func<Task> act = async () => await editArticleCommandHandler.Handle(editArticleCommand, new CancellationToken());

            act.Should().ThrowAsync<RestException>();
        }

        [Test]
        public void ShouldNotCallHandleIfArticleNotExist()
        {
            dbSetArticle.Setup(x => x.FindAsync(id)).Returns(null);
            context.Setup(x => x.Articles).Returns(dbSetArticle.Object);

            EditArticleCommandHandler editArticleCommandHandler = new EditArticleCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            EditArticleCommand editArticleCommand = new EditArticleCommand(articleDto);

            Func<Task> act = async () => await editArticleCommandHandler.Handle(editArticleCommand, new CancellationToken());

            act.Should().ThrowAsync<NotFoundException>();
        }
    }
}