using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.Categories.Commands.DeleteCategory;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Categories;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Categories.Commands
{
    public class DeleteCategoryCommandTest
    {
        private readonly Guid id;
        private readonly Mock<DbSet<Category>> dbSetCategory;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IStringLocalizer<CategoriesResource>> stringLocalizer;

        public DeleteCategoryCommandTest()
        {
            id = new Guid();
            dbSetCategory = new Mock<DbSet<Category>>();
            context = new Mock<IApplicationDbContext>();
            stringLocalizer = new Mock<IStringLocalizer<CategoriesResource>>();
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            dbSetCategory.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Category>(Task.FromResult(new Category { Id = id })));
            context.Setup(x => x.Categories).Returns(dbSetCategory.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            DeleteCategoryCommandHandler deleteCategoryCommandHandler = new DeleteCategoryCommandHandler(context.Object, stringLocalizer.Object);
            DeleteCategoryCommand deleteCategoryCommand = new DeleteCategoryCommand(id);

            var result = await deleteCategoryCommandHandler.Handle(deleteCategoryCommand, new CancellationToken());

            result.Should().Be(Unit.Value);
        }

        [Test]
        public void ShouldNotCallHandleIfNotSavedChanges()
        {
            dbSetCategory.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Category>(Task.FromResult(new Category { Id = id })));
            context.Setup(x => x.Categories).Returns(dbSetCategory.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            DeleteCategoryCommandHandler deleteCategoryCommandHandler = new DeleteCategoryCommandHandler(context.Object, stringLocalizer.Object);
            DeleteCategoryCommand deleteCategoryCommand = new DeleteCategoryCommand(id);

            Func<Task> act = async () => await deleteCategoryCommandHandler.Handle(deleteCategoryCommand, new CancellationToken());

            act.Should().ThrowAsync<RestException>();
        }

        [Test]
        public void ShouldNotCallHandleIfCategoryNotExist()
        {
            dbSetCategory.Setup(x => x.FindAsync(id)).Returns(null);
            context.Setup(x => x.Categories).Returns(dbSetCategory.Object);

            DeleteCategoryCommandHandler deleteCategoryCommandHandler = new DeleteCategoryCommandHandler(context.Object, stringLocalizer.Object);
            DeleteCategoryCommand deleteCategoryCommand = new DeleteCategoryCommand(id);

            Func<Task> act = async () => await deleteCategoryCommandHandler.Handle(deleteCategoryCommand, new CancellationToken());

            act.Should().ThrowAsync<NotFoundException>();
        }
    }
}