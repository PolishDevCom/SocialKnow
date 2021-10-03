using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.Categories.Commands;
using SK.Application.Categories.Commands.EditCategory;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Categories;
using SK.Application.Tags.Commands.EditTag;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Categories.Commands
{
    public class EditCategoryCommandTest
    {
        private readonly Guid id = new Guid();

        private readonly Mock<DbSet<Category>> dbSetCategory;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IStringLocalizer<CategoriesResource>> stringLocalizer;
        private readonly Mock<IMapper> mapper;

        private readonly Category category;
        private readonly CategoryCreateOrEditDto categoryDto;

        public EditCategoryCommandTest()
        {
            dbSetCategory = new Mock<DbSet<Category>>();
            context = new Mock<IApplicationDbContext>();
            stringLocalizer = new Mock<IStringLocalizer<CategoriesResource>>();
            mapper = new Mock<IMapper>();

            category = new Category { Id = id };
            categoryDto = new CategoryCreateOrEditDto { Id = id };
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            dbSetCategory.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Category>(Task.FromResult(category)));
            context.Setup(x => x.Categories).Returns(dbSetCategory.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            EditCategoryCommandHandler editCategoryCommandHandler = new EditCategoryCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            EditCategoryCommand editCategoryCommand = new EditCategoryCommand(categoryDto);

            var result = await editCategoryCommandHandler.Handle(editCategoryCommand, new CancellationToken());

            mapper.Verify(x => x.Map(editCategoryCommand, category), Times.Once);
            result.Should().Be(Unit.Value);
        }

        [Test]
        public void ShouldNotCallHandleIfNotSavedChanges()
        {
            dbSetCategory.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Category>(Task.FromResult(category)));
            context.Setup(x => x.Categories).Returns(dbSetCategory.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            EditCategoryCommandHandler editCategoryCommandHandler = new EditCategoryCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            EditCategoryCommand editCategoryCommand = new EditCategoryCommand(categoryDto);

            Func<Task> act = async () => await editCategoryCommandHandler.Handle(editCategoryCommand, new CancellationToken());

            act.Should().ThrowAsync<RestException>();
        }

        [Test]
        public void ShouldNotCallHandleIfTagNotExist()
        {
            dbSetCategory.Setup(x => x.FindAsync(id)).Returns(null);
            context.Setup(x => x.Categories).Returns(dbSetCategory.Object);

            EditCategoryCommandHandler editCategoryCommandHandler = new EditCategoryCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            EditCategoryCommand editCategoryCommand = new EditCategoryCommand(categoryDto);

            Func<Task> act = async () => await editCategoryCommandHandler.Handle(editCategoryCommand, new CancellationToken());

            act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
