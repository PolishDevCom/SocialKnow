using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Tags;
using SK.Application.Tags.Commands;
using SK.Application.Tags.Commands.CreateTag;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Tags.Commands
{
    public class CreateTagCommandTest
    {
        private readonly Mock<DbSet<Tag>> dbSetTag;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IStringLocalizer<TagsResource>> stringLocalizer;
        private readonly Mock<IMapper> mapper;

        public CreateTagCommandTest()
        {
            dbSetTag = new Mock<DbSet<Tag>>();
            context = new Mock<IApplicationDbContext>();
            stringLocalizer = new Mock<IStringLocalizer<TagsResource>>();
            mapper = new Mock<IMapper>();
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            var id = new Guid();
            var tagDto = new TagCreateOrEditDto { Id = id };
            var tag = new Tag { Id = id };

            context.Setup(x => x.Tags).Returns(dbSetTag.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            CreateTagCommandHandler createTagCommandHandler = new CreateTagCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            CreateTagCommand createTagCommand = new CreateTagCommand(tagDto);

            mapper.Setup(x => x.Map<Tag>(createTagCommand)).Returns(tag);

            var result = await createTagCommandHandler.Handle(createTagCommand, new CancellationToken());

            result.Should().Be(id);
        }

        [Test]
        public void ShouldNotCallHandleIfNotSavedChanges()
        {
            context.Setup(x => x.Tags).Returns(dbSetTag.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            CreateTagCommandHandler createTagCommandHandler = new CreateTagCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            CreateTagCommand createTagCommand = new CreateTagCommand();

            mapper.Setup(x => x.Map<Tag>(createTagCommand)).Returns(new Tag());

            Func<Task> act = async () => await createTagCommandHandler.Handle(createTagCommand, new CancellationToken());

            act.Should().Throw<RestException>();
        }
    }
}
