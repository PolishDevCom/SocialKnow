using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.Categories.Commands;
using SK.Application.Categories.Commands.CreateCategory;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Categories;
using SK.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Categories.Commands
{
    public class CreateCategoryCommandTest
    {
        private readonly Mock<DbSet<Category>> dbSetCategory;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IStringLocalizer<CategoriesResource>> stringLocalizer;
        private readonly Mock<IMapper> mapper;

        public CreateCategoryCommandTest()
        {
            dbSetCategory = new Mock<DbSet<Category>>();
            context = new Mock<IApplicationDbContext>();
            stringLocalizer = new Mock<IStringLocalizer<CategoriesResource>>();
            mapper = new Mock<IMapper>();
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            var id = new Guid();
            var categoryDto = new CategoryCreateOrEditDto { Id = id };
            var category = new Category { Id = id };

            context.Setup(x => x.Categories).Returns(dbSetCategory.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            CreateCategoryCommandHandler createCategoryCommandHandler = new CreateCategoryCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            CreateCategoryCommand createCategoryCommand = new CreateCategoryCommand(categoryDto);

            mapper.Setup(x => x.Map<Category>(createCategoryCommand)).Returns(category);

            var result = await createCategoryCommandHandler.Handle(createCategoryCommand, new CancellationToken());

            result.Should().Be(id);
        }

        [Test]
        public void ShouldNotCallHandleIfNotSavedChanges()
        {
            context.Setup(x => x.Categories).Returns(dbSetCategory.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            CreateCategoryCommandHandler createCategoryCommandHandler = new CreateCategoryCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            CreateCategoryCommand createCategoryCommand = new CreateCategoryCommand();

            mapper.Setup(x => x.Map<Category>(createCategoryCommand)).Returns(new Category());

            Func<Task> act = async () => await createCategoryCommandHandler.Handle(createCategoryCommand, new CancellationToken());

            act.Should().ThrowAsync<RestException>();
        }
    }
}