using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.User.Commands.DeleteUser;
using SK.Application.User.Commands.RegisterUser;
using System.Threading.Tasks;

namespace SK.Application.IntegrationTests.User.Commands
{

    using static Testing;
    public class DeleteUserTests : TestBase
    {
        [Test]
        public async Task ShouldDeleteUser()
        {
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");
            var deleteCommand = new DeleteUserCommand()
            {
                Username = loggedUser
            };

            //act
            var deleteUserStatus = await SendAsync(deleteCommand);

            //assert
            deleteUserStatus.Succeeded.Should().BeTrue();
            deleteUserStatus.Errors.Should().BeEmpty();
        }

        [Test]
        public async Task ShouldHaveErrorWhenTryingToDeleteOtherAccount()
        {
            //arrange
            var registerCommand = new RegisterUserCommand()
            {
                Username = "Jane101",
                Email = "jane101@localhost",
                Password = "Pa$$w0rd!"
            };
            await SendAsync(registerCommand);

            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");
            var deleteCommand = new DeleteUserCommand()
            {
                Username = "Jane101"
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(deleteCommand)).Should().ThrowAsync<RestException>();
        }

        [Test]
        public async Task ShouldHaveErrorWhenTryingToDeleteNotExistingAccount()
        {
            //arrange
            var loggedUser = await RunAsUserAsync("scott101@localhost", "Pa$$w0rd!");
            var deleteCommand = new DeleteUserCommand()
            {
                Username = "Jane101"
            };

            //act

            //assert
            FluentActions.Invoking(() =>
                SendAsync(deleteCommand)).Should().ThrowAsync<NotFoundException>();
        }
    }
}
