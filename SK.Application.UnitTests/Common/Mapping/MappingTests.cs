using AutoMapper;
using NUnit.Framework;
using SK.Application.Articles.Queries;
using SK.Application.Common.Mapping;
using SK.Application.Events.Queries;
using SK.Domain.Entities;
using System;

namespace SK.Application.UnitTests.Common.Mapping
{
    public class MappingTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Test]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [Test]
        [TestCase(typeof(Article), typeof(ArticleDto))]
        [TestCase(typeof(Event), typeof(EventDto))]
        [TestCase(typeof(UserEvent), typeof(AttendeeDto))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = Activator.CreateInstance(source);

            _mapper.Map(instance, source, destination);
        }
    }
}
