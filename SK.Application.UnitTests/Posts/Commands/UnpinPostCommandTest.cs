using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Posts;
using SK.Application.Posts.Commands.UnpinPost;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Posts.Commands
{
    public class UnpinPostCommandTest
    {
        private readonly Guid id = new Guid();
        private readonly Mock<DbSet<Post>> dbSetPost;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IStringLocalizer<PostsResource>> stringLocalizer;

        private readonly Post post;

        public UnpinPostCommandTest()
        {
            dbSetPost = new Mock<DbSet<Post>>();
            context = new Mock<IApplicationDbContext>();
            stringLocalizer = new Mock<IStringLocalizer<PostsResource>>();

            post = new Post { Id = id, IsPinned = true };
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            //Arrange
            dbSetPost.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Post>(Task.FromResult(post)));
            context.Setup(x => x.Posts).Returns(dbSetPost.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            UnpinPostCommandHandler unpinPostCommandHandler = new UnpinPostCommandHandler(context.Object, stringLocalizer.Object);
            UnpinPostCommand unpinPostCommand = new UnpinPostCommand(id);

            //Act
            var result = await unpinPostCommandHandler.Handle(unpinPostCommand, new CancellationToken());

            //Assert
            result.Should().Be(Unit.Value);
            post.IsPinned.Should().BeFalse();
        }

        [Test]
        public void ShouldNotCallHandleIfPostNotExist()
        {
            //Arrange
            dbSetPost.Setup(x => x.FindAsync(id)).Returns(null);
            context.Setup(x => x.Posts).Returns(dbSetPost.Object);

            UnpinPostCommandHandler unpinPostCommandHandler = new UnpinPostCommandHandler(context.Object, stringLocalizer.Object);
            UnpinPostCommand unpinPostCommand = new UnpinPostCommand(id);

            //Act
            Func<Task> act = async() => await unpinPostCommandHandler.Handle(unpinPostCommand, new CancellationToken());

            //Assert
            act.Should().Throw<NotFoundException>();
        }

        [Test]
        public void ShouldNotCallHandleIfPostIsNotPinned()
        {
            //Arrange
            post.IsPinned = false;
            dbSetPost.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Post>(Task.FromResult(post)));
            context.Setup(x => x.Posts).Returns(dbSetPost.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            UnpinPostCommandHandler unpinPostCommandHandler = new UnpinPostCommandHandler(context.Object, stringLocalizer.Object);
            UnpinPostCommand unpinPostCommand = new UnpinPostCommand(id);

            //Act
            Func<Task> act = async () => await unpinPostCommandHandler.Handle(unpinPostCommand, new CancellationToken());

            //Assert
            act.Should().Throw<RestException>();
        }

        [Test]
        public void ShouldNotCallHandleIfNotSavedChanges()
        {
            //Arrange
            dbSetPost.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Post>(Task.FromResult(post)));
            context.Setup(x => x.Posts).Returns(dbSetPost.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            UnpinPostCommandHandler unpinPostCommandHandler = new UnpinPostCommandHandler(context.Object, stringLocalizer.Object);
            UnpinPostCommand unpinPostCommand = new UnpinPostCommand(id);

            //Act
            Func<Task> act = async () => await unpinPostCommandHandler.Handle(unpinPostCommand, new CancellationToken());

            //Assert
            act.Should().Throw<RestException>();
        }
    }
}