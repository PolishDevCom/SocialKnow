using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SK.Application.Articles.Commands.CreateArticle;
using SK.Application.Common.Behaviours;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Common.Behaviours
{
    public class UnhandledExceptionBehaviourTests
    {
        private readonly Mock<ILogger<CreateArticleCommand>> _logger;
        private readonly Mock<RequestHandlerDelegate<CreateArticleCommandHandler>> _response;

        public UnhandledExceptionBehaviourTests()
        {
            _logger = new Mock<ILogger<CreateArticleCommand>>();
            _response = new Mock<RequestHandlerDelegate<CreateArticleCommandHandler>>();
            _response.Setup(r => r.Invoke()).Callback(() => throw new Exception());
        }

        [Test]
        public async Task ShouldLogError()
        {
            var requestLogger = new UnhandledExceptionBehaviour<CreateArticleCommand, CreateArticleCommandHandler>(_logger.Object);
            try
            {
                await requestLogger.Handle(new CreateArticleCommand
                {
                    Id = Guid.NewGuid(),
                    Title = "Article Title",
                    Abstract = "Article Abstract",
                    Image = null,
                    Content = "Article Content"
                },
                new CancellationToken(), _response.Object);
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