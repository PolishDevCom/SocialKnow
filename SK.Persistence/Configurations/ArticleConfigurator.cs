using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SK.Domain.Entities;

namespace SK.Persistence.Configurations
{
    public class ArticleConfigurator : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.Property(a => a.Title)
                .HasMaxLength(255)
                .IsRequired();
            builder.Property(a => a.Abstract)
                .IsRequired();
            builder.Property(a => a.Content)
                .IsRequired();
        }
    }
}