using AutoMapper;
using Bogus;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Events;
using SK.Application.Common.Resources.Profiles;
using SK.Application.Events.Commands;
using SK.Application.Events.Commands.EditEvent;
using SK.Application.Events.Queries;
using SK.Application.Profiles.Commands;
using SK.Application.Profiles.Commands.EditProfile;
using SK.Domain.Entities;
using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Profiles.Commands
{
    public class EditProfileCommandTest
    {
        private readonly Mock<DbSet<AppUser>> dbSetUser;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IStringLocalizer<ProfilesResource>> stringLocalizer;
        private readonly Mock<IMapper> mapper;
        private readonly ICurrentUserService currentUserService;

        private readonly AppUser user;
        private readonly ProfileCreateOrEditDto profileDto;

        public EditProfileCommandTest()
        {
            user = new Faker<AppUser>("en")
                .RuleFor(u => u.UserName, f => f.Random.String(10))
                .Generate();

            profileDto = new ProfileCreateOrEditDto();

            dbSetUser = new Mock<DbSet<AppUser>>();
            context = new Mock<IApplicationDbContext>();
            stringLocalizer = new Mock<IStringLocalizer<ProfilesResource>>();
            mapper = new Mock<IMapper>();
            currentUserService = Mock.Of<ICurrentUserService>(x => x.Username == user.UserName);
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            dbSetUser.Setup(x => x.FindAsync(user.UserName)).Returns(new ValueTask<AppUser>(Task.FromResult(user)));
            mapper.Setup(x => x.Map<AppUser>(profileDto)).Returns(user);
            context.Setup(x => x.Users).Returns(dbSetUser.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            EditProfileCommandHandler editProfileCommandHandler = new EditProfileCommandHandler(context.Object, stringLocalizer.Object, mapper.Object, currentUserService);
            EditProfileCommand editProfileCommand = new EditProfileCommand(profileDto);

            var result = await editProfileCommandHandler.Handle(editProfileCommand, new CancellationToken());

            result.Should().Be(Unit.Value);
        }

        [Test]
        public void ShouldNotCallHandleIfNotSavedChanges()
        {
            dbSetUser.Setup(x => x.FindAsync(user.UserName)).Returns(new ValueTask<AppUser>(Task.FromResult(user)));
            mapper.Setup(x => x.Map<AppUser>(profileDto)).Returns(user);
            context.Setup(x => x.Users).Returns(dbSetUser.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            EditProfileCommandHandler editProfileCommandHandler = new EditProfileCommandHandler(context.Object, stringLocalizer.Object, mapper.Object, currentUserService);
            EditProfileCommand editProfileCommand = new EditProfileCommand(profileDto);

            Func<Task> act = async () => await editProfileCommandHandler.Handle(editProfileCommand, new CancellationToken());
            act.Should().Throw<RestException>();
        }

        [Test]
        public void ShouldNotCallHandleIfProfileNotExist()
        {
            dbSetUser.Setup(x => x.FindAsync(user.UserName)).Returns(null);
            context.Setup(x => x.Users).Returns(dbSetUser.Object);

            EditProfileCommandHandler editProfileCommandHandler = new EditProfileCommandHandler(context.Object, stringLocalizer.Object, mapper.Object, currentUserService);
            EditProfileCommand editProfileCommand = new EditProfileCommand(profileDto);

            Func<Task> act = async () => await editProfileCommandHandler.Handle(editProfileCommand, new CancellationToken());

            act.Should().Throw<NotFoundException>();
        }
    }
}
