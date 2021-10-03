using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SK.Application.Common.Interfaces;

namespace SK.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            #region PostgreSQL configuration

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            });

            #endregion PostgreSQL configuration

            #region Redis configuration

            services.AddStackExchangeRedisCache(options => options.Configuration = configuration["Redis:ConnectionString"]);

            #endregion Redis configuration

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            return services;
        }
    }
}