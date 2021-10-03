using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.User.Commands.RegisterUser;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.User.Commands
{
    using static Testing;

    public class RegisterUserTests : TestBase
    {
        [Test]
        public async Task ShouldRegisterUser()
        {
            //arrange
            var command = new RegisterUserCommand()
            {
                Username = "Scott101",
                Email = "scott@localhost",
                Password = "Pa$$w0rd!"
            };
            //act
            var registeredUser = await SendAsync(command);

            //assert
            registeredUser.Username.Should().Be(command.Username);
            registeredUser.Image.Should().BeNull();
        }

        [Test]
        public async Task ShouldHaveStandardRole()
        {
            //arrange
            await SeedRoles();
            var command = new RegisterUserCommand()
            {
                Username = "Scott101",
                Email = "scott@localhost",
                Password = "Pa$$w0rd!"
            };
            //act
            var registeredUser = await SendAsync(command);
            var result = await GetRolesForUserAsync(registeredUser.Username);

            //assert
            result.Should().HaveCount(1);
            result.Should().Contain("Standard");
        }

        [Test]
        public void ShouldHaveErrorWhenNameIsNotProvided()
        {
            //arrange
            var command = new RegisterUserCommand()
            {
                Email = "scott@localhost",
                Password = "Pa$$w0rd!"
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public void ShouldHaveErrorWhenEmailIsNotProvided()
        {
            //arrange
            var command = new RegisterUserCommand()
            {
                Username = "Scott101",
                Password = "Pa$$w0rd!"
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public void ShouldHaveErrorWhenPasswordIsNotProvided()
        {
            //arrange
            var command = new RegisterUserCommand()
            {
                Username = "Scott101",
                Email = "scott@localhost"
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public void ShouldHaveErrorWhenEmailHasWrongFormat()
        {
            //arrange
            var command = new RegisterUserCommand()
            {
                Username = "Scott101",
                Email = "scottlocalhost",
                Password = "Pa$$w0rd!"
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public async Task ShouldHaveErrorWhenEmailAlreadyExists()
        {
            //arrange
            var command1 = new RegisterUserCommand()
            {
                Username = "Scott101",
                Email = "scott@localhost",
                Password = "Pa$$w0rd!"
            };

            var command2 = new RegisterUserCommand()
            {
                Username = "Scott102",
                Email = "scott@localhost",
                Password = "Pa$$w0rd!"
            };
            //act
            await SendAsync(command1);

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command2)).Should().ThrowAsync<RestException>();
        }

        [Test]
        public async Task ShouldHaveErrorWhenUsernameAlreadyExists()
        {
            //arrange
            var command1 = new RegisterUserCommand()
            {
                Username = "Scott101",
                Email = "scott@localhost",
                Password = "Pa$$w0rd!"
            };

            var command2 = new RegisterUserCommand()
            {
                Username = "Scott101",
                Email = "scott1@localhost",
                Password = "Pa$$w0rd!"
            };
            //act
            await SendAsync(command1);

            //assert
            FluentActions.Invoking(() =>
                SendAsync(command2)).Should().ThrowAsync<RestException>();
        }
    }
}