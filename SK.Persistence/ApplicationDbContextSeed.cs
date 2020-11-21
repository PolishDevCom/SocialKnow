using Microsoft.AspNetCore.Identity;
using SK.Domain.Entities;
using SK.Domain.Enums;
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
            if (!context.Articles.Any())
            {
                var articles = new List<Article>
                {
                    new Article 
                    {
                        Id=Guid.NewGuid(),
                        Title="First Article",
                        Abstract="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis eros quam, scelerisque eu odio a, maximus gravida sapien. Proin velit sapien, placerat nec lorem sit amet, tempor egestas velit. Integer ullamcorper erat dolor, quis sagittis enim pharetra eget. Vestibulum magna ex, semper in efficitur vitae, accumsan eget eros.",
                        Image=null,
                        Content="In hac habitasse platea dictumst. Nulla efficitur justo eu nisi commodo blandit. Integer tempus sit amet urna a lobortis. Praesent lacus ipsum, accumsan vitae lectus eget, sodales dapibus diam. Suspendisse hendrerit rutrum aliquet. Curabitur vel aliquet eros. Sed in lorem faucibus, pharetra odio vel, iaculis nunc.",
                        CreatedBy="Content Admin",
                        Created=DateTime.UtcNow
                    },
                    new Article
                    {
                        Id=Guid.NewGuid(),
                        Title="Second Article",
                        Abstract="Nulla ullamcorper justo ex, vel ultricies augue tincidunt porta. Phasellus eros lorem, vehicula tincidunt tempor consectetur, eleifend molestie urna.",
                        Image=null,
                        Content="Duis tempus dolor nec ante ullamcorper consequat. Pellentesque sagittis mauris condimentum sollicitudin aliquet. Fusce tortor lorem, dignissim ac scelerisque at, blandit viverra leo. Phasellus et lorem sit amet quam sollicitudin ultrices sit amet blandit nisi. Sed ultrices malesuada purus, eu blandit nibh sodales id. Praesent ac turpis eget arcu luctus ullamcorper.",
                        CreatedBy="Content Admin",
                        Created=DateTime.UtcNow.AddDays(-7)
                    },
                    new Article
                    {
                        Id=Guid.NewGuid(),
                        Title="Third Article",
                        Abstract="Donec sagittis tincidunt tempus. Vestibulum a enim est. Donec ut odio sollicitudin lacus blandit porttitor sed interdum quam. Curabitur eget auctor erat. Sed at enim imperdiet, efficitur lectus id, pellentesque dui.",
                        Image=null,
                        Content="Aenean odio dolor, mollis non diam a, dictum iaculis enim. Praesent ultrices pharetra dolor, ac bibendum felis scelerisque quis. Donec congue mi ligula, ac feugiat nunc porttitor at. Aenean ornare accumsan auctor. Vestibulum dictum orci vel aliquet sodales. Pellentesque pulvinar at eros nec euismod.",
                        CreatedBy="Content Admin",
                        Created=DateTime.UtcNow.AddDays(-3)
                    },
                };
                context.Articles.AddRange(articles);
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
            if (!context.Discussions.Any())
            {
                var discussion1Id = Guid.NewGuid();
                var discussion2Id = Guid.NewGuid();
                var discussion3Id = Guid.NewGuid();

                var discussions = new List<Discussion>
                {
                    new Discussion()
                    {
                        Id = discussion1Id,
                        Title = "Discussion No 1",
                        Description = "Description of discussion no 1",
                        IsClosed = true,
                        IsPinned = false,
                        Posts = new List<Post>()
                        {
                            new Post()
                            {
                                Body = "This is first post for this discussion 1",
                                IsPinned = false,
                                DiscussionId = discussion1Id
                            },
                            new Post()
                            {
                                Body = "This is second post for this discussion 1",
                                IsPinned = false,
                                DiscussionId = discussion1Id
                            },
                            new Post()
                            {
                                Body = "This is third post for this discussion 1",
                                IsPinned = false,
                                DiscussionId = discussion1Id
                            }
                        }                        
                    },
                    new Discussion()
                    {
                        Id = discussion2Id,
                        Title = "Discussion No 2",
                        Description = "Description of discussion no 2",
                        IsClosed = false,
                        IsPinned = true,
                        Posts = new List<Post>()
                        {
                            new Post()
                            {
                                Body = "This is first post for this discussion 2",
                                IsPinned = false,
                                DiscussionId = discussion2Id
                            },
                            new Post()
                            {
                                Body = "This is second post for this discussion 2",
                                IsPinned = false,
                                DiscussionId = discussion2Id
                            },
                            new Post()
                            {
                                Body = "This is third post for this discussion 2",
                                IsPinned = true,
                                DiscussionId = discussion2Id
                            }
                        }
                    },
                    new Discussion()
                    {
                        Id = discussion3Id,
                        Title = "Discussion No 3",
                        Description = "Description of discussion no 3",
                        IsClosed = false,
                        IsPinned = false,
                        Posts = new List<Post>()
                        {
                            new Post()
                            {
                                Body = "This is first post for this discussion 3",
                                IsPinned = false,
                                DiscussionId = discussion3Id
                            },
                            new Post()
                            {
                                Body = "This is second post for this discussion 3",
                                IsPinned = false,
                                DiscussionId = discussion3Id
                            },
                            new Post()
                            {
                                Body = "This is third post for this discussion 3",
                                IsPinned = false,
                                DiscussionId = discussion3Id
                            }
                        }
                    }
                };
                await context.Discussions.AddRangeAsync(discussions);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(IdentityRoles.Administrator.ToString()));
                await roleManager.CreateAsync(new IdentityRole(IdentityRoles.Moderator.ToString()));
                await roleManager.CreateAsync(new IdentityRole(IdentityRoles.Premium.ToString()));
                await roleManager.CreateAsync(new IdentityRole(IdentityRoles.Standard.ToString()));
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
                //adding users
                await userManager.CreateAsync(defaultUser, "Administrator1!");
                await userManager.CreateAsync(bobUser, "Pa$$w0rd!");
                await userManager.CreateAsync(tomUser, "Pa$$w0rd!");
                await userManager.CreateAsync(janeUser, "Pa$$w0rd!");

                //adding roles for users
                await userManager.AddToRoleAsync(defaultUser, IdentityRoles.Administrator.ToString());
                await userManager.AddToRoleAsync(bobUser, IdentityRoles.Moderator.ToString());
                await userManager.AddToRoleAsync(tomUser, IdentityRoles.Premium.ToString());
                await userManager.AddToRoleAsync(janeUser, IdentityRoles.Standard.ToString());
            }
        }

    }
}
