using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SK.Domain.Entities;

namespace SK.Persistence.Configurations
{
    internal class DiscussionConfigurator : IEntityTypeConfiguration<Discussion>
    {
        public void Configure(EntityTypeBuilder<Discussion> builder)
        {
            builder
                .HasKey(d => d.Id);
        }
    }
}