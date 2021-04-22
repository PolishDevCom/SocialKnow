using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Tags.Commands.CreateTag;
using SK.Application.Tags.Commands.DeleteTag;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.Tags.Commands
{
    using static Testing;
    public class DeleteTagTest : TestBase
    {
        [Test]
        public async Task ShouldDeleteTag()
        {
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");

            var command = new Faker<CreateTagCommand>("en")
                .RuleFor(a => a.Id, f => f.Random.Guid())
                .RuleFor(a => a.Title, f => f.Lorem.Sentence())
                .Generate();

            var createdTagId = await SendAsync(command);

            var result = await SendAsync(new DeleteTagCommand
            {
                Id = createdTagId
            });

            var actualEvent = await FindByGuidAsync<Tag>(createdTagId);

            actualEvent.Should().BeNull();
        }

        [Test]
        public void ShouldRequireValidTagId()
        {
            var command = new DeleteTagCommand { Id = Guid.NewGuid() };

            Func<Task> act = async () => await SendAsync(command);

            act.Should().Throw<NotFoundException>();
        }
    }
}
