using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.Articles.Commands.DeleteArticle;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Articles;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Articles.Commands
{
    public class DeleteArticleCommandTest
    {
        private readonly Guid id;
        private readonly Mock<DbSet<Article>> dbSetArticle;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IStringLocalizer<ArticlesResource>> stringLocalizer;

        public DeleteArticleCommandTest()
        {
            id = new Guid();
            dbSetArticle = new Mock<DbSet<Article>>();
            context = new Mock<IApplicationDbContext>();
            stringLocalizer = new Mock<IStringLocalizer<ArticlesResource>>();
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            //Arrange
            dbSetArticle.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Article>(Task.FromResult(new Article { Id = id })));
            context.Setup(x => x.Articles).Returns(dbSetArticle.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            DeleteArticleCommandHandler deleteArticleCommandHandler = new DeleteArticleCommandHandler(context.Object, stringLocalizer.Object);
            DeleteArticleCommand deleteArticleCommand = new DeleteArticleCommand(id);

            //Act
            var result = await deleteArticleCommandHandler.Handle(deleteArticleCommand, new CancellationToken());

            //Assert
            result.Should().Be(Unit.Value);
        }

        [Test]
        public void ShouldNotCallHandleIfNotSavedChanges()
        {
            //Arrange
            dbSetArticle.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Article>(Task.FromResult(new Article { Id = id })));
            context.Setup(x => x.Articles).Returns(dbSetArticle.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            DeleteArticleCommandHandler deleteArticleCommandHandler = new DeleteArticleCommandHandler(context.Object, stringLocalizer.Object);
            DeleteArticleCommand deleteArticleCommand = new DeleteArticleCommand(id);

            //Act
            Func<Task> act = async () => await deleteArticleCommandHandler.Handle(deleteArticleCommand, new CancellationToken());

            //Assert
            act.Should().Throw<RestException>();
        }

        [Test]
        public void ShouldNotCallHandleIfArticleNotExist()
        {
            //Arrange
            dbSetArticle.Setup(x => x.FindAsync(id)).Returns(null);
            context.Setup(x => x.Articles).Returns(dbSetArticle.Object);

            DeleteArticleCommandHandler deleteArticleCommandHandler = new DeleteArticleCommandHandler(context.Object, stringLocalizer.Object);
            DeleteArticleCommand deleteArticleCommand = new DeleteArticleCommand(id);

            //Act
            Func<Task> act = async () => await deleteArticleCommandHandler.Handle(deleteArticleCommand, new CancellationToken());

            //Assert
            act.Should().Throw<NotFoundException>();
        }
    }
}
