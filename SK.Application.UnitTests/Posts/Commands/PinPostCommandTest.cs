using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Posts;
using SK.Application.Posts.Commands.PinPost;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Posts.Commands
{
    public class PinPostCommandTest
    {
        private readonly Guid id = new Guid();
        private readonly Mock<DbSet<Post>> dbSetPost;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IStringLocalizer<PostsResource>> stringLocalizer;

        private readonly Post post;

        public PinPostCommandTest()
        {
            dbSetPost = new Mock<DbSet<Post>>();
            context = new Mock<IApplicationDbContext>();
            stringLocalizer = new Mock<IStringLocalizer<PostsResource>>();

            post = new Post { Id = id, IsPinned = false };
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            //Arrange
            dbSetPost.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Post>(Task.FromResult(post)));
            context.Setup(x => x.Posts).Returns(dbSetPost.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            PinPostCommandHandler pinPostCommandHandler = new PinPostCommandHandler(context.Object, stringLocalizer.Object);
            PinPostCommand pinPostCommand = new PinPostCommand(id);

            //Act
            var result = await pinPostCommandHandler.Handle(pinPostCommand, new CancellationToken());

            //Assert
            result.Should().Be(Unit.Value);
            post.IsPinned.Should().BeTrue();
        }

        [Test]
        public void ShouldNotCallHandleIfPostNotExist()
        {
            //Arrange
            dbSetPost.Setup(x => x.FindAsync(id)).Returns(null);
            context.Setup(x => x.Posts).Returns(dbSetPost.Object);

            PinPostCommandHandler pinPostCommandHandler = new PinPostCommandHandler(context.Object, stringLocalizer.Object);
            PinPostCommand pinPostCommand = new PinPostCommand(id);

            //Act
            Func<Task> act = async() => await pinPostCommandHandler.Handle(pinPostCommand, new CancellationToken());

            //Assert
            act.Should().Throw<NotFoundException>();
        }

        [Test]
        public void ShouldNotCallHandleIfPostIsPinned()
        {
            //Arrange
            post.IsPinned = true;
            dbSetPost.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Post>(Task.FromResult(post)));
            context.Setup(x => x.Posts).Returns(dbSetPost.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            PinPostCommandHandler pinPostCommandHandler = new PinPostCommandHandler(context.Object, stringLocalizer.Object);
            PinPostCommand pinPostCommand = new PinPostCommand(id);

            //Act
            Func<Task> act = async () => await pinPostCommandHandler.Handle(pinPostCommand, new CancellationToken());

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

            PinPostCommandHandler pinPostCommandHandler = new PinPostCommandHandler(context.Object, stringLocalizer.Object);
            PinPostCommand pinPostCommand = new PinPostCommand(id);

            //Act
            Func<Task> act = async () => await pinPostCommandHandler.Handle(pinPostCommand, new CancellationToken());

            //Assert
            act.Should().Throw<RestException>();
        }
    }
}