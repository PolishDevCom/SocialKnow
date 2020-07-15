using FluentAssertions.Common;
using MediatR;
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
    public class PerformanceBehaviourTests
    {
        private readonly Mock<ILogger<CreateTestValueCommand.Command>> _logger;
        private readonly Mock<RequestHandlerDelegate<CreateTestValueCommand.Handler>> _responseLong;
        private readonly Mock<RequestHandlerDelegate<CreateTestValueCommand.Handler>> _responseShort;

        public PerformanceBehaviourTests()
        {
            _logger = new Mock<ILogger<CreateTestValueCommand.Command>>();
            _responseLong = new Mock<RequestHandlerDelegate<CreateTestValueCommand.Handler>>();
            _responseLong.Setup(r => r.Invoke()).Callback(() => Thread.Sleep(1000));
            _responseShort = new Mock<RequestHandlerDelegate<CreateTestValueCommand.Handler>>();
            _responseShort.Setup(r => r.Invoke()).Callback(() => Thread.Sleep(100));
        }

        [Test]
        public async Task ShouldLogWarning()
        {
            var requestLogger = new PerformanceBehaviour<CreateTestValueCommand.Command, CreateTestValueCommand.Handler>(_logger.Object);
            await requestLogger.Handle(new CreateTestValueCommand.Command { Id = 123, Name = "Test" }, new CancellationToken(), _responseLong.Object);
            _logger.Verify(
            l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once
            );
        }

        [Test]
        public async Task ShouldNotLogWarning()
        {
            var requestLogger = new PerformanceBehaviour<CreateTestValueCommand.Command, CreateTestValueCommand.Handler>(_logger.Object);
            await requestLogger.Handle(new CreateTestValueCommand.Command { Id = 123, Name = "Test" }, new CancellationToken(), _responseShort.Object);
            _logger.Object.IsSameOrEqualTo(null);
        }
    }
}
