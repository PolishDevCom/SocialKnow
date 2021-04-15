using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Models;
using SK.Application.Common.Resources.Users;
using SK.Application.User.Commands;
using SK.Application.User.Commands.RemoveRoleFromUser;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.User.Commands
{
    public class RemoveRoleFromUserCommandTest
    {
        private readonly Mock<IIdentityService> identityService;
        private readonly Mock<IStringLocalizer<UsersResource>> stringLocalizer;
        private readonly Mock<RoleManager<IdentityRole>> roleManager;

        private readonly AppUser user;
        private readonly UserAndRoleDto userDto;

        public RemoveRoleFromUserCommandTest()
        {
            user = new Faker<AppUser>("en")
                .RuleFor(u => u.UserName, f => f.Random.String(10))
                .Generate();

            userDto = new Faker<UserAndRoleDto>("en")
                .RuleFor(u => u.Username, f => f.Random.String(10))
                .RuleFor(u => u.Role, f => f.Random.String(10))
                .Generate();

            identityService = new Mock<IIdentityService>();
            stringLocalizer = new Mock<IStringLocalizer<UsersResource>>();
            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            roleManager = new Mock<RoleManager<IdentityRole>>(roleStore.Object, null, null, null, null);
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            //Arrange
            identityService.Setup(x => x.GetUserByUsernameAsync(user.UserName)).Returns(Task.FromResult(user));

            RemoveRoleFromUserCommandHandler removeRoleFromUserCommandHandler = new RemoveRoleFromUserCommandHandler(identityService.Object, roleManager.Object, stringLocalizer.Object);
            RemoveRoleFromUserCommand removeRoleFromUserCommand = new RemoveRoleFromUserCommand(userDto);

            roleManager.Setup(x => x.RoleExistsAsync(removeRoleFromUserCommand.Role)).Returns(Task.FromResult(true));
            identityService.Setup(x => x.RemoveRoleFromUserAsync(user, removeRoleFromUserCommand.Role)).Returns(Task.FromResult(Result.Success()));

            //Act
            var result = await removeRoleFromUserCommandHandler.Handle(removeRoleFromUserCommand, new CancellationToken());

            //Assert
            result.Succeeded.Should().BeTrue();
        }

        [Test]
        public void ShouldNotCallHandleIfUserNotExist()
        {
            //Arrange
            identityService.Setup(x => x.GetUserByUsernameAsync(user.UserName)).Returns(Task.FromResult((AppUser)null));

            RemoveRoleFromUserCommandHandler removeRoleFromUserCommandHandler = new RemoveRoleFromUserCommandHandler(identityService.Object, roleManager.Object, stringLocalizer.Object);
            RemoveRoleFromUserCommand removeRoleFromUserCommand = new RemoveRoleFromUserCommand(userDto);

            //Act
            Func<Task> act = async () => await removeRoleFromUserCommandHandler.Handle(removeRoleFromUserCommand, new CancellationToken());

            //Assert
            act.Should().Throw<NotFoundException>();
        }

        [Test]
        public void ShouldNotCallHandleIfRoleNotExist()
        {
            //Arrange
            identityService.Setup(x => x.GetUserByUsernameAsync(user.UserName)).Returns(Task.FromResult(user));

            RemoveRoleFromUserCommandHandler removeRoleFromUserCommandHandler = new RemoveRoleFromUserCommandHandler(identityService.Object, roleManager.Object, stringLocalizer.Object);
            RemoveRoleFromUserCommand removeRoleFromUserCommand = new RemoveRoleFromUserCommand(userDto);

            roleManager.Setup(x => x.RoleExistsAsync(removeRoleFromUserCommand.Role)).Returns(Task.FromResult(false));

            //Act
            Func<Task> act = async () => await removeRoleFromUserCommandHandler.Handle(removeRoleFromUserCommand, new CancellationToken());

            //Assert
            act.Should().Throw<RestException>();
        }
    }
}