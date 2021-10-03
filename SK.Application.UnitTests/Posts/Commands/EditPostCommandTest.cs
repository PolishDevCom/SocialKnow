using Bogus;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Posts;
using SK.Application.Posts.Commands;
using SK.Application.Posts.Commands.EditPost;
using SK.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Posts.Commands
{
    public class EditPostCommandTest
    {
        private readonly Guid id = new Guid();
        private readonly Mock<DbSet<Post>> dbSetPost;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IStringLocalizer<PostsResource>> stringLocalizer;

        private readonly Post post;
        private readonly PostEditDto postDto;

        public EditPostCommandTest()
        {
            dbSetPost = new Mock<DbSet<Post>>();
            context = new Mock<IApplicationDbContext>();
            stringLocalizer = new Mock<IStringLocalizer<PostsResource>>();

            post = new Post { Id = id };
            postDto = new Faker<PostEditDto>("en")
                .RuleFor(p => p.Body, f => f.Random.String(10))
                .RuleFor(p => p.Id, f => f.PickRandomParam(id))
                .Generate();
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            dbSetPost.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Post>(Task.FromResult(post)));
            context.Setup(x => x.Posts).Returns(dbSetPost.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            EditPostCommandHandler editPostCommandHandler = new EditPostCommandHandler(context.Object, stringLocalizer.Object);
            EditPostCommand editPostCommand = new EditPostCommand(postDto);

            var result = await editPostCommandHandler.Handle(editPostCommand, new CancellationToken());

            result.Should().Be(Unit.Value);
        }

        [Test]
        public void ShouldNotCallHandleIfPostNotExist()
        {
            dbSetPost.Setup(x => x.FindAsync(id)).Returns(null);
            context.Setup(x => x.Posts).Returns(dbSetPost.Object);

            EditPostCommandHandler editPostCommandHandler = new EditPostCommandHandler(context.Object, stringLocalizer.Object);
            EditPostCommand editPostCommand = new EditPostCommand(postDto);

            Func<Task> act = async () => await editPostCommandHandler.Handle(editPostCommand, new CancellationToken());

            act.Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public void ShouldNotCallHandleIfNotSavedChanges()
        {
            dbSetPost.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Post>(Task.FromResult(post)));
            context.Setup(x => x.Posts).Returns(dbSetPost.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            EditPostCommandHandler editPostCommandHandler = new EditPostCommandHandler(context.Object, stringLocalizer.Object);
            EditPostCommand editPostCommand = new EditPostCommand(postDto);

            Func<Task> act = async () => await editPostCommandHandler.Handle(editPostCommand, new CancellationToken());

            act.Should().ThrowAsync<RestException>();
        }
    }
}