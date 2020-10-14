using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.User.Commands.AddRoleToUser;
using SK.Application.User.Commands.RegisterUser;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.User.Commands
{

    using static Testing;
    public class AddRoleToUserTests : TestBase
    {
        [Test]
        public async Task ShouldAddRoleToUser()
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

            //act
            await SendAsync(addRoleCommand);
            var result = await GetRolesForUserAsync(user.Username);

            //assert
            result.Should().HaveCount(2);
            result.Should().Contain("Premium");
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

            var addRoleCommand = new AddRoleToUserCommand()
            {
                Role = "NotExistingRole",
                Username = user.Username
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(addRoleCommand)).Should().Throw<RestException>();
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

            var addRoleCommand = new AddRoleToUserCommand()
            {
                Role = "Premium",
                Username = "NotExistingUser"
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(addRoleCommand)).Should().Throw<NotFoundException>();
        }
    }
}
