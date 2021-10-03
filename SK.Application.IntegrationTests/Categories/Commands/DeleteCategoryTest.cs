using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Categories.Commands.CreateCategory;
using SK.Application.Categories.Commands.DeleteCategory;
using SK.Application.Common.Exceptions;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Categories.Commands
{
    using static Testing;
    public class DeleteCategoryTest : TestBase
    {
        [Test]
        public async Task ShouldDeleteCategory()
        {
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var command = new Faker<CreateCategoryCommand>("en")
                .RuleFor(a => a.Id, f => f.Random.Guid())
                .RuleFor(a => a.Title, f => f.Lorem.Sentence())
                .Generate();

            var createdCategoryId = await SendAsync(command);

            var result = await SendAsync(new DeleteCategoryCommand
            {
                Id = createdCategoryId
            });

            var actualEvent = await FindByGuidAsync<Category>(createdCategoryId);

            actualEvent.Should().BeNull();
        }

        [Test]
        public void ShouldRequireValidCategoryId()
        {
            var command = new DeleteCategoryCommand { Id = Guid.NewGuid() };

            Func<Task> act = async () => await SendAsync(command);

            act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
