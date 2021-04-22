using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Tags;
using SK.Application.Tags.Commands.DeleteTag;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Tags.Commands
{
    public class DeleteTagCommandTest
    {
        private readonly Guid id;
        private readonly Mock<DbSet<Tag>> dbSetTag;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IStringLocalizer<TagsResource>> stringLocalizer;

        public DeleteTagCommandTest()
        {
            id = new Guid();
            dbSetTag = new Mock<DbSet<Tag>>();
            context = new Mock<IApplicationDbContext>();
            stringLocalizer = new Mock<IStringLocalizer<TagsResource>>();
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            dbSetTag.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Tag>(Task.FromResult(new Tag { Id = id })));
            context.Setup(x => x.Tags).Returns(dbSetTag.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            DeleteTagCommandHandler deleteTagCommandHandler = new DeleteTagCommandHandler(context.Object, stringLocalizer.Object);
            DeleteTagCommand deleteTagCommand = new DeleteTagCommand(id);

            var result = await deleteTagCommandHandler.Handle(deleteTagCommand, new CancellationToken());

            result.Should().Be(Unit.Value);
        }

        [Test]
        public void ShouldNotCallHandleIfNotSavedChanges()
        {
            dbSetTag.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Tag>(Task.FromResult(new Tag { Id = id })));
            context.Setup(x => x.Tags).Returns(dbSetTag.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            DeleteTagCommandHandler deleteTagCommandHandler = new DeleteTagCommandHandler(context.Object, stringLocalizer.Object);
            DeleteTagCommand deleteTagCommand = new DeleteTagCommand(id);

            Func<Task> act = async () => await deleteTagCommandHandler.Handle(deleteTagCommand, new CancellationToken());

            act.Should().Throw<RestException>();
        }

        [Test]
        public void ShouldNotCallHandleIfTagNotExist()
        {
            dbSetTag.Setup(x => x.FindAsync(id)).Returns(null);
            context.Setup(x => x.Tags).Returns(dbSetTag.Object);

            DeleteTagCommandHandler deleteTagCommandHandler = new DeleteTagCommandHandler(context.Object, stringLocalizer.Object);
            DeleteTagCommand deleteTagCommand = new DeleteTagCommand(id);

            Func<Task> act = async () => await deleteTagCommandHandler.Handle(deleteTagCommand, new CancellationToken());

            act.Should().Throw<NotFoundException>();
        }
    }
}
