using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using SK.Application.AdditionalInfoDefinitions.Commands;
using SK.Application.AdditionalInfoDefinitions.Commands.CreateAdditionalInfoDefinition;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Interfaces;
using SK.Application.Common.Resources.AdditionalInfoDefinitions;
using SK.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Application.UnitTests.AdditionalInfoDefinitions.Commands
{
    public class CreateAdditionalInfoDefinitionCommandTest
    {
        private readonly Mock<DbSet<AdditionalInfoDefinition>> dbSetAdditionalInfoDefinition;
        private readonly Mock<IApplicationDbContext> context;
        private readonly Mock<IStringLocalizer<AdditionalInfoDefinitionsResource>> stringLocalizer;
        private readonly Mock<IMapper> mapper;

        public CreateAdditionalInfoDefinitionCommandTest()
        {
            dbSetAdditionalInfoDefinition = new Mock<DbSet<AdditionalInfoDefinition>>();
            context = new Mock<IApplicationDbContext>();
            stringLocalizer = new Mock<IStringLocalizer<AdditionalInfoDefinitionsResource>>();
            mapper = new Mock<IMapper>();
        }

        [Test]
        public async Task ShouldCallHandle()
        {
            var id = new Guid();
            var additionalInfoDefinitionDto = new AdditionalInfoDefinitionCreateOrEditDto { Id = id };
            var additionalInfoDefinition = new AdditionalInfoDefinition { Id = id };

            context.Setup(x => x.AdditionalInfoDefinitions).Returns(dbSetAdditionalInfoDefinition.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            CreateAdditionalInfoDefinitionCommandHandler createAdditionalInfoDefinitionCommandHandler = new CreateAdditionalInfoDefinitionCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            CreateAdditionalInfoDefinitionCommand createAdditionalInfoDefinitionCommand = new CreateAdditionalInfoDefinitionCommand(additionalInfoDefinitionDto);

            mapper.Setup(x => x.Map<AdditionalInfoDefinition>(createAdditionalInfoDefinitionCommand)).Returns(additionalInfoDefinition);

            var result = await createAdditionalInfoDefinitionCommandHandler.Handle(createAdditionalInfoDefinitionCommand, new CancellationToken());

            result.Should().Be(id);
        }

        [Test]
        public void ShouldNotCallHandleIfNotSavedChanges()
        {
            context.Setup(x => x.AdditionalInfoDefinitions).Returns(dbSetAdditionalInfoDefinition.Object);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(0));

            CreateAdditionalInfoDefinitionCommandHandler createAdditionalInfoDefinitionCommandHandler = new CreateAdditionalInfoDefinitionCommandHandler(context.Object, stringLocalizer.Object, mapper.Object);
            CreateAdditionalInfoDefinitionCommand createAdditionalInfoDefinitionCommand = new CreateAdditionalInfoDefinitionCommand();

            mapper.Setup(x => x.Map<AdditionalInfoDefinition>(createAdditionalInfoDefinitionCommand)).Returns(new AdditionalInfoDefinition());

            Func<Task> act = async () => await createAdditionalInfoDefinitionCommandHandler.Handle(createAdditionalInfoDefinitionCommand, new CancellationToken());

            act.Should().ThrowAsync<RestException>();
        }
    }
}
