using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SK.Application.Common.Interfaces;
using SK.Persistence.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SK.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IDateTime, DateTimeService>();

            return services;
        }
    }
}
