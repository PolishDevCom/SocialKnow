using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Discussions;
using SK.Application.Discussions.Commands.UnpinDiscussion;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Discussions.Commands
{
    public class UnpinDiscussionCommandTest
    {
        private readonly Guid id;
        private readonly Mock<DbSet<Discussion>> dbSetDiscussion;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IStringLocalizer<DiscussionsResource>> stringLocalizer;

        private readonly Discussion discussion;

        public UnpinDiscussionCommandTest()
        {
            id = new Guid();
            dbSetDiscussion = new Mock<DbSet<Discussion>>();
            context = new Mock<IApplicationDbContext>();
            stringLocalizer = new Mock<IStringLocalizer<DiscussionsResource>>();

            discussion = new Discussion { Id = id, IsPinned = true };
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            //Arrange
            dbSetDiscussion.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Discussion>(Task.FromResult(discussion)));
            context.Setup(x => x.Discussions).Returns(dbSetDiscussion.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            UnpinDiscussionCommandHandler unpinDiscussionCommandHandler = new UnpinDiscussionCommandHandler(context.Object, stringLocalizer.Object);
            UnpinDiscussionCommand unpinDiscussionCommand = new UnpinDiscussionCommand(id);

            //Act
            var result = await unpinDiscussionCommandHandler.Handle(unpinDiscussionCommand, new CancellationToken());

            //Assert
            result.Should().Be(Unit.Value);
            discussion.IsPinned.Should().BeFalse();
        }

        [Test]
        public void ShouldNotCallHandleIfDiscussionNotExist()
        {
            //Arrange
            dbSetDiscussion.Setup(x => x.FindAsync(id)).Returns(null);
            context.Setup(x => x.Discussions).Returns(dbSetDiscussion.Object);

            UnpinDiscussionCommandHandler unpinDiscussionCommandHandler = new UnpinDiscussionCommandHandler(context.Object, stringLocalizer.Object);
            UnpinDiscussionCommand unpinDiscussionCommand = new UnpinDiscussionCommand(id);

            //Act
            Func<Task> act = async () => await unpinDiscussionCommandHandler.Handle(unpinDiscussionCommand, new CancellationToken());

            //Assert
            act.Should().Throw<NotFoundException>();
        }

        [Test]
        public void ShouldNotCallHandleIfDiscussionIsNotPinned()
        {
            //Arrange
            discussion.IsPinned = false;
            dbSetDiscussion.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Discussion>(Task.FromResult(discussion)));
            context.Setup(x => x.Discussions).Returns(dbSetDiscussion.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            UnpinDiscussionCommandHandler unpinDiscussionCommandHandler = new UnpinDiscussionCommandHandler(context.Object, stringLocalizer.Object);
            UnpinDiscussionCommand unpinDiscussionCommand = new UnpinDiscussionCommand(id);

            //Act
            Func<Task> act = async () => await unpinDiscussionCommandHandler.Handle(unpinDiscussionCommand, new CancellationToken());

            //Assert
            act.Should().Throw<RestException>();
        }

        [Test]
        public void ShouldNotCallHandleIfNotSavedChanges()
        {
            //Arrange
            dbSetDiscussion.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Discussion>(Task.FromResult(discussion)));
            context.Setup(x => x.Discussions).Returns(dbSetDiscussion.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            UnpinDiscussionCommandHandler unpinDiscussionCommandHandler = new UnpinDiscussionCommandHandler(context.Object, stringLocalizer.Object);
            UnpinDiscussionCommand unpinDiscussionCommand = new UnpinDiscussionCommand(id);

            //Act
            Func<Task> act = async () => await unpinDiscussionCommandHandler.Handle(unpinDiscussionCommand, new CancellationToken());

            //Assert
            act.Should().Throw<RestException>();
        }
    }
}