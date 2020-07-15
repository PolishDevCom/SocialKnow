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
    public class UnhandledExceptionBehaviourTests
    {
        private readonly Mock<ILogger<CreateTestValueCommand.Command>> _logger;
        private readonly Mock<RequestHandlerDelegate<CreateTestValueCommand.Handler>> _response;


        public UnhandledExceptionBehaviourTests()
        {
            _logger = new Mock<ILogger<CreateTestValueCommand.Command>>();
            _response = new Mock<RequestHandlerDelegate<CreateTestValueCommand.Handler>>();
            _response.Setup(r => r.Invoke()).Callback(() => throw new Exception());
        }

        [Test]
        public async Task ShouldLogError()
        {
            var requestLogger = new UnhandledExceptionBehaviour<CreateTestValueCommand.Command, CreateTestValueCommand.Handler>(_logger.Object);
            try
            {
                await requestLogger.Handle(new CreateTestValueCommand.Command { Id = 123, Name = "Test" }, new CancellationToken(), _response.Object);
            }
            catch (Exception)
            {
                _logger.Verify(
                l => l.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                    Times.Once
                );
            }
        }
    }
}
