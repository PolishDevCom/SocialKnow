using Microsoft.EntityFrameworkCore;
using SK.Application.Common.Interfaces;
using SK.Domain.Common;
using SK.Domain.Entities;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly IDateTime _dateTime;

        public ApplicationDbContext(DbContextOptions options, IDateTime dateTime) : base(options)
        {
            _dateTime = dateTime;
        }

        public DbSet<TestValue> TestValues { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.Entity.LastModified = _dateTime.Now;
                        entry.Entity.LastModifiedBy = "MADO";
                        break;
                    case EntityState.Added:
                        entry.Entity.Created = _dateTime.Now;
                        entry.Entity.CreatedBy = "MADO";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
            //builder.Entity<TestValue>()
            //    .HasData(
            //        new TestValue { Id = 10, Name = "Value201" },
            //        new TestValue { Id = 20, Name = "Value202" },
            //        new TestValue { Id = 30, Name = "Value203" },
            //        new TestValue { Id = 40, Name = "Value204" },
            //        new TestValue { Id = 50, Name = "Value205" },
            //        new TestValue { Id = 60, Name = "Value206" }
            //);
        }
    }
}
