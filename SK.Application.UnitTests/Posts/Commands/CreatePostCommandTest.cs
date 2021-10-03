using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Posts;
using SK.Application.Posts.Commands;
using SK.Application.Posts.Commands.CreatePost;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Posts.Commands
{
    public class CreatePostCommandTest
    {
        private readonly Guid id = new Guid();
        private readonly Mock<DbSet<Post>> dbSetPost;
        private readonly Mock<DbSet<Discussion>> dbSetDiscussion;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IStringLocalizer<PostsResource>> stringLocalizer;
        private readonly Mock<IMapper> mapper;

        private readonly PostCreateDto postDto;
        private readonly Post post;
        private readonly Discussion discussion;

        public CreatePostCommandTest()
        {
            dbSetPost = new Mock<DbSet<Post>>();
            dbSetDiscussion = new Mock<DbSet<Discussion>>();
            context = new Mock<IApplicationDbContext>();
            stringLocalizer = new Mock<IStringLocalizer<PostsResource>>();
            mapper = new Mock<IMapper>();

            postDto = new PostCreateDto { Id = id };
            post = new Post { Id = id };
            discussion = new Discussion { Id = id, IsClosed = false };
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            dbSetDiscussion.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Discussion>(Task.FromResult(discussion)));
            context.Setup(x => x.Posts).Returns(dbSetPost.Object);
            context.Setup(x => x.Discussions).Returns(dbSetDiscussion.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            CreatePostCommandHandler createPostCommandHandler = new CreatePostCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            CreatePostCommand createPostCommand = new CreatePostCommand(postDto);

            mapper.Setup(x => x.Map<Post>(createPostCommand)).Returns(post);

            var result = await createPostCommandHandler.Handle(createPostCommand, new CancellationToken());

            result.Should().Be(id);
        }

        [Test]
        public void ShouldNotCallHandleIfDiscussionNotExist()
        {
            dbSetDiscussion.Setup(x => x.FindAsync(id)).Returns(null);
            context.Setup(x => x.Posts).Returns(dbSetPost.Object);
            context.Setup(x => x.Discussions).Returns(dbSetDiscussion.Object);

            CreatePostCommandHandler createPostCommandHandler = new CreatePostCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            CreatePostCommand createPostCommand = new CreatePostCommand(postDto);

            Func<Task> act = async () => await createPostCommandHandler.Handle(createPostCommand, new CancellationToken());

            act.Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public void ShouldNotCallHandleIfDiscussionIsClosed()
        {
            discussion.IsClosed = true;
            dbSetDiscussion.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Discussion>(Task.FromResult(discussion)));
            context.Setup(x => x.Posts).Returns(dbSetPost.Object);
            context.Setup(x => x.Discussions).Returns(dbSetDiscussion.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            CreatePostCommandHandler createPostCommandHandler = new CreatePostCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            CreatePostCommand createPostCommand = new CreatePostCommand(postDto);

            mapper.Setup(x => x.Map<Post>(createPostCommand)).Returns(post);

            Func<Task> act = async () => await createPostCommandHandler.Handle(createPostCommand, new CancellationToken());

            act.Should().ThrowAsync<RestException>();
        }

        [Test]
        public void ShouldNotCallHandleIfNotSavedChanges()
        {
            dbSetDiscussion.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Discussion>(Task.FromResult(discussion)));
            context.Setup(x => x.Posts).Returns(dbSetPost.Object);
            context.Setup(x => x.Discussions).Returns(dbSetDiscussion.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            CreatePostCommandHandler createPostCommandHandler = new CreatePostCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            CreatePostCommand createPostCommand = new CreatePostCommand(postDto);

            mapper.Setup(x => x.Map<Post>(createPostCommand)).Returns(post);

            Func<Task> act = async () => await createPostCommandHandler.Handle(createPostCommand, new CancellationToken());

            act.Should().ThrowAsync<RestException>();
        }
    }
}
