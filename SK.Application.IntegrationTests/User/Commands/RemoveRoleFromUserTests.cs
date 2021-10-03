using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.User.Commands.AddRoleToUser;
using SK.Application.User.Commands.RegisterUser;
using SK.Application.User.Commands.RemoveRoleFromUser;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.User.Commands
{
    using static Testing;
    public class RemoveRoleFromUserTests : TestBase
    {
        [Test]
        public async Task ShouldRemoveRoleFromUser()
        {
            //arrange
            await SeedRoles();
            var registerUserCommand = new RegisterUserCommand()
            {
                Username = "Scott101",
                Email = "scott@localhost",
                Password = "Pa$$w0rd!"
            };
            var user = await SendAsync(registerUserCommand);

            var addRoleCommand = new AddRoleToUserCommand()
            {
                Role = "Premium",
                Username = user.Username
            };
            await SendAsync(addRoleCommand);

            var removeRoleCommand = new RemoveRoleFromUserCommand()
            {
                Role = "Premium",
                Username = user.Username
            };


            //act
            await SendAsync(removeRoleCommand);
            var result = await GetRolesForUserAsync(user.Username);

            //assert
            result.Should().HaveCount(1);
            result.Should().NotContain("Premium");
        }

        [Test]
        public async Task ShouldThrowExceptionWhenWrongRole()
        {
            //arrange
            await SeedRoles();
            var registerUserCommand = new RegisterUserCommand()
            {
                Username = "Scott101",
                Email = "scott@localhost",
                Password = "Pa$$w0rd!"
            };
            var user = await SendAsync(registerUserCommand);

            var removeRoleCommand = new RemoveRoleFromUserCommand()
            {
                Role = "NotExistingRole",
                Username = user.Username
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(removeRoleCommand)).Should().ThrowAsync<RestException>();
        }

        [Test]
        public async Task ShouldThrowExceptionWhenWrongUser()
        {
            //arrange
            await SeedRoles();
            var registerUserCommand = new RegisterUserCommand()
            {
                Username = "Scott101",
                Email = "scott@localhost",
                Password = "Pa$$w0rd!"
            };
            var user = await SendAsync(registerUserCommand);

            var removeRoleCommand = new RemoveRoleFromUserCommand()
            {
                Role = "Standard",
                Username = "NotExistingUser"
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(removeRoleCommand)).Should().ThrowAsync<NotFoundException>();
        }
    }
}
