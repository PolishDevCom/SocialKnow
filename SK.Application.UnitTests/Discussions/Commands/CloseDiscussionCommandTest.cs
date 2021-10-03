using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Discussions;
using SK.Application.Discussions.Commands.CloseDiscussion;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Discussions.Commands
{
    public class CloseDiscussionCommandTest
    {
        private readonly Guid id;
        private readonly Mock<DbSet<Discussion>> dbSetDiscussion;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IStringLocalizer<DiscussionsResource>> stringLocalizer;

        private readonly Discussion discussion;

        public CloseDiscussionCommandTest()
        {
            id = new Guid();
            dbSetDiscussion = new Mock<DbSet<Discussion>>();
            context = new Mock<IApplicationDbContext>();
            stringLocalizer = new Mock<IStringLocalizer<DiscussionsResource>>();

            discussion = new Discussion { Id = id };
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            discussion.IsClosed = false;

            dbSetDiscussion.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Discussion>(Task.FromResult(discussion)));
            context.Setup(x => x.Discussions).Returns(dbSetDiscussion.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            CloseDiscussionCommandHandler closeDiscussionCommandHandler = new CloseDiscussionCommandHandler(context.Object, stringLocalizer.Object);
            CloseDiscussionCommand closeDiscussionCommand = new CloseDiscussionCommand(id);

            var result = await closeDiscussionCommandHandler.Handle(closeDiscussionCommand, new CancellationToken());

            result.Should().Be(Unit.Value);
            discussion.IsClosed.Should().BeTrue();
        }

        [Test]
        public void ShouldNotCallHandleIfDiscussionNotExist()
        {
            dbSetDiscussion.Setup(x => x.FindAsync(id)).Returns(null);
            context.Setup(x => x.Discussions).Returns(dbSetDiscussion.Object);

            CloseDiscussionCommandHandler closeDiscussionCommandHandler = new CloseDiscussionCommandHandler(context.Object, stringLocalizer.Object);
            CloseDiscussionCommand closeDiscussionCommand = new CloseDiscussionCommand(id);

            Func<Task> act = async () => await closeDiscussionCommandHandler.Handle(closeDiscussionCommand, new CancellationToken());

            act.Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public void ShouldNotCallHandleIfDiscussionIsClosed()
        {
            discussion.IsClosed = true;

            dbSetDiscussion.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Discussion>(Task.FromResult(discussion)));
            context.Setup(x => x.Discussions).Returns(dbSetDiscussion.Object);

            CloseDiscussionCommandHandler closeDiscussionCommandHandler = new CloseDiscussionCommandHandler(context.Object, stringLocalizer.Object);
            CloseDiscussionCommand closeDiscussionCommand = new CloseDiscussionCommand(id);

            Func<Task> act = async () => await closeDiscussionCommandHandler.Handle(closeDiscussionCommand, new CancellationToken());

            act.Should().ThrowAsync<RestException>();
        }

        [Test]
        public void ShouldNotCallHandleIfNotSavedChanges()
        {
            discussion.IsClosed = false;

            dbSetDiscussion.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Discussion>(Task.FromResult(discussion)));
            context.Setup(x => x.Discussions).Returns(dbSetDiscussion.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            CloseDiscussionCommandHandler closeDiscussionCommandHandler = new CloseDiscussionCommandHandler(context.Object, stringLocalizer.Object);
            CloseDiscussionCommand closeDiscussionCommand = new CloseDiscussionCommand(id);

            Func<Task> act = async () => await closeDiscussionCommandHandler.Handle(closeDiscussionCommand, new CancellationToken());

            act.Should().ThrowAsync<RestException>();
        }
    }
}
