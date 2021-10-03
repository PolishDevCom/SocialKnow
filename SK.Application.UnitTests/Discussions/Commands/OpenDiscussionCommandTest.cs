using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Discussions;
using SK.Application.Discussions.Commands.OpenDiscussion;
using SK.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Discussions.Commands
{
    public class OpenDiscussionCommandTest
    {
        private readonly Guid id;
        private readonly Mock<DbSet<Discussion>> dbSetDiscussion;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IStringLocalizer<DiscussionsResource>> stringLocalizer;

        private readonly Discussion discussion;

        public OpenDiscussionCommandTest()
        {
            id = new Guid();
            dbSetDiscussion = new Mock<DbSet<Discussion>>();
            context = new Mock<IApplicationDbContext>();
            stringLocalizer = new Mock<IStringLocalizer<DiscussionsResource>>();

            discussion = new Discussion { Id = id, IsClosed = true };
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            dbSetDiscussion.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Discussion>(Task.FromResult(discussion)));
            context.Setup(x => x.Discussions).Returns(dbSetDiscussion.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            OpenDiscussionCommandHandler openDiscussionCommandHandler = new OpenDiscussionCommandHandler(context.Object, stringLocalizer.Object);
            OpenDiscussionCommand openDiscussionCommand = new OpenDiscussionCommand(id);

            var result = await openDiscussionCommandHandler.Handle(openDiscussionCommand, new CancellationToken());

            result.Should().Be(Unit.Value);
            discussion.IsClosed.Should().BeFalse();
        }

        [Test]
        public void ShouldNotCallHandleIfDiscussionNotExist()
        {
            dbSetDiscussion.Setup(x => x.FindAsync(id)).Returns(null);
            context.Setup(x => x.Discussions).Returns(dbSetDiscussion.Object);

            OpenDiscussionCommandHandler openDiscussionCommandHandler = new OpenDiscussionCommandHandler(context.Object, stringLocalizer.Object);
            OpenDiscussionCommand openDiscussionCommand = new OpenDiscussionCommand(id);

            Func<Task> act = async () => await openDiscussionCommandHandler.Handle(openDiscussionCommand, new CancellationToken());

            act.Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public void ShouldNotCallHandleIfDiscussionIsNotClosed()
        {
            discussion.IsClosed = false;
            dbSetDiscussion.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Discussion>(Task.FromResult(discussion)));
            context.Setup(x => x.Discussions).Returns(dbSetDiscussion.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            OpenDiscussionCommandHandler openDiscussionCommandHandler = new OpenDiscussionCommandHandler(context.Object, stringLocalizer.Object);
            OpenDiscussionCommand openDiscussionCommand = new OpenDiscussionCommand(id);

            Func<Task> act = async () => await openDiscussionCommandHandler.Handle(openDiscussionCommand, new CancellationToken());

            act.Should().ThrowAsync<RestException>();
        }

        [Test]
        public void ShouldNotCallHandleIfNotSavedChanges()
        {
            dbSetDiscussion.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Discussion>(Task.FromResult(discussion)));
            context.Setup(x => x.Discussions).Returns(dbSetDiscussion.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            OpenDiscussionCommandHandler openDiscussionCommandHandler = new OpenDiscussionCommandHandler(context.Object, stringLocalizer.Object);
            OpenDiscussionCommand openDiscussionCommand = new OpenDiscussionCommand(id);

            Func<Task> act = async () => await openDiscussionCommandHandler.Handle(openDiscussionCommand, new CancellationToken());

            act.Should().ThrowAsync<RestException>();
        }
    }
}