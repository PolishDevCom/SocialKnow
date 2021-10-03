using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SK.Domain.Entities;

namespace SK.Persistence.Configurations
{
    public class UserEventConfigurator : IEntityTypeConfiguration<UserEvent>
    {
        public void Configure(EntityTypeBuilder<UserEvent> builder)
        {
            builder.HasKey(ue => new { ue.AppUserId, ue.EventId });

            builder.HasOne(u => u.AppUser)
                .WithMany(e => e.UserEvents)
                .HasForeignKey(u => u.AppUserId);

            builder.HasOne(e => e.Event)
                .WithMany(u => u.UserEvents)
                .HasForeignKey(e => e.EventId);
        }
    }
}