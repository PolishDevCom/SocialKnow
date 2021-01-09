using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SK.Domain.Entities;

namespace SK.Persistence.Configurations
{
    public class PostConfiguratior : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder
                .HasOne(p => p.Discussion)
                .WithMany(d => d.Posts)
                .HasForeignKey(p => p.DiscussionId);
        }
    }
}
