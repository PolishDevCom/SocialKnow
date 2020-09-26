using FluentAssertions.Common;
using MediatR;
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
    public class PerformanceBehaviourTests
    {
        private readonly Mock<ILogger<CreateArticleCommand>> _logger;
        private readonly Mock<RequestHandlerDelegate<CreateArticleCommandHandler>> _responseLong;
        private readonly Mock<RequestHandlerDelegate<CreateArticleCommandHandler>> _responseShort;
        private readonly Mock<ICurrentUserService> _currentUserService;
        private readonly Mock<IIdentityService> _identityService;

        public PerformanceBehaviourTests()
        {
            _logger = new Mock<ILogger<CreateArticleCommand>>();
            _responseLong = new Mock<RequestHandlerDelegate<CreateArticleCommandHandler>>();
            _responseLong.Setup(r => r.Invoke()).Callback(() => Thread.Sleep(1000));
            _responseShort = new Mock<RequestHandlerDelegate<CreateArticleCommandHandler>>();
            _responseShort.Setup(r => r.Invoke()).Callback(() => Thread.Sleep(100));
            _currentUserService = new Mock<ICurrentUserService>();
            _identityService = new Mock<IIdentityService>();
        }

        [Test]
        public async Task ShouldLogWarning()
        {
            var requestLogger = new PerformanceBehaviour<CreateArticleCommand, CreateArticleCommandHandler>(_logger.Object, _currentUserService.Object, _identityService.Object);
            await requestLogger.Handle(new CreateArticleCommand { Id = 123, Name = "Test" }, new CancellationToken(), _responseLong.Object);
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
            var requestLogger = new PerformanceBehaviour<CreateArticleCommand, CreateArticleCommandHandler>(_logger.Object, _currentUserService.Object, _identityService.Object);
            await requestLogger.Handle(new CreateArticleCommand { Id = 123, Name = "Test" }, new CancellationToken(), _responseShort.Object);
            _logger.Object.IsSameOrEqualTo(null);
        }
    }
}
