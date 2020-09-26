using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SK.Domain.Entities;

namespace SK.Persistence.Configurations
{
    public class UserEventConfigurator : IEntityTypeConfiguration<UserEvent>
    {
        public void Configure(EntityTypeBuilder<UserEvent> builder)
        {
            builder.HasKey(ua => new { ua.AppUserId, ua.EventId });

            builder.HasOne(u => u.AppUser)
                .WithMany(a => a.UserEvents)
                .HasForeignKey(u => u.AppUserId);

            builder.HasOne(a => a.Event)
                .WithMany(u => u.UserEvents)
                .HasForeignKey(a => a.EventId);
        }
    }
}
