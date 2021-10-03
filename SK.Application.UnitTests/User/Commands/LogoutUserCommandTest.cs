using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SK.Application.Common.Interfaces;
using SK.Application.User.Commands.LogoutUser;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.User.Commands
{
    public class LogoutUserCommandTest
    {
        private readonly Mock<ITokenManager> tokenManager;

        public LogoutUserCommandTest()
        {
            tokenManager = new Mock<ITokenManager>();
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            LogoutUserCommandHandler logoutUserCommandHandler = new LogoutUserCommandHandler(tokenManager.Object);
            LogoutUserCommand logoutUserCommand = new LogoutUserCommand();

            var result = await logoutUserCommandHandler.Handle(logoutUserCommand, new CancellationToken());

            result.Should().Be(Unit.Value);
        }
    }
}