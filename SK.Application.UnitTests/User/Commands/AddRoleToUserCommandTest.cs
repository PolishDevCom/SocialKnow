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
using SK.Application.User.Commands.AddRoleToUser;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.User.Commands
{
    public class AddRoleToUserCommandTest
    {
        private const string userName = "User";
        private const string role = "User";

        private readonly Mock<IIdentityService> identityService;
        private readonly Mock<IStringLocalizer<UsersResource>> stringLocalizer;
        private readonly Mock<RoleManager<IdentityRole>> roleManager;

        private readonly AppUser user;
        private readonly UserAndRoleDto userDto;

        public AddRoleToUserCommandTest()
        {
            identityService = new Mock<IIdentityService>();
            stringLocalizer = new Mock<IStringLocalizer<UsersResource>>();
            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            roleManager = new Mock<RoleManager<IdentityRole>>(roleStore.Object, null, null, null, null);

            user = new AppUser { UserName = userName };
            userDto = new UserAndRoleDto { Role = role, Username = userName };
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            //Arrange
            identityService.Setup(x => x.GetUserByUsernameAsync(userName)).Returns(Task.FromResult(user));

            AddRoleToUserCommandHandler addRoleToUserCommandHandler = new AddRoleToUserCommandHandler(identityService.Object, roleManager.Object, stringLocalizer.Object);
            AddRoleToUserCommand addRoleToUserCommand = new AddRoleToUserCommand(userDto);

            roleManager.Setup(x => x.RoleExistsAsync(addRoleToUserCommand.Role)).Returns(Task.FromResult(true));
            identityService.Setup(x => x.AddRoleToUserAsync(user, addRoleToUserCommand.Role)).Returns(Task.FromResult(Result.Success()));

            //Act
            var result = await addRoleToUserCommandHandler.Handle(addRoleToUserCommand, new CancellationToken());

            //Assert
            result.Succeeded.Should().BeTrue();
        }

        [Test]
        public void ShouldNotCallHandleIfUserNotExist()
        {
            //Arrange
            identityService.Setup(x => x.GetUserByUsernameAsync(userName)).Returns(Task.FromResult((AppUser)null));

            AddRoleToUserCommandHandler addRoleToUserCommandHandler = new AddRoleToUserCommandHandler(identityService.Object, roleManager.Object, stringLocalizer.Object);
            AddRoleToUserCommand addRoleToUserCommand = new AddRoleToUserCommand(userDto);

            //Act
            Func<Task> act = async () => await addRoleToUserCommandHandler.Handle(addRoleToUserCommand, new CancellationToken());

            //Assert
            act.Should().Throw<NotFoundException>();
        }

        [Test]
        public void ShouldNotCallHandleIfRoleNotExist()
        {
            //Arrange
            identityService.Setup(x => x.GetUserByUsernameAsync(userName)).Returns(Task.FromResult(user));

            AddRoleToUserCommandHandler addRoleToUserCommandHandler = new AddRoleToUserCommandHandler(identityService.Object, roleManager.Object, stringLocalizer.Object);
            AddRoleToUserCommand addRoleToUserCommand = new AddRoleToUserCommand(userDto);

            roleManager.Setup(x => x.RoleExistsAsync(addRoleToUserCommand.Role)).Returns(Task.FromResult(false));

            //Act
            Func<Task> act = async() => await addRoleToUserCommandHandler.Handle(addRoleToUserCommand, new CancellationToken());

            //Assert
            act.Should().Throw<RestException>();
        }
    }
}