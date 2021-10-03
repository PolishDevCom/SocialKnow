using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SK.Application.Common.Interfaces;
using SK.Infrastructure.Photos;
using SK.Infrastructure.Security;
using SK.Infrastructure.Services;
using SK.Persistence.Services;
using System.Text;

namespace SK.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("IsEventHost", policy =>
                {
                    policy.Requirements.Add(new IsEventHostRequirement());
                });

                opt.AddPolicy("IsDiscussionOwner", policy =>
                {
                    policy.Requirements.Add(new IsDiscussionOwnerRequirement());
                });

                opt.AddPolicy("IsPostOwner", policy =>
                {
                    policy.Requirements.Add(new IsPostOwnerRequirement());
                });
            });
            services.AddTransient<IAuthorizationHandler, IsEventHostRequirementHandler>();
            services.AddTransient<IAuthorizationHandler, IsDiscussionOwnerRequirementHandler>();
            services.AddTransient<IAuthorizationHandler, IsPostOwnerRequirementHandler>();

            var jwtSection = configuration.GetSection("Jwt");
            var jwtOptions = new JwtOptions();
            jwtSection.Bind(jwtOptions);
            services.Configure<JwtOptions>(jwtSection);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = true
                    };
                });

            services.AddScoped<IJwtGenerator, JwtGenerator>();

            services.AddTransient<ITokenManager, TokenManager>();
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IPhotoService, PhotoService>();
            services.AddTransient(typeof(IPaginationService<,>), typeof(PaginationService<,>));

            services.AddSingleton<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });

            return services;
        }
    }
}