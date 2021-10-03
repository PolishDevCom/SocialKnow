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
        private readonly Mock<IIdentityService> identityService;
        private readonly Mock<IStringLocalizer<UsersResource>> stringLocalizer;
        private readonly Mock<RoleManager<IdentityRole>> roleManager;

        private readonly AppUser user;
        private readonly UserAndRoleDto userDto;

        public AddRoleToUserCommandTest()
        {
            user = new AppUser { UserName = "" };
            userDto = new UserAndRoleDto { Username = "", Role = "" };

            identityService = new Mock<IIdentityService>();
            stringLocalizer = new Mock<IStringLocalizer<UsersResource>>();
            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            roleManager = new Mock<RoleManager<IdentityRole>>(roleStore.Object, null, null, null, null);
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            identityService.Setup(x => x.GetUserByUsernameAsync(user.UserName)).Returns(Task.FromResult(user));

            AddRoleToUserCommandHandler addRoleToUserCommandHandler = new AddRoleToUserCommandHandler(identityService.Object, roleManager.Object, stringLocalizer.Object);
            AddRoleToUserCommand addRoleToUserCommand = new AddRoleToUserCommand(userDto);

            roleManager.Setup(x => x.RoleExistsAsync(addRoleToUserCommand.Role)).Returns(Task.FromResult(true));
            identityService.Setup(x => x.AddRoleToUserAsync(user, addRoleToUserCommand.Role)).Returns(Task.FromResult(Result.Success()));

            var result = await addRoleToUserCommandHandler.Handle(addRoleToUserCommand, new CancellationToken());

            result.Succeeded.Should().BeTrue();
        }

        [Test]
        public void ShouldNotCallHandleIfUserNotExist()
        {
            identityService.Setup(x => x.GetUserByUsernameAsync(user.UserName)).Returns(Task.FromResult((AppUser)null));

            AddRoleToUserCommandHandler addRoleToUserCommandHandler = new AddRoleToUserCommandHandler(identityService.Object, roleManager.Object, stringLocalizer.Object);
            AddRoleToUserCommand addRoleToUserCommand = new AddRoleToUserCommand(userDto);

            Func<Task> act = async () => await addRoleToUserCommandHandler.Handle(addRoleToUserCommand, new CancellationToken());

            act.Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public void ShouldNotCallHandleIfRoleNotExist()
        {
            identityService.Setup(x => x.GetUserByUsernameAsync(user.UserName)).Returns(Task.FromResult(user));

            AddRoleToUserCommandHandler addRoleToUserCommandHandler = new AddRoleToUserCommandHandler(identityService.Object, roleManager.Object, stringLocalizer.Object);
            AddRoleToUserCommand addRoleToUserCommand = new AddRoleToUserCommand(userDto);

            roleManager.Setup(x => x.RoleExistsAsync(addRoleToUserCommand.Role)).Returns(Task.FromResult(false));

            Func<Task> act = async() => await addRoleToUserCommandHandler.Handle(addRoleToUserCommand, new CancellationToken());

            act.Should().ThrowAsync<RestException>();
        }
    }
}