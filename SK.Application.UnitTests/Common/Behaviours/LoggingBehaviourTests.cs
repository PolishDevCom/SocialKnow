using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SK.Application.Articles.Commands.CreateArticle;
using SK.Application.Common.Behaviours;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Common.Behaviours
{
    public class LoggingBehaviourTests
    {
        private readonly Mock<ILogger<CreateArticleCommand>> _logger;
        private readonly Mock<ICurrentUserService> _currentUserService;
        private readonly Mock<IIdentityService> _identityService;

        public LoggingBehaviourTests()
        {
            _logger = new Mock<ILogger<CreateArticleCommand>>();
            _currentUserService = new Mock<ICurrentUserService>();
            _identityService = new Mock<IIdentityService>();
        }

        [Test]
        public async Task ShouldLogRequest()
        {
            var requestlogger = new LoggingBehaviour<CreateArticleCommand>(_logger.Object, _currentUserService.Object, _identityService.Object);
            await requestlogger.Process(new CreateArticleCommand 
            {
                Id = Guid.NewGuid(),
                Title = "Article Title",
                Abstract = "Article Abstract",
                Image = null,
                Content = "Article Content"
            }, 
            new CancellationToken());
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
            _currentUserService.Setup(x => x.Username).Returns("bob101@localhost");
            _identityService.Setup(i => i.GetUserByUsernameAsync("bob101@localhost")).Returns(Task.FromResult(new AppUser { UserName = "bob101@localhost" }));

            var requestLogger = new LoggingBehaviour<CreateArticleCommand>(_logger.Object, _currentUserService.Object, _identityService.Object);

            await requestLogger.Process(new CreateArticleCommand
            {
                Id = Guid.NewGuid(),
                Title = "Article Title",
                Abstract = "Article Abstract",
                Image = null,
                Content = "Article Content"
            }, 
            new CancellationToken());

            _identityService.Verify(i => i.GetUserByUsernameAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
        {
            var requestLogger = new LoggingBehaviour<CreateArticleCommand>(_logger.Object, _currentUserService.Object, _identityService.Object);

            await requestLogger.Process(new CreateArticleCommand
            {
                Id = Guid.NewGuid(),
                Title = "Article Title",
                Abstract = "Article Abstract",
                Image = null,
                Content = "Article Content"
            }, 
            new CancellationToken());

            _identityService.Verify(i => i.GetUserByUsernameAsync(null), Times.Never);
        }
    }
}
