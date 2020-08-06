using Microsoft.AspNetCore.Identity;
using SK.Domain.Entities;
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
        }

        public static async Task SeedDefaultUserAsync(UserManager<AppUser> userManager)
        {
            var defaultUser = new AppUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

            if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
            {
                await userManager.CreateAsync(defaultUser, "Administrator1!");
            }
        }
    }
}
