using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SK.API;
using SK.Application.Common.Interfaces;
using SK.Domain.Entities;
using SK.Domain.Enums;
using SK.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


[SetUpFixture]
public class Testing
{
    private static IConfigurationRoot _configuration;
    private static IServiceScopeFactory _scopeFactory;
    private static string _currentUserName;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddUserSecrets("89e3b1df-3fb4-4947-8463-699a1fe9657a")
            .AddEnvironmentVariables();

        _configuration = builder.Build();

        var startup = new Startup(_configuration);

        var services = new ServiceCollection();

        services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
            w.EnvironmentName == "Development" &&
            w.ApplicationName == "SK.API"));

        services.AddLogging();

        startup.ConfigureServices(services);

        // Replace service registration for ICurrentUserService
        // Remove existing registration
        var currentUserServiceDescriptor = services.FirstOrDefault(d =>
            d.ServiceType == typeof(ICurrentUserService));

        services.Remove(currentUserServiceDescriptor);

        // Register testing version
        services.AddTransient(provider =>
            Mock.Of<ICurrentUserService>(s => s.Username == _currentUserName));
        services.AddTransient(provider =>
            Mock.Of<IJwtGenerator>());

        _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();
        EnsureDatabase();
    }

    private static void EnsureDatabase()
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
        context.Database.Migrate();
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetService<IMediator>();

        return await mediator.Send(request);
    }

    public static async Task<string> RunAsDefaultUserAsync()
    {
        return await RunAsUserAsync("test@local", "Testing1234!");
    }

    public static async Task<string> RunAsUserAsync(string userName, string password)
    {
        using var scope = _scopeFactory.CreateScope();

        var userManager = scope.ServiceProvider.GetService<UserManager<AppUser>>();

        var user = new AppUser { UserName = userName, Email = userName };

        var result = await userManager.CreateAsync(user, password);

        _currentUserName = user.UserName;

        return _currentUserName;
    }

    public static async Task<IList<string>> GetRolesForUserAsync(string userName)
    {
        using var scope = _scopeFactory.CreateScope();
        var userManager = scope.ServiceProvider.GetService<UserManager<AppUser>>();
        var user = await userManager.FindByNameAsync(userName);
        return await userManager.GetRolesAsync(user);
    }

    public static async Task SeedRoles()
    {
        using var scope = _scopeFactory.CreateScope();
        var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

        if (!roleManager.Roles.Any())
        {
            await roleManager.CreateAsync(new IdentityRole(IdentityRoles.Administrator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(IdentityRoles.Moderator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(IdentityRoles.Premium.ToString()));
            await roleManager.CreateAsync(new IdentityRole(IdentityRoles.Standard.ToString()));
        }
    }

    public static async Task ResetState()
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        var allArticles = from c in context.Articles select c;
        var allUsers = from c in context.Users select c;
        var allEvents = from c in context.Events select c;
        var allDiscussions = from c in context.Discussions select c;
        var allPosts = from c in context.Posts select c;

        context.Articles.RemoveRange(allArticles);
        context.Users.RemoveRange(allUsers);
        context.Events.RemoveRange(allEvents);
        context.Discussions.RemoveRange(allDiscussions);
        context.Posts.RemoveRange(allPosts);

        await context.SaveChangesAsync();

        _currentUserName = null;
    }

    public static async Task<TEntity> FindAsync<TEntity>(int id)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        return await context.FindAsync<TEntity>(id);
    }

    public static async Task<TEntity> FindByGuidAsync<TEntity>(Guid id)
    where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        return await context.FindAsync<TEntity>(id);
    }

    public static List<UserEvent> FindUserEventsByEventGuidAsync(Guid id)
    {
        List<UserEvent> userEventsList = new List<UserEvent>();

        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        foreach (var userEvent in context.UserEvents.Select(ue => ue).Where(ue => ue.EventId == id).Include(a => a.AppUser))
        {
            userEventsList.Add(userEvent);
        }

        return userEventsList;
    }

    public static List<Post> FindPostsByDiscussionGuidAsync(Guid id)
    {
        List<Post> postsList = new List<Post>();

        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        foreach (var post in context.Posts.Select(p => p).Where(p => p.DiscussionId == id).Include(d => d.Discussion))
        {
            postsList.Add(post);
        }

        return postsList;
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
    }
}