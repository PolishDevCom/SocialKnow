using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.AdditionalInfoDefinitions.Commands.DeleteAdditionalInfoDefinition;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.AdditionalInfoDefinitions;
using SK.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.AdditionalInfoDefinitions.Commands
{
    public class DeleteAdditionalInfoDefinitionCommandTest
    {
        private readonly Guid id;
        private readonly Mock<DbSet<AdditionalInfoDefinition>> dbSetAdditionalInfoDefinition;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IStringLocalizer<AdditionalInfoDefinitionsResource>> stringLocalizer;

        public DeleteAdditionalInfoDefinitionCommandTest()
        {
            id = new Guid();
            dbSetAdditionalInfoDefinition = new Mock<DbSet<AdditionalInfoDefinition>>();
            context = new Mock<IApplicationDbContext>();
            stringLocalizer = new Mock<IStringLocalizer<AdditionalInfoDefinitionsResource>>();
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            dbSetAdditionalInfoDefinition.Setup(x => x.FindAsync(id)).Returns(new ValueTask<AdditionalInfoDefinition>(Task.FromResult(new AdditionalInfoDefinition { Id = id })));
            context.Setup(x => x.AdditionalInfoDefinitions).Returns(dbSetAdditionalInfoDefinition.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            DeleteAdditionalInfoDefinitionCommandHandler deleteAdditionalInfoDefinitionCommandHandler = new DeleteAdditionalInfoDefinitionCommandHandler(context.Object, stringLocalizer.Object);
            DeleteAdditionalInfoDefinitionCommand deleteAdditionalInfoDefinitionCommand = new DeleteAdditionalInfoDefinitionCommand(id);

            var result = await deleteAdditionalInfoDefinitionCommandHandler.Handle(deleteAdditionalInfoDefinitionCommand, new CancellationToken());

            result.Should().Be(Unit.Value);
        }

        [Test]
        public void ShouldNotCallHandleIfNotSavedChanges()
        {
            dbSetAdditionalInfoDefinition.Setup(x => x.FindAsync(id)).Returns(new ValueTask<AdditionalInfoDefinition>(Task.FromResult(new AdditionalInfoDefinition { Id = id })));
            context.Setup(x => x.AdditionalInfoDefinitions).Returns(dbSetAdditionalInfoDefinition.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            DeleteAdditionalInfoDefinitionCommandHandler deleteAdditionalInfoDefinitionCommandHandler = new DeleteAdditionalInfoDefinitionCommandHandler(context.Object, stringLocalizer.Object);
            DeleteAdditionalInfoDefinitionCommand deleteAdditionalInfoDefinitionCommand = new DeleteAdditionalInfoDefinitionCommand(id);

            Func<Task> act = async () => await deleteAdditionalInfoDefinitionCommandHandler.Handle(deleteAdditionalInfoDefinitionCommand, new CancellationToken());

            act.Should().ThrowAsync<RestException>();
        }

        [Test]
        public void ShouldNotCallHandleIfTagNotExist()
        {
            dbSetAdditionalInfoDefinition.Setup(x => x.FindAsync(id)).Returns(null);
            context.Setup(x => x.AdditionalInfoDefinitions).Returns(dbSetAdditionalInfoDefinition.Object);

            DeleteAdditionalInfoDefinitionCommandHandler deleteAdditionalInfoDefinitionCommandHandler = new DeleteAdditionalInfoDefinitionCommandHandler(context.Object, stringLocalizer.Object);
            DeleteAdditionalInfoDefinitionCommand deleteAdditionalInfoDefinitionCommand = new DeleteAdditionalInfoDefinitionCommand(id);

            Func<Task> act = async () => await deleteAdditionalInfoDefinitionCommandHandler.Handle(deleteAdditionalInfoDefinitionCommand, new CancellationToken());

            act.Should().ThrowAsync<NotFoundException>();
        }
    }
}