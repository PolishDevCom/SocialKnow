using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Discussions;
using SK.Application.Discussions.Commands;
using SK.Application.Discussions.Commands.EditDiscussion;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Discussions.Commands
{
    public class EditDiscussionCommandTest
    {
        private readonly Guid id;
        private readonly Mock<DbSet<Discussion>> dbSetDiscussion;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IStringLocalizer<DiscussionsResource>> stringLocalizer;

        private readonly Discussion discussion;
        private readonly DiscussionEditDto discussionDto;

        public EditDiscussionCommandTest()
        {
            id = new Guid();
            dbSetDiscussion = new Mock<DbSet<Discussion>>();
            context = new Mock<IApplicationDbContext>();
            stringLocalizer = new Mock<IStringLocalizer<DiscussionsResource>>();

            discussion = new Discussion { Id = id };
            discussionDto = new DiscussionEditDto { Id = id, Title = "Title", Description = "Description" };
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            //Arrange
            dbSetDiscussion.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Discussion>(Task.FromResult(discussion)));
            context.Setup(x => x.Discussions).Returns(dbSetDiscussion.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            EditDiscussionCommandHandler editDiscussionCommandHandler = new EditDiscussionCommandHandler(context.Object, stringLocalizer.Object);
            EditDiscussionCommand editDiscussionCommand = new EditDiscussionCommand(discussionDto);

            //Act
            var result = await editDiscussionCommandHandler.Handle(editDiscussionCommand, new CancellationToken());

            //Assert
            result.Should().Be(Unit.Value);
            discussion.Title.Should().Be(discussionDto.Title);
            discussion.Description.Should().Be(discussionDto.Description);
        }

        [Test]
        public void ShouldNotCallHandleIfDiscussionNotExist()
        {
            //Arrange
            dbSetDiscussion.Setup(x => x.FindAsync(id)).Returns(null);
            context.Setup(x => x.Discussions).Returns(dbSetDiscussion.Object);

            EditDiscussionCommandHandler editDiscussionCommandHandler = new EditDiscussionCommandHandler(context.Object, stringLocalizer.Object);
            EditDiscussionCommand editDiscussionCommand = new EditDiscussionCommand(discussionDto);

            //Act
            Func<Task> act = async () => await editDiscussionCommandHandler.Handle(editDiscussionCommand, new CancellationToken());

            //Assert
            act.Should().Throw<NotFoundException>();
        }

        [Test]
        public void ShouldNotCallHandleIfNotSavedChanges()
        {
            //Arrange
            dbSetDiscussion.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Discussion>(Task.FromResult(discussion)));
            context.Setup(x => x.Discussions).Returns(dbSetDiscussion.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            EditDiscussionCommandHandler editDiscussionCommandHandler = new EditDiscussionCommandHandler(context.Object, stringLocalizer.Object);
            EditDiscussionCommand editDiscussionCommand = new EditDiscussionCommand(discussionDto);

            //Act
            Func<Task> act = async () => await editDiscussionCommandHandler.Handle(editDiscussionCommand, new CancellationToken());

            //Assert
            act.Should().Throw<RestException>();
        }
    }
}
