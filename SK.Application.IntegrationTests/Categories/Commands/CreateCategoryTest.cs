using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Categories.Commands;
using SK.Application.Categories.Commands.CreateCategory;
using SK.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Categories.Commands
{
    using static Testing;

    public class CreateCategoryTest : TestBase
    {
        [Test]
        public async Task ShouldCreateCategory()
        {
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var categoryToCreate = new Faker<CategoryCreateOrEditDto>("en")
                .RuleFor(a => a.Id, f => f.Random.Guid())
                .RuleFor(a => a.Title, f => f.Lorem.Sentence())
                .Generate();

            var command = new CreateCategoryCommand(categoryToCreate);

            var createdId = await SendAsync(command);
            var createdCategory = await FindByGuidAsync<Category>(createdId);

            createdCategory.Id.Should().Be(createdId);
            createdCategory.Title.Should().Be(categoryToCreate.Title);
            createdCategory.CreatedBy.Should().Be(loggedUser);
            createdCategory.Created.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0,0,1));
        }

        [Test]
        public async Task ShouldNotCreateCategoryIfTitleIsEmpty()
        {
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var categoryToCreate = new Faker<CategoryCreateOrEditDto>("en")
                .RuleFor(a => a.Id, f => f.Random.Guid())
                .Generate();
            categoryToCreate.Title = null;

            var command = new CreateCategoryCommand(categoryToCreate);

            Func<Task> act = async () => await SendAsync(command);

            act.Should().ThrowAsync<Common.Exceptions.ValidationException>();
        }
    }
}