using Microsoft.AspNetCore.Identity;
using SK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SK.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            if (!context.TestValues.Any())
            {
                var testValues = new List<TestValue>
                {
                    new TestValue {Id=1, Name="Value101"},
                    new TestValue {Id=2, Name="Value102"},
                    new TestValue {Id=3, Name="Value103"},
                    new TestValue {Id=4, Name="Value104"},
                    new TestValue {Id=5, Name="Value105"},
                    new TestValue {Id=6, Name="Value106"}
                };
                context.TestValues.AddRange(testValues);
                await context.SaveChangesAsync();
            }

            if (!context.Events.Any())
            {
                var events = new List<Event>
                {
                    new Event
                    {
                        Title = "Past Event 1",
                        Date = DateTime.Now.AddMonths(-2),
                        Description = "Event 2 months ago",
                        Category = "webinar",
                        City = "Internet",
                        Venue = "Discord",
                        UserEvents = new List<UserEvent>
                        {
                            new UserEvent
                            {
                                AppUserId = "a",
                                IsHost = true,
                                DateJoined = DateTime.Now.AddMonths(-2)
                            }
                        }
                    },
                    new Event
                    {
                        Title = "Past Event 2",
                        Date = DateTime.Now.AddMonths(-1),
                        Description = "Event 1 month ago",
                        Category = "party",
                        City = "Warsaw",
                        Venue = "Club",
                        UserEvents = new List<UserEvent>
                        {
                            new UserEvent
                            {
                                AppUserId = "b",
                                IsHost = true,
                                DateJoined = DateTime.Now.AddMonths(-1)
                            },
                            new UserEvent
                            {
                                AppUserId = "a",
                                IsHost = false,
                                DateJoined = DateTime.Now.AddMonths(-1)
                            },
                        }
                    },
                    new Event
                    {
                        Title = "Future Event 1",
                        Date = DateTime.Now.AddMonths(1),
                        Description = "Event 1 month in future",
                        Category = "music",
                        City = "Cracov",
                        Venue = "Tauron Arena",
                        UserEvents = new List<UserEvent>
                        {
                            new UserEvent
                            {
                                AppUserId = "b",
                                IsHost = true,
                                DateJoined = DateTime.Now.AddMonths(1)
                            },
                            new UserEvent
                            {
                                AppUserId = "d",
                                IsHost = false,
                                DateJoined = DateTime.Now.AddMonths(1)
                            },
                        }
                    },
                    new Event
                    {
                        Title = "Future Event 2",
                        Date = DateTime.Now.AddMonths(2),
                        Description = "Event 2 months in future",
                        Category = "meeting",
                        City = "Danzig",
                        Venue = "Italian Restaurant",
                        UserEvents = new List<UserEvent>
                        {
                            new UserEvent
                            {
                                AppUserId = "c",
                                IsHost = true,
                                DateJoined = DateTime.Now.AddMonths(2)
                            },
                            new UserEvent
                            {
                                AppUserId = "d",
                                IsHost = false,
                                DateJoined = DateTime.Now.AddMonths(2)
                            },
                        }
                    },
                    new Event
                    {
                        Title = "Future Event 3",
                        Date = DateTime.Now.AddMonths(3),
                        Description = "Event 3 months in future",
                        Category = "drinks",
                        City = "Warsaw",
                        Venue = "Pub",
                        UserEvents = new List<UserEvent>
                        {
                            new UserEvent
                            {
                                AppUserId = "b",
                                IsHost = true,
                                DateJoined = DateTime.Now.AddMonths(3)
                            },
                            new UserEvent
                            {
                                AppUserId = "c",
                                IsHost = false,
                                DateJoined = DateTime.Now.AddMonths(3)
                            },
                        }
                    },
                };
                await context.Events.AddRangeAsync(events);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedDefaultUserAsync(UserManager<AppUser> userManager)
        {
            var defaultUser = new AppUser { Id="a", UserName = "administrator", Email = "administrator@localhost" };
            var bobUser = new AppUser { Id = "b", UserName = "bob", Email = "bob@localhost" };
            var tomUser = new AppUser { Id = "c", UserName = "tom", Email = "tom@localhost" };
            var janeUser = new AppUser { Id = "d", UserName = "jane", Email = "jane@localhost" };

            if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
            {
                await userManager.CreateAsync(defaultUser, "Administrator1!");
                await userManager.CreateAsync(bobUser, "Pa$$w0rd!");
                await userManager.CreateAsync(tomUser, "Pa$$w0rd!");
                await userManager.CreateAsync(janeUser, "Pa$$w0rd!");
            }
        }
    }
}
