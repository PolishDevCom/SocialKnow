using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SK.Application.Common.Behaviours;
using SK.Application.TestValues.Commands.Create;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Common.Behaviours
{
    public class LoggingBehaviourTests
    {
        private readonly Mock<ILogger<CreateTestValueCommand.Command>> _logger;

        public LoggingBehaviourTests()
        {
            _logger = new Mock<ILogger<CreateTestValueCommand.Command>>();
        }

        [Test]
        public async Task ShouldLogRequest()
        {
            var requestLogger = new LoggingBehaviour<CreateTestValueCommand.Command>(_logger.Object);
            await requestLogger.Process(new CreateTestValueCommand.Command { Id = 123, Name = "Test" }, new CancellationToken());
            _logger.Verify(
                l => l.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("SocialKnow Request:")),
                        It.IsAny<Exception>(),
                        It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true))
                );
        }
    }
}
