using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Posts;
using SK.Application.Posts.Commands.DeletePost;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Posts.Commands
{
    public class DeletePostCommandTest
    {
        private readonly Guid id = new Guid();
        private readonly Mock<DbSet<Post>> dbSetPost;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IStringLocalizer<PostsResource>> stringLocalizer;

        private readonly Post post;

        public DeletePostCommandTest()
        {
            dbSetPost = new Mock<DbSet<Post>>();
            context = new Mock<IApplicationDbContext>();
            stringLocalizer = new Mock<IStringLocalizer<PostsResource>>();

            post = new Post { Id = id };
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            dbSetPost.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Post>(Task.FromResult(post)));
            context.Setup(x => x.Posts).Returns(dbSetPost.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            DeletePostCommandHandler deletePostCommandHandler = new DeletePostCommandHandler(context.Object, stringLocalizer.Object);
            DeletePostCommand deletePostCommand = new DeletePostCommand(id);

            var result = await deletePostCommandHandler.Handle(deletePostCommand, new CancellationToken());

            result.Should().Be(Unit.Value);
        }

        [Test]
        public void ShouldNotCallHandleIfPostNotExist()
        {
            dbSetPost.Setup(x => x.FindAsync(id)).Returns(null);
            context.Setup(x => x.Posts).Returns(dbSetPost.Object);

            DeletePostCommandHandler deletePostCommandHandler = new DeletePostCommandHandler(context.Object, stringLocalizer.Object);
            DeletePostCommand deletePostCommand = new DeletePostCommand(id);

            Func<Task> act = async() => await deletePostCommandHandler.Handle(deletePostCommand, new CancellationToken());

            act.Should().Throw<NotFoundException>();
        }

        [Test]
        public void ShouldNotCallHandleIfNotSavedChanges()
        {
            dbSetPost.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Post>(Task.FromResult(post)));
            context.Setup(x => x.Posts).Returns(dbSetPost.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            DeletePostCommandHandler deletePostCommandHandler = new DeletePostCommandHandler(context.Object, stringLocalizer.Object);
            DeletePostCommand deletePostCommand = new DeletePostCommand(id);

            Func<Task> act = async () => await deletePostCommandHandler.Handle(deletePostCommand, new CancellationToken());

            act.Should().Throw<RestException>();
        }
    }
}