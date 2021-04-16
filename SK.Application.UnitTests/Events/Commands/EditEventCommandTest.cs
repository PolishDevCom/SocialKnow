﻿using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.Events;
using SK.Application.Events.Commands;
using SK.Application.Events.Commands.EditEvent;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.Events.Commands
{
    public class EditEventCommandTest
    {
        private readonly Guid id;
        private readonly Mock<DbSet<Event>> dbSetEvent;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IStringLocalizer<EventsResource>> stringLocalizer;
        private readonly Mock<IMapper> mapper;

        private readonly Event eventEntity;
        private readonly EventCreateOrEditDto eventDto;

        public EditEventCommandTest()
        {
            id = new Guid();
            dbSetEvent = new Mock<DbSet<Event>>();
            context = new Mock<IApplicationDbContext>();
            stringLocalizer = new Mock<IStringLocalizer<EventsResource>>();
            mapper = new Mock<IMapper>();

            eventEntity = new Event { Id = id };
            eventDto = new EventCreateOrEditDto { Id = id };
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            dbSetEvent.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Event>(Task.FromResult(eventEntity)));
            context.Setup(x => x.Events).Returns(dbSetEvent.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));
            mapper.Setup(x => x.Map<Event>(eventDto)).Returns(eventEntity);

            EditEventCommandHandler editEventCommandHandler = new EditEventCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            EditEventCommand editEventCommand = new EditEventCommand(eventDto);

            var result = await editEventCommandHandler.Handle(editEventCommand, new CancellationToken());

            result.Should().Be(Unit.Value);
        }

        [Test]
        public void ShouldNotCallHandleIfEventNotExist()
        {
            dbSetEvent.Setup(x => x.FindAsync(id)).Returns(null);
            context.Setup(x => x.Events).Returns(dbSetEvent.Object);

            EditEventCommandHandler editEventCommandHandler = new EditEventCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            EditEventCommand editEventCommand = new EditEventCommand(eventDto);

            Func<Task> act = async() => await editEventCommandHandler.Handle(editEventCommand, new CancellationToken());

            act.Should().Throw<NotFoundException>();
        }

        [Test]
        public void ShouldNotCallHandleIfNotSavedChanges()
        {
            dbSetEvent.Setup(x => x.FindAsync(id)).Returns(new ValueTask<Event>(Task.FromResult(eventEntity)));
            context.Setup(x => x.Events).Returns(dbSetEvent.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));
            mapper.Setup(x => x.Map<Event>(eventDto)).Returns(eventEntity);

            EditEventCommandHandler editEventCommandHandler = new EditEventCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            EditEventCommand editEventCommand = new EditEventCommand(eventDto);

            Func<Task> act = async () => await editEventCommandHandler.Handle(editEventCommand, new CancellationToken());

            act.Should().Throw<RestException>();
        }
    }
}
