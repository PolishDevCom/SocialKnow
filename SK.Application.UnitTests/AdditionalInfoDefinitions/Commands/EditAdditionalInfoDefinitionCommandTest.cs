using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.AdditionalInfoDefinitions.Commands;
using SK.Application.AdditionalInfoDefinitions.Commands.EditAdditionalInfoDefinition;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.AdditionalInfoDefinitions;
using SK.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.AdditionalInfoDefinitions.Commands
{
    public class EditAdditionalInfoDefinitionCommandTest
    {
        private readonly Guid id = new Guid();

        private readonly Mock<DbSet<AdditionalInfoDefinition>> dbSetAdditionalInfoDefinition;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IStringLocalizer<AdditionalInfoDefinitionsResource>> stringLocalizer;
        private readonly Mock<IMapper> mapper;

        private readonly AdditionalInfoDefinition additionalInfoDefinition;
        private readonly AdditionalInfoDefinitionCreateOrEditDto additionalInfoDefinitionDto;

        public EditAdditionalInfoDefinitionCommandTest()
        {
            dbSetAdditionalInfoDefinition = new Mock<DbSet<AdditionalInfoDefinition>>();
            context = new Mock<IApplicationDbContext>();
            stringLocalizer = new Mock<IStringLocalizer<AdditionalInfoDefinitionsResource>>();
            mapper = new Mock<IMapper>();

            additionalInfoDefinitionDto = new AdditionalInfoDefinitionCreateOrEditDto { Id = id };
            additionalInfoDefinition = new AdditionalInfoDefinition { Id = id };
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            dbSetAdditionalInfoDefinition.Setup(x => x.FindAsync(id)).Returns(new ValueTask<AdditionalInfoDefinition>(Task.FromResult(additionalInfoDefinition)));
            context.Setup(x => x.AdditionalInfoDefinitions).Returns(dbSetAdditionalInfoDefinition.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            EditAdditionalInfoDefinitionCommandHandler editAdditionalInfoDefinitionCommandHandler = new EditAdditionalInfoDefinitionCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            EditAdditionalInfoDefinitionCommand editAdditionalInfoDefinitionCommand = new EditAdditionalInfoDefinitionCommand(additionalInfoDefinitionDto);

            var result = await editAdditionalInfoDefinitionCommandHandler.Handle(editAdditionalInfoDefinitionCommand, new CancellationToken());

            mapper.Verify(x => x.Map(editAdditionalInfoDefinitionCommand, additionalInfoDefinition), Times.Once);
            result.Should().Be(id);
        }

        [Test]
        public void ShouldNotCallHandleIfNotSavedChanges()
        {
            dbSetAdditionalInfoDefinition.Setup(x => x.FindAsync(id)).Returns(new ValueTask<AdditionalInfoDefinition>(Task.FromResult(additionalInfoDefinition)));
            context.Setup(x => x.AdditionalInfoDefinitions).Returns(dbSetAdditionalInfoDefinition.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            EditAdditionalInfoDefinitionCommandHandler editAdditionalInfoDefinitionCommandHandler = new EditAdditionalInfoDefinitionCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            EditAdditionalInfoDefinitionCommand editAdditionalInfoDefinitionCommand = new EditAdditionalInfoDefinitionCommand(additionalInfoDefinitionDto);
            Func<Task> act = async () => await editAdditionalInfoDefinitionCommandHandler.Handle(editAdditionalInfoDefinitionCommand, new CancellationToken());

            act.Should().ThrowAsync<RestException>();
        }

        [Test]
        public void ShouldNotCallHandleIfTagNotExist()
        {
            dbSetAdditionalInfoDefinition.Setup(x => x.FindAsync(id)).Returns(null);
            context.Setup(x => x.AdditionalInfoDefinitions).Returns(dbSetAdditionalInfoDefinition.Object);

            EditAdditionalInfoDefinitionCommandHandler editAdditionalInfoDefinitionCommandHandler = new EditAdditionalInfoDefinitionCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            EditAdditionalInfoDefinitionCommand editAdditionalInfoDefinitionCommand = new EditAdditionalInfoDefinitionCommand(additionalInfoDefinitionDto);

            Func<Task> act = async () => await editAdditionalInfoDefinitionCommandHandler.Handle(editAdditionalInfoDefinitionCommand, new CancellationToken());

            act.Should().ThrowAsync<NotFoundException>();
        }
    }
}