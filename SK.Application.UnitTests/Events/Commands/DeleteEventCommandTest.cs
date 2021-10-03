using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Events;
using SK.Application.Events.Commands.DeleteEvent;
using SK.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Events.Commands
{
    public class DeleteEventCommandTest
    {
        private readonly Guid id;
        private readonly Mock<DbSet<Event>> dbSetEvent;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IStringLocalizer<EventsResource>> stringLocalizer;

        public DeleteEventCommandTest()
        {
            id = new Guid();
            dbSetEvent = new Mock<DbSet<Event>>();
            context = new Mock<IApplicationDbContext>();
            stringLocalizer = new Mock<IStringLocalizer<EventsResource>>();
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            dbSetEvent.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Event>(Task.FromResult(new Event { Id = id })));
            context.Setup(x => x.Events).Returns(dbSetEvent.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            DeleteEventCommandHandler deleteEventCommandHandler = new DeleteEventCommandHandler(context.Object, stringLocalizer.Object);
            DeleteEventCommand deleteEventCommand = new DeleteEventCommand(id);

            var result = await deleteEventCommandHandler.Handle(deleteEventCommand, new CancellationToken());

            result.Should().Be(Unit.Value);
        }

        [Test]
        public void ShouldNotCallHandleIfEventNotExist()
        {
            dbSetEvent.Setup(x => x.FindAsync(id)).Returns(null);
            context.Setup(x => x.Events).Returns(dbSetEvent.Object);

            DeleteEventCommandHandler deleteEventCommandHandler = new DeleteEventCommandHandler(context.Object, stringLocalizer.Object);
            DeleteEventCommand deleteEventCommand = new DeleteEventCommand(id);

            Func<Task> act = async () => await deleteEventCommandHandler.Handle(deleteEventCommand, new CancellationToken());

            act.Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public void ShouldNotCallHandleIfNotSavedChanges()
        {
            dbSetEvent.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Event>(Task.FromResult(new Event { Id = id })));
            context.Setup(x => x.Events).Returns(dbSetEvent.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            DeleteEventCommandHandler deleteEventCommandHandler = new DeleteEventCommandHandler(context.Object, stringLocalizer.Object);
            DeleteEventCommand deleteEventCommand = new DeleteEventCommand(id);

            Func<Task> act = async () => await deleteEventCommandHandler.Handle(deleteEventCommand, new CancellationToken());

            act.Should().ThrowAsync<RestException>();
        }
    }
}