using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Discussions;
using SK.Application.Discussions.Commands;
using SK.Application.Discussions.Commands.CreateDiscussion;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Discussions.Commands
{
    public class CreateDiscussionCommandTest
    {
        private readonly Guid id;
        private readonly Mock<DbSet<Discussion>> dbSetDiscussion;
        private readonly Mock<DbSet<Post>> dbSetPost;
        private readonly Mock<DbSet<Category>> dbSetCategory;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IStringLocalizer<DiscussionsResource>> stringLocalizer;
        private readonly Mock<IMapper> mapper;

        private readonly Discussion discussion;
        private readonly DiscussionCreateDto discussionDto;

        public CreateDiscussionCommandTest()
        {
            id = new Guid();
            dbSetDiscussion = new Mock<DbSet<Discussion>>();
            dbSetPost = new Mock<DbSet<Post>>();
            dbSetCategory = new Mock<DbSet<Category>>();
            context = new Mock<IApplicationDbContext>();
            stringLocalizer = new Mock<IStringLocalizer<DiscussionsResource>>();
            mapper = new Mock<IMapper>();

            discussion = new Discussion { Id = id };
            discussionDto = new DiscussionCreateDto { Id = id, PostBody = It.IsAny<string>() };
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            context.Setup(x => x.Discussions).Returns(dbSetDiscussion.Object);
            context.Setup(x => x.Posts).Returns(dbSetPost.Object);
            context.Setup(x => x.Categories).Returns(dbSetCategory.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));


            CreateDiscussionCommandHandler createDiscussionCommandHandler = new CreateDiscussionCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            CreateDiscussionCommand createDiscussionCommand = new CreateDiscussionCommand(discussionDto);

            mapper.Setup(x => x.Map<Discussion>(createDiscussionCommand)).Returns(discussion);

            var result = await createDiscussionCommandHandler.Handle(createDiscussionCommand, new CancellationToken());

            result.Should().Be(id);
        }

        [Test]
        public void ShouldNotCallHandleIfNotSavedChanges()
        {
            context.Setup(x => x.Discussions).Returns(dbSetDiscussion.Object);
            context.Setup(x => x.Posts).Returns(dbSetPost.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            CreateDiscussionCommandHandler createDiscussionCommandHandler = new CreateDiscussionCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            CreateDiscussionCommand createDiscussionCommand = new CreateDiscussionCommand(discussionDto);

            mapper.Setup(x => x.Map<Discussion>(createDiscussionCommand)).Returns(discussion);

            Func<Task> act = async () => await createDiscussionCommandHandler.Handle(createDiscussionCommand, new CancellationToken());

            act.Should().Throw<RestException>();
        }
    }
}
