using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Categories.Commands;
using SK.Application.Categories.Commands.CreateCategory;
using SK.Application.Categories.Commands.EditCategory;
using SK.Application.Common.Exceptions;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Categories.Commands
{
    using static Testing;

    public class EditCategoryTest : TestBase
    {
        [Test]
        public async Task ShouldUpdateCategory()
        {
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var categoryId = await CreateCategory();

            var categoryToModify = new Faker<CategoryCreateOrEditDto>("en")
                .RuleFor(a => a.Id, f => categoryId)
                .RuleFor(a => a.Title, f => f.Lorem.Sentence())
                .Generate();

            var command = new EditCategoryCommand(categoryToModify);
            await SendAsync(command);

            var modifiedCategory = await FindByGuidAsync<Category>(categoryId);

            modifiedCategory.Id.Should().Be(categoryToModify.Id);
            modifiedCategory.Title.Should().Be(categoryToModify.Title);
            modifiedCategory.LastModifiedBy.Should().Be(loggedUser);
            modifiedCategory.LastModified.Should().BeCloseTo(DateTime.UtcNow, 1000);
        }

        [Test]
        public void ShouldRequireValidCategoryId()
        {
            var categoryToEdit = new Faker<CategoryCreateOrEditDto>("en")
               .RuleFor(a => a.Id, f => f.Random.Guid())
               .RuleFor(a => a.Title, f => f.Lorem.Sentence())
               .Generate();

            var command = new EditCategoryCommand(categoryToEdit);

            Func<Task> act = async () => await SendAsync(command);

            act.Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldNotEditCategoryIfTitleIsEmpty()
        {
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var categoryId = await CreateCategory();

            var categoryToCreate = new Faker<CategoryCreateOrEditDto>("en")
                .RuleFor(a => a.Id, f => categoryId)
                .Generate();
            categoryToCreate.Title = null;

            var command = new EditCategoryCommand(categoryToCreate);

            Func<Task> act = async () => await SendAsync(command);

            act.Should().Throw<Common.Exceptions.ValidationException>();
        }

        private async Task<Guid> CreateCategory()
        {
            var categoryToCreate = new Faker<CategoryCreateOrEditDto>("en")
                .RuleFor(a => a.Id, f => f.Random.Guid())
                .RuleFor(a => a.Title, f => f.Lorem.Sentence())
                .Generate();

            return await SendAsync(new CreateCategoryCommand(categoryToCreate));
        }
    }
}
