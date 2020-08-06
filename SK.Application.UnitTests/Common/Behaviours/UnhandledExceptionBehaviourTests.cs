using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SK.Application.Common.Behaviours;
using SK.Application.TestValues.Commands.CreateTestValue;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Common.Behaviours
{
    public class UnhandledExceptionBehaviourTests
    {
        private readonly Mock<ILogger<CreateTestValueCommand>> _logger;
        private readonly Mock<RequestHandlerDelegate<CreateTestValueCommandHandler>> _response;


        public UnhandledExceptionBehaviourTests()
        {
            _logger = new Mock<ILogger<CreateTestValueCommand>>();
            _response = new Mock<RequestHandlerDelegate<CreateTestValueCommandHandler>>();
            _response.Setup(r => r.Invoke()).Callback(() => throw new Exception());
        }

        [Test]
        public async Task ShouldLogError()
        {
            var requestLogger = new UnhandledExceptionBehaviour<CreateTestValueCommand, CreateTestValueCommandHandler>(_logger.Object);
            try
            {
                await requestLogger.Handle(new CreateTestValueCommand { Id = 123, Name = "Test" }, new CancellationToken(), _response.Object);
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
