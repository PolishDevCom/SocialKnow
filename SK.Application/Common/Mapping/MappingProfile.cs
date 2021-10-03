using AutoMapper;
using SK.Application.AdditionalInfoDefinitions.Commands.CreateAdditionalInfoDefinition;
using SK.Application.AdditionalInfoDefinitions.Commands.EditAdditionalInfoDefinition;
using SK.Application.AdditionalInfoDefinitions.Queries;
using SK.Application.Common.Helpers;
using SK.Domain.Entities;
using System;
using System.Linq;
using System.Reflection;

namespace SK.Application.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AdditionalInfoDefinition, AdditionalInfoDefinitionDto>()
                .ForMember(desc => desc.TypeOfField,
                    opt => opt.MapFrom(src => ConvertHelper.ConvertStringTypeToTypeOfFieldEnum(src.InfoType)));

            CreateMap<EditAdditionalInfoDefinitionCommand, AdditionalInfoDefinition>()
                .ForMember(desc => desc.InfoType,
                    opt => opt.MapFrom(src => ConvertHelper.ConvertTypeOfFieldEnumToStringType(src.TypeOfField)));

            CreateMap<CreateAdditionalInfoDefinitionCommand, AdditionalInfoDefinition>()
                .ForMember(desc => desc.InfoType,
                    opt => opt.MapFrom(src => ConvertHelper.ConvertTypeOfFieldEnumToStringType(src.TypeOfField)));

            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var mapFromTypes = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();

            var mapToTypes = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapTo<>)))
                .ToList();

            foreach (var type in mapFromTypes)
            {
                var instance = Activator.CreateInstance(type);

                var methodInfo = type.GetMethod("Mapping") ?? type.GetInterface("IMapFrom`1").GetMethod("Mapping");

                methodInfo?.Invoke(instance, new object[] { this });
            }

            foreach (var type in mapToTypes)
            {
                var instance = Activator.CreateInstance(type);

                var methodInfo = type.GetMethod("Mapping") ?? type.GetInterface("IMapTo`1").GetMethod("Mapping");

                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}