using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Discussions;
using SK.Application.Discussions.Commands.DeleteDiscussion;
using SK.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Discussions.Commands
{
    public class DeleteDiscussionCommandTest
    {
        private readonly Guid id;
        private readonly Mock<DbSet<Discussion>> dbSetDiscussion;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IStringLocalizer<DiscussionsResource>> stringLocalizer;

        private readonly Discussion discussion;

        public DeleteDiscussionCommandTest()
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
            dbSetDiscussion.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Discussion>(Task.FromResult(discussion)));
            context.Setup(x => x.Discussions).Returns(dbSetDiscussion.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            DeleteDiscussionCommandHandler deleteDiscussionCommandHandler = new DeleteDiscussionCommandHandler(context.Object, stringLocalizer.Object);
            DeleteDiscussionCommand deleteDiscussionCommand = new DeleteDiscussionCommand(id);

            var result = await deleteDiscussionCommandHandler.Handle(deleteDiscussionCommand, new CancellationToken());

            result.Should().Be(Unit.Value);
        }

        [Test]
        public void ShouldNotCallHandleIfDiscussionNotExist()
        {
            dbSetDiscussion.Setup(x => x.FindAsync(id)).Returns(null);
            context.Setup(x => x.Discussions).Returns(dbSetDiscussion.Object);

            DeleteDiscussionCommandHandler deleteDiscussionCommandHandler = new DeleteDiscussionCommandHandler(context.Object, stringLocalizer.Object);
            DeleteDiscussionCommand deleteDiscussionCommand = new DeleteDiscussionCommand(id);

            Func<Task> act = async () => await deleteDiscussionCommandHandler.Handle(deleteDiscussionCommand, new CancellationToken());

            act.Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public void ShouldNotCallHandleIfNotSavedChanges()
        {
            dbSetDiscussion.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Discussion>(Task.FromResult(discussion)));
            context.Setup(x => x.Discussions).Returns(dbSetDiscussion.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            DeleteDiscussionCommandHandler deleteDiscussionCommandHandler = new DeleteDiscussionCommandHandler(context.Object, stringLocalizer.Object);
            DeleteDiscussionCommand deleteDiscussionCommand = new DeleteDiscussionCommand(id);

            Func<Task> act = async () => await deleteDiscussionCommandHandler.Handle(deleteDiscussionCommand, new CancellationToken());

            act.Should().ThrowAsync<RestException>();
        }
    }
}