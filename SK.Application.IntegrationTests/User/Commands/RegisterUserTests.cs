using FluentAssertions;
using FluentValidation.TestHelper;
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
        public void ShouldHaveErrorWhenNameIsNotProvided()
        {
            //arrange
            var command = new RegisterUserCommand()
            {
                Email = "scott@localhost",
                Password = "Pa$$w0rd!"
            };
            var validator = new RegisterUserCommandValidator();

            //act
            var result = validator.TestValidate(command);

            //assert
            result.ShouldHaveValidationErrorFor(command => command.Username);
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
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
            var validator = new RegisterUserCommandValidator();

            //act
            var result = validator.TestValidate(command);

            //assert
            result.ShouldHaveValidationErrorFor(command => command.Email);
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
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
            var validator = new RegisterUserCommandValidator();

            //act
            var result = validator.TestValidate(command);

            //assert
            result.ShouldHaveValidationErrorFor(command => command.Password);
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
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
            var validator = new RegisterUserCommandValidator();

            //act
            var result = validator.TestValidate(command);

            //assert
            result.ShouldHaveValidationErrorFor(command => command.Email);
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
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
                SendAsync(command2)).Should().Throw<RestException>();
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
                SendAsync(command2)).Should().Throw<RestException>();
        }
    }
}
