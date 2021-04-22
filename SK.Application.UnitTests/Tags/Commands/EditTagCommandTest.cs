using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Tags;
using SK.Application.Tags.Commands;
using SK.Application.Tags.Commands.EditTag;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Tags.Commands
{
    public class EditTagCommandTest
    {
        private readonly Guid id = new Guid();

        private readonly Mock<DbSet<Tag>> dbSetTag;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IStringLocalizer<TagsResource>> stringLocalizer;
        private readonly Mock<IMapper> mapper;

        private readonly Tag tag;
        private readonly TagCreateOrEditDto tagDto;

        public EditTagCommandTest()
        {
            dbSetTag = new Mock<DbSet<Tag>>();
            context = new Mock<IApplicationDbContext>();
            stringLocalizer = new Mock<IStringLocalizer<TagsResource>>();
            mapper = new Mock<IMapper>();

            tag = new Tag { Id = id };
            tagDto = new TagCreateOrEditDto { Id = id };
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            dbSetTag.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Tag>(Task.FromResult(tag)));
            context.Setup(x => x.Tags).Returns(dbSetTag.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            EditTagCommandHandler editTagCommandHandler = new EditTagCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            EditTagCommand editTagCommand = new EditTagCommand(tagDto);

            var result = await editTagCommandHandler.Handle(editTagCommand, new CancellationToken());

            mapper.Verify(x => x.Map(editTagCommand, tag), Times.Once);
            result.Should().Be(Unit.Value);
        }

        [Test]
        public void ShouldNotCallHandleIfNotSavedChanges()
        {
            dbSetTag.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Tag>(Task.FromResult(tag)));
            context.Setup(x => x.Tags).Returns(dbSetTag.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            EditTagCommandHandler editTagCommandHandler = new EditTagCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            EditTagCommand editTagCommand = new EditTagCommand(tagDto);

            Func<Task> act = async () => await editTagCommandHandler.Handle(editTagCommand, new CancellationToken());

            act.Should().Throw<RestException>();
        }

        [Test]
        public void ShouldNotCallHandleIfTagNotExist()
        {
            dbSetTag.Setup(x => x.FindAsync(id)).Returns(null);
            context.Setup(x => x.Tags).Returns(dbSetTag.Object);

            EditTagCommandHandler editTagCommandHandler = new EditTagCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            EditTagCommand editTagCommand = new EditTagCommand(tagDto);

            Func<Task> act = async () => await editTagCommandHandler.Handle(editTagCommand, new CancellationToken());

            act.Should().Throw<NotFoundException>();
        }
    }
}
