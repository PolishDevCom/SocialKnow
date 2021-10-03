using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.User.Commands.RegisterUser;
using SK.Application.User.Queries.LoginUser;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.User.Queries
{
    using static Testing;

    public class LoginUserTests : TestBase
    {
        [Test]
        public async Task ShouldLoginUser()
        {
            //arrange
            var command = new RegisterUserCommand()
            {
                Username = "Scott101",
                Email = "scott@localhost",
                Password = "Pa$$w0rd!"
            };
            await SendAsync(command);

            var query = new LoginUserQuery()
            {
                Email = "scott@localhost",
                Password = "Pa$$w0rd!"
            };

            //act
            var loginUser = await SendAsync(query);

            //assert
            loginUser.Username.Should().Be(command.Username);
            loginUser.Image.Should().BeNull();
        }

        [Test]
        public void ShouldHaveErrorWhenEmailNotProvided()
        {
            //arrange
            var query = new LoginUserQuery()
            {
                Password = "Pa$$w0rd!"
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(query)).Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public void ShouldHaveErrorWhenPasswordNotProvided()
        {
            //arrange
            var query = new LoginUserQuery()
            {
                Email = "scott@localhost"
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(query)).Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public async Task ShouldHaveErrorWhenPasswordNotCorrect()
        {
            //arrange
            var command = new RegisterUserCommand()
            {
                Username = "Scott101",
                Email = "scott@localhost",
                Password = "Pa$$w0rd!"
            };
            var registeredUser = await SendAsync(command);

            var query = new LoginUserQuery()
            {
                Email = "scott@localhost",
                Password = "WrongPassword"
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(query)).Should().ThrowAsync<RestException>();
        }

        [Test]
        public async Task ShouldHaveErrorWhenEmailNotCorrect()
        {
            //arrange
            var command = new RegisterUserCommand()
            {
                Username = "Scott101",
                Email = "scott@localhost",
                Password = "Pa$$w0rd!"
            };
            var registeredUser = await SendAsync(command);

            var query = new LoginUserQuery()
            {
                Email = "scottnotagoodemail@localhost",
                Password = "Pa$$w0rd!"
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(query)).Should().ThrowAsync<RestException>();
        }
    }
}