﻿using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Tags.Commands;
using SK.Application.Tags.Commands.CreateTag;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Tags.Commands
{
    using static Testing;

    public class CreateTagTest : TestBase
    {
        [Test]
        public async Task ShouldCreateTag()
        {
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var tagToCreate = new Faker<TagCreateOrEditDto>("en")
                .RuleFor(a => a.Id, f => f.Random.Guid())
                .RuleFor(a => a.Title, f => f.Lorem.Sentence())
                .Generate();

            var command = new CreateTagCommand(tagToCreate);

            var createdId = await SendAsync(command);
            var createdTag = await FindByGuidAsync<Tag>(createdId);

            createdTag.Id.Should().Be(createdId);
            createdTag.Title.Should().Be(tagToCreate.Title);
            createdTag.CreatedBy.Should().Be(loggedUser);
            createdTag.Created.Should().BeCloseTo(DateTime.UtcNow, 1000);
        }

        [Test]
        public async Task ShouldNotCreateTagIfTitleIsEmpty()
        {
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var tagToCreate = new Faker<TagCreateOrEditDto>("en")
                .RuleFor(a => a.Id, f => f.Random.Guid())
                .Generate();
            tagToCreate.Title = null;

            var command = new CreateTagCommand(tagToCreate);

            Func<Task> act = async () => await SendAsync(command);

            act.Should().Throw<Common.Exceptions.ValidationException>();
        }
    }
}
