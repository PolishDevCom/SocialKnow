using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Models;
using SK.Application.Common.Resources.Users;
using SK.Application.User.Commands.DeleteUser;
using SK.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.User.Commands
{
    public class DeleteUserCommandTest
    {
        private readonly Mock<IIdentityService> identityService;
        private readonly ICurrentUserService currentUserService;
        private readonly Mock<IStringLocalizer<UsersResource>> stringLocalizer;

        private readonly AppUser user;

        public DeleteUserCommandTest()
        {
            user = new Faker<AppUser>("en")
                .RuleFor(u => u.UserName, f => f.Random.String(10))
                .Generate();

            identityService = new Mock<IIdentityService>();
            currentUserService = Mock.Of<ICurrentUserService>(x => x.Username == user.UserName);
            stringLocalizer = new Mock<IStringLocalizer<UsersResource>>();
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            identityService.Setup(x => x.GetUserByUsernameAsync(user.UserName)).Returns(Task.FromResult(user));
            identityService.Setup(x => x.DeleteUserAsync(user.UserName)).Returns(Task.FromResult(Result.Success()));

            DeleteUserCommandHandler deleteUserCommandHandler = new DeleteUserCommandHandler(identityService.Object, currentUserService, stringLocalizer.Object);
            DeleteUserCommand deleteUserCommand = new DeleteUserCommand(user.UserName);

            var result = await deleteUserCommandHandler.Handle(deleteUserCommand, new CancellationToken());

            result.Succeeded.Should().BeTrue();
        }

        [Test]
        public void ShouldNotCallHandleIfUserNotExist()
        {
            identityService.Setup(x => x.GetUserByUsernameAsync(user.UserName)).Returns(Task.FromResult((AppUser)null));

            DeleteUserCommandHandler deleteUserCommandHandler = new DeleteUserCommandHandler(identityService.Object, currentUserService, stringLocalizer.Object);
            DeleteUserCommand deleteUserCommand = new DeleteUserCommand(user.UserName);

            Func<Task> act = async () => await deleteUserCommandHandler.Handle(deleteUserCommand, new CancellationToken());

            act.Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public void ShouldNotCallHandleIfCurrentUserNotMatch()
        {
            user.UserName = It.IsAny<string>();
            identityService.Setup(x => x.GetUserByUsernameAsync(user.UserName)).Returns(Task.FromResult(user));
            identityService.Setup(x => x.DeleteUserAsync(user.UserName)).Returns(Task.FromResult(Result.Success()));

            DeleteUserCommandHandler deleteUserCommandHandler = new DeleteUserCommandHandler(identityService.Object, currentUserService, stringLocalizer.Object);
            DeleteUserCommand deleteUserCommand = new DeleteUserCommand(user.UserName);

            Func<Task> act = async () => await deleteUserCommandHandler.Handle(deleteUserCommand, new CancellationToken());

            act.Should().ThrowAsync<RestException>();
        }
    }
}