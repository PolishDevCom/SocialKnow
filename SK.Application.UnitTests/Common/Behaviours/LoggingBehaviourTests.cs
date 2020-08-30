using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SK.Application.Common.Behaviours;
using SK.Application.Common.Interfaces;
using SK.Application.TestValues.Commands.CreateTestValue;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Common.Behaviours
{
    public class LoggingBehaviourTests
    {
        private readonly Mock<ILogger<CreateTestValueCommand>> _logger;
        private readonly Mock<ICurrentUserService> _currentUserService;
        private readonly Mock<IIdentityService> _identityService;

        public LoggingBehaviourTests()
        {
            _logger = new Mock<ILogger<CreateTestValueCommand>>();
            _currentUserService = new Mock<ICurrentUserService>();
            _identityService = new Mock<IIdentityService>();
        }

        [Test]
        public async Task ShouldLogRequest()
        {
            var requestlogger = new LoggingBehaviour<CreateTestValueCommand>(_logger.Object, _currentUserService.Object, _identityService.Object);
            await requestlogger.Process(new CreateTestValueCommand { Id = 123, Name = "test" }, new CancellationToken());
            _logger.Verify(
                l => l.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("SocialKnow Request:")),
                        It.IsAny<Exception>(),
                        It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true))
                );
        }

        [Test]
        public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
        {
            _currentUserService.Setup(x => x.Username).Returns("Administrator");

            var requestLogger = new LoggingBehaviour<CreateTestValueCommand>(_logger.Object, _currentUserService.Object, _identityService.Object);

            await requestLogger.Process(new CreateTestValueCommand { Id = 123, Name = "test" }, new CancellationToken());

            _identityService.Verify(i => i.GetUserByUsernameAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
        {
            var requestLogger = new LoggingBehaviour<CreateTestValueCommand>(_logger.Object, _currentUserService.Object, _identityService.Object);

            await requestLogger.Process(new CreateTestValueCommand { Id = 123, Name = "test" }, new CancellationToken());

            _identityService.Verify(i => i.GetUserByUsernameAsync(null), Times.Never);
        }
    }
}
